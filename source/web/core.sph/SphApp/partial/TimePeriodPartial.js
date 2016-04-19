/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.TimePeriodPartial = function (period) {
    var toString = function() {
        return ko.unwrap(period.Count) + " " + ko.unwrap(period.Unit);
    };
    return {
        toString: toString
    };
};