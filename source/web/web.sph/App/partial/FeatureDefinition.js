/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../durandal/amd/require.js" />
var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.FeatureDefinitionPartial = function (model) {
    var getAvailableQuantityOptions = function () {
        var q = ko.observableArray();
        for (var i = 0; i <= model.AvailableQuantity(); i++) {
            q.push(i);
        }
        return q;
    };
    return {
        AvailableQuantityOptions: getAvailableQuantityOptions,
    };
};