#PropertyChangedField
##Overview
Provides previous value of the property specified. This is normally used to detect a change. For example to find out if the `Status` property was changed from `New`, then you would write a [`PropertyChangedField`](PropertyChangedField.html) with `Path` = `Status` and operator is `=(Eq)` and the right field is a [`ConstantField`](ConstantField.html) with value `New`.

To get a full rule when the field is changed from `New` to `Closed` for example, you have to write two rules with the second one using [`DocumentField`](DocumentField.html) path is set to `Status` and compare it [`ConstantField`](ConstantField.html) with value `Closed`
![Two rules to detects changes](http://i.imgur.com/uyQDN4P.png)


## How to use PropertyChangedField to detect changes in Trigger rules

If you need to fire a trigger when a field value has changed, regardless of its value, then you can just compare the previous value and the current value, if they are not equal then you know you get it.

![Property changed field value](http://i.imgur.com/GdJsRjh.png)

![Current value via DocumentField](http://i.imgur.com/e4gPVCW.png)

![Rule](http://i.imgur.com/idkX0IL.png)

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
[AssemblyField](AssemblyField.html)
[FunctionField](FunctionField.html)
[ConstantField](ConstantField.html)
[DocumentField](DocumentField.html)
[PropertyChangedField](PropertyChangedField.html)