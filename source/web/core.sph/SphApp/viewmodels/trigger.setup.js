﻿/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="~/Scripts/_task.js" />
define(["services/datacontext", "services/jsonimportexport", "plugins/router", objectbuilders.app, objectbuilders.system, objectbuilders.logger],
    function (context, eximp, router, app, system, logger) {

        let originalEntity = "",
            itemDeleted = false;

        const trigger = ko.observable(new bespoke.sph.domain.Trigger()),
            typeaheadEntity = ko.observable(),
            isBusy = ko.observable(false),
            id = ko.observable(),
            errors = ko.observableArray(),
            warnings = ko.observableArray(),
            actionOptions = ko.observableArray(),
            entities = ko.observableArray(),
            operationOptions = ko.observableArray(),
            operations = ko.observableArray(),
            activate = function (id2) {
                id(id2);

                var query = String.format("Id eq '{0}' ", id()),
                    actionOptionsTask = $.get("/api/triggers/actions"),
                    entitiesTask = context.getListAsync("EntityDefinition", "Id ne ''", "Name"),
                    loadOperationOptions = function (ent) {
                        return context.loadOneAsync("EntityDefinition", String.format("Name eq '{0}'", ent))
                            .then(function (ed) {
                                if (!ed) {
                                    return Task.fromResult([]);
                                }
                                return context.getListAsync("OperationEndpoint", "Entity eq '" + ko.unwrap(ed.Name) + "'", "Name");
                            })
                        .then(function (list) {
                            operationOptions(list);
                        });
                    };

                return $.when(entitiesTask, actionOptionsTask).then(function (list, actions) {
                    entities(list);
                    actionOptions(actions[0]);
                    return context.loadOneAsync("Trigger", query);
                })
                .then(function (trg) {
                    trigger(trg);
                    typeaheadEntity(trg.Entity());
                    window.typeaheadEntity = trg.Entity();
                    operations(trg.FiredOnOperations().split(","));
                    loadOperationOptions(trg.Entity());

                    trigger().Entity.subscribe(function (ent) {
                        typeaheadEntity(ent);
                        window.typeaheadEntity = ent;
                        loadOperationOptions(ent);
                    });
                });

            },
            attached = function () {

                originalEntity = ko.toJSON(trigger);
            },
            save = function () {
                trigger().FiredOnOperations(operations().join());
                var data = ko.mapping.toJSON(trigger);
                isBusy(true);

                return context.post(data, "/api/triggers")
                    .then(function () {
                        isBusy(false);
                        originalEntity = ko.toJSON(trigger);
                        logger.info("Your Trigger has been successfully saved");
                    });
            },
            canDeactivate = function () {
                if (itemDeleted) {
                    return true;
                }
                var tcs = new $.Deferred();
                if (originalEntity !== ko.toJSON(trigger)) {
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
                trigger().FiredOnOperations(operations().join());
                const data = ko.mapping.toJSON(trigger);
                isBusy(true);
                errors([]);
                warnings([]);

                return context.put(data, `/api/triggers/${ko.unwrap(trigger().Id)}/publish`)
                    .fail(e => {
                        logger.error(e);
                    })
                    .then(function (result) {
                        trigger().IsActive(true);
                        originalEntity = ko.toJSON(trigger);
                        if (!result.success) {
                            errors(result.errors);
                            warnings(result.warnings);
                            logger.error(result.message);
                        } else {
                            logger.info("Your trigger has been succesfully published, and will be added to the exchange shortly");
                        }
                    })
                    .done(() => {
                        isBusy(false);
                    });
            },
            depublishAsync = function () {
                trigger().FiredOnOperations(operations().join());
                var data = ko.mapping.toJSON(trigger);
                isBusy(true);

                return context.put(data, "/api/triggers/" + ko.unwrap(trigger().Id) + "/depublish")
                    .then(function () {
                        isBusy(false);
                        trigger().IsActive(false);
                        logger.info("Your trigger has been succesfully depublished, and will be removed from the exchange shortly");
                    });
            },
            remove = function () {
                var tcs = new $.Deferred();

                app.showMessage("Are you sure you want to remove this trigger, this action cannot be undone, and you'll need to restart your workers to complete this operation", "Rx Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.sendDelete("/api/triggers/" + ko.unwrap(trigger().Id))
                                .fail(tcs.reject)
                                .done(function (result) {
                                    isBusy(false);
                                    trigger().IsActive(false);
                                    itemDeleted = true;
                                    tcs.resolve(result);
                                    logger.info("Your trigger has been succesfully removed");
                                    window.location = "#trigger.list";
                                });
                        } else {
                            tcs.resolve(false);
                        }
                    });



                return tcs.promise();
            },

            exportJson = function () {
                return eximp.exportJson("trigger." + trigger().Id() + ".json", ko.mapping.toJSON(trigger));

            },

            importJson = function () {
                return eximp.importJson()
                    .done(function (json) {
                        var clone = context.toObservable(JSON.parse(json));
                        trigger(clone);
                        trigger().Id(0);

                    });
            },
            reload = function () {
                return app.showMessage("This discard all your changed, do you wish to continue", "Reload", ["Yes", "No"])
                     .done(function (dialogResult) {
                         if (dialogResult === "Yes") {
                             activate({ id: id() });
                         }
                     });
            };



        var vm = {
            isBusy: isBusy,
            activate: activate,
            canDeactivate: canDeactivate,
            attached: attached,
            trigger: trigger,
            errors: errors,
            warnings: warnings,
            actionOptions: actionOptions,
            operationOptions: operationOptions,
            operations: operations,
            entities: entities,
            typeaheadEntity: typeaheadEntity,
            toolbar: {
                saveCommand: save,
                reloadCommand: reload,
                removeCommand: remove,
                canExecuteRemoveCommand: ko.computed(function () {
                    return ko.unwrap(trigger().Id);
                }),
                exportCommand: exportJson,
                commands: ko.observableArray([
                    {
                        icon: "fa fa-upload",
                        caption: "import",
                        command: importJson
                    },
                    {
                        command: publishAsync,
                        caption: "Build",
                        icon: "fa fa-sign-in",
                        enable: ko.computed(function () {
                            return ko.unwrap(trigger().Id);
                        })
                    },
                    {
                        command: depublishAsync,
                        caption: "Depublish",
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return ko.unwrap(trigger().Id);
                        })
                    }
                ])
            }

        };

        return vm;

    });
