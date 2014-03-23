#EmailAction
##Overview
Email action allows you to send email when an [`Trigger`](Trigger.html) is fired. 
![email action](http://i.imgur.com/fPobznC.png)

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>From</td><td> - The email from</td></tr>
<tr><td>To</td><td> - Email address to sent to, use <code>,</code> for multiple receivers</td></tr>
<tr><td>SubjectTemplate</td><td> - The subject template, see <a href="RezorTemplate">RazorTemplate</a> for details </td></tr>
<tr><td>BodyTemplate</td><td> - The email body template, see <a href="RezorTemplate">RazorTemplate</a> for details  </td></tr>
<tr><td>Bcc</td><td> - email bcc, (blank carbon copy) so it will not appear in the sent to and cc </td></tr>
<tr><td>Cc</td><td> - Carbon copy email for attention, with the original receiever being able to see the cc addresses </td></tr>
<tr><td>UseAsync</td><td> - (Internal use)</td></tr>
</tbody></table>



## See also

[CustomAction](CustomAction.html)
[AssemblyAction](AssemblyAction.html)
[EmailAction](EmailAction.html)
[SetterAction](SetterAction.html)
[StartWorkflowAction](StartWorkflowAction.html)