/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />

bespoke.sph.domain.ReceivePortPartial = function (port) {

    const isWizardOk = ko.computed(function() {
        return ko.unwrap(port.Name) && ko.unwrap(port.Formatter) && ko.unwrap(port.Entity);
    });

    return {
        isWizardOk: isWizardOk

    };
};
