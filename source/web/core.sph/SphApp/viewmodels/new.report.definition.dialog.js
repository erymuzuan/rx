/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="~/Scripts/_task.js" />
/// <reference path="~/SphApp/schemas/report.builder.g.js" />

define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var rd = ko.observable(),
            id = ko.observable(),
            entities = ko.observableArray(),
            activate = function () {
                rd(new bespoke.sph.domain.ReportDefinition({"IsActive" : true}));

                var entitiesTask = context.getListAsync("EntityDefinition", "Id ne ''", "Name");

                return entitiesTask.done(function (list) {
                    entities(list);
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

                var json = ko.mapping.toJSON(rd);
                return context.post(json, "/Sph/ReportDefinition/Save")
                    .then(function (result) {
                        if (result) {
                            id(result.id);
                            dialog.close(data, "OK");
                        }
                    });

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            rd: rd,
            entities: entities,
            id: id,
            attached: attached,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
