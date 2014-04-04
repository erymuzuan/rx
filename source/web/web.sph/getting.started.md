
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css" rel="stylesheet" />
#Getting started
This is a simple walkthrough to help you to get started developing with Rx Developer.

## Pre-requisites

[`Microsoft .Net 4.5.1 SDK`](http://www.msdn.com/net) is the runtime for all the Rx Developer. Rx Developer it self is built on top of Microsoft Asp.Net MVC 5.

You need Oracla Java runtime installed, go and get the here [`Java`](http://www.oracle.com/java), and makes sure your `JAVA_HOME` environment variable is properly set. `ElasticSearch` is the default content indexer for Rx Developer and built using Java


You will also needs a [`Erlang`](http://www.erlang.org), this is a runtime for [`RabbitMq`](http://www/rabbitmq.com), the default message broker used by Rx Developer, although the RabbitMq it self is bundled with the Rx Developer package.

## Downloading and extracting
Point your browser to [`Rx Develeper donwload page`](http://www.sph.my/download) to get all the necessary package.

Download the sph.package.&lt;version&gt;.7z and extract it to an empty folder of your choice.
Once extracted, your directory will look more or less like this
![dir](http://i.imgur.com/xQVL0MH.png)

Double click `ControlCenter.bat` to start the Rx Developer control center app.

![alt](http://i.imgur.com/yPD7zeJ.png)
You'll need to start these 3 services

* SQL Server localdb
* Elastic Search
* Rabbit MQ

then open you command prompt, got to your directory, type powershell and enter
![alt](http://i.imgur.com/cF5TBcb.png)

then type the following `.\Setup-SphApp.ps1 -ApplicationName myapp -Port 50352`, replace `myapp` with your desired application name, it must be lowercase letters only, no `.`, `,` numbers or special symbols allowed. Then specify the port to run your application on, choose those over 50000 to avoid conlict with your other services
![alt](http://i.imgur.com/dJBcOGI.png)

Once you are done, you can go back to your `ControlCenter`, now starts `SPH Worker` and `IIS Express`


## Writing your first application
Once it's been set up, you can browser to your app by clicking on the `ViewApp` link in the `ControlCenter`, A default user `admin` and password `123456` has already been created by the `SetUp-SphApp.ps1` script for your convenience.
go to 
http://localhost:&lt;port&gt;/sph#dev.home

![alt](http://i.imgur.com/8T46sAg.png)
click on the `New Entity Definition`, this will take you to the `EntityDefinition` editor where you can design your first entity. For now lets just assume that an `EntityDefinition` is the main object in your application. Let's do a Hospital application , you'll need a `Patient`
![alt](http://i.imgur.com/1v4LkQA.png)

Then click save, suddenly the screen will have new things appeared at the bottom. Right click on the empty folder, and select `Add Child`, give it a name `Mrn`
![alt](http://i.imgur.com/8XyrM5k.png)

![alt](http://i.imgur.com/CZ9JUol.png)
press enter, the click on the `Mrn` again, this time the background will turn blue, to say that it's been selected, got to your right
![alt](http://i.imgur.com/ajov5Wl.png)

Then add 1 more member , called `FullName`, do the same thing.

then click `Save`

### Create the 1st form
Go to the `Forms` section
![alt](http://i.imgur.com/rlytQQQ.png)
Click creates new form, you'll get this, fill in as seen on the screen
![alt](http://i.imgur.com/IONIskG.png),
now click on the `Add a field` tab, the click on the `Single Line Text` twice
![alt](http://i.imgur.com/O7nbsXs.png)

Click on `Properties` tab and Click on the `Label 0`, now fill this according to the screen
![alt](http://i.imgur.com/a17JJ5q.png)
Then click on `Label 1` and do this
![alt](http://i.imgur.com/oNqvLZ3.png)

Click `Save` then click publish
![alt](http://i.imgur.com/aloyow2.png)
you will see this in your controlcenter
![alt](http://i.imgur.com/3vRbjaV.png)

Now you'll need to publish the `Patient` entity, use your keyboard `Ctrl + ,`, then click on the `Patient`
![alt](http://i.imgur.com/YIb4qq7.png)

in your `Patient` entity definition click `Publish`, now you will see some activities in your ControlCenter and your CPU is kicking up, to generate the source code, compiles it and deploy it your app. This will take a while , may be around 15, 20 seconds.

After a while just hard refresh your browser, may be `F5` few times then click on the navigation bar
![alt](http://i.imgur.com/BSrz3s1.png)
you'll see `Patients` is listed, click that
![alt](http://i.imgur.com/CNuGvrR.png)
Click `Patient Registraion`, fill in the details and click `Save`, the `Save` button spin a little but nothing happened(because we have not specified the action, we will get into that later).
![alt](http://i.imgur.com/4yZtwq5.png)
press the back button, click `Patients` on the nav bar, click on the `Recent Patients` link

![alt](http://i.imgur.com/ZusUc1N.png)

You now have successfully created your first app.
To get to know the full power of Rx Developer, we can arrange a training session as well the full set of documentation that can be reached at 
[`http://localhost:<port>/docs`](/docs)






