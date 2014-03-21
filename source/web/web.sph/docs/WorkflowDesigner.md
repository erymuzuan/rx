#WorkflowDesigner
##Overview
Provides the design surface for `WorkflowDefinition`


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
We use Bootstrap typeaheadjs to provide the autosuggest feature, the default behaviour is to cache the list of autosuggest in localstorage. You should clear the localstorage from your browser in order to refresh the list

![](http://i.imgur.com/wgZb5is.png)
## See also

