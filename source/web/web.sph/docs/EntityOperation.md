#Entity Operation


##Overview

`EntityDefinition` is like a class in object oriented world, as such there are 2 things an object has

1. Attributes - set of properties that define an object
2. Operations -  set of method or function that an object can invoke

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Rules</td><td> - Set of rules executed before executing the actions </td></tr>
<tr><td>Permissions</td><td> -et of roles the use must have to perform this action </td></tr>
<tr><td>SetterActionChildCollection</td><td> - Set of <a href="SetterActionChild.htm">SetterActionChild></a> that allows you to set member values after the Business Rule succeefully run and before the Operation is executed </td></tr>
<tr><td>Name</td><td> - The name of the operation</td></tr>
<tr><td>SuccessMessage</td><td> - If the operation is successfully executed, this will be the notification message</td></tr>
<tr><td>NavigateSuccessUrl</td><td> - After the operation is successfully executed and notification is dismissed, the page will navigate to this url </td></tr>
</tbody></table>


Attributes is defined by the schema designer, where you can design your object in a hierachical manner. `Rx Developer' call this `Member` [see more](Member.html)

`Rx Developer' also allows you to creates a set of operations to your entity definition by means of `EntityOperation`, this is available from `EntityDefinition` designer.
![EntityOperation](http://i.imgur.com/PD4IAz9.png). 
You can click `+Operation` to add a new operation.






This will bring you this screen.

![EntityOperation designer](http://i.imgur.com/1dPutz0.png)

1. The name of the `EntityOperation` , it must be a valid identifier for method name, as such cannot contain spaces, or special chars except `_`, and must start with a letter. For consistency we recommend using `PascalCase` e.g. '`Demote`
2. The `EntityDefinition` that you are working on
3. The name of the `EntityOperation`, refer #1
4. A set of `BusinessRule` you want to validate with, prior to executing your operation. If any of the rules failed to operation will not proceed and error messages will be show to the user.
   Please refer to the [BusinessRule](businessrule.htm) for more details about `BusinessRule`
5. Set of roles/authorization required to execute the operation. The use must be in one of these roles in other for the execution to succeed, else status code [Status 403](http.403.htm) will be returned.
6. You can specify [setter actions](childsetteraction.htm) to set the values of your entity fields/attributes. For example setting the `Rating` field for `Customer` entity to be 1 less than the previous value using a [FuctionField](functionfield.htm) `item.Rating -1`

The `EntityOperation` designer will not compile your code, clicking save will only save it to your `EntityDefinition` in which you will have to `Publish` it from the `EntityDefinition` designer.

Once compiled, a new Asp.Net MVC `Action` will be created with the name of the operation. Where it would take an instance of the `EntityDefinition` from the input stream, and run the specified business rules and if all passed the setter action will be called before submitting it to the persistence layer.
If you need to modify or do other works from this `EntityOperation` apart from simple field settings, you can use [`Trigger`](Trigger.html) where you
can call additional [`SetterAction`](SetterAction.html), or [`EmailAction`](EmailAction.html) or [`StartWorkflowAction`](StartWorkflowAction.html).


## See also
[FunctionField](FunctionField.html)
[DocumentField](DocumentField.html)
[ConstantField](ConstantField.html)
