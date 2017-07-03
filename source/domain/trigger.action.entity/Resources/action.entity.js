/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />



define(["services/datacontext", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function (context, logger, dialog, system) {

        bespoke.sph.domain.MessagingAction = function (optionOrWebid) {

            const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
            v.OutboundMap = ko.observable();
            v.Adapter = ko.observable();
            v.Operation = ko.observable();
            v.Table = ko.observable();
            v.Schema = ko.observable();
            v.Crud = ko.observable();
            v.Retry = ko.observable();
            v.RetryInterval = ko.observable();
            v.RetryIntervalTimeSpan = ko.observable(1);
            v.RetryAlgorithm = ko.observable("Constant");
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



        const action = ko.observable(new bespoke.sph.domain.MessagingAction(system.guid())),
            trigger = ko.observable(),
            mappingOptions = ko.observableArray(),
            adapterOptions = ko.observableArray(),
            operationOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            crudOptions = ko.observableArray(),
            populateOptions = function (name) {
                const adapter = _(adapterOptions()).find(v => ko.unwrap(v.Name) === name),
                    operations = _(adapter.OperationDefinitionCollection()).map(v => ko.unwrap(v.Name)),
                    tables = _(adapter.TableDefinitionCollection()).map(v =>({ name: ko.unwrap(v.Name), schema: ko.unwrap(v.Schema), text: `${ko.unwrap(v.Schema)}.${ko.unwrap(v.Name)}` }));

                tableOptions(tables);
                operationOptions(operations);
                crudOptions(["Insert", "Update", "Delete"]);
            },
            activate = function () {
                const query = (`InputTypeName eq '${trigger().TypeOf()}'`);

                context.loadAsync("TransformDefinition", query)
                    .then(function (lo) {
                        mappingOptions(lo.itemCollection);
                    });

                return context.loadAsync("Adapter", "Id ne '0'")
                    .then(function (lo) {
                        adapterOptions(lo.itemCollection);
                        const name = action().Adapter();
                        if (name) {
                            populateOptions(name);
                        }
                    });

            },
            attached = function (view) {
                setTimeout(() => $(view).find("input#title-option").focus(), 500);
                action().Adapter.subscribe(populateOptions);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        const vm = {
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
