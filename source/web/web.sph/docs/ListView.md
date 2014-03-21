#ListView
##Overview
Provides a tabular view of your collection, `ListView` is one of the container `FormElement` in which it could contains other elements. The main properties of the `ListView` is the [`ListViewColumnCollection`](ListViewColumn.html) which represents individual column in the table.

![ListView](http://i.imgur.com/irz9aYD.png)
##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>ChildItemType</td><td> - Full name for the child item type, this is normallu in form `bespoke.<applicationName>_<EntityDefinitionId>.domain.<ChildItem>` for entity form. While for <code>ScreenActivity</code> in workflow it's in the form `bespoke.w_<WorkflowDefinitionId>_<version>.sph.<ChildItemType>` when the child item is generated from XML Schema</td></tr>
<tr><td>IsChildItemFunction</td><td> - (Obsolete) </td></tr>
<tr><td>ListViewColumnCollection</td><td> - Columns for the listview see <a href="ListViewColum.html"><code>ListViewColum</code></a> for details </td></tr>
</tbody></table>



## See also

[FormElement](FormElement.html)
[Button](Button.html)
[DateTimePicker](DateTimePicker.html)
[DownloadLink](DownloadLink.html)
[FileUploadElement](FileUploadElement.html)
[ImageElement](ImageElement.html)
[ListView](ListView.html)
[MapElement](MapElement.html)
[AddressElement](AddressElement.html)
[CheckBox](CheckBox.html)
[ComboBox](ComboBox.html)
[DatePicker](DatePicker.html)
[EmailFormElement](EmailFormElement.html)
[HtmlElement](HtmlElement.html)
[NumberTextBox](NumberTextBox.html)
[SectionFormElement](SectionFormElement.html)
[TextAreaElement](TextAreaElement.html)
[TextBox](TextBox.html)
[WebsiteFormElement](WebsiteFormElement.html)