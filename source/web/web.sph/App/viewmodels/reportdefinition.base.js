/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../durandal/app.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.3.js" />

define(['schemas/report.builder.g', 'services/datacontext', 'durandal/system', 'durandal/app', 'services/logger'],
    function (reportg, context, system, app, logger) {
        var isBusy = ko.observable(false),
            rdl = ko.observable(new bespoke.sph.domain.ReportDefinition()),
            activate = function () {

                var toolboxElements = [];

                var labelToolbox = new bespoke.sph.domain.LabelItem(system.guid());
                labelToolbox.CssClass("fa fa-report-label");
                labelToolbox.Name("Label");
                labelToolbox.Icon("fa fa-font icon-2x");
                labelToolbox.Html("Label 1");

                var lineToolbox = new bespoke.sph.domain.LineItem(system.guid());
                lineToolbox.CssClass("fa fa-report-line");
                lineToolbox.Name("Line");
                lineToolbox.Icon("fa fa-ellipsis-horizontal icon-2x");

                var tableToolbox = new bespoke.sph.domain.DataGridItem(system.guid());
                tableToolbox.CssClass("fa fa-report-table");
                tableToolbox.Name("Table");
                tableToolbox.Icon("fa fa-table icon-2x");

                var barChartToolbox = new bespoke.sph.domain.BarChartItem(system.guid());
                barChartToolbox.CssClass("fa fa-report-barchart");
                barChartToolbox.Name("Bar Chart");
                barChartToolbox.Icon("fa fa-bar-chart-o icon-2x");

                var lineChartToolbox = new bespoke.sph.domain.LineChartItem(system.guid());
                lineChartToolbox.CssClass("fa fa-report-linechart");
                lineChartToolbox.Name("Line Chart");
                lineChartToolbox.Icon("fa fa-bar-chart-o icon-2x");

                var pieChartToolbox = new bespoke.sph.domain.PieChartItem(system.guid());
                pieChartToolbox.CssClass("fa fa-report-piechart");
                pieChartToolbox.Name("Pie Chart");
                pieChartToolbox.Icon("fa fa-circle icon-2x");

                toolboxElements.push(labelToolbox);
                toolboxElements.push(lineToolbox);
                toolboxElements.push(tableToolbox);
                toolboxElements.push(barChartToolbox);
                toolboxElements.push(lineChartToolbox);
                toolboxElements.push(pieChartToolbox);

                vm.toolboxItems(toolboxElements);

                return true;

            },
            sortableLayout = function () {
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
            },
            attached = function (view) {

                $('#layout-toolbox').on('click', 'input[type=radio]', function () {

                    var $radio = $(this),
                        layout = $radio.val(),
                        buildLayout = function () {

                            rdl().ReportLayoutCollection.removeAll();

                            var header = new bespoke.sph.domain.ReportLayout(),
                                footer = new bespoke.sph.domain.ReportLayout();
                            header.Name("header");
                            footer.Name("footer");

                            rdl().ReportLayoutCollection.push(header);

                            if (layout === "TwoColumns") {
                                var body1 = new bespoke.sph.domain.ReportLayout();
                                body1.Name("Content 1");
                                body1.Column(0);
                                body1.ColumnSpan(6);
                                rdl().ReportLayoutCollection.push(body1);

                                var body2 = new bespoke.sph.domain.ReportLayout();
                                body2.Name("Content 2");
                                body2.Column(6);
                                body2.ColumnSpan(6);
                                rdl().ReportLayoutCollection.push(body2);

                            } else {
                                var body = new bespoke.sph.domain.ReportLayout();
                                body.Name("Content");
                                rdl().ReportLayoutCollection.push(body);
                            }
                            rdl().ReportLayoutCollection.push(footer);
                            sortableLayout();

                        };

                    if (rdl().ReportLayoutCollection().length) {
                        app.showMessage("This will cause all the report layout will be reset", "RESET", ["Yes", "No"])
                            .done(function (dialogResult) {
                                if (dialogResult === "Yes") {
                                    buildLayout();
                                }
                            });
                    } else {
                        buildLayout();
                    }

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

                $('#entity-columns').on('click', 'input[type=checkbox]', function () {
                    var entity = ko.dataFor(this);
                    if (entity.IsSelected) {
                        var efc = new bespoke.sph.domain.EntityField(system.guid());
                        efc.Name(entity.Name);
                        efc.TypeName(entity.TypeName);
                        rdl().DataSource().EntityFieldCollection.push(efc);
                    }
                });

                sortableLayout();

            },
            activeLayout = ko.observable(),
            selectReportItem = function (ri) {
                $('.selected-form-element').each(function () {
                    var item = ko.dataFor(this);
                    item.isSelected(false);
                });
                vm.selectedReportItem(ri);
                if (!ri.isSelected) ri.isSelected = ko.observable(true);
                ri.isSelected(true);
            },
            removeReportItem = function (item) {
                activeLayout().ReportItemCollection.remove(item);
            };

        var vm = {
            reportDefinition: rdl,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            selectReportItem: selectReportItem,
            selectedReportItem: ko.observable(),
            toolboxItems: ko.observableArray(),
            removeReportItem: removeReportItem
        };

        return vm;

    });
