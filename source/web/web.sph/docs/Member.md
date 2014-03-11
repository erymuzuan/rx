#Member
##Overview
Member represent a field definition in your `EntityDefinition`, it allows you to specify the data type that it holds as well as other attributes.

## Things to be aware
For a member with type set to `Collection`, the name must be appended with the word `Collection` e.g. `Visit` list should names as `VisitCollection`

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>FullName</td><td> - Must be a valid identifier for e.g. `FullName` instead of `Full name` </td></tr>
<tr><td>Type</td><td> - The data type for the member, these are string, integer, datetime, boolean and decimal</td></tr>
<tr><td>MemberCollection</td><td> - Child members for aggregates</td></tr>
<tr><td>FieldPermissionCollection</td><td> - The user permission to the member</td></tr>
<tr><td>Name</td><td> - The members name, Must be valid identifier </td></tr>
<tr><td>TypeName</td><td> - Used internally, or at least this is what the system use </td></tr>
<tr><td>IsNullable</td><td> - Do you allow the user to leave blank or null for this member</td></tr>
<tr><td>IsNotIndexed</td><td> - If checked it will not be indexed by the search engine so this member will be searchable, it might be filterable if you set IsFilterable to true</td></tr>
<tr><td>IsAnalyzed</td><td> - For string member, you can enable full text search and tokens search using Lucene power search engine</td></tr>
<tr><td>IsFilterable</td><td> - Makes the member filterable in you REST api as well make it available for your ReportDefinition Filters</td></tr>
<tr><td>IsExcludeInAll</td><td> - If checked the member will not be included in searches </td></tr>
<tr><td>Boost</td><td> - The scale in which how important is this member to the search engine</td></tr>
</tbody></table>



## See also
[FiedPermission](FieldPermission.html)
