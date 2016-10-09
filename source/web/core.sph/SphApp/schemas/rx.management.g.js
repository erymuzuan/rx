
/// <reference path="~/scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />
/// <reference path="~/Scripts/require.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.WorkersConfig = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.WorkersConfig, domain.sph",
        Id: ko.observable("0"),
        Name: ko.observable(""),
        Description: ko.observable(""),
        IsEnabled: ko.observable(false),
        Environment: ko.observable(""),
        SubscriberConfigs: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    },
    context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) { return context.toObservable(ai); }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkersConfigPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkersConfigPartial(model, optionOrWebid));
    }
    return model;
};



bespoke.sph.domain.SubscriberConfig = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.SubscriberConfig, domain.sph",
        QueueName: ko.observable(""),
        FullName: ko.observable(""),
        Assembly: ko.observable(""),
        TypeName: ko.observable(""),
        InstancesCount: ko.observable(),
        PrefetchCount: ko.observable(),
        Priority: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    },
    context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) { return context.toObservable(ai); }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SubscriberConfigPartial) {
        return _(model).extend(new bespoke.sph.domain.SubscriberConfigPartial(model, optionOrWebid));
    }
    return model;
};

