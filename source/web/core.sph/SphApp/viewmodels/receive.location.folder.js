/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {
        let originalEntity = "";
        const isBusy = ko.observable(false),
            location = ko.observable(new bespoke.sph.domain.FolderReceiveLocation()),
            port = ko.observable(new bespoke.sph.domain.ReceivePort()),
            endpointOptions = ko.observableArray(),
            errors = ko.observableArray(),
            canActivate = function (id) {
                const tcs = new $.Deferred();
                context.loadOneAsync("ReceiveLocation", `Id eq '${id}'`)
                    .done(function (loc) {
                        if (!loc) {

                            app.showMessage(`Cannot find any Receive Location with id = ${id}`, "Not Found", ["OK"])
                            .done(function () {
                                tcs.resolve(false);
                            });
                            return;
                        }
                        originalEntity = ko.mapping.toJS(loc);
                        tcs.resolve(true);
                        location(loc);
                    });

                return tcs.promise();
            },
            activate = function (id) {
                return context.loadOneAsync("ReceiveLocation", `Id eq '${id}'`)
                 .then(function (loc) {
                     location(loc);
                     return context.loadOneAsync("ReceivePort", `Id eq '${ko.unwrap(loc.ReceivePort)}'`);
                 })
                .then(function (p) {
                    port(p);
                    return context.loadAsync("OperationEndpoint", `Entity eq '${ko.unwrap(p.Entity)}'`);
                })
                  .then(lo => endpointOptions(lo.itemCollection));
            },
            attached = function () {

            },
            save = function () {
                const data = ko.mapping.toJSON(location);
                isBusy(true);

                return context.post(data, "/receive-locations")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            originalEntity = ko.toJSON(port);
                            logger.info(result.message);
                            errors.removeAll();
                        } else {
                            errors(result.errors);
                            logger.error("There are errors in your entity, !!!");
                        }
                    });
            },
            canDeactivate = function () {
                var tcs = new $.Deferred();
                if (!originalEntity) {
                    return true;
                }

                if (originalEntity !== ko.toJSON(port)) {
                    app.showMessage("Save change to the item", "Rx Developer", ["Yes", "No", "Cancel"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {
                                save().done(function () {
                                    tcs.resolve(true);
                                });
                            }
                            if (dialogResult === "No") {
                                tcs.resolve(true);
                            }
                            if (dialogResult === "Cancel") {
                                tcs.resolve(false);
                            }

                        });
                } else {
                    return true;
                }
                return tcs.promise();
            },
            publishAsync = function () {
                const data = ko.mapping.toJSON(location);
                isBusy(true);

                return context.post(data, `/receive-locations/${ko.unwrap(location().Id)}/publish`)
                    .then(function (result) {

                        originalEntity = ko.toJSON(location);
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your receive location, !!!");
                        }
                    });
            },
            validateAsync = function () {
                const data = ko.mapping.toJSON(location);
                isBusy(true);

                return context.post(data, `/receive-locations/${ko.unwrap(location().Id)}/validate`)
                    .then(function (result) {

                        originalEntity = ko.toJSON(location);
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your receive location, !!!");
                        }
                    });

            },
            removeAsync = function () {
                var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(location);
                isBusy(true);
                app.showMessage("Are you sure you want to permanently remove this Receive Location?", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.send(data, `/receive-locations/${location().Id()}`, "DELETE")
                                .then(function (result) {
                                    isBusy(false);
                                    if (result.success) {
                                        logger.info(result.message);
                                        setTimeout(function () {
                                            window.location = "/sph#dev.home";
                                        }, 2000);
                                    } else {

                                        errors(result.Errors);
                                        logger.error("There are errors in your schema, !!!");
                                    }
                                    tcs.resolve(result);
                                });
                        } else {

                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();
            };

        const vm = {
            canActivate: canActivate,
            activate: activate,
            canDeactivate: canDeactivate,
            endpointOptions: endpointOptions,
            location: location,
            isBusy: isBusy,
            attached: attached,

            toolbar: {
                saveCommand: save,
                removeCommand: removeAsync,
                canExecuteRemoveCommand: function () {
                    return false;
                },
                commands: ko.observableArray([
                    {
                        command: publishAsync,
                        caption: "Publish",
                        icon: "fa fa-sign-in"
                    },
                    {
                        command: validateAsync,
                        caption: "Validate",
                        icon: "fa fa-check"
                    }])
            }
        };

        return vm;

    });
