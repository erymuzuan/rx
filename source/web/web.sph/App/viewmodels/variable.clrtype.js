/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />



define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function(context, logger, router) {

        var okClick = function(data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                this.modal.close("OK");
            }

        },
            cancelClick = function() {
                this.modal.close("Cancel");
            };

        var vm = {
            variable: ko.observable(new bespoke.sph.domain.ClrTypeVariable()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
