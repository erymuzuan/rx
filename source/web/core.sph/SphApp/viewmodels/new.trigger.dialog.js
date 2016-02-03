/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="~/Scripts/_task.js" />
define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var trigger = ko.observable(),
            id = ko.observable(),
            entity = ko.observable(),
            entities = ko.observableArray(),
            actionOptions = ko.observableArray(),
            operationOptions = ko.observableArray(),
            operations = ko.observableArray(),
            activate = function () {
                trigger(new bespoke.sph.domain.Trigger({ "IsActive": true, "Entity": ko.unwrap(entity) }));

                var actionOptionsTask = $.get("/sph/trigger/actions"),
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

                if (ko.unwrap(entity)) {
                    loadOperationOptions(ko.unwrap(entity));
                } else {
                    operationOptions([]);
                }

                return $.when(entitiesTask, actionOptionsTask).done(function (list, actions) {
                    entities(list);
                    actionOptions(actions[0]);
                    trigger().Entity.subscribe(loadOperationOptions);
                });
            },
            attached = function () {
                setTimeout(function () {
                    $("#name-input").focus();
                }, 500);

            },

            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                trigger().FiredOnOperations(operations().join());
                var json = ko.mapping.toJSON(trigger);
                return context.post(json, "/trigger/save")
                    .then(function (result) {
                        if (result) {
                            id(result);
                            dialog.close(data, "OK");
                        }
                    });

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            trigger: trigger,
            entity: entity,
            entities: entities,
            actionOptions: actionOptions,
            operationOptions: operationOptions,
            operations: operations,
            id: id,
            attached: attached,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
