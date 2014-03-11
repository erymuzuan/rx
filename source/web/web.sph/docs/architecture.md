#Architecture
SPH is an enterprise ready application platform built using the well known and best practices available from the industries. It's not a traditional `n-tier` application, but something what we called `Reactive` architecture.It's a combination of `Command Query Segregation Responsibility(CQRS)` and `Pub-Sub` with messaging at it's heart instead of the usual `Remote Procedure Call(RPC)` used in traditional `n-tier` apps.



`Reactive` is this case, refer to the fact that it's event driven with publisher and subscribers concepts`Pub-Sub`. This kind kind of archtecture allows you to easily scales without burdening your application with bottlenecks and un-necessary complexities and pulling requests for updates.

`Pub-Sub` allows you to asyncronously react when something happened or when an event of your interest is raised. Take an example, if in a hospital environment
##Scenarios
Given the event is when a new `patient is registered`
Then ask the ward to `prepare a bed`
At the same time `let the dietician know`s that the patient might have special diet.
and may be at the same time `open a billing record` in the finacial system.


## Pub-Sub with Triggers
These series of event and the sequence of actions could easily design and deployed in SPH via an `Entity Trigger`.What happened is when the event is raised, it will be submitted into the message broker, where the broker will distribute the information about the event to any subscribers that has registered for that particular event via topic subscription.
A `Trigger` is a subscriber that subscribe to the event and can perform the subsequent action.

