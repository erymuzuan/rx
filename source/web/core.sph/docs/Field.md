#Field
##Overview
[`Field`](Field.html) is the base class for all `fields` which provides a notion of a value that can be evaluated at run time.`Fields` are normally used in conjuction with and evaluator such as [`Rule`](Rule.html), [`Filter`](Filter.html) and all the run time evaluation. [`Field`](Field.html) it self is an abstract class which on one method `GetValue` in which it's implemented by one of the `Field` class.


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Name</td><td> - The developer identifier</td></tr>
<tr><td>Note</td><td> - Developers' note</td></tr>
</tbody></table>



## See also
[`DocumentField`](DocumentField.html)
[AssemblyField](AssemblyField.html)
[FunctionField](FunctionField.html)
[ConstantField](ConstantField.html)
[DocumentField](DocumentField.html)
[PropertyChangedField](PropertyChangedField.html)
