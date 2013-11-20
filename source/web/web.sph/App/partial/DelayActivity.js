/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.DelayActivityPartial = function () {

    return {
        IsAsync: ko.observable(true)
    };
};