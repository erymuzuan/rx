///<reference path="~/Scripts/__domain.js"/>
///<reference path="~/Scripts/require.js"/>
///<reference path="~/Scripts/knockout-3.4.0.debug.js"/>
///<reference path="~/Scripts/jquery-2.2.0.intellisense.js"/>

bespoke.sph.domain.Adapters = bespoke.sph.domain.Adapters || {};
bespoke.sph.domain.Adapters.HttpHeader = function(model) {
    model = model || {};
    return {
        Key: ko.observable( model.Key || ""),
        Value : ko.observable(model.Value || "")
    }
}
bespoke.sph.domain.Adapters.RestApiReceiveLocation = function (model) {
    const v = new bespoke.sph.domain.ReceiveLocation(model);
    v.Id = ko.observable(null);
    v.BufferAllRows = ko.observable(false);
    v.RejectPartial = ko.observable(false);
    v.InboundMapping = ko.observable("");
    v.InboundType = ko.observable("");
    v.ContentType = ko.observable("");
    v.BaseAddress = ko.observable("");
    v.Method = ko.observable("POST");
    v.Route = ko.observable(""); // for get turns into query string
    v.InProcess = ko.observable(true);
    v.Headers = ko.observableArray(),
    v["$type"] = "Bespoke.Sph.Integrations.Adapters.RestApiReceiveLocation, restapi.adapter";

    v.InProcess.subscribe(function(rx) {
        if (rx) {
            v.SubmitEndpoint("-");
            v.SubmitMethod("-");
        } else {
            v.SubmitEndpoint("");
            v.SubmitMethod("");
        }
    });

    if (v.InProcess()) {
        v.SubmitEndpoint("-");
        v.SubmitMethod("-");
    }

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

    if (bespoke.sph.domain.RestApiReceiveLocationPartial) {
        return _(v).extend(new bespoke.sph.domain.FolderReceiveLocationPartial(v, model));
    }
    return v;
};