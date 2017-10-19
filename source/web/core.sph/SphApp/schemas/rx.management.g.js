/// <reference path="~/scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />
/// <reference path="~/Scripts/require.js" />

const bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.WorkersConfig = function (optionOrWebid) {

    const model = {
            "$type": "Bespoke.Sph.Domain.WorkersConfig, domain.sph",
            Id: ko.observable("0"),
            Name: ko.observable(""),
            Description: ko.observable(""),
            IsEnabled: ko.observable(false),
            Environment: ko.observable(""),
            HomeDirectory: ko.observable(""),
            SubscriberConfigs: ko.observableArray([]),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (let n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    const values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
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

    const model = {
            "$type": "Bespoke.Sph.Domain.SubscriberConfig, domain.sph",
            QueueName: ko.observable(""),
            FullName: ko.observable(""),
            Assembly: ko.observable(""),
            TypeName: ko.observable(""),
            DelayProcessedOnAcceptedEmailTemplate: ko.observable(""),
            DelayProcessedOnPersistedEmailTemplate: ko.observable(""),
            Entity: ko.observable(""),
            InstancesCount: ko.observable(),
            PrefetchCount: ko.observable(),
            Priority: ko.observable(),
            MaxInstances: ko.observable(),
            MinInstances: ko.observable(),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (let n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    const values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
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


bespoke.sph.domain.WebServerConfig = function (optionOrWebid) {

    const model = {
            "$type": "Bespoke.Sph.Domain.WebServerConfig, domain.sph",
            Id: ko.observable("0"),
            Name: ko.observable(""),
            Environment: ko.observable(""),
            ComputerName: ko.observable(""),
            HomeDirectory: ko.observable(""),
            ApplicationPool: ko.observable(""),
            ApplicationPoolCredential: ko.observable(""),
            IsConsole: ko.observable(false),
            EnableRemoteManagement: ko.observable(false),
            HostNameBinding: ko.observable(""),
            PortBinding: ko.observable(0),
            UseSsl: ko.observable(false),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (let n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    const values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
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


    if (bespoke.sph.domain.WebServerConfigPartial) {
        return _(model).extend(new bespoke.sph.domain.WebServerConfigPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.WorkerServerConfig = function (optionOrWebid) {

    const model = {
            "$type": "Bespoke.Sph.Domain.WorkerServerConfig, domain.sph",
            Id: ko.observable("0"),
            Name: ko.observable(""),
            Environment: ko.observable(""),
            ComputerName: ko.observable(""),
            HomeDirectory: ko.observable(""),
            ApplicationPool: ko.observable(""),
            ApplicationPoolCredential: ko.observable(""),
            IsConsole: ko.observable(false),
            EnableRemoteManagement: ko.observable(false),
            HostNameBinding: ko.observable(""),
            PortBinding: ko.observable(0),
            UseSsl: ko.observable(false),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (let n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    const values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
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


    if (bespoke.sph.domain.WorkerServerConfigPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkerServerConfigPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.BrokerConfig = function (optionOrWebid) {

    const model = {
            "$type": "Bespoke.Sph.Domain.BrokerConfig, domain.sph",
            Name: ko.observable(""),
            ComputerName: ko.observable(""),
            UserName: ko.observable(""),
            Password: ko.observable(""),
            Environment: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (let n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    const values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
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


    if (bespoke.sph.domain.BrokerConfigPartial) {
        return _(model).extend(new bespoke.sph.domain.BrokerConfigPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.ElasticsearchConfig = function (optionOrWebid) {

    const model = {
            "$type": "Bespoke.Sph.Domain.ElasticsearchConfig, domain.sph",
            Name: ko.observable(""),
            Environment: ko.observable(""),
            Port: ko.observable(0),
            ComputerName: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (let n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    const values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
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


    if (bespoke.sph.domain.ElasticsearchConfigPartial) {
        return _(model).extend(new bespoke.sph.domain.ElasticsearchConfigPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.DscConfig = function (optionOrWebid) {

    const model = {
            "$type": "Bespoke.Sph.Domain.DscConfig, domain.sph",
            Id: ko.observable("0"),
            Name: ko.observable(""),
            Environment: ko.observable(""),
            Configs: ko.observableArray([]),
            WebServers: ko.observableArray([]),
            WorkerServers: ko.observableArray([]),
            ElasticsearchServers: ko.observableArray([]),
            BrokerServers: ko.observableArray([]),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (let n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    const values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
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


    if (bespoke.sph.domain.DscConfigPartial) {
        return _(model).extend(new bespoke.sph.domain.DscConfigPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.ConfigData = function (optionOrWebid) {

    const model = {
            "$type": "Bespoke.Sph.Domain.ConfigData, domain.sph",
            Key: ko.observable(""),
            Value: ko.observable(""),
            IsRequired: ko.observable(false),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (let n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    const values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
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


    if (bespoke.sph.domain.ConfigDataPartial) {
        return _(model).extend(new bespoke.sph.domain.ConfigDataPartial(model, optionOrWebid));
    }
    return model;
};

        