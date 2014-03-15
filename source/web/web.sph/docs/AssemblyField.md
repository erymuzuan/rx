#AssemblyField
##Overview
Allows you to execute a custom method in your own assembly. 

## Thing to know
If your method is an async method, you must return Task&lt;object>. 

Since the Field.GetValue is a synchronous method, it will wrap inside Wait and ContinueWith, so problem might arise with the thread waiting for the await. So make sure any async call is wrap with ConfigureAwait(false). 
The word of warning : You might run into deadlock issue in this situation


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Location</td><td> - Assembly path</td></tr>
<tr><td>TypeName</td><td> - The name of the class, FullName</td></tr>
<tr><td>Method</td><td> - The method name</td></tr>
<tr><td>IsAsync</td><td> - Use async or not</td></tr>
<tr><td>AsyncTimeout</td><td> - Set the timeout for async call</td></tr>
<tr><td>MethodArgCollection</td><td> - The method arguements</td></tr>
</tbody></table>



## See also

[Field](Field.html)
[AssemblyField](AssemblyField.html)
[FunctionField](FunctionField.html)
[ConstantField](ConstantField.html)
[DocumentField](DocumentField.html)
[PropertyChangedField](PropertyChangedField.html)