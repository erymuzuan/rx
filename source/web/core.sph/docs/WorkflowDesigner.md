#WorkflowDesigner
##Overview
Provides the design surface for `WorkflowDefinition`. This allow developers to quickly build a `WorkflowDefinition` visually. It consist of a design surface where you can draf and drop selected artivities. Each activity could also be configure via their own property window.



## Toolbar
![Toolbar](http://i.imgur.com/4iUUtqO.png)
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Toolbar</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Save</td><td> - Save the current `WorkflowDefinition` to the repository. NOTE : No validation or compilation occurs</td></tr>
<tr><td>Delete</td><td> -Remove the current `WorkflowDefinition` from repository. Will not remove it the deployed versions, use management tools to remove deployed versions</td></tr>
<tr><td>Reload</td><td> - Redraw the current design surface</td></tr>
<tr><td>Export</td><td> -Export an archived version of this `WorkflowDefinition`</td></tr>
<tr><td>Import</td><td> - Import the archived `WorkflowDefinition`</td></tr>
<tr><td>Pages</td><td> -Link to the [Page](Page.html) list</td></tr>
<tr><td>Build</td><td> - Compile and validates the `WorkflowDefinition`</td></tr>
<tr><td>Publish</td><td> -Validate, compile and publish the current `WorkflowDefinition`</td></tr>
<tr><td>Title</td><td> -The name of the current `WorkflowDefinition`</td></tr>
</tbody></table>

##Toolbox
Toolbox represent a floating palletes on your right(or bottom). It consist of 4 tab pages. You can close the toolbox by clicking on the `x` on the top right corner, to re open the toolbox use `[Ctrl] + [Alt] + x`

![Toolbox](http://i.imgur.com/LJXnbaI.png)

###Toolbox
List of [`Activities`](Activity.html) that you can drag on drop onto the design surface

### Variables
Lets you add and configure [`Variable`](Variable.html) for you [`WorkflowDefinition`](WorkflowDefinition.html)

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>X</td><td> - The position in the design surface for an `Activity`</td></tr>
<tr><td>Y</td><td> - The position in the design surface for an `Activity`</td></tr>
</tbody></table>

##Known issues
The `Path` autosuggest in ScreenActivity do not refresh to the new path, when you change the data type for the variable.

### Workaround
We use Bootstrap typeaheadjs to provide the autosuggest feature, the default behaviour is to cache the list of autosuggest in localstorage. You should clear the localstorage from your browser in order to refresh the list. Click on all relevant keys to your WorkflowDefinition with corresponding id(10002 in this example), and press [delete] key on your keyboard.

![](http://i.imgur.com/wgZb5is.png)
## See also

