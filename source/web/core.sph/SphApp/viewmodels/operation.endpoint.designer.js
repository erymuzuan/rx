/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="~/Scripts/_task.js" />
define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.config, objectbuilders.app],
    function (context, logger, router, system, config, app) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            errors = ko.observableArray(),
            roles = ko.observableArray(),
            operation = ko.observable(new bespoke.sph.domain.OperationEndpoint()),
            isBusy = ko.observable(false),
            suggetRoute = function (enable) {
                if (!enable) {
                    return;
                }
                if (ko.unwrap(operation().Route)) {
                    return;
                }
                var route = "{id:guid}/actions/" + ko.unwrap(operation().Name);
                operation().Route(route.toLocaleLowerCase());
            },
            activate = function (id) {
                if (!roles().length) {
                    roles(config.allRoles);
                    roles.splice(0, 0, "Anonymous");
                    roles.splice(0, 0, "Everybody");
                }

                var query = String.format("Id eq '{0}'", id);
                return context.loadOneAsync("OperationEndpoint", query)
                    .then(function (o) {
                        operation(o);
                        o.IsHttpPut.subscribe(suggetRoute);
                        o.IsHttpPatch.subscribe(suggetRoute);
                        return context.loadOneAsync("EntityDefinition", "Name eq '" + ko.unwrap(o.Entity) + "'");
                    }).then(function (e) {
                        entity(e);
                        if (!ko.unwrap(operation().Resource)) {
                            operation().Resource(ko.unwrap(e.Plural).toLowerCase());
                        }
                    });

            },
            attached = function () {

            },
            save = function () {
                if (!document.getElementById("entity-operation-detail-form").checkValidity()) {
                    logger.error("Please correct all the validations errors");
                    return Task.fromResult(0);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(operation);
                isBusy(true);

                context.post(data, "/api/operation-endpoints")
                    .then(function (result) {
                        tcs.resolve(true);
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            entity().Id(result.id);
                            errors.removeAll();
                        } else {
                            errors(result.Errors);
                            logger.error("There are errors in your entity, !!!");
                        }
                    });
                return tcs.promise();
            },
        publishAsync = function () {

            var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(operation);
            isBusy(true);

            context.put(data, "/api/operation-endpoints/" + ko.unwrap(operation().Id) + "/publish")
                .then(function (result) {
                    isBusy(false);
                    if (result.success) {
                        logger.info(result.message);
                        entity().Id(result.id);
                        errors.removeAll();
                    } else {

                        errors(result.Errors);
                        logger.error("There are errors in your schema, !!!");
                    }
                    tcs.resolve(result);
                    entity().IsPublished(true);
                });
            return tcs.promise();
        },
            removeAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(operation);
                isBusy(true);
                app.showMessage("Are you sure you want to permanently remove this Endpoint, this action cannot be undone and will also remove related forms, views, triggers, reports and business rules", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.send(data, "/api/operation-endpoints/" + operation().Id(), "DELETE")
                                .then(function (result) {
                                    isBusy(false);
                                    if (result.success) {
                                        logger.info(result.message);
                                        setTimeout(function () {
                                            window.location = "/sph#entity.details/" + entity().Id();
                                        }, 2000);
                                    }
                                    tcs.resolve(result);
                                });
                        } else {

                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();
            },
            curl = function () {

                require(["viewmodels/curl.command.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.endpoint(operation());
                    app2.showDialog(dialog);
                });

                return Task.fromResult(0);
            };

        var vm = {
            errors: errors,
            roles: roles,
            entity: entity,
            operation: operation,
            isBusy: isBusy,
            config: config,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAsync,
                commands: ko.observableArray([
                            {
                                command: publishAsync,
                                caption: "Publish",
                                icon: "fa fa-sign-in",
                                enable: ko.computed(function () {
                                    return entity().Id() && entity().Id() !== "0";
                                })
                            },
                            {
                                caption: "Clone",
                                icon: "fa fa-copy",
                                command: function () {
                                    operation().Name(operation().Name() + " Copy (1)");
                                    operation().Id("0");
                                    return Task.fromResult(0);
                                }
                            },
                            {
                                caption: "CURL",
                                icon: "fa fa-paper-plane",
                                command: curl
                            }])
            }
        };

        return vm;

    });
