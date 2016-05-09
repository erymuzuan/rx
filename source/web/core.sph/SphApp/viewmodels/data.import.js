/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery.signalR-2.2.0.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="~/Scripts/_task.js" />

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {

        var model = ko.observable({
            delayThrottle: ko.observable(),
            name: ko.observable(),
            adapter: ko.observable(),
            table: ko.observable(),
            batchSize: ko.observable(40),
            entity: ko.observable(),
            sql: ko.observable(),
            sqlRetry: ko.observable(),
            esRetry: ko.observable(),
            sqlWait: ko.observable(),
            esWait: ko.observable(),
            map: ko.observable(),
            ignoreMessaging: ko.observable(true)
        }),
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
                return model().adapter()
                    && model().table()
                    && model().sql()
                    && model().batchSize();
            }),
            canImport = ko.computed(function () {
                return canPreview() && model().map() && model().entity();
            }),
            activate = function () {
                console.log(router);
                context.getTuplesAsync("EntityDefinition", null, "Id", "Name")
                    .done(entityOptions);
                context.getTuplesAsync("Adapter", null, "Id", "Name")
                    .done(adapterOptions);

                return $.getScript("/signalr/js")
                            .then(function () {
                                connection = $.connection.hub;
                                hub = $.connection.dataImportHub;

                                return connection.start();
                            });
            },
            modelChanged = function () {
                var cloned = ko.toJS(model);

                model().adapter.subscribe(function (a) {
                    if (!a) {
                        return;
                    }
                    isBusy(true);
                    context.loadOneAsync("Adapter", String.format("Id eq '{0}'", a))
                    .done(function (adp) {
                        tableOptions(adp.TableDefinitionCollection().map(ko.mapping.toJS));
                        isBusy(false);

                        model().table(cloned.table);
                    });
                });

                var filterMapOptions = function (list) {
                    var filtered = _(list).filter(function (v) {
                        return v.InputTypeName.indexOf(model().table()) > -1 && v.OutputTypeName.indexOf(model().entity()) > -1;
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

                    };
                model().table.subscribe(function (a) {
                    var sql1 = String.format("select * from {0} ", a),
                        sql0 = model().sql();
                    if (sql0.indexOf(a) < -1)
                        model().sql(sql1);

                    if (!(a && model().entity())) {
                        return Task.fromResult(0);
                    }
                    return context.loadAsync("TransformDefinition")
                    .done(loadMappings);
                });
                model().entity.subscribe(function (a) {
                    if (!(a && model().entity())) {
                        return Task.fromResult(0);
                    }
                    return context.loadAsync("TransformDefinition")
                    .done(loadMappings)
                    .done(function () {
                        model().map(cloned.map);
                    });
                });

                if (cloned.table) {
                    model().adapter("");
                    model().adapter(cloned.adapter);
                    model().table(cloned.table);
                }
                if (cloned.map) {
                    model().entity("");
                    model().entity(cloned.entity);
                    model().map(cloned.map);
                }
            },
            attached = function (view) {
                modelChanged(view);
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
                return hub.server.execute(ko.unwrap(model().Name), ko.mapping.toJS(model))
                        .fail(function (e) {
                            logger.error(e);
                        })
                        .progress(function (p) {

                            if (p.Exception) {
                                logger.error(p.Exception.Message);
                                errorRows.push(p);
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
            ignoreRow = function(row) {
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
                var data = ko.toJSON(model);
                return context.post(data, "/api/data-imports");
            },
            removeAsync = function () {
                var data = ko.toJSON(model);
                return context.send("DELETE", data, "/api/data-imports/" + ko.unwrap(model().Name));
            },
            openSnippet = function () {
                var tcs = new $.Deferred();

                require(["viewmodels/data.import.list", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                isBusy(true);

                                var md = model(),
                                    di = dialog.model();
                                md.adapter(di.adapter);
                                md.entity(di.entity);
                                md.sql(di.sql);
                                md.batchSize(di.batchSize);
                                md.name(di.name);
                                md.delayThrottle(di.delayThrottle);
                                md.sqlRetry(di.sqlRetry);
                                md.sqlWait(di.sqlWait);
                                md.esRetry(di.esRetry);
                                md.esWait(di.esWait);

                                modelChanged();

                                setTimeout(function () {
                                    md.table(di.table);

                                    setTimeout(function () {
                                        md.map(di.map);
                                        tcs.resolve(true);
                                        isBusy(false);
                                    }, 800);

                                }, 500);
                            } else {
                                tcs.resolve(false);
                                isBusy(false);
                            }
                        });

                });


                return tcs.promise();

            };

        var vm = {
            progressData: progress,
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
                openCommand: openSnippet,
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
