/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.VariablePartial = function () {

    var system = require('durandal/system'),
        hasError = ko.observable(),
        errors = ko.observableArray();
    return {
        hasError: hasError,
        errors: errors
    };
};