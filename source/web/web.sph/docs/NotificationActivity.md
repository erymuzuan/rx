#NotificationActivity
##Overview
[`NotificationActivity`](NotificationActivity.html) lets you to send email and for internal user it will create a [`Message`](Message.html) in the message center.
![Message Center](http://i.imgur.com/3ePhbd9.png)

Internal user is determined by the email addres lookup in [`UserProfile`](UserProfile.html).


![NotificationActivity Dialog](http://i.imgur.com/KJ1CcIY.png)

Almost all of the properties in [`NotificationActivity`](NotificationActivity.html) is an expression+Razor field denoted with ![expression+razor icon](http://i.imgur.com/eJG3fxl.png).
These fields allow you to write an expression with `=` notation.
e.g. to create a piece of string concatenation
<pre>
="Notification about new application from " + Applicant.FullName
</pre>
NOTE :These must be valid C# expression.

Ommiting `=` at the begining will automaticatlly convert the expression into a `Razor` template expression, as such it allows you to do things like
<pre>
    Notification about new application from  @Model.Applicant.FullName
</pre>
NOTE : Since `@` is an identifier in `Razor`, and if you need to provide a literal value for `@` then you need to escape it. For example the email address `someone@company.com` have to be written in `someone@@company.com`


Learn more about Razor [`here`](Razor.html)



##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>From</td><td> - In more settings, an email address for From</td></tr>
<tr><td>Subject</td><td> - Subject for the email, this will be the Message Title</td></tr>
<tr><td>Body</td><td> - The emails body</td></tr>
<tr><td>To</td><td> - Where to sent to, use , to add more that one recipients</td></tr>
<tr><td>Cc</td><td> - Carbon copy</td></tr>
<tr><td>Bcc</td><td> - Blind carbon copy - a copy of the email will be sent to this person witout other recipients in To and Cc knowledge</td></tr>
<tr><td>IsHtmlEmail</td><td> - Reserved for future use </td></tr>
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