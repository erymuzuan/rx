# EntityDefinition data as source

There are times when all you need is some sort of reference data for your app. A good example of this is a set of lookup data that rarely changes such as the list of states used as options in your drop down list.

Once you have identify the needs for reference data, you might want to make them as part of your application source codes so that they can be deployed together with your application or share them with your colleagues using a version control system.

Checking this option will create a different kind of code to your `EntityDefinition`, if you examined the *generated* source folder for your `EntityDefinition`(the default location is `source\_generated\<YOUR_ENTITY_DEFINITION>`), you will find this

```csharp

    [StoreAsSource(IsElasticsearch = true, IsSqlDatabase = true)]
    public class YOUR_ENTITY_DEFINITION : Entity
    {
      //....
```
This simple `Attribute` will instruct Reactive Developer to treat any data for your EntityDefinition as source and it will write them to a source file, these source files can then be added to your version control.


If you are on the receiving end of the source, your build your `EntityDefinition` with `sph.builder` and it will automatically populate the data from the sources
