/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery.signalR-2.2.0.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="~/Scripts/_task.js" />

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {

        var model = ko.observable(new bespoke.sph.domain.DataTransferDefinition()),
            connection = null,
            hub = null,
            errorRows = ko.observableArray(),
            adapterOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            entityOptions = ko.observableArray(),
            mapOptions = ko.observableArray(),
            progress = ko.observable({
                busy: ko.observable(false),
                Rows: ko.observable(0),
                errors: ko.observable(0),
                SqlRows: ko.observable(0),
                ElasticsearchRows: ko.observable(0),
                ElasticsearchQueue: {
                    Name: ko.observable("es.data-import"),
                    MessagesCount: ko.observable("NA"),
                    Rate: ko.observable("NA"),
                    Unacked: ko.observable("NA")
                },
                SqlServerQueue: {
                    Name: ko.observable("es.data-import"),
                    MessagesCount: ko.observable("NA"),
                    Rate: ko.observable("NA"),
                    Unacked: ko.observable("NA")
                }
            }),
            isBusy = ko.observable(false),
            canPreview = ko.computed(function () {
                return model().InboundAdapter()
                    && model().Table()
                    && model().SelectStatement()
                    && model().BatchSize();
            }),
            canImport = ko.computed(function () {
                return canPreview()
                    && model().InboundMap()
                    && model().Entity()
                    && !progress().busy();
            }),
            filterMapOptions = function (list) {
                var filtered = _(list).filter(function (v) {
                    return v.InputTypeName.indexOf(model().Table()) > -1 && v.OutputTypeName.indexOf(model().Entity()) > -1;
                });
                mapOptions(filtered);
            },
            loadMappings = function (lo) {
                var tuples = _(lo.itemCollection).map(function (v) {
                    return {
                        Id: ko.unwrap(v.Id),
                        Name: ko.unwrap(v.Name),
                        InputTypeName: ko.unwrap(v.InputTypeName),
                        OutputTypeName: ko.unwrap(v.OutputTypeName)
                    }
                });
                filterMapOptions(tuples);

            },
            activate = function (id) {
                console.log(router);
                context.getTuplesAsync("EntityDefinition", null, "Id", "Name")
                    .done(entityOptions);
                context.getTuplesAsync("Adapter", null, "Id", "Name")
                    .done(adapterOptions);

                var temp = new bespoke.sph.domain.DataTransferDefinition();
                return context.loadOneAsync("DataTransferDefinition", "Id eq '" + id + "'")
                    .then(function (b) {
                        if (b) {
                            temp = b;
                            return context.loadOneAsync("Adapter", String.format("Id eq '{0}'", ko.unwrap(b.InboundAdapter)));
                        }
                        return Task.fromResult(false);

                    })
                    .then(function (adp) {
                        if (adp) {
                            tableOptions(adp.TableDefinitionCollection().map(ko.mapping.toJS));
                            return context.loadAsync("TransformDefinition");
                        }
                        return Task.fromResult(false);

                    })
                    .then(function (mappingResult) {
                        if (mappingResult) {
                            loadMappings(mappingResult);
                        }
                        model(temp);
                        return $.getScript("/signalr/js");
                    })
                    .then(function () {
                        connection = $.connection.hub;
                        hub = $.connection.dataImportHub;

                        return connection.start();
                    });
            },
            modelChanged = function () {
                var cloned = ko.toJS(model);

                model().InboundAdapter.subscribe(function (a) {
                    if (!a) {
                        return;
                    }
                    isBusy(true);
                    context.loadOneAsync("Adapter", String.format("Id eq '{0}'", a))
                        .done(function (adp) {
                            tableOptions(adp.TableDefinitionCollection().map(ko.mapping.toJS));
                            isBusy(false);

                            model().Table(cloned.Table);
                        });
                });

                model().Table.subscribe(function (a) {
                    if (!a) {
                        return Task.fromResult(0);
                    }
                    var sql1 = String.format("select * from {0} ", a),
                        sql0 = model().SelectStatement() || "";
                    if (sql0.indexOf(a) <= -1)
                        model().SelectStatement(sql1);

                    if (!(model().Entity())) {
                        return Task.fromResult(0);
                    }
                    return context.loadAsync("TransformDefinition")
                        .done(loadMappings);
                });
                model().Entity.subscribe(function (a) {
                    if (!(a && model().Entity())) {
                        return Task.fromResult(0);
                    }
                    return context.loadAsync("TransformDefinition")
                        .done(loadMappings)
                        .done(function () {
                            model().InboundMap(cloned.InboundMap);
                        });
                });

                if (cloned.Table) {
                    model().InboundAdapter("");
                    model().InboundAdapter(cloned.InboundAdapter);
                    model().Table(cloned.Table);
                }
                if (cloned.InboundMap) {
                    model().Entity("");
                    model().Entity(cloned.Entity);
                    model().InboundMap(cloned.InboundMap);
                }
            },
            attached = function (view) {
                modelChanged();
                $(view).on("click", "div.modal-footer>button", function () {
                    $("div.modal-backdrop").remove();
                });
                var pb = $("#progressbar"),
                    label = $(".progress-label");
                pb.progressbar({
                    change: function () {
                        label.text(pb.progressbar("value") + "%");
                    },
                    complete: function () {
                        label.text("Complete!");
                    }
                });

            },
            preview = function () {
                var tcs = new $.Deferred();
                require(["viewmodels/data.import.result.preview.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.model(ko.unwrap(model));
                        dialog.tableOptions(ko.unwrap(tableOptions));
                        app2.showDialog(dialog).done(tcs.resolve);
                    });

                return tcs.promise();

            },
            importData = function () {
                progress().busy(true);
                errorRows([]);
                return hub.server.execute(ko.unwrap(model().Id), ko.mapping.toJS(model))
                    .fail(function (e) {
                        logger.error(e);
                    })
                    .progress(function (p) {

                        if (p.Exception) {
                            progress().errors(progress().errors() + 1);
                            console.error(p.Exception.Message);
                            // do paging
                            if (errorRows().length < 20) {
                                errorRows.push(p);
                            }
                            return;
                        }

                        if (p.Rows === -1) {
                            progress().ElasticsearchQueue.MessagesCount(p.ElasticsearchQueue.MessagesCount);
                            progress().ElasticsearchQueue.Rate(p.ElasticsearchQueue.Rate);
                            progress().SqlServerQueue.MessagesCount(p.SqlServerQueue.MessagesCount);
                            progress().SqlServerQueue.Rate(p.SqlServerQueue.Rate);
                            progress().SqlRows(p.SqlRows);
                            progress().ElasticsearchRows(p.ElasticsearchRows);
                        } else {
                            progress().Rows(p.Rows);
                        }
                    })
                    .done(function (result) {
                        logger.info(result.message);
                        progress().busy(false);
                    });
            },
            resume = function (log) {
                progress().busy(true);
                errorRows([]);
                return hub.server.resume(ko.unwrap(model().Name), log)
                    .fail(function (e) {
                        logger.error(e);
                    })
                    .progress(function (p) {

                        if (p.Exception) {
                            progress().errors(progress().errors() + 1);
                            console.error(p.Exception.Message);
                            // do paging
                            if (errorRows().length < 20) {
                                errorRows.push(p);
                            }
                            return;
                        }

                        if (p.Rows === -1) {
                            progress().ElasticsearchQueue.MessagesCount(p.ElasticsearchQueue.MessagesCount);
                            progress().ElasticsearchQueue.Rate(p.ElasticsearchQueue.Rate);
                            progress().SqlServerQueue.MessagesCount(p.SqlServerQueue.MessagesCount);
                            progress().SqlServerQueue.Rate(p.SqlServerQueue.Rate);
                            progress().SqlRows(p.SqlRows);
                            progress().ElasticsearchRows(p.ElasticsearchRows);
                        } else {
                            progress().Rows(p.Rows);
                        }
                    })
                    .done(function (result) {
                        logger.info(result.message);
                        progress().busy(false);
                    });
            },
            ignoreRow = function (row) {
                return hub.server.ignoreRow(row.ErrorId);
            },
            importOneRow = function (row) {
                return hub.server.importOneRow(row.ErrorId, ko.mapping.toJS(model), JSON.stringify(row.Data))
                    .fail(function (e) {
                        logger.error(e);
                    })
                    .done(function (result) {
                        logger.info(result.message);
                    });
            },
            requestCancel = function () {
                return hub.server.requestCancel();
            },
            truncateData = function () {
                var tcs = new $.Deferred();

                app.showMessage("TRUNCATE all data, you'll lose all the data is SQL and Elasticsearch", "RX Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            hub.server.truncateData(ko.unwrap(model().Name), ko.mapping.toJS(model))
                                .done(tcs.resolve);
                        } else {
                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();
            },
            save = function () {
                var schedules = ko.unwrap(model().IntervalScheduleCollection),
                    list = _(schedules).map(function (v) {
                        return v.Metadata;
                    });
                model().ScheduledDataTransferCollection(list);

                var data = ko.toJSON(model);
                return context.post(data, "/api/data-imports");
            },
            removeAsync = function () {
                var data = ko.toJSON(model);
                return context.send("DELETE", data, "/api/data-imports/" + ko.unwrap(model().Id));
            };

        var vm = {
            progressData: progress,
            resume: resume,
            importOneRow: importOneRow,
            ignoreRow: ignoreRow,
            entityOptions: entityOptions,
            tableOptions: tableOptions,
            adapterOptions: adapterOptions,
            mapOptions: mapOptions,
            model: model,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            errorRows: errorRows,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAsync,
                commands: ko.observableArray([
                    {
                        caption: "Preview",
                        icon: "fa fa-th-list",
                        command: preview,
                        enable: canPreview
                    },
                    {
                        caption: "Starts",
                        icon: "fa fa-play-circle",
                        command: importData,
                        enable: canImport
                    },
                    {
                        caption: "Stop",
                        icon: "fa fa-stop-circle-o",
                        command: requestCancel,
                        enable: progress().busy
                    },
                    {
                        caption: "Truncate all data",
                        icon: "fa fa-trash-o",
                        command: truncateData,
                        enable: canImport
                    }
                ])
            }
        };

        return vm;

    });
