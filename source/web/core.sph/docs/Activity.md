#Activity
##Overview
Activity is the basis of any processing step in a [WorkflowDefinition](WorkflowDefinition.html), it's basically an individual task that will be executed as steps.



##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>MethodName</td><td> - The name of the Method for this Activity - Used internally</td></tr>
<tr><td>IsAsync</td><td> - Is the method invoked asynchronously - Used internally</td></tr>
<tr><td>ExecutedCode</td><td> - The piece of code that will be executed after the Activity method is executed - Used internally </td></tr>
<tr><td>ExecutingCode</td><td> - The piece of code that will be executed prior  to the Activity method is executed - Used internally</td></tr>
<tr><td>WorkflowDesigner</td><td> - The information about the Activity in the WorkflowDesigner - Used internally</td></tr>
<tr><td>IsInitiator</td><td> - </td></tr>
<tr><td>NextActivityWebId</td><td> - The next activity that will be executed, you would normally use the Workflow designer to connection the activities</td></tr>
<tr><td>Name</td><td> - The identifier for the activity</td></tr>
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
