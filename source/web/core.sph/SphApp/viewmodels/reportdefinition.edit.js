/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/report.builder.g.js" />

define(['services/datacontext', 'services/logger', 'durandal/system',
        './reportdefinition.base', './_reportdefinition.preview', 'services/jsonimportexport', objectbuilders.app],
    function (context, logger, system, designer, preview, eximp, app) {
        var isBusy = ko.observable(false),
            entities = ko.observableArray(),
            reportDefinitionId = ko.observable(),

            setRdl = function (d) {
                _(d.ReportLayoutCollection()).each(function (layout) {
                    _(layout.ReportItemCollection()).each(function (t) {
                        t.isSelected = ko.observable(false);
                    });
                });

                vm.reportDefinition(d);
                designer.reportDefinition(d);
                preview.activate(d);
                loadSelectedEntityColumns(d.DataSource().EntityFieldCollection());

                d.DataSource().EntityName.subscribe(loadEntityColumns);
            },
            activate = function (rid) {
                designer.activate();
                var id = parseInt(rid);
                reportDefinitionId(id);

                context.getListAsync("EntityDefinition", "EntityDefinitionId gt 0", "Name")
                .done(function (list) {
                    entities(list);
                });


                if (!id) {
                    var rdl = new bespoke.sph.domain.ReportDefinition(system.guid());
                    setRdl(rdl);

                } else {
                    var query = String.format("ReportDefinitionId eq {0}", id);
                    var tcs = new $.Deferred();
                    context.loadOneAsync("ReportDefinition", query)
                        .done(function (r) {
                            loadEntityColumns(r.DataSource().EntityName())
                                .done(function () {
                                    setRdl(r);
                                    tcs.resolve(true);
                                });
                        });

                    return tcs.promise();
                }

                return true;
            },
            attached = function (view) {
                designer.attached(view);
                $('a.btn-close-configuration-dialog').click(function () {
                    loadSelectedEntityColumns(vm.reportDefinition().DataSource().EntityFieldCollection());
                });

            },
            loadSelectedEntityColumns = function (fields) {
                var columns = _(fields).map(function (v) {
                    var name = v.Name(),
                        aggregate = v.Aggregate();
                    if (aggregate) {
                        if (aggregate !== "GROUP") {
                            name = v.Name() + "_" + aggregate;
                        }
                    }
                    return "[" + name + "]";
                }),
                    columns2 = _(fields).map(function (v) {
                        var name = v.Name(),
                            aggregate = v.Aggregate();
                        if (aggregate) {
                            if (aggregate !== "GROUP") {
                                name = v.Name() + "_" + aggregate;
                            }
                        }
                        return name;
                    });
                vm.dataGridColumnOptions(columns);
                vm.columnOptions(columns2);
            },
            loadEntityColumns = function (entity) {
                var tcs = new $.Deferred();
                $.get('/sph/ReportDefinition/GetEntityColumns/' + entity)
                        .done(function (columns) {
                            vm.entityColumns(columns);
                            var filterable = _(columns).filter(function (v) {
                                return v.IsFilterable;
                            });
                            vm.filterableEntityColumns(filterable);
                        })
                        .done(tcs.resolve);

                return tcs.promise();

            },
            save = function () {
                // get the reordered report items
                $('.report-layout').each(function (i, v) {
                    var items = _($(v).find('.report-item')).map(function (div) {
                        return ko.dataFor(div);
                    });
                    var layout = ko.dataFor(v);
                    layout.ReportItemCollection(items);
                });



                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.reportDefinition);

                context.post(data, "/Sph/ReportDefinition/Save")
                    .then(function (result) {
                        logger.log("RDL has been saved", data, this, true);
                        tcs.resolve(result);
                    });
                return tcs.promise();

            },
            remove = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(vm.reportDefinition);
                app.showMessage("Are you sure you want to trash this ReportDefinition, this action cannot be undone!!", "SPH", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.post(data, "/Sph/ReportDefinition/Remove")
                                .then(function (result) {
                                    logger.log("RDL has been remove", data, this, true);
                                    tcs.resolve(result);
                                    window.location = "/sph#reportdefinition.list";
                                });
                        } else {
                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();

            },
            removeReportItem = function (ri, e) {
                designer.removeReportItem(ri);
                $(e.target).parents(".report-item");
            },
             configure = function () {
                 var tcs = new $.Deferred();
                 setTimeout(function () {
                     tcs.resolve(true);
                 }, 500);

                 $('#rdl-configuration-panel').modal();
                 return tcs.promise();
             },
            removeParameter = function (p) {
                vm.reportDefinition().DataSource().ParameterCollection.remove(p);
            },
            addParameter = function () {
                vm.reportDefinition().DataSource().ParameterCollection.push(new bespoke.sph.domain.Parameter());
            },
            addDataGridColumn = function (datagridItem) {
                datagridItem.ReportColumnCollection.push(new bespoke.sph.domain.ReportColumn());
            },
            addFilter = function () {
                vm.reportDefinition().DataSource().ReportFilterCollection.push(new bespoke.sph.domain.ReportFilter());

            },
            removeFilter = function (filter) {
                vm.reportDefinition().DataSource().ReportFilterCollection.remove(filter);
            },
            removeField = function (field) {
                vm.reportDefinition().DataSource().EntityFieldCollection.remove(field);
            },
            removeDataGridColumn = function (col, e) {
                var grid = ko.dataFor($(e.target).parents("table")[0]);
                grid.ReportColumnCollection.remove(col);

            },

            exportTemplate = function () {
                return eximp.exportJson("report.definition." + vm.reportDefinition().ReportDefinitionId() + ".json", ko.mapping.toJSON(vm.reportDefinition));
            },

            importTemplateJson = function () {
                return eximp.importJson()
                    .done(function (json) {
                        try {
                            var clone = context.toObservable(JSON.parse(json));

                            _(clone.DataSource.ParameterCollection).each(function (p) {
                                if (!p.DefaultValue) {
                                    p.DefaultValue = "";
                                }
                            });
                            clone.ReportDefinitionId(0);
                            loadEntityColumns(clone.DataSource().EntityName());
                            setRdl(clone);
                        } catch (error) {
                            logger.error('Fail template import tidak sah', error);
                        }
                    });
            };

        var vm = {
            reportDefinition: designer.reportDefinition,
            title: ko.observable('Report Builder'),
            isBusy: isBusy,
            entities: entities,
            activate: activate,
            attached: attached,
            removeReportItem: removeReportItem,
            selectReportItem: designer.selectReportItem,
            selectedReportItem: designer.selectedReportItem,
            toolboxitems: designer.toolboxItems,
            selectedEntityName: ko.observable(''),
            entityColumns: ko.observableArray(),
            filterableEntityColumns: ko.observableArray(),

            removeParameter: removeParameter,
            addParameter: addParameter,

            addFilter: addFilter,
            removeFilter: removeFilter,
            removeField: removeField,

            removeDataGridColumn: removeDataGridColumn,
            addDataGridColumn: addDataGridColumn,
            dataGridColumnOptions: ko.observableArray(),
            columnOptions: ko.observableArray(),

            toolbar: {
                saveCommand: save,
                exportCommand: exportTemplate,
                removeCommand: remove,
                importCommand: importTemplateJson,
                commands: ko.observableArray([{
                    command: configure,
                    caption: "Configuration",
                    icon: "fa fa-gear"
                },
                    {

                        command: preview.showPreview,
                        caption: "Preview",
                        icon: "fa fa-file-text-o"
                    }])
            }
        };

        vm.reportDefinition().DataSource().EntityName.subscribe(loadEntityColumns);
        return vm;

    });