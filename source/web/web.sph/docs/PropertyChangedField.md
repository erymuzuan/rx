#PropertyChangedField
##Overview
Provides previous value of the property specified. This is normally used to detect a change. For example to find out if the `Status` property was changed from `New`, then you would write a [`PropertyChangedField`](PropertyChangedField.html) with `Path` = `Status` and operator is `=(Eq)` and the right field is a [`ConstantField`](ConstantField.html) with value `New`.



##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Path</td><td> - The property path to the document</td></tr>
<tr><td>TypeName</td><td> - Reserved</td></tr>
<tr><td>OldValue</td><td> - Reserved</td></tr>
<tr><td>NewValue</td><td> - Reserved</td></tr>
<tr><td>Type</td><td> - Reserved</td></tr>
</tbody></table>



## See also

[Field](Field.html)
[Field](Field.html)
[AssemblyField](AssemblyField.html)
[FunctionField](FunctionField.html)
[ConstantField](ConstantField.html)
[DocumentField](DocumentField.html)
[PropertyChangedField](PropertyChangedField.html)