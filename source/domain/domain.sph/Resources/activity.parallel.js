/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

define(['plugins/dialog'],
    function (dialog) {

        var isBusy = ko.observable(false),
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            activate = function () {

            },
            attached = function (view) {
                
            };

        var vm = {
            okClick: okClick,
            cancelClick: cancelClick,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            activity: ko.observable(),
            wd: wd
        };

        return vm;

    });
