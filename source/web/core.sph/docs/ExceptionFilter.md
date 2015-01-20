#ExceptionFilter
ExceptionFilter allows you to specify any .Net Exception types and act on those exception. The basic filter allows you to retry the operation, while a more complex filter allow you carry out compensation action for the work your [`WorkflowDefinition`](WorkflowDefinition.html) has done.




##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>TypeName</td><td> - The full typename for the Exception for example System.Data.DbException </td></tr>
<tr><td>Filter</td><td> - </td></tr>
<tr><td>Interval</td><td> - The number of unit for your interval for example 1000</td></tr>
<tr><td>IntervalPerion</td><td> - The interval unit in miliseconds, seconds, minutes or hours.</td></tr>
<tr><td>MaxRequeue</td><td> - If specified/bigger than 0, it will retry the operation </td></tr>

</tbody></table>