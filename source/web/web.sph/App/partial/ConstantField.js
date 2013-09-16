/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ConstantFieldPartial = function () {
   
    return {
        Type: ko.observable(),
        Value: ko.observable()
    };
};