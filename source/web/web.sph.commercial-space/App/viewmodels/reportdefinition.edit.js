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

define(['services/datacontext', 'services/logger', 'durandal/system', './reportdefinition.base'],
    function (context, logger, system, reportDefinitionBase) {
        var isBusy = ko.observable(false),
            reportDefinitionId = ko.observable(),
            activate = function (routeData) {
                reportDefinitionBase.activate();
                var id = parseInt(routeData.id),
                    setRdl = function (d) {

                        _(d.ReportLayoutCollection()).each(function (layout) {
                            _(layout.ReportItemCollection()).each(function (t) {
                                t.isSelected = ko.observable(false);
                            });
                        });

                        if (typeof d.DataSource === "object") {
                            d.DataSource = ko.observable(d.DataSource);
                        }

                        vm.reportDefinition(d);
                        reportDefinitionBase.reportDefinition(d);
                    };
                reportDefinitionId(id);



                if (!id) {
                    var rdl = new bespoke.sphcommercialspace.domain.ReportDefinition();
                    setRdl(rdl);
                } else {
                    var query = String.format("ReportDefinitionId eq {0}", id);
                    var tcs = new $.Deferred();
                    context.loadOneAsync("ReportDefinition", query)
                        .done(setRdl)
                        .done(tcs.resolve);

                    return tcs.promise();
                }
                return true;
            },
            viewAttached = function (view) {
                reportDefinitionBase.viewAttached(view);
            },
            save = function () {
                // get the reordered report items
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.reportDefinition);

                context.post(data, "/ReportDefinition/Save")
                    .then(function (result) {
                        logger.log("RDL has been saved", data, this, true);
                        tcs.resolve(result);
                    });
                return tcs.promise();

            },
            removeReportItem = function (ri) {
                console.log("remove " + ri.Name());
            },
             configure = function () {
                 var tcs = new $.Deferred();
                 setTimeout(function () {
                     tcs.resolve(true);
                 }, 500);

                 $('#configuration-panel').modal();
                 return tcs.promise();
             },
            removeParameter = function (p) {
                vm.reportDefinition().DataSource().ParameterCollection.remove(p);
            },
            addParameter = function () {
                vm.reportDefinition().DataSource().ParameterCollection.push(new bespoke.sphcommercialspace.domain.Parameter());
            },
            addDataGridColumn = function (datagridItem) {
                datagridItem.ReportColumnCollection.push(new bespoke.sphcommercialspace.domain.ReportColumn());
            };

        var vm = {
            reportDefinition: ko.observable(new bespoke.sphcommercialspace.domain.ReportDefinition()),
            title: ko.observable('Report Builder'),
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            removeReportItem: removeReportItem,
            selectReportItem: reportDefinitionBase.selectReportItem,
            selectedReportItem: reportDefinitionBase.selectedReportItem,
            toolboxitems: reportDefinitionBase.toolboxItems,

            removeParameter: removeParameter,
            addParameter: addParameter,

            addDataGridColumn: addDataGridColumn,

            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([{
                    command: configure,
                    caption: "Configuration",
                    icon: "icon-gear"
                }])
            }
        };

        return vm;

    });