#Rule
##Overview
[`Rule`](Rule.html) provides an `Expression` withouth the need to do any code. The basic building block for a [`Rule`](Rule.html) are expression on the `Left`, an [`Operator`](Operator.html) and an expression on the `Right`. At run time the expressions will be evaluated and the binary operator will executed, the result of the execution will become the result of the rule.

The other aspect of the rule is the [`RuleContext`](RuleContext.html), under certain circumstances, there are time in whic left and right expreesion are evaluated with an entity. as such the item will be the rule context.


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Left</td><td> - Expression field on the left</td></tr>
<tr><td>Right</td><td> - Expression field on the right </td></tr>
<tr><td>Operator</td><td> - Binary operator to be executed </td></tr>
</tbody></table>



## See also

[`Field`](Field.html)
[`FunctionField`](FunctionField.html)
[`DocumentField`](DocumentField.html)
[`ConstantField`](ConstantField.html)
[`JavascripExpressionField`](JavascripExpressionField.html)