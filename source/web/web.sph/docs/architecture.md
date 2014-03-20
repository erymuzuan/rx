﻿#Architecture
SPH is an enterprise ready application platform built using the well known patterns and architecture style available from the industries. It's not a traditional `n-tier` application, but something what we called `Reactive` architecture.It's a combination of `Command Query Segregation Responsibility(CQRS)` and `Pub-Sub` with messaging at it's heart instead of the usual `Remote Procedure Call(RPC)` used in traditional `n-tier` apps.



`Reactive` is this case, refer to the fact that it's event driven with publisher and subscribers concepts called `Pub-Sub`. This kind of archtecture allows you to easily scales without burdening your application with bottlenecks and un-necessary complexities and pulling requests for updates.

`Pub-Sub` allows you to asyncronously react when something happened or when an event of your interest is raised. Take an example, if in a hospital environment
##Scenarios
Given the event is when a new `patient is registered`
Then ask the ward to `prepare a bed`
At the same time `let the dietician know`s that the patient might have special diet.
and may be at the same time `open a billing record` in the finacial system.


In a tradiontional `n-tier` app, this would normally designed as  method named `Register` that will internally call the methods
<pre>

public class Patient
{
    public async Task&lt;string&gt; Register()
    {
        var s = await Persistence.Save(this);
        var f = Financial.RegisterBillingAsync(this);
        var w = Ward.RegisterBed(this);
        var d = Diet.RegisterDiet(this)
    
        await Task.WhenAll(f,w,d);
    
        return s.Mrn;
    }

    //.. other methods
    //.. other members
}
</pre>

The traditional `n-tier` requires you to think hard up front about your business process, thus making you code very rigid and pretty risky to changes.

While with `Reactive` architecture, you will not pre define the call to the next 3 methods, instead you will listen to the `Patient.Register` event, you will then attach event handlers to this `event` to do any of the subsequent action. This archicture allow great deal of flexibility as you can add and remove event handlers without compiling the code with our messaging `Pub-Sub`.
<pre>
// a simpler approach with Reactive architecture
public class Patient
{
    public async Task&lt;string&gt; Register()
    {
        var s = await Persistence.Save(this);    
        return s.Mrn;
    }

    //.. other methods
    //.. other members
}
</pre>
and some where else you just create an event handler that subscribe to the `Patient.Register` topic
<pre>
public void AddFinancialBilling()
{
    this.AddBindings("Patient.#.Register", Financial.RegisterBill);
}
</pre>

and this piece of code doesn't need to be anywhere near `Patient`, i.e. `Patient` class is ignorant about your `Subscriber`, it could even be on different machine.

`Reactive` architecture allows you to respond to changes quickly, without the risk of breaking existing code.

SPH allow you to use `Reative` with code, and to to think about the internals of the `Pub-Sub` messaging. All you have to worry is your business requirements.

## Pub-Sub with Triggers
These series of event and the sequence of actions could easily design and deployed in SPH via an `Entity Trigger`.What happened is when the event is raised, it will be submitted into the message broker, where the broker will distribute the information about the event to any subscribers that has registered for that particular event via topic subscription.
A `Trigger` is a subscriber that subscribe to the event and can perform the subsequent action.

![Pub sub](http://i.imgur.com/efWfOWP.png[/IMG)