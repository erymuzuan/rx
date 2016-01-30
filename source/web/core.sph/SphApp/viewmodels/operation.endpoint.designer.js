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
define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.config],
    function (context, logger, router, system, config) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            errors = ko.observableArray(),
            roles = ko.observableArray(),
            operation = ko.observable(new bespoke.sph.domain.OperationEndpoint()),
            isBusy = ko.observable(false),
            activate = function (id) {
                if (!roles().length) {
                    roles(config.allRoles);
                    roles.splice(0, 0, "Anonymous");
                    roles.splice(0, 0, "Everybody");
                }

                var query = String.format("Id eq '{0}'", id),
                    tcs = new $.Deferred();
                context.loadOneAsync("OperationEndpoint", query)
                    .done(function (o) {
                        operation(o);
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            attached = function () {

            },
            save = function () {
                if (!document.getElementById("entity-operation-detail-form").checkValidity()) {
                    logger.error("Please correct all the validations errors");
                    return Task.fromResult(0);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);
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
                data = ko.mapping.toJSON(entity);
            isBusy(true);

            context.post(data, "/api/operation-endpoints" + ko.unwrap(operation().Id) + "/publish")
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
                commands: ko.observableArray([
                            {
                                command: publishAsync,
                                caption: "Publish",
                                icon: "fa fa-sign-in",
                                enable: ko.computed(function () {
                                    return entity().Id() && entity().Id() !== "0";
                                })
                            }])
            }
        };

        return vm;

    });
