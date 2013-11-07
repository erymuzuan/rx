/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />



define([],
    function () {

        var okClick = function (data, ev) {
            if (ev.target.form.checkValidity()) {
                this.modal.close("OK");
            }

        },
            cancelClick = function () {
                this.modal.close("Cancel");
            };

        var vm = {
            activity: ko.observable(new bespoke.sph.domain.EndActivity()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
