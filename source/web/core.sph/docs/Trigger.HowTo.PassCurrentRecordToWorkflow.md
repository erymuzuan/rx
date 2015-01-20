#How to pass trigger current record  to a StartWorkflowAction
Your [`WorkflowDefinition`](WorkflowDefinition.html) might contains a [`ClrTypeVariable`](ClrTypeVariable.html) which normally contains one of the [`EntityDefinition`](EntityDefinition.html). It's common situation where you want to pass the current record from your [`Trigger`](Trigger.html) to the workflow in order to start it. The [`StartWorkflowAction`](StartWorkflowAction.html) contains `MappingCollection` which allows you to map the `WorkflowDefinition` variables.

Lets pick an example, where your [`WorkflowDefinition`](WorkflowDefinition.html) contains a variable called `Patient` and is of type [`ClrTypeVariable`](ClrTypeVariable.html). From your [`Trigger`](Trigger.html) which point to the `Patient` [`EntityDefinition`](EntityDefinition.html), The current  `item` in your [`FunctionField`](FunctionField.html) is always the record.

![alt](http://i.imgur.com/9hMluDn.png)

So all you have to do is just write a [`FunctionField`](FunctionField.html) with one line of script
<pre>
item
</pre>

or
<pre>
return item;
</pre>