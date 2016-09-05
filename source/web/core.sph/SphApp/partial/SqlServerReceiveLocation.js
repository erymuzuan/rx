///<reference path="../schemas/form.designer.g.js"/>
///<reference path="~/Scripts/require.js"/>
///<reference path="~/Scripts/knockout-3.4.0.debug.js"/>
///<reference path="~/Scripts/jquery-2.2.0.intellisense.js"/>


bespoke.sph.domain.Adapters = bespoke.sph.domain.Adapters || {};
bespoke.sph.domain.Adapters.Polling = function(model) {
    model = model || {};
    return {
        Interval: ko.observable( model.Interval || 5 * 60 * 1000), // 5 minutes
        Query : ko.observable(model.Query || "")
    }
}
bespoke.sph.domain.Adapters.SqlServerReceiveLocation = function (model) {
    const v = new bespoke.sph.domain.ReceiveLocation(model);
    v.Id = ko.observable(null);
    v.Server = ko.observable("(localdb)\\Projects");
    v.Database = ko.observable(null);
    v.Trusted = ko.observable(true);
    v.UseQueryNotification = ko.observable(true);
    v.UserId = ko.observable(null);
    v.Password = ko.observable(null);
    v.Query = ko.observable("");
    v.Polling = ko.observable(new bespoke.sph.domain.Adapters.Polling()),
    v["$type"] = "Bespoke.Sph.Integrations.Adapters.SqlServerReceiveLocation, sqlserver.adapter";

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

    if (bespoke.sph.domain.SqlServerReceiveLocationPartial) {
        return _(v).extend(new bespoke.sph.domain.FolderReceiveLocationPartial(v, model));
    }
    return v;
};