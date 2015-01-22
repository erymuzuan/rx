#How to create value object that contains many record

Assuming that you have a form named "Employee Details Form". In that form there are section that employee has to fill in their emergency contacts. It could be more than 1 record. So how can you setup your application to store these value?

In this example, at `Schema Designer`  we add *EmergencyContactCollection* in *Employee* entity. This child item must be of type `Collection` and the name must be appended with "*Collection*" . 

![alt](http://i.imgur.com/gsGkVrq.png)