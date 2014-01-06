/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['plugins/dialog'],
    function (dialog) {
        
        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activity: ko.observable(new bespoke.sph.domain.NotificationActivity()),
            okClick: okClick,
            wd : ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            cancelClick: cancelClick
        };


        return vm;

    });
