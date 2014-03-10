#EntityDefinition
The first core concept of SPH is the `Entity`. This plays a very vital role in the whole SPH programming concept. `Entity` is not a `table` translated from relational world. Thus it is hugely different than `table` and `tuples` from relational world.

To really understand the power of `Entity` is to forget everthing you know about relational database and it's related technologies such as `object relational mapper(ORM)`.The concepts of foreign key, parent child, and normal forms just simply didn't exist in the first place.

An `Entity` is an object that you are interested in to track their life in your problem domains. This could be `Customer` if you are developing a Customer relationship management or `Patient` in case clinical information system. It's also know as `Root` object. Then there are `Value` objects, these are objects that attached to an entity, and would not exist without the `Entity` for example `Address` for your `Customer`, the `Address` alone is not an `Entity` as it is quite useless without the `Customer`. But if you are developing a Geographic information System for postal service than an `Address` could be your `Entity`.


As such, think an `Entity` as the a traditional paper folder where it holds many paper records inside. Take a folder for a `Patient` in a hospital environment. It might have these fields writen on it.
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

In SPH, this could easily be modelled as follows

```

    Entity:Patient
        Value:FullName
        Value:Mrn
        Value:Address
            Value:Street
            Value:Postalcode
            Value:State
        Value:Visit list**
            Value:DateTime
            Value:Title
            Value:Doctor     
        Value:Medical History list
            Value:DateTime
            Value:ClinicalNote
          ...
           ... 

```

This is very natural way storing your data, in a way that we all understand, there's no need to artificially model after tables and relatioships. The way SPH take care of your data is known as `Document` store database. You can read more about [Document database](http://en.wikipedia.org/wiki/Document-oriented_database) in Wikipedia.

Notice that `Address` is just another object that belongs to the `Patient`, what makes the `Patient` and `Address` diferrent is very minimal
*`Patient` is an object that you interested to track it's life time
* `Address` is just an object that belong to another `Value` object or an `Entity`
*`Patient` has an identitfier field or what we call RecordName
*`Address` doesn't have any identifier field, as such with it's parent, it's imposible to tell if an `Address` belong to patient A or patient B. `Value` object does not live in isolation.
