/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
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
            entities = ko.observableArray(),
            actionOptions = ko.observableArray(),
            operationOptions = ko.observableArray(),
            operations = ko.observableArray(),
            activate = function () {
                trigger(new bespoke.sph.domain.Trigger({"IsActive" : true}));

                var actionOptionsTask = $.get("/sph/trigger/actions"),
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
