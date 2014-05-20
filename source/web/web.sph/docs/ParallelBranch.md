#ParallelBranch
##Overview
Represent each branch in a parallel workflow execution, started by a [`ParallelActivity`](ParallelActivity.html). Each branch will have it's own `CorrelationId` to keep track of any async response from any async activities received.


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>ActivityCollection</td><td> - List of activities in the branch execution</td></tr>
</tbody></table>



## See also

[Activity](Activity.html)
[ParallelActivity](ParallelActivity.html)
[JoinActivity](JoinActivity.html)
