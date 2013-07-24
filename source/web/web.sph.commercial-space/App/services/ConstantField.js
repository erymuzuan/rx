/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />

bespoke.sphcommercialspace.domain.ConstantFieldPartial = function (model) {
   
    return {
        Type: ko.observable(),
        Value: ko.observable()
    };
};