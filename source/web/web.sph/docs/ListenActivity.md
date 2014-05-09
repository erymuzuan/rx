#ListenActivity
##Overview
ListenActivity is one of the Async activity available for your workflow, it starts by initiating it's child activities which in turns are async activity themselves. What it does is to create a race for it's children and let the winners takes all.

Once of the child activity is fired, it will quickly cancel all other activities. Lets talk about a simple race condition where ListenActivity could be useful.

Consider a purchasing manager that needs to get a quotation from 3 different suppliers, since this is an urgent purchase, no other consideration will be taken apart from the first one to reply , they will get the job.

You may create 3 separate ScreenActivity for each suppliers, these activities must be the children of a ListenActivity.


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>ListenBranchCollection</td><td> - The children activities</td></tr>
<tr><td>IsAsync</td><td> - Internal used only</td></tr>
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