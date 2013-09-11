/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};



bespoke.sphcommercialspace.domain.HourlySchedulePartial = function () {

    return {
        name: ko.observable("Hourly Schedule"),
        icon: ko.observable("icon-time")
    };
};

bespoke.sphcommercialspace.domain.DailySchedulePartial = function () {

    return {
        name: ko.observable("Daily Schedule"),
        icon: ko.observable("icon-calendar")
    };
};
bespoke.sphcommercialspace.domain.WeeklySchedulePartial = function () {

    return {
        name: ko.observable("Weekly Schedule"),
        icon: ko.observable("icon-th-list")
    };
};
bespoke.sphcommercialspace.domain.MonthlySchedulePartial = function () {

    return {
        name: ko.observable("Monthly Schedule"),
        icon: ko.observable("icon-calendar-empty")
    };
};

