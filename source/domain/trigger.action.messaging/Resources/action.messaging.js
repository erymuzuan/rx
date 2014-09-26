/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />



define(['services/datacontext', 'services/logger', 'plugins/dialog', objectbuilders.system],
    function (context, logger, dialog, system) {

        bespoke.sph.domain.MessagingAction = function (optionOrWebid) {

            var v = new bespoke.sph.domain.CustomAction(optionOrWebid);
            v.OutboundMap = ko.observable();
            v.Adapter = ko.observable();
            v.Operation = ko.observable();
            v.Table = ko.observable();
            v.Crud = ko.observable();
            v.Retry = ko.observable();
            v.RetryInterval = ko.observable();
            v.RetryIntervalTimeSpan = ko.observable(1);
            v["$type"] = "Bespoke.Sph.Messaging.MessagingAction, trigger.action.messaging";
            if (optionOrWebid && typeof optionOrWebid === "object") {
                for (var n in optionOrWebid) {
                    if (typeof v[n] === "function") {
                        v[n](optionOrWebid[n]);
                    }
                }
            }
            if (optionOrWebid && typeof optionOrWebid === "string") {
                v.WebId(optionOrWebid);
            }


            if (bespoke.sph.domain.MessagingActionPartial) {
                return _(v).extend(new bespoke.sph.domain.MessagingPartial(v));
            }
            return v;
        };



        var action = ko.observable(new bespoke.sph.domain.MessagingAction(system.guid())),
            trigger = ko.observable(),
            mappingOptions = ko.observableArray(),
            adapterOptions = ko.observableArray(),
            operationOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            crudOptions = ko.observableArray(),
            activate = function () {
                var query = String.format("InputTypeName eq '{0}'", trigger().TypeOf()),
                    tcs = new $.Deferred();

                context.loadAsync("Adapter", "Id ne '0'")
                    .then(function (lo) {
                        adapterOptions(lo.itemCollection);
                        tcs.resolve(true);
                    });
                context.loadAsync("TransformDefinition", query)
                    .then(function (lo) {
                        mappingOptions(lo.itemCollection);
                    });
                return tcs.promise();

            },
            attached = function (view) {
                action().Adapter.subscribe(function(name) {
                    var adapter = _(adapterOptions()).find(function(v) {
                        return ko.unwrap(v.Name) === name;
                    });
                    var operations = _(adapter.OperationDefinitionCollection()).map(function(v) {
                        return ko.unwrap(v.Name);
                    });
                    operationOptions(operations);
                    var tables = _(adapter.TableDefinitionCollection()).map(function(v) {
                        return ko.unwrap(v.Name);
                    });
                    tableOptions(tables);
                    crudOptions(['Insert', 'Update', 'Delete']);
                });
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    //var wd = _(wdOptions()).find(function (v) {
                    //    return ko.unwrap(v.WorkflowDefinitionId) == ko.unwrap(action().WorkflowDefinitionId);
                    //});
                    //action().Title(ko.unwrap(wd.Name));
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            trigger: trigger,
            action: action,
            mappingOptions: mappingOptions,
            adapterOptions: adapterOptions,
            operationOptions: operationOptions,
            tableOptions: tableOptions,
            crudOptions: crudOptions,
            activate: activate,
            attached: attached,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
