#How to do a While Loop

A while loop is a control flow statement that allows code to be executed repeatedly based on a given condition. The while loop can be thought of as a repeating if statement.

The while construct consists of a block of code and a condition. The condition is evaluated, and if the condition is true, the code within the block is executed. This repeats until the condition becomes false. 

For this example, your workflow contains a while loop that keeps assigning a new Task item to the *Approver* until the *LastApprover* approved the form. In this case, there are 3 *Approver*.

You can handle this by populate the *CurrentApproverIndex* using the [`Expression Activity`](ExpressionActivity.html) and condition evaluation using [`Decision Activity`](DecisionActivity.html). By attached these two activities with your [`Screen activity`](ScreenActivity.html), you will be able to set your while loop.

![alt](http://i.imgur.com/cukPzj2.png)

Refering to figure above:
1) First, you have to initiate the *CurrentApproverIndex* to 0 . *CurrentApproverIndex* is a variable define in your workflow.
2) Once the Current Approver approved the form, you increase the value of *CurrentApproverIndex*
3)The loop will be continued until the value of *CurrentApproverIndex* is equal to the total number of approver. 
4) Process End when decision is made by *LastApprover*


 