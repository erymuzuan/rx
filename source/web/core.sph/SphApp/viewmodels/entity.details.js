/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../objectbuilders.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.system, objectbuilders.app],
    function (context, logger, router, system, app) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            isBusy = ko.observable(false),
            errors = ko.observableArray(),
            forms = ko.observableArray(),
            views = ko.observableArray(),
            member = ko.observable(new bespoke.sph.domain.Member(system.guid())),
            activate = function (entityid) {
                var id = parseInt(entityid);
                if (isNaN(id)) {
                    var query = String.format("Id eq '{0}'", entityid),
                        tcs = new $.Deferred();
                    context.loadOneAsync("EntityDefinition", query)
                        .done(function (b) {
                            entity(b);
                            window.typeaheadEntity = b.Name();
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                }
                var ed = new bespoke.sph.domain.EntityDefinition(system.guid());
                ed.Name.subscribe(function (name) {
                    if (!entity().Plural()) {
                        $.get('/entity-definition/plural/' + name, function (v) {
                            entity().Plural(v);
                        });
                    }
                    window.typeaheadEntity = name;
                });
                ed.IconStoreId("sph-img-document");

                entity(ed);
                return Task.fromResult(true);

            },
            attached = function () {

            },
            save = function () {
                if (!document.getElementById('entity-form').checkValidity()) {
                    logger.error("Please correct all the validations erros");
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
                            if (!entity().Id()) {
                                //reload forms and views 
                                context.loadAsync("EntityForm", "EntityDefinitionId eq '" + result.id + "'")
                                    .done(function (lo) {
                                        forms(lo.itemCollection);
                                    });
                                context.loadAsync("EntityView", "EntityDefinitionId eq '" + result.id + "'")
                                    .done(function (lo) {
                                        views(lo.itemCollection);
                                    });

                            }
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
            },
            depublishAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);
                isBusy(true);

                context.post(data, "/entity-definition/depublish")
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
                    });
                return tcs.promise();
            },
            removeAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);
                isBusy(true);
                app.showMessage("Are you sure you want to permanently remove this entity definition, this action cannot be undone and will also remove related forms, views, triggers, reports and business rules", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.send(data, "/entity-definition/" + entity().Id(), "DELETE")
                                .then(function (result) {
                                    isBusy(false);
                                    if (result.success) {
                                        logger.info(result.message);
                                        setTimeout(function () {
                                            window.location = "/sph#entities.list";
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


        var vm = {
            forms: forms,
            views: views,
            errors: errors,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            entity: entity,
            member: member,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAsync,
                canExecuteRemoveCommand: function () {
                    return false;
                },
                commands: ko.observableArray([{
                    caption: 'Clone',
                    icon: 'fa fa-copy',
                    command: function () {
                        entity().Name(entity().Name() + ' Copy (1)');
                        entity().Plural(null);
                        entity().Id('');
                        forms([]);
                        views([]);
                        return Task.fromResult(0);
                    }
                },
                    {
                        command: publishAsync,
                        caption: 'Publish',
                        icon: "fa fa-sign-in",
                        enable: ko.computed(function () {
                            return entity().Id();
                        })
                    },
                    {
                        command: depublishAsync,
                        caption: 'Depublish',
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return entity().Id() && entity().IsPublished();
                        })
                    }])
            }
        };

        return vm;

    });
