﻿/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
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


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app],
    function (context, logger, router, system, app) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            isBusy = ko.observable(false),
            errors = ko.observableArray(),
            triggers = ko.observableArray(),
            forms = ko.observableArray(),
            templateOptions = ko.observableArray(),
            views = ko.observableArray(),
            member = ko.observable(new bespoke.sph.domain.Member(system.guid())),
            activate = function (entityid) {
                var id = parseInt(entityid);

                context.getListAsync("ViewTemplate", "Id ne '0'", "Name")
                .done(templateOptions);

                member(new bespoke.sph.domain.Member("-"));

                if (isNaN(id)) {
                    var query = String.format("Id eq '{0}'", entityid);
                    return context.loadOneAsync("EntityDefinition", query)
                        .done(function (b) {
                            entity(b);
                            window.typeaheadEntity = b.Name();
                        });

                }
                var ed = new bespoke.sph.domain.EntityDefinition(system.guid());
                ed.Name.subscribe(function (name) {
                    if (!entity().Plural()) {
                        $.get("/entity-definition/plural/" + name, function (v) {
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
                if (entity().Id() === "0") {
                    //TODO : should do the modal
                }
                var setDesignerHeight = function () {
                    if ($("#schema-tree-panel").length === 0) {
                        return;
                    }

                    var dev = $("#developers-log-panel").height(),
                        top = $("#schema-tree-panel").offset().top,
                        height = dev + top;
                    $("#schema-tree-panel").css("max-height", $(window).height() - height);

                };
                $("#developers-log-panel-collapse,#developers-log-panel-expand").on("click", setDesignerHeight);
                setDesignerHeight();
            },
            publishDashboard = function () {
                var data = ko.mapping.toJSON(entity);
                return context.post(data, "/entity-definition/publish-dashboard");
            },
            save = function () {
                if (!document.getElementById("entity-form").checkValidity()) {
                    logger.error("Please correct all the validations errors");
                    return Task.fromResult(0);
                }
                var data = ko.mapping.toJSON(entity);
                isBusy(true);

                return context.post(data, "/entity-definition")
                    .then(function (result) {
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
                            errors(result.errors);
                            logger.error("There are errors in your entity, !!!");
                        }
                    });
            },
            publishAsync = function () {

                var data = ko.mapping.toJSON(entity);
                isBusy(true);

                return context.post(data, "/entity-definition/publish")
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
                        entity().IsPublished(true);
                    });
            },
            depublishAsync = function () {

                var data = ko.mapping.toJSON(entity);
                isBusy(true);

                return context.post(data, "/entity-definition/depublish")
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
                    });
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
            },
            exportPackage = function () {
                var tcs = new $.Deferred();
                require(["viewmodels/entity.export.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.entity(entity());

                    app2.showDialog(dialog)
                        .done(tcs.resolve);
                });

                return tcs.promise();

            },
            importData = function () {
                var tcs = new $.Deferred();
                require(["viewmodels/entity.import.data.dialog", "durandal/app"], function (dialog, app2) {

                    app2.showDialog(dialog)
                        .done(tcs.resolve);
                });

                return tcs.promise();
            };


        var vm = {
            triggers: triggers,
            templateOptions: templateOptions,
            publishDashboard: publishDashboard,
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
                exportCommand: exportPackage,
                importCommand: importData,
                canExecuteRemoveCommand: function () {
                    return false;
                },
                commands: ko.observableArray([{
                    caption: "Clone",
                    icon: "fa fa-copy",
                    command: function () {
                        entity().Name(entity().Name() + " Copy (1)");
                        entity().Plural(null);
                        entity().Id("0");
                        forms([]);
                        views([]);
                        return Task.fromResult(0);
                    }
                },
                    {
                        command: publishAsync,
                        caption: "Publish",
                        icon: "fa fa-sign-in",
                        enable: ko.computed(function () {
                            return entity().Id();
                        })
                    },
                    {
                        command: depublishAsync,
                        caption: "Depublish",
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return entity().Id() && entity().IsPublished();
                        })
                    }])
            }
        };

        return vm;

    });
