# Messaging Action

Messaging action allows you to use RX Developer as simple messaging infrastructure to connect to external data store.

Messaging infrastructure is a simple way of getting one piece of data coming in and going out in another format. In this case, the data that coming in is the current `item` in the `Trigger`. The `item` will be transformed into the final destination using a `TransformDefinition`

The inbound data must be one of `EntityDefinition` and the outbound data could be one of types defined by your `Adapter`. A simple example is

For every new item of type `<your EntityDefinition type here>`, insert a new row into one of your `Adapter`'s table.

![Messaging action dialog](https://lh3.googleusercontent.com/-iRYOzjiJNaA/VtoTajwAytI/AAAAAAAA5t0/jEFaAz6kfm8/s2048-Ic42/%25255BUNSET%25255D.png)


1. Give your action a valid name, for you easily identify it later
2. Select an `Adapter` defined in your solution
3. You can optionally select an `Operation`, for example a Stored Procedure defined by the selected adapter
4. Or you can select a `Table` defined by your `Adapter`.
5. If `Table` is selected, you can select the operation you want to perform
6. Select a `TransformDefinition` map, to transform your `item` into the one needed by your Adapter
7. Set a maximum attempt to invoke the operation on your `Adapter`.
8. How long to wait before retry another attempt.

## Protecting your back end failure with `Messaging Action`
 RX Developer will automatically attempt to perform the operation up to the number specified. This will help you achieve a higher availability rate of your back end, without upsetting your user.

 Once the maximum number of attempts has been reached, RX Developer will direct the item to the `Dead-Letter-Queue` where your administrator can manually process the message.
