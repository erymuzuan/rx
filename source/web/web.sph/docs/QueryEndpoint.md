# Query Endpoint

`QueryEndpoint` allows you to create a `GET` HTTP API to your data source. This basically is just a simple HTTP endpoint that can be invoke with HTTP `GET` to return a list of result. Among the things that could be done with `QueryEndpoint` are

* Caching
* HTTP headers for `E-Tag` and `Last-Modified-Date`

## Fields selection
If you need to return all the fields, do not check any items in the tree, but if your `QueryEndpoint` needs to return just a number of specified fields, then you could select those fields.

**Note :** You're not allowed to select the root, or any parent of a `ComplexMember` or `ValueObjectMember`.


## General Properties

* Name - `Endpoint` name for easy identification
* Route - HTTP routing for your Endpoint, you have few options here
  1. Absolute route - If you prepend the route with a `~`, then it will be come absolute route, e.g. `~/api/patients/my-patients-list`, then it could be accessed via `http://<BaseUrl>/api/patients/my-patients-list`
  2. Named route - if you put a valid values `([a-z09-])`, then your endpoint will be `http://<BaseUrl>/api/<entity-plural>/<route-name>`, e.g. `http://www.sample.com/api/patients/my-patients`
  3. Empty route - When you leave the value blank, it will create an endpoint with the following url `http://<BaseUrl>/api/<entity-plural>`. **Note-** this option may create a conflict with your `ServiceContract` Odata endpoint.

## Filters

Create predicates to your queries, for example you want to create a `QueryEndpoint` that returns all female patients. Thus your `QueryEndpoint` should contains a filter for `Gender == "Female"`.
* Term is the field name you want to Filters
* Operator - how do you want it be evaluated
* Value - You have few options, you can refer to [Field](field.html)
