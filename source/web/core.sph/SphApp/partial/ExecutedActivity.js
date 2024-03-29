﻿/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.ExecutedActivityPartial = function () {

    var system = require('durandal/system'),
        breakpoint = ko.observable(false),
        hit = ko.observable(false),
        errors = ko.observableArray();
    return {
        breakpoint: breakpoint,
        hit: hit,
        errors: errors
    };
};