/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.LineChartItemPartial = function () {
    var system = require('durandal/system'),
        addSeries = function() {
            this.ChartSeriesCollection.push(new bespoke.sph.domain.ChartSeries(system.guid()));
        },
        removeSeries = function (series) {
            var self = this;
            return function() {
                self.ChartSeriesCollection.remove(series);
            };

        };
    return {
        addSeries: addSeries,
        removeSeries: removeSeries
    };
};