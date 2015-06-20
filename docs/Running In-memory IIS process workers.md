What happen if your PC don't have much memory or lots of CPU power, running a full version Reactive Developer with RabbitMQ and out of process workers really going to make you sweat a lot. In memory broker provides the same topical messaging as the RabbitMQ AMQP model, you can be sure you topic will still work as usual, but what's the catch.



* In memory broker doesn't provide the kind of reliabilty needed to run highly concurrent request

* In memory broker lacks the durable state provided by RabbitMq, so you might lose your message if things go teribbly wrong

* In memory broker run the subsribers ProcessMessage synchronously , in the same thread as the request.



Given all these, why we take all the efforts just to become less. It's all about convenience and simplicity especially during development phases.

* In memory broker starts much more quickly * You don't need to install Erlang

* You don't need to install RabbitMQ So it's great, now how do we get started.



First you need to update to at least build 10307. Then you need to update you web.config file, there are few places that you need to edit. Log4Net.dll assembly binding, make sure you web/bin contains log4net.dll, if not you can copy it from subscribers folder



![http://i.imgur.com/hDULGEu.png](http://i.imgur.com/hDULGEu.png)



You can copy this snippet and paste it just like the image shown above



```xml  
<dependentAssembly>
    <assemblyIdentity name="log4net" publicKeyToken="669e0ddf0bb1aa2a" culture="neutral" />
    <bindingRedirect oldVersion="0.0.0.0-1.2.13.0" newVersion="1.2.13.0" />
 </dependentAssembly>
```




 Next you need edit your `Logger`, `IEntityChangePublisher` and `IEntityChangeListener`



![](http://i.imgur.com/0ytjBRY.png)



For the logger, replace `<object type="web.console.logger.Logger, web.console.logger" />` with


 ```xml

<object type="Bespoke.Sph.Messaging.Logger, memory.broker" />
```


and for `IEntityChangePublisher`

```xml
<object name="IEntityChangePublisher" type="Bespoke.Sph.Messaging.Broker, memory.broker" />
```

There 2 properties you can change for the memory broker

1. `WaitTimeForProcessMessage` The maximum wait time in seconds, for calling ProcessMessage on each subscriber, this let you gracefully timeout if your subscriber takes too long to execute. The default is 5 seconds

2. `WebSocketPort` Port no in which the broker will host a websocket server to talk to your developers panel in your development page. The default is 50238

To change the default, use the property tags
```xml

<object name="IEntityChangePublisher" type="Bespoke.Sph.Messaging.Broker, memory.broker">
   <property name="WaitTimeForProcessMessage" value="10"/>
   <property name="WebSocketPort" value="50240"/>
</object>
```



then the `IEntityChangeListener`



```xml
<object name="IEntityChangedListener&lt;Message&gt;" type="Bespoke.Sph.Messaging.ChangeListener&lt;Message&gt;,memory.broker" />
<object name="IEntityChangedListener&lt;Page&gt;" type="Bespoke.Sph.Messaging.ChangeListener&lt;Page&gt;,memory.broker" />
 ```



Now its time to start your control center, edit your ControlCenter.bat in your root directory to

```bash
IF EXIST "Update.bat" ( Update.bat )
control.center\controlcenter.exe /log:console /in-memory-broker
```

Save it, and run the ControlCenter.bat file, you'll get a Control Center window open up with

![](http://i.imgur.com/nr8SJOx.png)

 Start you Elasticsearch and the start the IIS Express.

 In the future we will make it easier for you to switch from RabbitMQ broker to the in memory broker and back. In the meantime, download it and give it a try, there may be few kinks hiding around some corners some where.

 
