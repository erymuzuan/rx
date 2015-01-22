#WorkflowDefinition
##Overview
`WorkflowDefinition` represent the core work that your app needs to do, while `EntityDefinition` represents set of data definition for you app.`WorkflowDefinition` is much richer in terms of capabilities and complexities. It used primarily to build a `state-machine` for your application, which could represent a long running process with the ability to be persisted and de-hydrated when the moment comes.




##Variables
`WorkflowDefinition` is just a simple object with attributes, or fields, but we call them `Variable`. The `Variables` holds the user state of an `instance`.These are normally things that goes into your `ScreenActivity` forms, as well as other states variables such loop counter etc.

`Variables` are the things that your workflow interact with. Lets walk through an example for a workflow to process a referral request for specialist, in a traditional code development, we might write something like

```csharp
public class ProcessReferral : Workflow
{
    // a member to hold the patient object
    public Patient Patient{set;get;}

    // a member for the requested specialist
    public string Specialist{set;get;}
    
    //.. other members and methods


}
```

So all your variables will be written as public field in your `Workflow` class, in which you will manipulate these `Variables` as `Activities` are executed.

[See more about variables here](Variable.html)

##Activities
`Variables` to lets you define states for your `Workflow`, and `Activity` is the way you define `Operation` for your `Workflow`. If we were to illustrate a `Workflow` with traditional code approach. This is what we have

```csharp
public class ProcessReferral : Workflow
{
    // a member to hold the patient object
    public Patient Patient{set;get;}

    // a member for the requested specialist
    public string Specialist{set;get;}

    // a member for the specialist
    public string Specialist{set;get;}

    // a member for the specialist response
    public string Response{set;get;}
    
    // The actual workflow
    public void Start()
    {
        this.SendEmailToSpecialist();               // 1
        await this.InitSpecialistConfirmation();    // 2
        if(this.Response == "Yes")                  //
            this.CreateAppointmentActivity();       // 3
        else
            this.EmailPatientDeclined();            // 3
    }

    // send email activity
    public void SendEmailToSpecialist()
    {
        var smtp = new SmtpClient();
        smtp.Send(to:this.Specialist,subject: " Referral request for " + this.Patient.FullName, body:"...");
    }


    //.. other members and methods
    


}
```
As you can see, `Activities` is nothing more than a function to perform an `Operation`, what it differs between the normal class is, it's ability to be persisted between activities.

`Rx Developer` `WorkflowDesigner` on the hand lets you forget about writing the code altogether, instead it will help generate the codes for you. The benefits of using `WorkflowDesigner` are tremendous, the code are tested to be robust, less chances for bugs, it also encapsulate the developers from the inner working of a state machince and let you focus on your actual business needs.


Core concepts of `Workflow` is a series of `Activities` it contains, these activities are generally can be divided into 2 categories

* System Activity
* Human Activity

An `Activity` is a unit of work performed by the Workflow engine, it's like a method in your `EntityDefinition`, except that it could be persisted when it's done,befor continuing for the `NextActivity`

### System Activity
Represent set of `Activities` executed by the workflow engine


### Human Activity
Represent set of `Activities` performed by human or external system. `ScreenActivity` and `ReceiveActivity` are the most common one.

### Asynchronous Activity
There are other aspects in `Activity` execution, what we call it's `Asynchronous`. This is a behaviour that some activities have which allow them to be initiated and at some point of time in the future, be executed. Most of human `Activity` are at the same time `Asynchronous`, i.e. they get initiated before it could be executed. Take `ScreenActivity` for example, it will not be executed directly, but initiated when it turns to execute. What happen is, in the initializing process, emails and notification will be sent to the `performers` for the task. Once the email sent, the `performer` will have a link point for the execution, the moment he clicks the submit button, the `Activity` is executed.




##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>ActivityCollection</td><td> - The list of activities for you Workflow, use the designer to add or remove activities </td></tr>
<tr><td>VariableDefinitionCollection</td><td> - The list of variables available to your workflow</td></tr>
<tr><td>WorkflowDefinitionId</td><td> - The primary identity for your workflow, this is auto assigned and it will be embedded within your workflow</td></tr>
<tr><td>Name</td><td> - The name of your workflow </td></tr>
<tr><td>Note</td><td> - Any note for developers used only</td></tr>
<tr><td>IsActive</td><td> - Is it running or not</td></tr>
<tr><td>SchemaStoreId</td><td> - A default empty schema is provided, you should replace this schema with your own</td></tr>
<tr><td>Version</td><td> - Version is an internal identification number for your workflow. It used primarily when you have breaking changes to your workflow and there are existing one still running.</td></tr>
<tr><td>WorkflowTypeName</td><td> - Internal use</td></tr>
<tr><td>CodeNamespace</td><td> - Internal use</td></tr>
</tbody></table>



## See also

