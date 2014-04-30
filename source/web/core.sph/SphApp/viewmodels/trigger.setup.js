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

define(['services/datacontext', 'services/jsonimportexport', objectbuilders.app, objectbuilders.system, objectbuilders.logger],
    function (context, eximp, app, system, logger) {

        var trigger = ko.observable(new bespoke.sph.domain.Trigger()),
            typeaheadEntity = ko.observable(),
            isBusy = ko.observable(false),
            id = ko.observable(),
            entities = ko.observableArray(),
            operationOptions = ko.observableArray(),
            operations = ko.observableArray(),
            activate = function (id2) {
                id(parseInt(id2));

                var query = String.format("TriggerId eq {0} ", id()),
                    tcs = new $.Deferred(),
                    triggerTask = context.loadOneAsync("Trigger", query),
                    entitiesTask = context.getListAsync("EntityDefinition", "EntityDefinitionId gt 0", "Name"),
                    loadOperationOptions = function (ent) {

                        context.loadOneAsync("EntityDefinition", String.format("Name eq '{0}'", ent))
                            .done(function (ed) {
                                if (!ed) {
                                    return;
                                }
                                operationOptions(_(ed.EntityOperationCollection()).map(function (v) { return v.Name(); }));


                            });
                    };

                $.when(triggerTask, entitiesTask).done(function (t, list) {
                    entities(list);
                    if (t) {
                        trigger(t);
                        typeaheadEntity(t.Entity());
                        operations(t.FiredOnOperations().split(','));
                        loadOperationOptions(t.Entity());
                    } else {
                        trigger(new bespoke.sph.domain.Trigger(system.guid()));
                        operations.removeAll();
                    }
                    trigger().Entity.subscribe(function (ent) {
                        typeaheadEntity(ent);
                        loadOperationOptions(ent);
                    });
                    tcs.resolve(true);
                });

                return tcs.promise();
            },
            attached = function () {

            },
            save = function () {
                vm.trigger().FiredOnOperations(operations().join());
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(vm.trigger);
                isBusy(true);

                context.post(data, "/Trigger/Save")
                    .then(function (result) {
                        isBusy(false);
                        vm.trigger().TriggerId(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            publishAsync = function () {
                vm.trigger().FiredOnOperations(operations().join());
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(vm.trigger);
                isBusy(true);

                context.post(data, "/Trigger/Publish")
                    .then(function (result) {
                        isBusy(false);
                        vm.trigger().TriggerId(result);
                        vm.trigger().IsActive(true);
                        tcs.resolve(result);
                        logger.info("Your trigger has been succesfully published, and will be added to the exchange shortly");
                    });
                return tcs.promise();
            },
            depublishAsync = function () {
                vm.trigger().FiredOnOperations(operations().join());
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(vm.trigger);
                isBusy(true);

                context.post(data, "/Trigger/Depublish")
                    .then(function (result) {
                        isBusy(false);
                        vm.trigger().IsActive(false);
                        vm.trigger().TriggerId(result);
                        tcs.resolve(result);
                        logger.info("Your trigger has been succesfully depublished, and will be removed from the exchange shortly");
                    });
                return tcs.promise();
            },

            exportJson = function () {
                return eximp.exportJson("trigger." + vm.trigger().TriggerId() + ".json", ko.mapping.toJSON(vm.trigger));

            },

            importJson = function () {
                return eximp.importJson()
                    .done(function (json) {
                        var clone = context.toObservable(JSON.parse(json));
                        vm.trigger(clone);
                        vm.trigger().TriggerId(0);

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
            attached: attached,
            trigger: trigger,
            operationOptions: operationOptions,
            operations: operations,
            entities: entities,
            typeaheadEntity: typeaheadEntity,
            toolbar: {
                saveCommand: save,
                reloadCommand: reload,
                exportCommand: exportJson,
                commands: ko.observableArray([
                    {
                        icon: 'fa fa-upload',
                        caption: 'import',
                        command: importJson
                    },
                    {
                        command: publishAsync,
                        caption: 'Publish',
                        icon: "fa fa-sign-in",
                        enable: ko.computed(function () {
                            return trigger().TriggerId() > 0;
                        })
                    },
                    {
                        command: depublishAsync,
                        caption: 'Depublish',
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return trigger().TriggerId() > 0;
                        })
                    }
                ])
            }

        };

        return vm;

    });
