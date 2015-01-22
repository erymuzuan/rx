#How to Set a DateLine to your Screen Activity

It's a common in business process, you will need to set a timeline for some particular action to be taken. You may want to trigger another action if your system doesnt get any respond from user for some period of time. Here, you could set a dateline for your [`Screen Activity`](ScreenActivity.html) by attach the [`Delay Activity`](DelayActivity.html) in your workflow.

In this example, we want to set the *Leave Application Form* to be available to the supervisor to approve within 3days of the application. When system detect no action from user within these period, it will automatically canceled the `Screen Activity` and execute another activity defined in the workflow.

You must define full namespace of your custom entity if you wish to retrieve your record from your custom entity:
<pre>
var repos = ObjectBuilder.GetObject<IRepository<bespoke.ehrms.domain.Leave>>();
dynamic leave = await repos.LoadOneAsync(this.LeaveId);
this.Leave = leave;
item.CurrentEmployeeEmail = leave.Email;
item.CurrentSupervisorEmail = leave.ImmediateSupervisorEmail;
</pre>

You can use predefine data context if you want to retrieve record from existing entity:
<pre>
var context = new SphDataContext();
var username = await context.GetScalarAsync<UserProfile,string>(e => e.Email == item.CurrentSupervisorEmail, e => e.UserName); 
item.SupervisorUsername = username;
</pre>