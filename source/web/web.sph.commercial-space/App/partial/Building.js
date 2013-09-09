/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.BuildingPartial = function (model) {
    var getCustomField = function(name) {
        var cs = _(model.CustomFieldValueCollection()).find(function (v) {
            return v.Name() === name;
        });
        if (!cs) {
            throw "Cannot find custom field for " + name + " in Building";
        }
        return cs.Value;
    };
    return {
        CustomField : getCustomField
    };
};