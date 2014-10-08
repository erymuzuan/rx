<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference Relative="..\source\web\web.sph\bin\RabbitMQ.Client.dll">C:\project\work\sph\source\web\web.sph\bin\RabbitMQ.Client.dll</Reference>
  <Namespace>RabbitMQ.Client</Namespace>
</Query>

 var factory = new ConnectionFactory
            {
                UserName ="guest",
                Password = "guest",
                HostName = "localhost",
                Port = 5672,
                VirtualHost = "Dev"
            };
var connection = factory.CreateConnection();
var channel = connection.CreateModel();



const string RETRY_EXCHANGE = "test.retry.exchange";
const string RETRY_QUEUE = "test.retry.queue";
var delay = 15000; // in ms

// Messages will drop off RetryQueue into WorkExchange for reprocessing
// All messages in queue will expire at same rate
var queueArgs = new Dictionary<string, object> {
     { "x-dead-letter-exchange", "sph.topic" },
	 {"x-dead-letter-routing-key", "persistence"},
     { "x-message-ttl", delay }
 };

channel.ExchangeDeclare(RETRY_EXCHANGE, "direct");
channel.QueueDeclare(RETRY_QUEUE, true, false, false, queueArgs);
channel.QueueBind(RETRY_QUEUE, RETRY_EXCHANGE, string.Empty, null);

var body = Encoding.UTF8.GetBytes("Test message " + DateTime.Now);
 var props = channel.CreateBasicProperties();
props.DeliveryMode = 2;
props.ContentType = "application/json";
props.Headers = new Dictionary<string, object>();

channel.BasicPublish(RETRY_EXCHANGE, string.Empty, props, body);
			
			
channel.Close();
connection.Close();