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

bespoke.sph.domain.WorkersConfigPartial = function (model, raw) {

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
            webServers = ko.observableArray(),
            computers = ko.observableArray(),
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
                    .done(function (result) {
                        const list = result.map(x => new bespoke.sph.domain.WorkersConfig(x));
                        configs(list);
                    });
                hub.server.getWebServerConfigs(env)
                    .done(function (result) {
                        const list = result.map(x => new bespoke.sph.domain.WebServerConfig(x));
                        webServers(list);
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
            },
            getComputers = function () {
                return hub.server.getComputers().done(computers);
            },
            save = function (parameters) {

            },
            removeAsync = function (parameters) {

            },
            exportPackage = function (parameters) {

            },
            importData = function (parameters) {

            },
            publishAsync = function (parameters) {

            },
            depublishAsync = function (parameters) {

            },
            ps1 = ko.observable(),
            executePowershell = function () {
                return hub.server.executePowershell(ko.unwrap(ps1))
                    .progress(function (d) {
                        console.log("progress :", d);
                    })
                    .done(function (r) {
                        console.info("Powershell result", r);
                    });
            },
            addWebServer = function () {
                const web = new bespoke.sph.domain.WebServerConfig({ Environment: ko.unwrap(selectedEnvironment) });
                require(["viewmodels/management.web.server.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.webServer(web);
                        getComputers().done(dialog.computers);
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {
                                    webServers.push(web);
                                    hub.server.saveWebServer(ko.toJS(web));
                                }
                            });
                    });
            };

        const vm = {
            addWebServer: addWebServer,
            webServers: webServers,
            ps1: ps1,
            executePowershell: executePowershell,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            configs: configs,
            computers: computers,
            selectEnvironment: selectEnvironment,
            selectedEnvironment: selectedEnvironment,
            addWorkersConfig: addWorkersConfig,
            editWorkersConfig: editWorkersConfig,
            getComputers: getComputers,
            environments: environments,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAsync,
                exportCommand: exportPackage,
                importCommand: importData,
                canExecuteRemoveCommand: function () {
                    return false;
                },
                commands: ko.observableArray([{
                    caption: "Clone",
                    icon: "fa fa-copy",
                    command: function () {
                    }
                },
                    {
                        command: publishAsync,
                        caption: "Publish",
                        icon: "fa fa-sign-in",
                        enable: ko.computed(function () {
                            return true;
                        })
                    },
                    {
                        command: depublishAsync,
                        caption: "Depublish",
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return true;
                        })
                    }])
            }
        };

        return vm;

    });
