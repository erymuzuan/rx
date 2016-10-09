/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/jquery.signalR-2.2.0.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/rx.management.g.js" />

bespoke.sph.domain.WorkersConfigPartial = function(model, raw) {

    return {
        ChangedDate: ko.observable(raw.ChangedDate)
    };
};

define(["services/datacontext", "services/logger", "plugins/router", "schemas/rx.management.g"],
    function (context, logger, router) {

        let connection = null,
            hub = null;

        const isBusy = ko.observable(false),
            configs = ko.observableArray(),
            environments = ko.observableArray(),
            selectedEnvironment = ko.observable(),
            activate = function () {
                return $.getScript("/signalr/js")
                    .then(function () {

                        connection = $.connection.hub;
                        hub = $.connection.deploymentHub;

                        return connection.start();
                    });
            },
            attached = function (view) {
                hub.server.getDeploymentEnvironments()
                    .done(environments);
            },
            selectEnvironment = function (env) {
                selectedEnvironment(env);
                hub.server.getConfigs(env)
                    .done(function(result) {
                        const list = result.map(x => new bespoke.sph.domain.WorkersConfig(x));
                        configs(list);
                    });
            },
            addWorkersConfig = function () {
                const clone = new bespoke.sph.domain.WorkersConfig({ Environment: ko.unwrap(selectedEnvironment) });
                require(["viewmodels/worker.config.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.config(clone);
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {
                                    configs.push(clone);
                                    hub.server.addWorkersConfig(ko.toJS(dialog.config));
                                }
                            });
                    });
            },
            editWorkersConfig = function (wc) {
                const clone = context.clone(wc);
                require(["viewmodels/worker.config.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.config(clone);
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {


                                }
                            });
                    });
            };

        const vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            configs: configs,
            selectEnvironment: selectEnvironment,
            selectedEnvironment: selectedEnvironment,
            addWorkersConfig: addWorkersConfig,
            editWorkersConfig: editWorkersConfig,
            environments: environments
        };

        return vm;

    });
