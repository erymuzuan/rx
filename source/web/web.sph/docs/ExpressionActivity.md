#ExpressionActivity
##Overview
[`ExpressionActivity`](ExpressionActivity.html) allows you to embed your own code in a [`WorkflowDefinition`](WorkflowDefinition.html). This is the main extensibilty point you can find in `Rx Developer` if the provided activities did not meet requirements. On the other hand [`ExpressionActivity`](ExpressionActivity.html) could also be used to do other things such as setting the [`Variable`](Variable.html) values, this is useful for example in a loop where you need to set the `CurrentItem` or the `CurrentIndex` for your loop. 

##Referencing external assemblies
If you need to call external assemblies there are 2 ways of achieving this at the moment

* Via `Add Referenced Assembly` dialog in your [`WorkflowDefinition`](WorkflowDefinition.html)
* Via [`ObjectBuilder`](ObjectBuilder.html) registration.


### Add reference assembly
Allow you to add commonly used assembly , those that currently loaded in your `AppDomain`, if this assembly is not in `GAC` then you'll have to copy it to the Subscriber folder.
![Use Referenced Assembly tab](http://i.imgur.com/Rex8SF6.png)

You'll have to fully qualified your the type used.
<pre>

// example of calling Membership is 
var me = System.Web.Security.Membership.GetUser("me");
</pre>

### Using ObjectBuilder registration
ObjectBuilder is a little more flexible, that it allows you to add almost any assembly, all you have to do is register them in your `.config` files(subscribers.host\workers.console.runner.exe.config or subscribers.host\workers.windowsservice.runner.exe.config).

<pre>
   &lt;object name="MyObject" type="MyCompany.Myproject.MyObject,mycustomdll">
        &lt;constructor-arg name="arg" value="myname" />
      &lt;/object>
</pre>

This simple registration will allow you to write code like this
<pre>
dynamic myobject = ObjectBuilder.GetObject("MyObject"); // refer to the name property
myobject.Call();// where Call is a method defined in your class

</pre>
The use of `dynamic` keyword allows far greated flexibilty, since `Rx Developer` didn't have to know about your object. Thou you are losing the compile time checking.

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Expression</td><td> - Your code, must be a valid C# code</td></tr>
</tbody></table>



## See also

[Activity](Activity.html)
[CreateEntityActivity](CreateEntityActivity.html)
[DecisionActivity](DecisionActivity.html)
[DecisionBranch](DecisionBranch.html)
[DelayActivity](DelayActivity.html)
[DeleteEntityActivity](DeleteEntityActivity.html)
[EndActivity](EndActivity.html)
[ExpressionActivity](ExpressionActivity.html)
[ScreenActivity](ScreenActivity.html)
[NotificationActivity](NotificationActivity.html)
[UpdateEntityActivity](UpdateEntityActivity.html)
[ReceiveActivity](ReceiveActivity.html)
[SendActivity](SendActivity.html)
[ListenActivity](ListenActivity.html)
[ParallelActivity](ParallelActivity.html)
[JoinActivity](JoinActivity.html)
[ThrowActivity](ThrowActivity.html)
[ParallelBranch](ParallelBranch.html)
[ListenBranch](ListenBranch.html)
[ScheduledTriggerActivity](ScheduledTriggerActivity.html)