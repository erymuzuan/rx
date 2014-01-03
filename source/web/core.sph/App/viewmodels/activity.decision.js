/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />



define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.system],
    function(context, logger, routerm , system) {

        var okClick = function(data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                this.modal.close("OK");
            }

        },
            cancelClick = function() {
                this.modal.close("Cancel");
            };

        var vm = {
            activity: ko.observable(new bespoke.sph.domain.DecisionActivity()),
            wd : ko.observable(new bespoke.sph.domain.WorkflowDefinition(system.guid())),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
