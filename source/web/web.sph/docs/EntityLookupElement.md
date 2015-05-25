#EntityLookupElement

The sole purpose of `EntityLookupElement` is to provide you an easy way for your users to provide a value to a field, where the field could come from another member in another `Entity`. For example to create a new `Appointment` for a `Patient` you have to provide the `Patient` `MRN`. Of course you could always provide a `TextBox` and set the `Autocomplete` field but `EntityLookupElement` provides you with additional features for your users to search for the `Patient`.


<table class="table">
    <thead>
        <tr>
        <th>Property</th>
        <th>Description</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Entity</td>
            <td>The EntityDefinition you wish the lookup coming from, in the Patient's appointment case, it should be to Patient</td>
        </tr>
        <tr>
            <td>ValueMemberPath</td>
            <td>The path to the lookup Entity, you wish the value to be extracted from and this value will be persited to your edited Entity <em>Path</em></td>
        </tr>
        <tr>
            <td>DisplayMemberPath</td>
            <td>The value to displayed with the default ko text binding next to the search icon, Normally set to the same value as your path</td>
        </tr>
        <tr>
            <td>DisplayTemplate</td>
            <td>If you wish to display the value in any other way, this will override DisplayMemberPath ko text binding</td>
        </tr>
        <tr>
            <td>Columns</td>
            <td>The list of columns from the lookup entity to be displayed in the lookup dialog result list</td>
        </tr>
    </tbody>
</table>