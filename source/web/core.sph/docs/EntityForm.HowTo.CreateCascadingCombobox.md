#How To : Create Cascading Combobox
Cascading combobox is common scenarion in when developing a form, it helps your users to narrow down the selection and makes it easier for them to avoid mistakes. Cascading combobox is when a combobox options depends on other values that might change, this could be the value from another combobox.

Take an example where you have a list of `Car makers` such as BMW, Mercedes and Mazda, then these makes has their own models. It makes sense for your user to select the car make first, then present her the corresponding models from the make.

In this example, we are going to do a simple `State` and `District` cascading combobox.
![address field](http://i.imgur.com/yjihbqk.png)

and when the State changed, so does the list of District options
![District options](http://i.imgur.com/PThi4wE.png)

## Creating the dependent entities
You'll need an [`EntityDefinition`](EntityDefinition.html) for `State` and one for `District`

![States](http://i.imgur.com/OIUGCoa.png)

![Districts](http://i.imgur.com/cozv794.png)

## The form
Drops 2 [`CombobBox`](CombobBox.html) on the [`FormDesign`](FormDesign.html) surface, for the first one, configure it for `State`. Set the lookup entity to `State`, and for `Value` and `Display` path to `Name`, leave the `Query` blank.

![State combobox](http://i.imgur.com/oyPMkwf.png)

Now go to the second combobox, configure it for the `District`.Set the lookup entity to `District`, for `Value` and `Display` path to `Name`.

![District combobox](http://i.imgur.com/5SDyRSA.png)
(1) Make sure to check the `Computed Query` checkbox, thus the `Query` field will be evaluated as function.

(2) Now the interesting part is how we are going to manipulate the `Query` property for this combobox.
<pre>
'State eq \'' + State() + '\''
</pre>
now if the `State` field is changed to Kelantan this will trigger the query to change to
<pre>
'State eq \'Kelantan\''
</pre>
and the dropdown will be refreshed from the server.


Please note that you have to use `'`(single quote) in the query, as `"`(double quote) is not allowed as the generated functionwill be embedded in your HTML attribute. As such we have to escape any occurence of `'` in our query.
![code](http://i.imgur.com/AbTouu1.png)