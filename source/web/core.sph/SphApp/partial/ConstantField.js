/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />

bespoke.sph.domain.ConstantFieldPartial = function () {
   
    return {
        Type: ko.observable(),
        Value: ko.observable()
    };
};