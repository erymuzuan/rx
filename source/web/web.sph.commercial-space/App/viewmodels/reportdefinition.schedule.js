/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/report.g', 'services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function(reportg,context, logger, router) {

        var isBusy = ko.observable(false),
            rdl = ko.observable(),
            mode = ko.observable(),
            activate = function(routeData) {
                var id = parseInt(routeData.id),
                    query = String.format("ReportDefinitionId eq {0}", id),
                    tcs = new $.Deferred();
                context.loadOneAsync("ReportDefinition", query)
                    .done(tcs.resolve)
                    .done(rdl);

                return tcs.promise();
            },
            viewAttached = function(view) {

            };

        mode.subscribe(function (m) {
            if (typeof rdl().Schedule !== "function") {
                rdl().Schedule = ko.observable();
            }
            if (m === "h") {
                rdl().Schedule(new bespoke.sphcommercialspace.domain.HourlySchedule());
            }
            if (m === "d") {
                rdl().Schedule(new bespoke.sphcommercialspace.domain.DailySchedule());
            }
            if (m === "w") {
                rdl().Schedule(new bespoke.sphcommercialspace.domain.WeeklySchedule());
            }
            if (m === "m") {
                rdl().Schedule(new bespoke.sphcommercialspace.domain.MonthlySchedule());
            }
        });

        var vm = {
            isBusy: isBusy,
            rdl: rdl,
            activate: activate,
            viewAttached: viewAttached,
            mode : mode
        };

        return vm;

    });
