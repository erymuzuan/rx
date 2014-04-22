#How to Retrieve a Record

You can use [`Expression Activity`](ExpressionActivity.html) to retrieve your record.  You can retrieve it either from your custom entity Or directly from existing entity in the system. *(This depends on what records you need in your workflow)* 

In this example our workflow retrieved the record from `Custom Entity` *Leave* and also it try to get user's email from `SPH entity` *User Profile* .

Retrieve record from custom entity. You must write the full namespace for which entity you would like to retrieve data from:

<pre>
var repos = ObjectBuilder.GetObject<IRepository<Bespoke.ehrms.Domain.Leave>>();
dynamic leave = await repos.LoadOneAsync(this.LeaveId); 
this.Leave = leave;
item.CurrentEmployeeEmail = leave.Email;
item.CurrentSupervisorEmail = leave.ImmediateSupervisorEmail;
</pre>

Retrieve record from predefine data context:

<pre>
var context = new SphDataContext();
var username = await context.GetScalarAsync<UserProfile,string>(e => e.Email == item.CurrentSupervisorEmail, e => e.UserName); 
item.SupervisorUsername = username;
</pre>
