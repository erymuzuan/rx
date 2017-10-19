# Message Tracking and SLA *(this pre-release feature, please comment)*

Reactive Developer allows you to set your message to be delivered in a reliable and scalable manner, and one of the features in this messaging infrastructure is the ability to have a certain level of SLA towards your messaging

## Common scenario case study

API(Your OperationEndpoint) -> Trigger(Your custom trigger) -> Messaging Action(or any trigger action)

Lets say, once a message is accepted with 202 HTTP response code at your `OperationEndpoint API`, you want them to be delivered to you adapter and finish executing it with a time period , let's say 15 seconds.

In your `EntityDefinition` you should `EnableTracking` to `true`
```json
{
  "$type": "Bespoke.Sph.Domain.EntityDefinition, domain.sph",
  "Name": "Patient",
  "EnableTracking": true,
  "Id": "patient",
   // remove for brevity
}
```

or use the designer

![https://i.imgur.com/Vn8WcHh.png
](https://i.imgur.com/Vn8WcHh.png
)

For the `Trigger` you will to do
```json
{
  "$type": "Bespoke.Sph.Domain.Trigger, domain.sph",
  "Name": "Patient trigger no 1",
  "Entity": "Patient",
  "EnableTracking": true,
  "ShouldProcessedOnceAccepted": 5000,
   // remove for brevity
}

```

or via the designer

![Trigger](https://i.imgur.com/VDrtv6b.png)

1. Mark the trigger to track the process
2. The time span that this trigger must be successfully processed once a message is accepted(HTTP status code 202) by your `OperationEndpoint`

## Notification action
`MessageSlaNotificationAction` is the base class where you should write your own notification. Reactive Developer provides one notification
```csharp

public class SlaNotCompletedEmailAction : MessageSlaNotificationAction
{
    public SlaNotCompletedEmailAction(string emailTemplateMapping, string toAddresses){}
    public override bool UseAsync => true; // if false, you should implement bool Execute(...)
    public override async Task<bool> ExecuteAsync(MessageTrackingStatus status, MessageSlaEvent @event)
    {
        // removed for brevity
    }
}

```

Registering your `NotificationAction` via your worker config file
```xml
<object name="IMessageSlaManager" type="Bespoke.Sph.RabbitMqPublisher.MessageSlaManager, rabbitmq.changepublisher, Version=1.0.2.1007, Culture=neutral">
<property name="NotStartedActionCollection">
    <list element-type="Bespoke.Sph.Domain.MessageSlaNotificationAction, domain.sph">
    <object type="Bespoke.Sph.RabbitMqPublisher.SlaNotCompletedEmailAction, rabbitmq.changepublisher">
        <constructor-arg name="emailTemplateMapping" value="Patient:patient-email-not-completed"/>
        <constructor-arg name="toAddresses" value="erymuzuan@gmail.com"/>
    </object>
    </list>
</property>
</object>
```



## Message cancellation

Your `NotificationAction.Execute(Async)` method returns a Boolean value, if any return `false` then Reactive Developer will put up a flag to cancel your message. Thus it will not be process by your original subscriber anymore

## `IMessageCancellationRepository`
This interface is used by `Subscriber<T>` to verify that if your message is marked for cancellation. If the implementation return `true` when `CheckMessageAsync(messageId, worker)` is called, then the `Subscriber<T>` will not call `ProcessItem` method(skipping all your action) it just quickly `BacicAck` the message and  track the `Cancelled` event

Currently there's 2 implementation for `IMessageCancellationRepository`
1. Elasticsearch (this has been tested to some degree)
2. SQL Server (still buggy, and a bit slower, but offers a degree of consitency a notch)

this is how register your implementation in worker.config
```xml

<object name="ICancelledMessageRepository"
    type="Bespokse.Sph.ElasticsearchRepository.CancelledMessageRepository, elasticsearch.repository"
    init-method="Initialize" />

```

The `Initialize` method is used to create the mapping in your elastisearch index



## Troubleshooting tips
The key is understanding how these features are built., using these combinations

1. RabbitMQ expiring message
2. Elasticsearch repository for event tracking


This is high level view of how it works
1. One an `EntityDefinition` is `EnableTracking`is configured, `IMessageTracker`, will inject a `message-id` headers. This is not a item id, but a unique id to identify a message rather than the item(message body)
2. The `IMessageTracker` will track the `Entity` for every movement
3. If your set `shouldProcessedOnceAccepted` value, then `IMessageSlaManager` will create an expiring message and publish it to `rx.delay.exchange.messages.sla` which only have 1 queue bind to `rx.delay.exchange.messages.sla`.
4. When the message in `rx.delay.exchange.messages.sla` expires, it will be routed to `rx.notification.queue.messages.sla` where `Bespoke.Sph.MessageTrackerSla.MessageSlaTrackerSubscriber, subscriber.message.sla` subscriber is consuming to.
5. The subscriber, once receive a message, will check with `IMessageTracker` for `ProcessingCompleted` event for `message-id` and `worker` combination
