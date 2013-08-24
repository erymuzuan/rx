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

define(['services/report.g', 'services/datacontext', 'durandal/system', 'services/logger'],
    function (reportg, context, system, logger) {
        var isBusy = ko.observable(false),
            rdl = ko.observable(new bespoke.sphcommercialspace.domain.ReportDefinition()),
            activate = function () {

                var toolboxElements = [];

                var labelToolbox = new bespoke.sphcommercialspace.domain.LabelItem(system.guid());
                labelToolbox.CssClass("icon-report-label");
                labelToolbox.Name("Label");
                labelToolbox.Icon("icon-font icon-2x");
                labelToolbox.Html("Label 1");

                var lineToolbox = new bespoke.sphcommercialspace.domain.LineItem(system.guid());
                lineToolbox.CssClass("icon-report-line");
                lineToolbox.Name("Line");
                lineToolbox.Icon("icon-ellipsis-horizontal icon-2x");

                var tableToolbox = new bespoke.sphcommercialspace.domain.DataGridItem(system.guid());
                tableToolbox.CssClass("icon-report-table");
                tableToolbox.Name("Table");
                tableToolbox.Icon("icon-table icon-2x");

                var barChartToolbox = new bespoke.sphcommercialspace.domain.BarChartItem(system.guid());
                barChartToolbox.CssClass("icon-report-barchart");
                barChartToolbox.Name("Bar Chart");
                barChartToolbox.Icon("icon-bar-chart icon-2x");

                var lineChartToolbox = new bespoke.sphcommercialspace.domain.LineChartItem(system.guid());
                lineChartToolbox.CssClass("icon-report-linechart");
                lineChartToolbox.Name("Line Chart");
                lineChartToolbox.Icon("icon-bar-chart icon-2x");

                var pieChartToolbox = new bespoke.sphcommercialspace.domain.PieChartItem(system.guid());
                pieChartToolbox.CssClass("icon-report-piechart");
                pieChartToolbox.Name("Pie Chart");
                pieChartToolbox.Icon("icon-circle icon-2x");

                toolboxElements.push(labelToolbox);
                toolboxElements.push(lineToolbox);
                toolboxElements.push(tableToolbox);
                toolboxElements.push(barChartToolbox);
                toolboxElements.push(lineChartToolbox);
                toolboxElements.push(pieChartToolbox);

                vm.toolboxItems(toolboxElements);

                return true;

            },
            viewAttached = function (view) {

                $('#layout-toolbox').on('click', 'input[type=radio]', function () {
                    var $radio = $(this),
                        layout = $radio.val();
                    rdl().ReportLayoutCollection.removeAll();

                    var header = new bespoke.sphcommercialspace.domain.ReportLayout(),
                        footer = new bespoke.sphcommercialspace.domain.ReportLayout();
                    header.Name("header");
                    footer.Name("footer");

                    rdl().ReportLayoutCollection.push(header);

                    if (layout === "TwoColumns") {
                        var body1 = new bespoke.sphcommercialspace.domain.ReportLayout();
                        body1.Name("Content 1");
                        body1.Column(0);
                        body1.ColumnSpan(6);
                        rdl().ReportLayoutCollection.push(body1);
                        
                        var body2 = new bespoke.sphcommercialspace.domain.ReportLayout();
                        body2.Name("Content 2");
                        body2.Column(6);
                        body2.ColumnSpan(6);
                        rdl().ReportLayoutCollection.push(body2);

                    } else {
                        var body = new bespoke.sphcommercialspace.domain.ReportLayout();
                        body.Name("Content");
                        rdl().ReportLayoutCollection.push(body);
                    }
                    rdl().ReportLayoutCollection.push(footer);

                    $.getScript('/Scripts/jquery-ui-1.10.3.custom.min.js')// only contains UI core and interactions API 
                          .done(function () {
                              var initDesigner = function () {
                                  $('#report-designer>div.report-layout').sortable({
                                      items: '>div>div',
                                      placeholder: 'ph',
                                      helper: 'original',
                                      dropOnEmpty: true,
                                      forcePlaceholderSize: true,
                                      forceHelperSize: false
                                  });
                              };
                              initDesigner();
                          });
                });

                // select layout
                $(view).on('click', 'div.report-layout', function () {
                    $('div.report-layout').removeClass("active");
                    $(this).addClass('active');

                    activeLayout(ko.dataFor(this));
                });

                // add items to layout
                $('#reportitems-toolbox').on('click', 'a', function (e) {
                    e.preventDefault();
                    if (!activeLayout()) {
                        logger.logError("No active view selected", this, this, true);
                        return;
                    }
                    $('.selected-form-element').each(function () {
                        var item = ko.dataFor(this);
                        item.isSelected(false);
                    });
                    
                    var reportitem = ko.dataFor(this);
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(reportitem));
                    clone.isSelected = ko.observable(true);
                    clone.WebId(system.guid());
                    clone.CssClass("");
                    
                    activeLayout().ReportItemCollection.push(clone);

                });


                // select item
                $('#report-designer').on('click', 'div.report-item', function (e) {
                    e.preventDefault();
                    var item = ko.dataFor(this);
                    selectReportItem(item);
                });

            },
            activeLayout = ko.observable(),
            selectReportItem = function (ri) {
                $('.selected-form-element').each(function () {
                    var item = ko.dataFor(this);
                    item.isSelected(false);
                });
                vm.selectedReportItem(ri);
                ri.isSelected(true);
            };

        var vm = {
            reportDefinition: rdl,
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            selectReportItem: selectReportItem,
            selectedReportItem: ko.observable(),
            toolboxItems: ko.observableArray()
        };

        return vm;

    });
