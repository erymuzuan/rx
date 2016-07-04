/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />


bespoke.sph.domain.RouteParameterPartial = function (model) {

    model.Type.subscribe(function(v) {
        if (v === "System.DateTime, mscorlib") {
            model.DefaultValue(null);
        }
    });
    return {};
};