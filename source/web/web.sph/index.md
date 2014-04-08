
    <link href="http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css" rel="stylesheet" />
<style>    p,ul {        margin-left:10px;        margin-right:10px;    }    img { border:2px solid gray;padding:10px;margin-top:6px;    }</style>
#Getting started
This is a simple walkthrough to help you to get started developing with Rx Developer.

## Pre-requisites

#### Operating System
`Rx Developer` only runs on these operating system

* Windows 7 Professional
* Windows 8 Professional
* Windows 8.1 Professional
* Windows Server 2008 R2
* Windows Server 2012 and later

#### Microsoft .Net 4.5.1 SDK
[Microsoft .Net 4.5.1 SDK](http://www.msdn.com/net) is the runtime for all the Rx Developer. Rx Developer it self is built on top of Microsoft Asp.Net MVC 5. Or you can get the SDK [here](mu_.net_fx_4_5_1_dp_win_vistasp2_win_7sp1_win_8_win_8_1_win_server_2008sp2_win_server_2008_r2sp1_win_server_2012_win_server_2012r2_x86_x64_3009815.exe)
#### Powershell
You will need at least [Powershell v3.0](http://www.microsoft.com/en-us/download/details.aspx?id=34595), or [Powershell V4.0](http://www.microsoft.com/en-us/download/details.aspx?id=40855) is better but it didn't run on Windows 8, I think it will work on Windows 7 and Windows 8.1.

Check Powershell version on your computer  with this command, open a command prompt, type `powershell`, then `$PSVersionTable`
![alt](http://i.imgur.com/07BBiea.png)

#### Database - SQL Server LocalDb
For the database , download Microsoft Sql Localdb here

* [en_sql_server_2012_sqllocaldb_with_sp1_x64.msi](http://download.microsoft.com/download/8/D/D/8DD7BDBA-CEF7-4D8E-8C16-D9F69527F909/ENU/x64/SqlLocalDB.MSI)
* [en_sql_server_2012_sqllocaldb_with_sp1_x86.msi](http://download.microsoft.com/download/8/D/D/8DD7BDBA-CEF7-4D8E-8C16-D9F69527F909/ENU/x86/SqlLocaLDB.MSI)

You also need Sql Server Command Line Utilities (v11) download it [here](http://www.microsoft.com/en-us/download/details.aspx?id=36433)

#### Java
You need Oracla Java runtime installed, go and get the here [Java](http://www.oracle.com/java), and makes sure your `JAVA_HOME` environment variable is properly set. `ElasticSearch` is the default content indexer for Rx Developer and built using Java

1) Click Downloads > Java > Java Runtime Environment (JRE) > Click Free Java Download
2) Go to Advanced System Settings in  Control Panel>All Control Panel Items>System  
3) Click Environment Variables to set your  `JAVA_HOME` environment variable. 

![JAVA_HOME](http://i.imgur.com/EGaJD1g.png)


#### Erlang
Download [Erlang](http://www.erlang.org) runtime, this is the runtime for [RabbitMq](http://www/rabbitmq.com), the default message broker used by Rx Developer, although the RabbitMq it self is bundled with the Rx Developer package. Make sure your variable path `ERLANG_HOME` is correctly set. Download the Windows 32-bit Binary or Windows 64-bit binary according to your workstation.

![ERLANG_HOME](http://i.imgur.com/rfCfuKQ.png)

## Downloading and extracting
Point your browser to [Rx Developer download page](sph.package.1.0.10278.7z) to get all the necessary package.

Download the sph.package.&lt;version&gt;.7z and extract it to an empty folder of your choice.
Once extracted, your directory will look more or less like this
![dir](http://i.imgur.com/xQVL0MH.png)

Double click `ControlCenter.bat` to start the Rx Developer control center app.

Fill in on your settings accordingly, if you don;t what it means, see the explanation below
![alt](http://i.imgur.com/GEJY17S.png)

* Application Name is the simple name `WITHOUT` any spaces, period `.`,comma `,` numbers or symbols, e.g. myapp. Honestly don't try to be fancy here
* Project Directory, it's where you extracted the package
* IIS port, use any number, but pick one over 50000 for less chance of conflict
* IIS Express - bundled with the package so the default is `IIS Express\iisexpress.exe`
* SQL localdb instance, pick from one, if don't have one, please download and install it from the link above
* RabbitMQ - this bundled with your package, you just need to install `Erlang`, and set your `ERLANG_HOME` variable.
* Elastic Search - this is bundled with the package, your just need `Java` and correctly configure your `JAVA_HOME` variable

 Take a note at the LocalDb instance, in this case I'm using `Projects`, you can pick one from the drop list. Click `Save Settings` button


![alt](http://i.imgur.com/WnrRSSE.png)
Now starts these 3 services

* SQL Server localdb
* Elastic Search
* Rabbit MQ

Make sure there are no errors coming out from the control center console,then open you command prompt, go to your directory, type `powershell` and `enter`
![alt](http://i.imgur.com/cF5TBcb.png)

then type the following `.\Setup-SphApp.ps1 -ApplicationName ofg -Port 50332`, it must be lowercase letters only, no `.`, `,` numbers or special symbols allowed. Then specify the port to run your application on, choose those over 50000 to avoid conlict with your other services. Make sure the SqlServer is specified according to what you had set in the control center window

![alt](http://i.imgur.com/JS6m80D.png)

This going to take a while, to set up your database, elastic search and rabbitmq vhost.
Once you are done, you can go back to your `ControlCenter`, now starts `SPH Worker` and `IIS Express`
![alt](http://i.imgur.com/swWEPyL.png)


## Writing your first application
Once it's been set up, you can browser to your app by clicking on the `ViewApp` link in the `ControlCenter`, A default user `admin` and password `123456` has already been created by the `SetUp-SphApp.ps1` script for your convenience.
go to 
http://localhost:50332/sph#dev.home

![alt](http://i.imgur.com/8T46sAg.png)
click on the `New Entity Definition`, this will take you to the `EntityDefinition` editor where you can design your first entity. For now lets just assume that an `EntityDefinition` is the main object in your application. Let's do a Hospital application , you'll need a `Patient`
![alt](http://i.imgur.com/1v4LkQA.png)

Then click save, suddenly the screen will have new things appeared at the bottom. Right click on the empty folder, and select `Add Child`, give it a name `Mrn`. In case you don't know what Mrn, it stands for  Medical Record No, a primary way of identitfy a patient in a hospital
![alt](http://i.imgur.com/8XyrM5k.png)

![alt](http://1.bp.blogspot.com/-oxkbDsgv9wE/TWX2SGMC8qI/AAAAAAAAACM/6AUzB4_fK34/s1600/MRN.JPG)
sample MRN

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
[http://localhost:50332/docs](http://localhost:50332/docs)






