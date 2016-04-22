# Pending Tasks in Workflow

When you have a workflow which contains `ReceiveActivity`, you might want to retrive the list of the workflow instances which is currently waiting for the `ReceiveActivity` to be invoked.

Let's take a simple `ReceiveActivity`

```
GET /wf/<<Workflow route>>/v<<version>>/<<ReceiveActivity WebId>>/pendingtasks/<<variablePath?>>/<<value?>> HTTP/1.1

Authorization: Bearer <<Your API token>>
Cache-Control: no-cache

```

1. Your workflow controller route is likely is the `Id`
2. The version number for your `WorkflowDefinition`
3. ReceiveActivity WebId, you can copy this value from the designer
3. Optional : the path to variable
4. Optional : the value to the variable
5. Your valid API token

Let see a sample request using POSTMAN

```
GET /wf/permohonan-kuarters/v1/fd27e0bc-8ac1-45d1-d9f1-b0b377d92017/pendingtasks/Permohonan.NoKp/002 HTTP/1.1
Host: localhost:4436
Authorization: Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImRldmVsb3BlcnMiXSwiZW1haWwiOjE0NjQzMDcyMDAsInN1YiI6ImY0MmE1NGVmLWU0YWYtNDQ3Ny05NDRjLWQxZDQ4MmY1ZGFkZCIsIm5iZiI6MTQ3NzEwNzU4NiwiaWF0IjoxNDYxMjk2Mzg2LCJleHAiOjE0NjQzMDcyMDAsImF1ZCI6IkRldlYxIn0.dZVdTc2xT54BHT3cFDamP8uLqiGn2_TCt0Wv-h6AHCs
Cache-Control: no-cache
Postman-Token: ee7f2412-924d-3d1e-141d-1e4a99f3a1d0

```


Calling this REST endpoint will returns you the list of instances `Id`, that met your criteria which currently waiting for the `ReceiveActivity`.

```javascript
[
  "id" :"3e9ea479-f244-4527-b11f-049596879e08"
  {
    "href":  "/wf/permohonan-kuarters/v1/3e9ea479-f244-4527-b11f-049596879e08",
    "rel" : "self"   
  }
]
```
