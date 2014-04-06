#Trigger
##Overview
`Reactive` architecture is the backbone of `Rx Developer`, and `Trigger` is the center of how this is achieved. `Trigger` provides highly decoupled arhitecture for your business process.Lets walk through an example in a simple hospital environment when a patient is registered. Registration will normally follows with other operation as well such as create an account in billing system, provides the list of allegies and drugs to the pharmacist, or sending a message to a dietician aboout the illness and allergies. In a traditional n-tier application we would normally write this code

<pre>
private void Register(Patient patient)
{
    persistence.Save(patient);
    billing.AddAccount(patient);
    dietician.Register(patient);
    pharmacy.Register(patient);
}
</pre>

This is all fine and well, except that, you had pre defined the steps needed in the registration process. What if after few weeks you need to add another service , such as radiologist. Now you have to go back to your code, define a new service and recompile.



`Rx Developer` provides a simpler, means of achieving the same thing without any code and completey decouple from the main event. In `Rx Developer` we have a message broker, in topical manner, i.e. for every [`EntityOperation`](EntityOperation.html) a message will be sent to the message broker, and every message has a `topic`. All we have to do is get a notification from these event is to subscribe to the topic.

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>RuleCollection</td><td> - You can define a set of <a href="Rule.html">Rule</a> to further filter your <code>Trigger</code></td></tr>
<tr><td>ActionCollection</td><td> - List of <a href="CustomAction.html">Action</a> to fire once the Trigger is succefully filtered</td></tr>
<tr><td>Name</td><td> - The name of the trigger</td></tr>
<tr><td>Entity</td><td> - The `EntityDefinition` in which you are interested to listed to </td></tr>
<tr><td>TypeOf</td><td> - (Intenal) </td></tr>
<tr><td>TriggerId</td><td>
    - The indentitfier, a new subscriber will be created with this number
    <img src="http://i.imgur.com/28z2c51.png" alt="Alternate Text" />
</td></tr>
<tr><td>Note</td><td> - Developers note </td></tr>
<tr><td>IsActive</td><td> - Set it to active or not </td></tr>
<tr><td>IsFiredOnAdded</td><td> - Check to fire when a record is created</td></tr>
<tr><td>IsFiredOnDeleted</td><td> - Check to fire when a record is deleted </td></tr>
<tr><td>IsFiredOnChanged</td><td> -  Check to fire when a record is changed</td></tr>
<tr><td>FiredOnOperations</td><td> - Filter by `EntityOperation`,if you need it to fire on more than one operation, use <code>,</code> top seperated the operations  </td></tr>
<tr><td>CodeNamespace</td><td> -(Internal) use </td></tr>
</tbody></table>

##Example
So in the case of patient registration, a message will be created with `Patient.changed.Register` as the topic. Patient do not need to know anything about the other services, but it's completely the other way around. If Billing is interested to get a notification for a patient registration, it could register to the message broker with `Patient.changed.Register` as the topic. As a message arrived with the topic, a copy of the message will be sent to the Billing.



![Trigger](http://i.imgur.com/rbElfmm.png)
Basic trigger properties to listen to Patient register event

##Rules
Sometimes, it's not enough to just listen to entity operation, you will also what to do something else if the record or any other parameters differ. For example for a child Patient, you might want to trigger another action for the billing. This could be done via [`Rule`](Rule.html)
![Age](http://i.imgur.com/ww6netA.png)

##Custom Actions
Having a trigger registered is about , being able to do something about it. `Rx Developer' allows you to do at least 3 things

* Send a notification via [`EmailAction`](EmailAction.html) or `Message`
* Run a [`SetterAction`](SetterAction.html) to set the properties of the record
* Run a [`Workflow`](StartWorkflowAction.html), this will open up any other possibilities.

![Action](http://i.imgur.com/GSuYtMr.png)

You can add 1 or more actions, these actions will run synchronously except for `StartWorkflowAction`


## See also

[Rule](Rule.html)
[CustomAction](CustomAction.html)
[Field](Field.html)
[AssemblyField](AssemblyField.html)
[FunctionField](FunctionField.html)
[ConstantField](ConstantField.html)
[DocumentField](DocumentField.html)
[PropertyChangedField](PropertyChangedField.html)
[CustomAction](CustomAction.html)
[AssemblyAction](AssemblyAction.html)
[EmailAction](EmailAction.html)
[SetterAction](SetterAction.html)
[StartWorkflowAction](StartWorkflowAction.html)
