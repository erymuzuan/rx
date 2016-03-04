/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />

define(["plugins/dialog"],
    function (dialog) {

        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            attached = function (view) {
                setTimeout(function () { $(view).find("#complex-variable-name").focus(); }, 500);
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition());

        var vm = {
            attached : attached,
            variable: ko.observable(new bespoke.sph.domain.ComplexVariable()),
            wd: wd,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
