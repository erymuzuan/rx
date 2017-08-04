<Query Kind="Program">
  <Reference Relative="..\source\web\web.sph\bin\domain.sph.dll">F:\project\work\rx.pos-entt\source\web\web.sph\bin\domain.sph.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\Newtonsoft.Json.dll">F:\project\work\rx.pos-entt\source\web\web.sph\bin\Newtonsoft.Json.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\RabbitMQ.Client.dll">F:\project\work\rx.pos-entt\source\web\web.sph\bin\RabbitMQ.Client.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>RabbitMQ.Client</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <AppConfig>
    <Content>
      <configuration>
        <appSettings>
          <add key="sph:ApplicationName" value="DevV1" />
        </appSettings>
      </configuration>
    </Content>
  </AppConfig>
</Query>

void Main()
{
	var name = ConfigurationManager.ApplicationName;
	Console.WriteLine(name);
	Console.WriteLine("RabbitMQ host : " + ConfigurationManager.RabbitMqHost);
	Console.WriteLine("RabbitMQ username : " + ConfigurationManager.RabbitMqUserName);
	Console.WriteLine("RabbitMQ password : " + ConfigurationManager.RabbitMqPassword);

	var batch = new QueueBatch(name);
	try
	{
		batch.StartAsync()
		.Wait();
	}
	finally
	{
		batch.Stop();
	}

}

// Define other methods and classes here
public class QueueBatch
{
	private const int BATCH_SIZE = 10;
	// TODO : name your queue, it should be unique
	private const string QueueName = "batch-processing";
	// TODO : add your routing key(s)
	private readonly static string[] RoutingKeys = new string[] { "Patient.added.Register" };

	// TODO : increase you prefectch  accordingly
	private const int PrefetchCount = 1;
	const bool NO_ACK = false;
	const string EXCHANGE_NAME = "sph.topic";
	const string DEAD_LETTER_EXCHANGE = "sph.ms-dead-letter";
	// TODO : you could change your DLQ
	const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";

	public QueueBatch(string applicationName)
	{

	}

	public async Task StartAsync()
	{
		var channel = this.OpenChannel();
		var br = channel.BasicGet(QueueName, NO_ACK);

		var bucket = new Queue<Tuple<string, ulong>>();
		var tags = new Queue<ulong>();
		int? initialMessagesCount = null;
		int processedMessagesCount = 0;

		// you can use C# 7.0 local function, but if you use domain.sph.dll, then use Rosylyn 1.0, you can't use C# 7.0
		System.Func<Task> processBucket = async () =>
		{
			var ipc = new Ipc(bucket);
			var errors = await ipc.SendAsync();

			// send the ack, once IPC has processed them successfully
			while (tags.Count > 0)
			{
				var tag = tags.Dequeue();
				
				// DLQ
				if(errors.Contains(tag))
					channel.BasicReject(tag, false);
				else
					channel.BasicAck(tag, false);
			}
		};


		while (null != br)
		{
			tags.Enqueue(br.DeliveryTag);
			var body = await DecompressAsync(br.Body);
			initialMessagesCount = initialMessagesCount ?? (int)br.MessageCount;

			bucket.Enqueue(new Tuple<string, ulong>(body, br.DeliveryTag));
			processedMessagesCount++;
			if (bucket.Count >= BATCH_SIZE)
			{
				await processBucket();
				Console.WriteLine($"Processed {processedMessagesCount} messages from {initialMessagesCount}");
				Console.WriteLine($"Estimated {(processedMessagesCount + br.MessageCount) - initialMessagesCount} new messages after we started");
			}

			// NOTE : we only takes all the messages in the queue when we started
			// additional messages that coming to the queue after we started will not be processed, instead it will to the next batch
			if (processedMessagesCount >= initialMessagesCount)
				break;

			br = channel.BasicGet(QueueName, NO_ACK);
		}

		// process the last bucket
		if (bucket.Count > 0)
		{
			Console.WriteLine();
			Console.WriteLine($"Processing the last {bucket.Count} messages");
			await processBucket();
		}
	}

	private IConnection m_connection;
	private IModel m_channel;
	private IModel OpenChannel()
	{
		this.Stop(); // stop any previous connection
		var factory = new ConnectionFactory
		{
			UserName = ConfigurationManager.RabbitMqUserName,
			VirtualHost = ConfigurationManager.ApplicationName,
			Password = ConfigurationManager.RabbitMqPassword,
			HostName = ConfigurationManager.RabbitMqHost,
			Port = ConfigurationManager.RabbitMqPort
		};
		m_connection = factory.CreateConnection();
		m_channel = m_connection.CreateModel();


		m_channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);
		m_channel.ExchangeDeclare(DEAD_LETTER_EXCHANGE, ExchangeType.Topic, true);
		var args = new Dictionary<string, object> { { "x-dead-letter-exchange", DEAD_LETTER_EXCHANGE } };
		m_channel.QueueDeclare(QueueName, true, false, false, args);

		m_channel.QueueDeclare(DEAD_LETTER_QUEUE, true, false, false, args);
		m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "#", null);

		foreach (var s in RoutingKeys)
		{
			m_channel.QueueBind(QueueName, EXCHANGE_NAME, s, null);
		}
		m_channel.BasicQos(0, PrefetchCount, false);

		return m_channel;
	}


	public void Stop()
	{
		m_connection?.Close();
		m_connection?.Dispose();
		m_connection = null;

		m_channel?.Close();
		m_channel?.Dispose();
		m_channel = null;


	}

	private static async Task<string> DecompressAsync(byte[] content)
	{
		using (var orginalStream = new MemoryStream(content))
		using (var destinationStream = new MemoryStream())
		using (var gzip = new GZipStream(orginalStream, CompressionMode.Decompress))
		{
			try
			{
				await gzip.CopyToAsync(destinationStream);
			}
			catch (InvalidDataException)
			{
				orginalStream.CopyTo(destinationStream);
			}
			destinationStream.Position = 0;
			using (var sr = new StreamReader(destinationStream))
			{
				var text = await sr.ReadToEndAsync();
				return text;
			}
		}
	}
}


public class Ipc
{
	private Queue<Tuple<string, ulong>> m_bucket;
	public Ipc(Queue<Tuple<string, ulong>> bucket)
	{
		m_bucket = bucket;
	}

	//
	public async Task<ulong[]> SendAsync()
	{
		var errorList = new List<ulong>();
		
		// TODO : remove the delay, just to simulate the actual process takes some time
		await Task.Delay(500);
		var message = m_bucket.Dequeue();
		while (m_bucket.Count > 0)
		{
			// TODO : Deserialize your json send to your IPC
			var json = JObject.Parse(message.Item1);
			// TODO : simulate error, and log the exception/error somewhere
			// in this case I use Id when it ends with a "9"
			// you should handle your network/database availability with Polly policies
			var id = json.SelectToken("$.Id").Value<string>();
			if (id.EndsWith("9")) 
			{
				Console.WriteLine("error processing " + message.Item2);
				await Task.Delay(500);
				errorList.Add(message.Item2);
			}
			Console.Write("*");
			message = m_bucket.Dequeue();
		}
		Console.WriteLine();
		
		
		return errorList.ToArray();
	}
}