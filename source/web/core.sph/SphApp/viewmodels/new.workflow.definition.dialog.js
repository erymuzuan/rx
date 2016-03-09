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

        var wd = ko.observable(),
            id = ko.observable(),
            activate = function() {
                wd(new bespoke.sph.domain.WorkflowDefinition({ "Version": 1 }));
            },
            attached = function() {
                setTimeout(function () {
                    $("#wd-name").focus();
                }, 500);
            },
            saveAsync = function(data) {

                var json = ko.mapping.toJSON(wd);
                return context.post(json, "/api/workflow-definitions")
                    .then(function (result) {
                        if (result.success) {
                            id(result.id);
                            dialog.close(data, "OK");
                        }
                    });
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                return saveAsync(data,ev);

            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            wd: wd,
            id: id,
            attached: attached,
            activate : activate,
            okClick: okClick,
            cancelClick: cancelClick,
            saveAsync : saveAsync
        };


        return vm;

    });
