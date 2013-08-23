/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/report.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.3.js" />

define(['services/datacontext', 'durandal/system', './reportdefinition.base'],
    function (context, system, reportDefinitionBase) {
        var isBusy = ko.observable(false),
            reportDefinitionId = ko.observable(),
            activate = function (routeData) {
                reportDefinitionBase.activate();
                var id = parseInt(routeData.id);
                reportDefinitionId(id);
                if (!id) {
                    vm.reportDefinition(new bespoke.sphcommercialspace.domain.ReportDefinition());
                    //reportDefinitionBase.reportDefinition(vm.reportDefinition());
                    
                    reportDefinitionBase.reportDefinition(vm.reportDefinition());
                }
                return true;
            },
            viewAttached = function (view) {
                reportDefinitionBase.viewAttached(view);
            },
            save = function () {

            };

        var vm = {
            reportDefinition: ko.observable(new bespoke.sphcommercialspace.domain.ReportDefinition()),
            title: ko.observable('Report Builder'),
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            selectReportItem: reportDefinitionBase.selectReportItem,
            selectedReportItem: reportDefinitionBase.selectedReportItem,
            toolboxitems: reportDefinitionBase.toolboxItems,
            toolbar: {
                saveCommand: save
            }
        };

        return vm;

    });
