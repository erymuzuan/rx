# Message Tracking and SLA *(this pre-release feature, please comment)*

Reactive Developer allows you to set your message to be delivered in a reliable and scalable manner, and one of the features in this messaging infrastructure is the ability to have a certain level of SLA towards your messaging

## Common scenario case study

API(Your OperationEndpoint) -> Trigger(Your custom trigger) -> Messaging Action(or any trigger action)

Lets say, once a message is accepted with 202 HTTP response code at your `OperationEndpoint API`, you want them to be delivered to you adapter and finish executing it with a time period , let's say 15 seconds.

In your `WorkersConfig` select your config file which contains the `subsriber` for the trigger
```json
{
    "name": "patient",
    "description": "Development on local machines",
    "isEnabled": true,
    "environment": "dev",
    "subscriberConfigs": [{
        "entity": "Patient",
        "instancesCount": 1,
        "prefetchCount": 1,
        "priority": 0,
        "queueName": "trigger_subs_patient-patient-trigger-no-1",
        "fullName": "Bespoke.DevV1.TriggerSubscribers.PatientPatientTriggerNo1TriggerSubscriber",
        "assembly": "subscriber.trigger.patient-patient-trigger-no-1",
        "shouldProcessedOnceAccepted": 5000,
        "trackingEnabled": true
    }],
    "createdBy": "erymuzuan",
    "id": "patient-email",
    "createdDate": "2016-10-07T00:00:00+08:00",
    "changedBy": "erymuzuan",
    "changedDate": "2016-10-07T00:00:00+08:00",
    "webId": "dev"
}
```

There are at least 3 properties you have to set
1. `entity` is the name of your `EntityDefinition`
2. `shouldProcessedOnceAccepted` once your `OperationEndpoint API` accepted with HTT 202 response code, this where the clock starts ticking, once it reach this time span (the default is in miliseconds). Reactive Developer will notify your `MessageSlaNotificationAction`.
3. `trackingEnabled` tracking should be enabled for this trigger


## Notification action
`MessageSlaNotificationAction` is the base class where you should write your own notification. Reactive Developer provides one notification
```csharp

public class SlaNotCompletedEmailAction : MessageSlaNotificationAction
{
    public SlaNotCompletedEmailAction(string emailTemplateMapping, string toAddresses){}
    public override bool UseAsync => true; // if false, you should implement bool Execute(...)
    public override async Task<bool> ExecuteAsync(MessageTrackingStatus status, Entity item, MessageSlaEvent @event)
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

Your `NotificationAction.Execute(Async)` method returns a boolean value, if any return `false` then Reactive Developer will put up a flag to cancel your message. Thus it will not be process by your original subscriber anymore

## `IMessageCancellationRepository`
This interface is used by `Subscriber<T>` to verify that if your message is marked for cancellation. If the implementation return `true` when `CheckMessageAsync(messageId, worker)` is called, then the `Subscriber<T>` will not call `ProcessItem` method(skipping all your action) it just quickly `BacicAck` the message and  track the `Cancelled` event

Currently there's 2 implementation for `IMessageCancellationRepository`
1. Elasticsearch (this has been tested to some degree)
2. SQL Server (still buggy, and a bit slower, but offers a degree of consitency a notch)

this is how register your implementation in worker.config
```xml

<object name="ICancelledMessageRepository"
    type="Bespokse.Sph.ElasticsearchRepository.CancelledMessageRepository, elasticsearch.repository"
    init-method="InitializeAsync" />

```

The `InitializeAsync` method is used to create the mapping in your elastisearch index



## Troubleshooting tips
The key is understanding how these features are built., using these combinations

1. RabbitMQ expiring message
2. Elasticsearch repository for event tracking


This is high level view of how it works
1. When you configure your `Trigger` in your `WorkersConfig` to `trackingEnabled:true`, IMessageTracker, will inject an `id` to your message header `message-id` field.
2. The `IMessageTracker` will track the `Entity` for every movement
3. If your set `shouldProcessedOnceAccepted` value, then `IMessageSlaManager` will create an expiring message and publish it to `rx.delay.exchange.messages.sla` which only have 1 queue bind to `rx.delay.exchange.messages.sla`.
4. When the message in `rx.delay.exchange.messages.sla` expires, it will be routed to `rx.notification.queue.messages.sla` where `Bespoke.Sph.MessageTrackerSla.MessageSlaTrackerSubscriber, subscriber.message.sla` subscriber is consuming to.
5. The subscriber, once receive a message, will check with `IMessageTracker` for `ProcessingCompleted` event for `message-id` and `worker` combination
