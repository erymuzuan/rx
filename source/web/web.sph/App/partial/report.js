﻿/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};



bespoke.sph.domain.HourlySchedulePartial = function () {

    return {
        name: ko.observable("Hourly Schedule"),
        icon: ko.observable("fa fa-clock-o")
    };
};

bespoke.sph.domain.DailySchedulePartial = function () {

    return {
        name: ko.observable("Daily Schedule"),
        icon: ko.observable("fa fa-calendar")
    };
};
bespoke.sph.domain.WeeklySchedulePartial = function () {

    return {
        name: ko.observable("Weekly Schedule"),
        icon: ko.observable("fa fa-th-list")
    };
};
bespoke.sph.domain.MonthlySchedulePartial = function () {

    return {
        name: ko.observable("Monthly Schedule"),
        icon: ko.observable("fa fa-calendar-o")
    };
};

