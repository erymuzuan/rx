///<reference path="~/Scripts/adapter.api.g.js"/>
///<reference path="~/Scripts/__domain.js"/>
///<reference path="~/Scripts/require.js"/>
///<reference path="~/Scripts/knockout-3.4.0.debug.js"/>
///<reference path="~/Scripts/jquery-2.2.0.intellisense.js"/>

bespoke.sph.domain.api.RestApiOperationDefinition = function (model) {
    const v = new bespoke.sph.domain.api.OperationDefinition(model);
    v.busy = ko.observable(false);
    v.HarStoreId = ko.observable("");
    v.RequestHeadersSample = ko.observable("");
    v.RequestBodySample = ko.observable("");
    v.ResponseHeadersSample = ko.observable("");
    v.ResponseBodySample = ko.observable("");
    v.RequestContentType = ko.observable("");
    v.ResponseContentType = ko.observable("");
    v.BaseAddress = ko.observable("");
    v.HttpMethod = ko.observable("POST");
    v.Route = ko.observable(""); // for get turns into query string
    v["$type"] = "Bespoke.Sph.Integrations.Adapters.RestApiOperationDefinition, restapi.adapter";

    var context = require("services/datacontext");
    if (model && typeof model === "object") {
        for (let n in model) {
            if (model.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && "push" in v[n]) {
                    const values = model[n].$values || model[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) { return context.toObservable(ai); }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](model[n]);
                }
            }
        }
    }
    if (model && typeof model === "string") {
        v.WebId(model);
    }

    v.Name.subscribe(function (name) {
        if (!name) {
            return;
        }
        v.MethodName(name);
        const requestHeader = v.RequestMemberCollection().find(x => ko.unwrap(x.Name) === "Headers"),
            requestQueryString = v.RequestMemberCollection().find(x => ko.unwrap(x.Name) === "QueryStrings"),
            requestBody = v.RequestMemberCollection().find(x => ko.unwrap(x.Name) === "Body"),
            requestRoute = v.RequestMemberCollection().find(x => ko.unwrap(x.Name) === "RouteParameters"),
            responseHeader = v.ResponseMemberCollection().find(x => ko.unwrap(x.Name) === "Headers"),
            responseBody = v.ResponseMemberCollection().find(x => ko.unwrap(x.Name) === "Body");
        responseHeader.TypeName(`${name}ResponseHeader`);
        responseBody.TypeName(`${name}ResponseBody`);

        requestHeader.TypeName(`${name}RequestHeader`);
        requestBody.TypeName(`${name}RequestBody`);
        requestRoute.TypeName(`${name}Route`);
        if (requestQueryString)
            requestQueryString.TypeName(`${name}QueryString`);
    });
    return v;
};

bespoke.sph.domain.api.RequestWithoutBodyApiOperationDefinition = function (model) {
    const v = new bespoke.sph.domain.api.RestApiOperationDefinition(model);
    v.$type = "Bespoke.Sph.Integrations.Adapters.RequestWithoutBodyApiOperationDefinition, restapi.adapter";

    return v;

}

bespoke.sph.domain.Adapters.RestApiAdapter = function (model) {
    const v = new bespoke.sph.domain.api.Adapter(model);
    v.$type = "Bespoke.Sph.Integrations.Adapters.RestApiAdapter, restapi.adapter";
    v.BaseAddress = ko.observable(model.BaseAddress || "");
    v.AuthenticationType = ko.observable(model.AuthenticationType || "");
    v.SecurityHeaderCollection = ko.observableArray([]);
    v.DefaultValue = ko.observable(model.DefaultValue || {});


    var context = require("services/datacontext");
    if (model && typeof model === "object") {
        for (let n in model) {
            if (model.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && "push" in v[n]) {
                    const values = model[n].$values || model[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) { return context.toObservable(ai); }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](model[n]);
                }
            }
        }
    }

    v.Name.subscribe(function (name) {
        if (!ko.unwrap(v.FullName)) {
            v.FullName(name);
        }
    });

    return _(v).extend(new bespoke.sph.domain.FieldContainer(v, model));
}
bespoke.sph.domain.Adapters.QueryStringMember = function (model) {
    const v = new bespoke.sph.domain.SimpleMember(model);
    if (!ko.isObservable(v.FullName)) {
        v.FullName = ko.observable(model.FullName || "");
    }
    v.$type = "Bespoke.Sph.Integrations.Adapters.QueryStringMember, restapi.adapter";


    v.Name.subscribe(function (name) {
        if (!ko.unwrap(v.FullName)) {
            v.FullName(name);
        }
    });

    return _(v).extend(new bespoke.sph.domain.FieldContainer(v, model));
}

bespoke.sph.domain.Adapters.RouteParameterMember = function (model) {
    const v = new bespoke.sph.domain.SimpleMember(model);
    if (!ko.isObservable(v.FullName)) {
        v.FullName = ko.observable(model.FullName || "");
    }
    v.$type = "Bespoke.Sph.Integrations.Adapters.QueryStringMember, restapi.adapter";


    v.Name.subscribe(function (name) {
        if (!ko.unwrap(v.FullName)) {
            v.FullName(name);
        }
    });

    return _(v).extend(new bespoke.sph.domain.FieldContainer(v, model));
}


bespoke.sph.domain.Adapters.HttpHeaderMember = function (model) {
    const v = new bespoke.sph.domain.SimpleMember(model);
    if (!ko.isObservable(v.FullName)) {
        v.FullName = ko.observable(model.FullName || "");
    }
    v.$type = "Bespoke.Sph.Integrations.Adapters.HttpHeaderMember, restapi.adapter";


    v.Name.subscribe(function (name) {
        if (!ko.unwrap(v.FullName)) {
            v.FullName(name);
        }
    });


    return _(v).extend(new bespoke.sph.domain.FieldContainer(v, model));

}
