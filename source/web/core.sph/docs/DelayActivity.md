#DelayActivity
##Overview
Creates a one time trigger that happen in the future. It's synonym with `Tread.Sleep` without consuming system resources such as a Thread.

Internally a Windows Schedule Task is created, that run the `scheduler.delayactivity.exe ` in your schedulers folder, with 2 parameters, 1 is for the [`Workflow`](Workflow.html) instance id(WorkflowId) and the 2nd one is the `WebId` for the [`DelayActivity`](DelayActivity.html)

![delay](http://i.imgur.com/xQFlN1C.png)

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>IsAsync</td><td> - Used internally, always return true</td></tr>
<tr><td>Expression</td><td>
    - Must be a valid C# expression that returns a DateTime object sometime in the future. <br />
NOTE : If you have an expression defined then you must not specify other fields</td></tr>
<tr><td>Miliseconds</td><td> - Timespan in miliseconds from the initialization</td></tr>
<tr><td>Seconds</td><td> - Timespan in seconds from the initialization</td></tr>
<tr><td>Hour</td><td> - Timespan in hours from the initialization</td></tr>
<tr><td>Days</td><td> - Timespan in days from the initialization</td></tr>
</tbody></table>



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