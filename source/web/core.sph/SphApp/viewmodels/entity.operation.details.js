/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.system, objectbuilders.config],
    function (context, logger, router, system, config) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            errors = ko.observableArray(),
            roles = ko.observableArray(),
            operation = ko.observable(new bespoke.sph.domain.EntityOperation()),
            isBusy = ko.observable(false),
            activate = function (eid, name) {


                if (!roles().length) {
                    roles(config.allRoles);
                    roles.splice(0, 0, 'Anonymous');
                    roles.splice(0, 0, 'Everybody');
                }

                var query = String.format("Id eq '{0}'", eid),
                    tcs = new $.Deferred();
                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        entity(b);
                        var o = _(b.EntityOperationCollection()).find(function (v) {
                            return v.Name() === name;
                        });
                        if (!o) {
                            o = new bespoke.sph.domain.EntityOperation({
                                WebId: system.guid(),
                                Name: name
                            });
                            entity().EntityOperationCollection.push(o);
                        }
                        operation(o);
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            attached = function (view) {

            },
            save = function () {
                if (!document.getElementById('entity-operation-detail-form').checkValidity()) {
                    logger.error("Please correct all the validations errors");
                    return Task.fromResult(0);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);
                isBusy(true);

                context.post(data, "/entity-definition")
                    .then(function (result) {
                        tcs.resolve(true);
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            entity().Id(result.id);
                            errors.removeAll();
                            setTimeout(function () {
                                router.navigate('/entity.details/' + entity().Id());
                            }, 1000);                            
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

            context.post(data, "/entity-definition/publish")
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
                                caption: 'Publish',
                                icon: "fa fa-sign-in",
                                enable: ko.computed(function () {
                                    return entity().Id() && entity().Id() !== "0";
                                })
                            }])
            }
        };

        return vm;

    });
