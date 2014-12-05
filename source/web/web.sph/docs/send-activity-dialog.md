#Send Activity

SendActivity allows you to send a message to one of the adapter available in your solution.
##Properties
<table class="table table-condensed table-bordered">
    <thead>
        <tr>
            <th>Property</th>
            <th>Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td>AdapterAssembly</td><td> - Select the assembly(dll) where your adapter is defined </td></tr>
        <tr><td>Adapter</td><td> - Select a type where your adapter is</td></tr>
        <tr><td>Method</td><td> - The operation that will be called</td></tr>
        <tr><td>ArgumentPath</td><td> - The argument or input path in your WorkflowDefinition that will be passed to the adapter method</td></tr>
        <tr><td>ReturnValuePath</td><td> - You can optionally store the return value of the operation call</td></tr>
        <tr><td>IsSynchronous</td><td> - If your method doesnot contails any async, then you should check this</td></tr>
        <tr><td>ExceptionFilterCollection</td><td> - Allows you to filter and retry the operation based on the Exception</td></tr>
    </tbody>
</table>

## Using the ExceptionFilter to retry your Method
SendActivity allows you to filter any Exception and retry the Method as many times as you like, you can also set interval for the next retry will happen.
See [`ExceptionFilter`](ExceptionFilter.html) for details.

## See also

[Activity](Activity.html)
[CreateEntityActivity](CreateEntityActivity.html)
[DecisionActivity](DecisionActivity.html)
[DecisionBranch](DecisionBranch.html)
[DelayActivity](DelayActivity.html)
[DeleteEntityActivity](DeleteEntityActivity.html)
[EndActivity](EndActivity.html)
[ExpressionActivity](ExpressionActivity.html)
[ScreenActivity](ScreenActivity.html)
[NotificationActivity](NotificationActivity.html)
[UpdateEntityActivity](UpdateEntityActivity.html)
[ReceiveActivity](ReceiveActivity.html)
[SendActivity](SendActivity.html)
[ListenActivity](ListenActivity.html)
[ParallelActivity](ParallelActivity.html)
[JoinActivity](JoinActivity.html)
[ThrowActivity](ThrowActivity.html)
[ParallelBranch](ParallelBranch.html)
[ListenBranch](ListenBranch.html)
[ScheduledTriggerActivity](ScheduledTriggerActivity.html)