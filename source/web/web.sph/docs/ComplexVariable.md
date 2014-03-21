#ComplexVariable
##Overview
Let you define a `Variable` from your XSD schema that you had upload into your [`WorkflowDefinition`](WorkflowDefinition.html)


##Known limitiations

* No enumeration is supported
* Only the `All` model is supported for `element` or `complexType`
* List must be define in a special way, see how an `element` called `Parent` has a list for `Child`
<pre>
&lt;xs:element name=&quot;Parent&quot;&gt;
		&lt;xs:complexType&gt;
			&lt;xs:all&gt;
				&lt;xs:element name=&quot;ChildCollection&quot;&gt;
					&lt;xs:complexType&gt;
						&lt;xs:sequence&gt;
							&lt;xs:element name=&quot;Child&quot; type=&quot;Child&quot; minOccurs=&quot;0&quot; maxOccurs=&quot;unbounded&quot;/&gt;
						&lt;/xs:sequence&gt;
					&lt;/xs:complexType&gt;
				&lt;/xs:element&gt;
				&lt;xs:element name=&quot;Property1&quot; type=&quot;xs:int&quot; nillable=&quot;true&quot;/&gt;
			&lt;/xs:all&gt;
			&lt;xs:attribute name=&quot;Propertty2&quot; type=&quot;xs:string&quot; use=&quot;required&quot;/&gt;
		&lt;/xs:complexType&gt;
	&lt;/xs:element&gt;
</pre>

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