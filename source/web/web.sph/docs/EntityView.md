#EntityView
##Overview
A set of user friendly tools to create a view for your entities. It allows you to display your entities in a table.
![alt](http://i.imgur.com/AWNnB9i.png)
A tile will be displayed on the [`EntityDefinition`](EntityDefinition.html) landing page

You can also apply [`Filter`](Filter.html) to your list, which will naturally filter the list. List is read from ElasticSearch repository.

[Sort](Sort.html) can also be applied to your list.

[ViewColumn](ViewColumn.html) allows you to specify the columns to your table. A leftmost column will automatically inserted with the value of your `RecordName` and a link point to the default form to your `EntityDefinition`

![drag to arrange column](http://i.imgur.com/QWr9y94.png)
Drag to arrange columns, and click to select a column for you to edit the column's properties

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>FilterCollection</td><td> - The list filter to apply to the list</td></tr>
<tr><td>ViewColumnCollection</td><td> - The list of columns</td></tr>
<tr><td>SortCollection</td><td> - Sorts</td></tr>
<tr><td>EntityViewId</td><td> - </td></tr>
<tr><td>IconClass</td><td> - </td></tr>
<tr><td>IconStoreId</td><td> - The store id for the icon </td></tr>
<tr><td>EntityDefinitionId</td><td> - </td></tr>
<tr><td>Name</td><td> - Name</td></tr>
<tr><td>Route</td><td> - Route to your view, must be unique</td></tr>
<tr><td>Note</td><td> - Developers note</td></tr>
<tr><td>Query</td><td> - </td></tr>
<tr><td>IsPublished</td><td> - make it available</td></tr>
<tr><td>Visibilty</td><td> - (Reserved)</td></tr>
</tbody></table>



## See also

