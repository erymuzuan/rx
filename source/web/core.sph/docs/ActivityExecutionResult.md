#ActivityExecutionResult
##Overview
Represent the basic unit for outcome when an Activity is executed. The most important thing in the result is the NextActivities which is an array of uniuqe id that points to the next activities to be executed. Only ParallelActivity could have 2 or more items, while EndActivity contains no item at all.

The other aspects of the result is the Correlation set, used to identify each branch and instances for  activities execution.Think a correlation set as a unique identifier for each execution set.


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Status</td><td> - The status of the executed activity, normally it's set to OK</td></tr>
<tr><td>Message</td><td> - Reserve for future use</td></tr>
<tr><td>Result</td><td> - Success or not</td></tr>
<tr><td>NextActivities</td><td> - The list of next activities to execute if any</td></tr>
<tr><td>ActivityId</td><td> - The executed activity id</td></tr>
<tr><td>Correlation</td><td> - The identifier for execution branch and workflow instance</td></tr>
</tbody></table>



## See also
