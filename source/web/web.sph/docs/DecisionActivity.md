#DecisionActivity
##Overview
`If-elseif-else` construct for your workflow, using [`DecisionBranch`](DecisionBranch.html) to evaluate the next path of Activity to be executed

Each branch will be evaluated squentially in the order that you put, and if the expression is evaluated to `true`, then the branch will be excuted, ignoring the subsequent branches. If none were evaluated to true, then the default branch will be executed
![Decicision dialog](http://i.imgur.com/Fa1Pxdb.png)

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>DecisionBranchCollection</td><td> - The list of branches</td></tr>
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