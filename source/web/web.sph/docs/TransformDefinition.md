# Transform Definition

`TransformDefinition` is a data mapping features provided by Reactive Developer to enable visual style of mapping data from sources to a destination type.

There are many situation where data mapping designer are needed, for example if you need to create integration between your external database and your `EntityDefinition`, the schema between these 2 data sources might not be the same.


## Visual Mapping Designer
![Map designer](https://lh3.googleusercontent.com/-OeP8fX70UCk/Vkp6LMsFEbI/AAAAAAAAKgk/XrvzDRo29Ng/s2048-Ic42/%25255BUNSET%25255D.png)

Reactive Developer provides a visual mapping designer with clean and easy to use HTML based UI. There are basically 5 major areas in the visual mapping designer that you should be aware of.

1. Toolbar for common commands
2. Source(s) tree for your source data type schema
3. Destination data type schema
4. Mapping designer surface for [functoids](Functoid.html)
5. [Functoids](Functoid.html) toolbox

The designer surface will automatically changes when you have properly configured your `TransformDefinition` properties

![mapping designer](https://lh3.googleusercontent.com/-b9a9_qIX7U8/Vkp7sefOMMI/AAAAAAAAKgw/oCo82Sb0Buo/s2048-Ic42/%25255BUNSET%25255D.png)


## Configure your mapping
From the toolbar you can click `Edit Properties`

![Mapping properties](https://lh3.googleusercontent.com/-PLdcFmr6mDU/Vkp9Jos1blI/AAAAAAAAKhE/rseujcdWVOc/s2048-Ic42/%25255BUNSET%25255D.png)

1. TransformDefinition name - must be valid C# identifier
2. Developers note
3. Some mapping can be used with more than one input data, if your map required data to come from multiple input source then check this
4. Select a input assembly where the type is defined
5. Select the type name for the input, populated from the input assembly
6. Select the assembly for your output type
7. Select your output type
8. If your map requires additional assembly references that you use in your functoids, then you can add them here.



 Go to [Mapping Configuration](TransformDefinitionConfiguration.html)


## Testing your mapping

It takes a lot of time to get your mapping right, Rx Developer makes it easier by providing you with a simple to use test runner for your `TransformDefinition`. Go to the test menu to create a sample input
![](https://lh3.googleusercontent.com/-VwwfrgN0-rU/Vz5hZ1B35dI/AAAAAAAA8Mk/RvHfhIht_AM5lLxVPry9Rh8hbn_hKhZPwCCo/s2048/%255BUNSET%255D)

1. Click on the `Edit test input` drop down menu, where you'll be presented with a default implementation of your input
2. Edit your input definition as you see fit.
3. Click `Save & Close`
4. Now you can begin test, by clicking on the `Test Mapping` drop down menu.
5. You'll be presented with the output , if test pass
6. An exception details will be presented if an Exception is thrown during the mapping process
