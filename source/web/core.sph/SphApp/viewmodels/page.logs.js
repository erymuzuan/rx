/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />



define(['plugins/dialog'],
    function (dialog) {
        var self = this,
            okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            viewCode = function (log) {
                var change = _(log.ChangeCollection()).find(function (v) {
                    return v.PropertyName() === "Code";
                });
                if (change) {
                    vm.code(change.OldValue());
                    vm.modal.close("OK");
                }
            };

        var vm = {
            page: ko.observable(new bespoke.sph.domain.Page()),
            logs: ko.observableArray(),
            code: ko.observable(),
            okClick: okClick,
            cancelClick: cancelClick,
            viewCode: viewCode
        };


        return vm;

    });
