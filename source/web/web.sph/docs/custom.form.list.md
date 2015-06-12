#Custom Code
Reactive Developer allows you to easily extend your application with custom forms and code, once you hit the limit with Reactive Developer visual designer, you know it's not the end of the world.

There are 4 types of code that you can creates in your Rx Developer solution

* Custom Form with route
* Partial view
* Dialog
* Javascripts files


##Custom Form with route
When you create an `EntityForm` or `EntityView`, Rx Developer will compile those into DurandalJS view(.html) and viewmodel(.js), and insert a route information into the application`router`. So you can actually do the same with you custom code

## Partial View
When writing a lot custom forms, chances are you have a lot of repertive code, isn' it nice to have some kind of reusable piece of code that can be shared accross you custom form, We are not talking about code snippet, what we want is something universal, change once and it reflects every where.

For example you want to have a piece of menu in all your custom forms, does it mean you have to write this menu over and over again, what if you need new item on the menu, do you have to go back to dozens of you custom forms to make the change

##Dialog
Dialog it's easy way to get user attention or input, it present in a modal, where it will block the rest of the page.

##Javascript files
Create your commonly used library, now these scripts will be available in your custom forms, and partial classes.