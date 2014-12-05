#Mapping Activity

MappingActivity allows you to create a mapping task with your [`WorkflowDefinition`](WorkflowDefinition.html). This will basicall execute `TransformAsync` method in your [`TransformDefinition`](TransformDefinition.html).



##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>TransformDefinition</td><td> - The name of your TransformDefinition object </td></tr>
<tr><td>InputPath</td><td> - The variable name that will be the input to your transformation</td></tr>
<tr><td>OutputPath</td><td> - The variable name where the output of the transformation will be stored</td></tr>

</tbody></table>


##Remarks
This will generally will create a piece of code in your [`Workflow`](Workflow.html) thats intantiate the [`TransformDefinition`](TransformDefinition.html) and invoke the `TransformAsyn` method.

Give you [`WorkflowDefinition`](WorkflowDefinition.html) has 2 [`Variable`](Variable.html) named 
* Input
* Output

it will create a code like

```
var map = new MyTransformDefinition();
this.Output = await map.TransformAsync(this.Input);

```
as such your `Input` and `Output` must be of the same type specifed in the [`TransformDefinition`](TransformDefinition.html) Source and Destination