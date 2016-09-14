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

    return v;
};



bespoke.sph.domain.api.RequestWithoutBodyApiOperationDefinition = function (model) {
    const v = new bespoke.sph.domain.api.RestApiOperationDefinition(model);
    v.$type = "Bespoke.Sph.Integrations.Adapters.RequestWithoutBodyApiOperationDefinition, restapi.adapter";

    return v;

}