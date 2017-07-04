# XML ReceivePort

Used when there's no XML Schema(XSD) available


These are few things to be aware of
* Meant for XML document
* Must have single XML namespace defined or default namespace ,e.g `xmlns=""` or `xmlns="http://your-namespace"`
* Must be valid XML document, i.e. with root
* Items path must be presented in `\` seperator

```xml
<a comment="Modern music">
  <b genre="Metal">
      <band name="Megadeth"/>
      <band name="Slayer"/>
  </b>
  <b genre="Blues">
      <band name="SRV"/>
  </b>
</a>
```
In this example, the `RootPath` should be `a\b\band`

To create/read value from Parent or Root attribute, you can edit source json file, the wizard do helps a bit, but not much

```js

{
       "$type": "Bespoke.Sph.Domain.XmlAttributeTextFieldMapping, domain.sph",
       "Name": "genre",
       "TypeName": "System.String, mscorlib",
       "Path" : "$Parent$.genre",
       "WebId": "My-Guid"
}

```

to read the Root property, you can use `$Root$` alias, e.g. `$Root$.comment`


this will create a Port called 'Band' with the following members

1. genre
2. name
