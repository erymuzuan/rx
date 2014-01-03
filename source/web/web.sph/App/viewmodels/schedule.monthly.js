/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />



define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var activate = function () {
            //NOTE: a workaround when binding to array of int in checkbox
            var stringDays = _(vm.schedule().Days()).map(function (v) {
                return v.toString();
            });
            vm.schedule().Days(stringDays);
        },
        okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                this.modal.close("OK");
            }

        },
            cancelClick = function () {
                this.modal.close("Cancel");
            };

        var vm = {
            schedule: ko.observable(new bespoke.sph.domain.MonthlySchedule()),
            okClick: okClick,
            activate: activate,
            cancelClick: cancelClick
        };


        return vm;

    });
