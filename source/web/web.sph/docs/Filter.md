#Filter
##Overview
[`Filter`](Filter.html) provide easy to use for composing [`QueryDsl`](QueryDsl.html). It's provide 3 basic building block. [`Term`](http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/query-dsl-term-query.html) points to the  property path in your [`EntityDefinition`](EntityDefinition.html), the reason it's called `Term` instead of `Path` is internally it will generate [`BooleanFilter`](BooleanFilter.html) to your ElasticSearch using `term` query. As such it's only good for string value with no analyzer specified on the member, or any other non string [`Member`](Member.html) where [`RangeQuery`](http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/query-dsl-range-query.html) is used insted. Spefifying a [`Member`](Member.html) to be analyzed breaking up terms making this query unpredictable. 
The other property is the [`Field`](Field.html) which you can specify using one of the built in [`Field`](Field.html), the only unique feature is the  ability to use [`JavascriptExpressionField`](JavascriptExpressionField.html) with only 1 value only for now `config.userName` to allow you to specify the current user.

These operators are not supported thoug.

* Not contains
* Not starts with
* Not ends with
 


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Term</td><td> - Term query define the path to your property</td></tr>
<tr><td>Operator</td><td> - Binary Operator </td></tr>
<tr><td>Field</td><td> - The field to compare to</td></tr>
</tbody></table>



## See also
[`Field`](Field.html)
[`ConstantField`](ConstantField.html)
[`Operator`](Operator.html)
[`FunctionField`](FunctionField.html)
[`JavascriptExpressionField`](JavascriptExpressionField.html)
