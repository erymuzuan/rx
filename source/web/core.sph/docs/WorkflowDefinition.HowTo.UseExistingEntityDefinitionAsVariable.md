#Use Existing Entity Definition As Variable

Assuming that you have a form that contains of Employee details. When the employee fill in the form and submit it, you would like to notify the HR Admin so he/she can verify the form. Thus, you create a workflow of type [`Trigger Workflow`](TriggerWorkflow.html) . Your workflow here will be executed when it's getting the records passed from the form submited by the user. In this case you might want to create variables to store the value of the records. Instead of you have to create variable for each fields in the form you can simplify it by adding a variable of type [`ClrTypeVariable`](ClrTypeVariable.html).

![alt](http://i.imgur.com/owoDiYL.png)

A window is prompt where you can select the entity you defined in your [`EntityDefinition`](EntityDefinition.html). You may choose the entity that is related to your Employee details form.

![alt](http://i.imgur.com/5RxjD2I.png)




 