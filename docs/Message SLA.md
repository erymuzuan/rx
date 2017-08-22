# Message SLA

Reactive Developer allows you to set your message to be delivered in a reliable and scalable manner, and one of the features in this messaging infrastructure is the ability to have a certain level of SLA towards your messaging

## Common scenario case study

API(Your OperationEndpoint) -> Trigger(Your custom trigger) -> Messaging Action(or any trigger action)

Lets say, once a message is accepted witn 202 HTTP response code at your `OperationEndpoint API`, you want them to be delivered to you adapter and finish executing it with a time period , let's say 15 seconds.

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
        "shouldProcessedOnceAccepted": true
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
3. `shouldProcessedOnceAccepted` tracking should be enabled for this trigger


## Notification action
`MessageSlaNotificationAction` is the base class where you should write your own notification. Reactive Developer provides one notification
```csharp

public class SlaNotCompletedEmailAction : MessageSlaNotificationAction
{
    public SlaNotCompletedEmailAction(string emailTemplateMapping, string toAddresses){}
    public override bool UseAsync => true;
    public override async Task<bool> ExecuteAsync(MessageTrackingStatus status, Entity item, MessageSlaEvent @event)
    {
        // removed for bervity
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

**NOTE : NOT FULLY IMPLEMENTED YET**

