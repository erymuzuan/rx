#Variable
##Overview
`WorkflowDefinition` is just a simple object with attributes, or fields, but we call them `Variable`. The `Variables` holds the user state of an `instance`.These are normally things that goes into your `ScreenActivity` forms, as well as other states variables such loop counter etc.

`Variables` are the things that your workflow interact with. Lets walkthrough an example for a workflow to process a referal request for specialist, in a traditional code development, we might write something like

<pre>public class ProcessReferal : Workflow
{
    // a member to hold the patient object
    public Patient Patient{set;get;}

    // a member for the requested specialist
    public string Specialist{set;get;}
    
    //.. other members and methods


}</pre>

So all your variables will be written as public field in your `Workflow` class, in which you will manipulate these `Variables` as `Activities` are executed.

## How to add a variable to your WorkflowDefinition
Go to your `WorkflowDefinition` designer, select `Variable` tab page from the `Toolbox`. You can see the list of variable and add new one by click on the "Add Variable" button
![](http://i.imgur.com/s5JZ7ks.png)


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Name</td><td> - Valid identifier for your variable</td></tr>
<tr><td>TypeName</td><td> - The CLR type</td></tr>
<tr><td>DefaultValue</td><td> - Future </td></tr>
</tbody></table>



## See also

[WorkflowDefinition](WorkflowDefinition.html)
[ClrTypeVariable](ClrTypeVariable.html)
[ComplexVariable](ComplexVariable.html)
[SimpleVariable](SimpleVariable.html)