#EntityForm
##Overview
`EntityForm` lets you create a HTML5 form for your entity, these form allows every possible scenarios that you might want how your users interact with the form. It allows for a simple insert , edit or read only options for your entity. `EntityForm` provides a great deal of built tools that take almost 99% of your needs, for the rest there are great extensibility features available that allow you to add custom elements. For more serious customization you can always edit the HTML and Javascript file generated by the `EntityForm` renderer, thus allowing almost limitless customization options.

##Concepts
`EntityForm` is a set of definitions for how a form should behave and look. It contains a component called `FormDesign` which is the main design surface. You don't normally have to deal with `FormDesign` directly. `FormDesign` may contains a list of `FormElement` implementation, this could be `Button`,`TextBox`,`ComboBox` , `ListView` etc.

`EntityForm` also may contains a set of commands, i.e. operation in which it could be invoked

![Designer](http://i.imgur.com/NVQLZKm.png)

1. Toolbar
2. Title
3. Toolbox
4. Design Surface



![Toolbox pallete](http://i.imgur.com/mXK17LN.png)
Toolbox pallete let you drag or click a `FormElement` into the designer surface. Drag and drop if you want to drop at a specified position, or just click will add a new element to the bottom of the designer 

![Property tab](http://i.imgur.com/NKpgQs5.png)
Property tab lets you configure individual form element , once selected in the design surface. More setting link allow you to see more configs while Advance settings allow you to further customize your element

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>FormDesign</td><td> - The design surface for the form - Use internally by SPH web designer </td></tr>
<tr><td>Rules</td><td> - Set of rules appliend when the standard "save" operation is invoked</td></tr>
<tr><td>EntityFormId</td><td> - The form id</td></tr>
<tr><td>EntityDefinitionId</td><td> - The id of the <code>EntityDefinition</code> </td></tr>
<tr><td>Name</td><td> - The forms name - this just an identitifier</td></tr>
<tr><td>Route</td><td> - Route is where the uri for the form, thus it must be unique to whole system. It must be a valid uri identitfier normally in lowercase seperated by periods or "-"</td></tr>
<tr><td>Note</td><td> - Developers note</td></tr>
<tr><td>IsAllowedNewItem</td><td> - Allow new entity to be added, A toolbar will be shown in the entity landing page, when this property is set to true </td></tr>
<tr><td>IconClass</td><td> - The font-awesome icons</td></tr>
<tr><td>IconStoreId</td><td> -For png images</td></tr>
<tr><td>IsPublished</td><td> -A flag for if the form has been published, if it is a route will be generated</td></tr>
<tr><td>IsDefault</td><td> - Is it a primary form for the entity</td></tr>
<tr><td>IsWatchAvailable</td><td> - Allow watch button to be visible, allowing your user to wath the entity instance and get a message when it's ipdated </td></tr>
<tr><td>IsEmailAvailable</td><td> - Allow user to email the entity instance, You can use <a href="EmailTemplate.html">EmailTemplate</a> </td></tr>
<tr><td>IsPrintAvailable</td><td> -Allow the print version of the entity </td></tr>
<tr><td>IsAuditTrailAvailable</td><td> -Show audit trail/logs panel </td></tr>
<tr><td>IsRemoveAvailable</td><td> - Allow the instance to be removed</td></tr>
<tr><td>IsImportAvailable</td><td> - Allow import/migration external data</td></tr>
<tr><td>IsExportAvailable</td><td> - Allow export to csv or json of the current instance</td></tr>
<tr><td>Operation</td><td> - The <code>EntityOperation</code> for the save button, when selected this operation is invoked instead of the default save </td></tr>
</tbody></table>



## See also
[FormDesign](FormDesign.html)
[EntityDefinition](EntityDefinition.html)
[EntityView](EntityView.html)