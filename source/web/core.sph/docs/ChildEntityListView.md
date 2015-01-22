#ChildEntityListView
[`ChildEntityListView`](ChildEntityListView.html) form allows your to embed mini [`EntityView`](EntityView.html) that belongs to another entity in a [`EntityForm`](EntityForm.html). Basically it creates some sort of `Children` list of remote entities.

A simple use case is, if your `Patient` entity has a collection of `Appointment`, and you model `Appointment` as a seperate `EntityDefinition`, and `Appointment` entity definition a a field called `Mrn` which is basically linked back  to the `Patient` Mrn.

![Appointment form](http://i.imgur.com/SkziC8K.png)

and you want your `Patient` details form to look like this
![patient detail](http://i.imgur.com/nfnytEw.png)


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
   <tr>
       <td>Entity</td>
       <td> - Lets you pick the type of the child Entity</td>
   </tr>
   <tr>
       <td>Query</td>
       <td> - The filter query for the API to get the child items, this must be valid odata API as implemented by Rx Developer and all the member sepecified in the query mus be IsFilterable</td>
   </tr>
   <tr>
       <td>ViewColumnCollection</td>
       <td>
           - The list of columns to be displayed
           <a href="ViewColumn.html">ViewColumn</a> for details
           </td>
</tr>
   <tr>
       <td></td>
       <td></td>
   </tr>
</tbody></table>

![alt](http://i.imgur.com/1luk73l.png)
