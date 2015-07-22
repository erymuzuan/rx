/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../objectbuilders.js" />

define(["services/datacontext", "services/jsonimportexport", "plugins/router", objectbuilders.app, objectbuilders.system, objectbuilders.logger],
    function (context, eximp, router, app, system, logger) {

        var trigger = ko.observable(new bespoke.sph.domain.Trigger()),
            originalEntity = "",
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
                    triggerTask = context.loadOneAsync("Trigger", query),
                    actionOptionsTask = $.get("/sph/trigger/actions"),
                    entitiesTask = context.getListAsync("EntityDefinition", "Id ne ''", "Name"),
                    loadOperationOptions = function (ent) {

                        context.loadOneAsync("EntityDefinition", String.format("Name eq '{0}'", ent))
                            .done(function (ed) {
                                if (!ed) {
                                    return;
                                }
                                operationOptions(_(ed.EntityOperationCollection()).map(function (v) { return v.Name(); }));


                            });
                    };

                return $.when(triggerTask, entitiesTask, actionOptionsTask).done(function (t, list, actions) {
                    entities(list);
                    actionOptions(actions[0]);
                    if (t) {
                        originalEntity = ko.toJSON(t);
                        trigger(t);
                        typeaheadEntity(t.Entity());
                        window.typeaheadEntity = t.Entity();
                        operations(t.FiredOnOperations().split(","));
                        loadOperationOptions(t.Entity());
                    } else {
                        trigger(new bespoke.sph.domain.Trigger(system.guid()));
                        operations.removeAll();
                    }
                    trigger().Entity.subscribe(function (ent) {
                        typeaheadEntity(ent);
                        window.typeaheadEntity = ent;
                        loadOperationOptions(ent);
                    });
                });

            },
            attached = function () {

            },
            save = function () {
                trigger().FiredOnOperations(operations().join());
                var data = ko.mapping.toJSON(trigger);
                isBusy(true);

                return context.post(data, "/Trigger/Save")
                    .then(function (result) {
                        isBusy(false);
                        trigger().Id(result);
                        originalEntity = ko.toJSON(trigger);
                    });
            },
            canDeactivate = function () {
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
                var data = ko.mapping.toJSON(trigger);
                isBusy(true);

                return context.post(data, "/Trigger/Publish")
                    .then(function (result) {
                        isBusy(false);
                        trigger().Id(result);
                        trigger().IsActive(true);
                        originalEntity = ko.toJSON(trigger);
                        logger.info("Your trigger has been succesfully published, and will be added to the exchange shortly");
                    });
            },
            depublishAsync = function () {
                trigger().FiredOnOperations(operations().join());
                var data = ko.mapping.toJSON(trigger);
                isBusy(true);

                return context.post(data, "/Trigger/Depublish")
                    .then(function (result) {
                        isBusy(false);
                        trigger().IsActive(false);
                        trigger().Id(result);
                        logger.info("Your trigger has been succesfully depublished, and will be removed from the exchange shortly");
                    });
            },
            remove = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(trigger);

                app.showMessage("Are you sure you want to remove this trigger, this action cannot be undone", "Rx Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.post(data, "/Trigger/Remove")
                                .fail(tcs.reject)
                                .done(function (result) {
                                    isBusy(false);
                                    trigger().IsActive(false);
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
                    return trigger().Id();
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
                        caption: "Publish",
                        icon: "fa fa-sign-in",
                        enable: ko.computed(function () {
                            return trigger().Id();
                        })
                    },
                    {
                        command: depublishAsync,
                        caption: "Depublish",
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return trigger().Id();
                        })
                    }
                ])
            }

        };

        return vm;

    });
