#ComplexVariable
##Overview
Let you define a `Variable` from your XSD schema that you had upload into your [`WorkflowDefinition`](WorkflowDefinition.html)


##Known limitiations

* No enumeration is supported
* Only the `All` model is supported for `element` or `complexType`
* List must be define in a special way, see how an `element` called `Parent` has a list for `Child`
```xml
<xs:element name="Parent">
		<xs:complexType>
			<xs:all>
				<xs:element name="ChildCollection">
					<xs:complexType>
						<xs:sequence>
							<xs:element name="Child" type="Child" minOccurs="0" maxOccurs="unbounded"/>
						</xs:sequence>
					</xs:complexType>
				</xs:element>
				<xs:element name="Property1" type="xs:int" nillable="true"/>
			</xs:all>
			<xs:attribute name="Propertty2" type="xs:string" use="required"/>
		</xs:complexType>
	</xs:element>
```

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
</tbody></table>



## See also

[Variable](Variable.html)
[ClrTypeVariable](ClrTypeVariable.html)
[ComplexVariable](ComplexVariable.html)
[SimpleVariable](SimpleVariable.html)