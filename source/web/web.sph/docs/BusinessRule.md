#BusinessRule
##Overview
[`BusinessRule`](BusinessRule.html) provides a way for the devekoper to define their custom business rules without code. A [`BusinessRule`](BusinessRule.html) can contains 1 or more [`Rule`](Rule.html), if any of this rule is evaluated to `false` then the whole [`BusinessRule`](BusinessRule.html) will be considered broken.

![Business rule dialog](http://i.imgur.com/KobnjRa.png)

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>FilterCollection</td><td>
    - The list of rules that must be satisfied before the BusinessRule could be run, if no filter is specified the rule will be run every time.
    <br/>
    When filters are specified, these filters will be evaluated and if one of the filter fail to satifiy, the rule will not run.
</td></tr>
<tr><td>RuleCollection</td><td> - The list of Rule</td></tr>
<tr><td>Description</td><td> - Developer descriptive note </td></tr>
<tr><td>Name</td><td> - Developer descriptive identifier</td></tr>
<tr><td>ErrorLocation</td><td> - Where the error will be show - NOT IMPLEMENTED </td></tr>
<tr><td>ErrorMessage</td><td> - The message will be displayed if the rule is not satisfied</td></tr>
</tbody></table>



## See also
[Rule](Rule.html)

<a href="Field.html">Field</a>
<a href="AssemblyField.html">AssemblyField</a>
<a href="FunctionField.html">FunctionField</a>
<a href="ConstantField.html">ConstantField</a>
<a href="DocumentField.html">DocumentField</a>
<a href="PropertyChangedField.html">PropertyChangedField</a></p>
