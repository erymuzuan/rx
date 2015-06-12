#Custom Form with Route
Route is the information that's available for the application that enables it to browse to a specific form, this is presented as the value in your `URL` and the `#`

For example `http://localhost:4436/sph#custom.form.list`, the value `custom.form.list` is called the `route`, Route can optionally contains parameter the term we called `RouteParameter`. So if you url looks like `http://localhost:4436/sph#/entity.details/patient` anything after '#' is a route up to the next, with additional parameters seperated with `/`. So in this case we have a route with a RouteParameter, and the parameter value is `patient`.

To create a route without parameter just simply enter the route name in Route field, so if you want a home page for everybody then the route value is `home-page`

To create a route with parameter, you need to specify the parameter in the route itself using `/:<parametername>`, so if you a custom for your `Patient`, then the route will be `patient-custom-form/:id`, In which your user can navigate to `#patient-custom-form/abc`, where `abc` is the value for `id`.

This will allow you to use the `id` value in your viewmodels
<pre>
var patient = ko.observable(),
    activate  = function(id){
        return context.loadOneAsync("Patient", String.format("Id eq '{0}'", id))
            .done(patient);
}
return {
    patient:patient,
    activate:activate
};
</pre>


