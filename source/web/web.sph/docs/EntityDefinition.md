#EntityDefinition
##Overview
The first core concept of `Rx Developer` is the `Entity`. This plays a very vital role in the whole `Rx Developer` programming concept. `Entity` is not a `table` translated from relational world. It is hugely different than `table` and `tuples` from relational world that you may know.

To really understand the power of `Entity` is to forget everthing you know about relational database and it's related technologies such as `object relational mapper(ORM)`.The concepts of foreign key, parent child, and normal forms just simply didn't exist in the first place.

An `Entity` is a type of object that you are interested to track it's life in your problem domains. This could be `Customer` if you are developing a Customer relationship management or `Patient` in case clinical information system. It's also know as `Root` object. Then there are `Value` objects, these are objects that attached to an entity, and would not exist without the `Entity` for example `Address` for your `Customer`, the `Address` alone is not an `Entity` as it is quite useless without the `Customer`. But if you are developing a Geographic information System for postal service then an `Address` could be your `Entity`.


As such, think an `Entity` as the a traditional paper folder where one folder hold one record, that may contains many documents inside. Take a folder for a `Patient` in a hospital environment. It might have these fields writen on it.
* The name of the patient
* The registration number(MRN)
* Date of birth
* Address
    * Street
    * State
    * Postal code
* etc

and there could be a number of documents inside it, such as visit records, medical history, referals etc.

So your `Entity` will be called `Patient`, and `Visit` will be your value object, well in this case there will be many `Visit`s

In `Rx Developer`, this could easily be modelled as follows

```

    Entity : Patient
        Value : FullName
        Value : Mrn
        Value : Address
            Value : Street
            Value : Postalcode
            Value : State
        Value : Visit list**
            Value : DateTime
            Value : Title
            Value : Doctor     
        Value : Medical History list
            Value : DateTime
            Value : ClinicalNote
          ...
           ... 

```

This is very natural way storing your data, in a way that we all understand, there's no need to artificially model after tables and relatioships. The way `Rx Developer` take care of your data is known as `Document` store database. You can read more about [Document database](http://en.wikipedia.org/wiki/Document-oriented_database) in Wikipedia.

Notice that `Address` is just another object that belongs to the `Patient` entity, what makes  `Patient` and `Address` diferrent is very minimal
*`Patient` is an object that you interested to track it's life time
* `Address` is just an object that belong to another `Value` object or an `Entity`
*`Patient` has an identifier field or what we call RecordName
*`Address` doesn't have any identifier field, so without it's parent, it's imposible to tell if an `Address` belong to patient A or patient B. The key point to take home is `Value` object does not live in isolation.



##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>CodeNamespace</td><td> - System generated</td></tr>
<tr><td>MemberCollection</td><td> - The fields or member that you define in `EntityDefiniton` schema designer</td></tr>
<tr><td>BusinessRuleCollection</td><td> - Business rules that you may apply to this `Entity`. Rules can be applied to a `EntityForm` and/or `EntityOperation` </td></tr>
<tr><td>EntityOperationCollection</td><td> - The list of valid `EntityOperation` or executable method to perform an operation to the entity instance</td></tr>
<tr><td>EntityDefinitionId</td><td> - The internal identifier key. This is used in many parts</td></tr>
<tr><td>Name</td><td> - The name of the `EntityDefinition`, must be a valid identifier</td></tr>
<tr><td>Plural</td><td> -The plural name for the `EntityDefinition` </td></tr>
<tr><td>IconStoreId</td><td> - Set a icon image for your landing page, this should a PNG file. We recommend getting using Syncfusion Metro Studio for your image </td></tr>
<tr><td>IconClass</td><td> - Icon for the item and navigation menu - pick one from fontawesome or bootstrap glyphicon </td></tr>
<tr><td>RecordName</td><td> - The primary member where a reference is normally made, it's used as the primary way of identifying a record(instance of an `Entitydefinition`) apart from `Name`Id </td></tr>
<tr><td>IsPublished</td><td> - The state wether the EntityDefinition is in development or has been published</td></tr>
</tbody></table>



## See also
<ul>

                <li><a href="Member.html">Member</a></li>
                <li><a href="EntityForm.html">EntityForm</a></li>
                <li><a href="EntityView.html">EntityView</a></li>
                <li><a href="FieldPermission.html">FieldPermission</a></li>
                <li><a href="BusinessRule.html">BusinessRule</a></li>
                <li><a href="EntityOperation.html">EntityOperation</a></li>
            </ul>
