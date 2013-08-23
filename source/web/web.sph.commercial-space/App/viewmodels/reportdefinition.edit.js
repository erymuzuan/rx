/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/report.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.3.js" />

define(['services/report.g','services/datacontext', 'durandal/system'],
    function (reportg, context, system) {
    var isBusy = ko.observable(false),
        activate = function () {
            
            var toolboxElements = [];
            
            var labelToolbox = new bespoke.sphcommercialspace.domain.LabelItem(system.guid());
            labelToolbox.CssClass = "icon-report-label";
            labelToolbox.Name = "Label";

            var barChartToolbox = new bespoke.sphcommercialspace.domain.BarChartItem(system.guid());
            barChartToolbox.CssClass = "icon-report-barchart";
            barChartToolbox.Name = "Bar Chart";
            
            var lineChartToolbox = new bespoke.sphcommercialspace.domain.LineChartItem(system.guid());
            lineChartToolbox.CssClass = "icon-report-linechart";
            lineChartToolbox.Name = "Line Chart";
            
            toolboxElements.push(labelToolbox);
            toolboxElements.push(barChartToolbox);
            toolboxElements.push(lineChartToolbox);

            vm.toolboxitems(toolboxElements);

            return true;

        },
        viewAttached = function(view) {
            
        },
        selectReportItem = function(ri) {
            
        },
        save = function() {
            
        };
        
    var vm = {
        reportDefinition: ko.observable(new bespoke.sphcommercialspace.domain.ReportDefinition()),
        title: ko.observable('Report Builder'),
        isBusy: isBusy,
        activate: activate,
        viewAttached: viewAttached,
        selectReportItem: selectReportItem,
        selectedReportItem: ko.observable(),
        toolboxitems: ko.observableArray(),
        columns: ko.observableArray(),
        toolbar: {
            saveCommand: save,
            groupCommands: ko.observableArray(),
            
        }
    };

    return vm;
    
});