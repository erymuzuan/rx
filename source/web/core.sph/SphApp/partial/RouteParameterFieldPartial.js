/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />

bespoke.sph.domain.RouteParameterFieldPartial = function (model) {

    var vm = {};

    model.Name.subscribe(function(v) {
        if (!ko.unwrap(model.Expression)) {
            model.Expression(v);
        }
    });
    return vm;
};
