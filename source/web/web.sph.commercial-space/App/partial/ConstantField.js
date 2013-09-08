/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.ConstantFieldPartial = function (model) {
   
    return {
        Type: ko.observable(),
        Value: ko.observable()
    };
};