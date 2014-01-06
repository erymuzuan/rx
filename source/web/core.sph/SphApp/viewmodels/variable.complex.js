/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />

define(['plugins/dialog'],
    function (dialog) {

        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition());

        var vm = {
            variable: ko.observable(new bespoke.sph.domain.ComplexVariable()),
            wd : wd,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
