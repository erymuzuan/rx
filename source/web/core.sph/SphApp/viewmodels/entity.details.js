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


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app, "services/app"],
    function (context, logger, router, system, app, servicesApp) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            originalEntity = "",
            isBusy = ko.observable(false),
            errors = ko.observableArray(),
            triggers = ko.observableArray(),
            forms = ko.observableArray(),
            templateOptions = ko.observableArray(),
            views = ko.observableArray(),
            member = ko.observable(new bespoke.sph.domain.Member(system.guid())),
            activate = function (id) {

                context.getListAsync("ViewTemplate", "Id ne '0'", "Name")
                .done(templateOptions);

                member(new bespoke.sph.domain.Member("-"));

                var query = String.format("Id eq '{0}'", id);
                return context.loadOneAsync("EntityDefinition", query)
                    .then(function (b) {
                        if (!b) {
                            originalEntity = null;
                            return app.showMessage("Cannot find any EntityDefinition with id = " + id, "Not Found", ["OK"])
                            .done(function () {
                                return router.navigate("#dev.home");
                            });
                        }
                        entity(b);
                        window.typeaheadEntity = b.Name();
                        return Task.fromResult(true);
                    });

            },
            attached = function () {

                originalEntity = ko.toJSON(entity);
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
                            originalEntity = ko.toJSON(entity);
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
            canDeactivate = function () {
                var tcs = new $.Deferred();
                if (!originalEntity) {
                    return true;
                }
               


                if (originalEntity !== ko.toJSON(entity)) {
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

                var data = ko.mapping.toJSON(entity);
                isBusy(true);

                return context.post(data, "/entity-definition/publish")
                    .then(function (result) {

                        originalEntity = ko.toJSON(entity);
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
            },
            addTriggerAsync = function () {

                return servicesApp.showDialog("new.trigger.dialog", function (dialog) {
                    dialog.entity(entity().Name());
                })
                      .done(function (dialog, result) {
                          if (result === "OK") {
                              router.navigate("#trigger.setup/" + ko.unwrap(dialog.id));
                          }
                      });
            };


        var vm = {
            triggers: triggers,
            templateOptions: templateOptions,
            publishDashboard: publishDashboard,
            addTriggerAsync: addTriggerAsync,
            forms: forms,
            views: views,
            errors: errors,
            isBusy: isBusy,
            activate: activate,
            canDeactivate: canDeactivate,
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
                            var ent = ko.unwrap(entity);
                            if (!ent) return false;
                            return ko.unwrap(ent.Id);
                        })
                    },
                    {
                        command: depublishAsync,
                        caption: "Depublish",
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            var ent = ko.unwrap(entity);
                            if (!ent) return false;
                            return ko.unwrap(ent.Id) && ko.unwrap(ent.IsPublished);
                        })
                    }])
            }
        };

        return vm;

    });
