/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../../web/core.sph/sphapp/schemas/adapter.api.g.js" />
/// <reference path="c:\project\work\sph\source\adapters\sqlserver.adapter\_sql.server.adapter.domain.js" />
/// <reference path="../domain/domain.sph/Scripts/objectbuilders.js" />

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app, objectbuilders.system, "sqlserver-adapter/resource/_sql.server.adapter.domain.js", "ko/_ko.adapter.sqlserver"],
    function (context, logger, router, app,system) {

        var adapter = ko.observable(),
            selected = ko.observable(),
            isBusy = ko.observable(false),
            loadingDatabases = ko.observable(false),
            connected = ko.observable(false),
            errors = ko.observableArray(),
            databaseOptions = ko.observableArray(),
            connect = function () {

                var tcs = new $.Deferred(),
                    adp = ko.unwrap(adapter),
                    server = ko.unwrap(adp.Server),
                    trusted = ko.unwrap(adp.TrustedConnection),
                    userid = ko.unwrap(adp.UserId),
                    password = ko.unwrap(adp.Password);
                loadingDatabases(true);
                isBusy(true);
                $.getJSON("sqlserver-adapter/databases/?server=" + server + "&trusted=" + trusted + "&userid=" + userid + "&password=" + password)
                    .done(function (result) {
                        loadingDatabases(false);
                        if (result.success) {
                            connected(true);
                            databaseOptions(result.databases);
                            logger.info("You are now connected, please select your database");
                        } else {
                            connected(false);
                            logger.error(result.message);
                        }
                        tcs.resolve(result);
                        isBusy(false);
                    });

                return tcs.promise();
            },
            populateAdapterObjectsAsync = function (id, server, database, trusted, userid, password) {
                var tcs = new $.Deferred();
                $.getJSON("/sqlserver-adapter/" + id + "?server=" + server + "&database=" + database + "&trusted=" + trusted + "&userid=" + userid + "&password=" + password)
                    .done(function (result) {
                        var adp = context.toObservable(result);
                        tcs.resolve(adp);
                    });

                return tcs.promise();
            },
            activate = function (sid) {

                var query = String.format("Id eq '{0}'", sid),
                    b = null;
                return context.loadOneAsync("Adapter", query)
                    .then(function (result) {
                        b = result;
                        var initialized = ko.unwrap(b.Database) && ko.unwrap(b.Server);
                        if (!initialized) {
                            b.TrustedConnection(true);
                            return Task.fromResult(0);
                        }
                        adapter(new bespoke.sph.integrations.adapters.SqlServerAdapter({
                            "Server": ko.unwrap(b.Server),
                            "TrustedConnection": ko.unwrap(b.TrustedConnection),
                            "UserId": ko.unwrap(b.UserId),
                            "Password": ko.unwrap(b.Password)
                        }));
                        return connect();
                    })
                    .then(function () {

                        var adp = ko.unwrap(b),
                            server = ko.unwrap(adp.Server),
                            id = ko.unwrap(adp.Id),
                            database = ko.unwrap(adp.Database),
                            trusted = ko.unwrap(adp.TrustedConnection),
                            userid = ko.unwrap(adp.UserId),
                            password = ko.unwrap(adp.Password);
                        // now -repopulate
                        return populateAdapterObjectsAsync(id, server, database, trusted, userid, password);

                    }).then(adapter);


            },
            attached = function (view) {
                adapter().Database.subscribe(function (db) {
                    if (!db) {
                        return;
                    }
                    isBusy(true);
                    var adp = ko.unwrap(adapter),
                        server = ko.unwrap(adp.Server),
                        database = ko.unwrap(adp.Database),
                        trusted = ko.unwrap(adp.TrustedConnection),
                        userid = ko.unwrap(adp.UserId),
                        password = ko.unwrap(adp.Password);
                    populateAdapterObjectsAsync(ko.unwrap(adp.Id), server, database, trusted, userid, password)
                        .done(function (result) {
                            adapter(result);
                            isBusy(false);
                        });
                });

                var adapterTreePanel = $(view).find("#table-tree"),
                    setDesignerHeight = function () {
                        if (adapterTreePanel.length === 0) {
                            return;
                        }

                        var dev = $("#developers-log-panel").height(),
                            top = adapterTreePanel.offset().top,
                            height = dev + top;
                        adapterTreePanel.css("max-height", $(window).height() - height);

                    };
                $("#developers-log-panel-collapse,#developers-log-panel-expand").on("click", setDesignerHeight);
                setDesignerHeight();


            },
            generate = function () {

                var data = ko.mapping.toJSON(adapter);
                isBusy(true);

                return context.post(data, "/sqlserver-adapter/generate")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                        } else {
                            logger.error(result.message);
                        }
                    });


            },
            save = function () {

                // load the table
                var data = ko.mapping.toJSON(adapter);
                isBusy(true);

                return context.put(data, "/adapter/" + ko.unwrap(adapter().Id))
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            errors.removeAll();
                            logger.info("Your Sql Server adapter has been saved");

                        } else {
                            errors(result.errors);
                            logger.error("Please check for any errors in your adapter");
                        }

                    });

            },
            removeAdapter = function () {
                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want to permanently remove this adapter", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.send(ko.mapping.toJSON(adapter), "adapter", "DELETE")
                                .done(function () {
                                    router.navigate("#dev.home");
                                    tcs.resolve(dialogResult);
                                });
                        } else {
                            tcs.resolve(dialogResult);
                        }

                    });

                return tcs.promise();
            },
            editTable = function (table) {
                require(["viewmodels/_adapter.sqlserver.table.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.table(context.clone(table));
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {
                                    adapter().TableDefinitionCollection.replace(table, dialog.table());

                                }
                            });
                    });
            },
            addTable = function () {
                require(["viewmodels/_adapter.sqlserver.add.table.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.adapter(adapter());
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {
                                    _(dialog.selectedTables()).each(function (v) {
                                        adapter().TableDefinitionCollection.push(v);
                                    });
                                }
                            });
                    });
            },
            removeTable = function (table) {
                adapter().TableDefinitionCollection.remove(table);
            },
            addOperation = function () {
                require(["viewmodels/_adapter.sqlserver.add.operation.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.adapter(adapter());
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {
                                    _(dialog.selectedOperations()).each(function (v) {
                                        adapter().OperationDefinitionCollection.push(v);
                                    });
                                }
                            });
                    });
            },
            removeOperation = function (table) {
                adapter().OperationDefinitionCollection.remove(table);
            },
            showFieldDialog = function (accessor, field, path) {
                require(["viewmodels/" + path, "durandal/app"], function (dialog, app2) {
                    dialog.field(field);


                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                accessor(field);
                            }
                        });

                });
            },
            addField = function (accessor, type) {
                return function (){
                    var field = new bespoke.sph.domain[type + "Field"](system.guid());
                    showFieldDialog(accessor, field, "field." + type.toLowerCase());
                }
            },
            editField = function (field) {
                var self = this;
                return function () {
                    var fieldType = ko.unwrap(field.$type),
                        clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                        pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                        type = pattern.exec(fieldType)[1];


                    showFieldDialog(self.Field, clone, "field." + type.toLowerCase());
                };
            }
            ;

        var vm = {
            errors: errors,
            addTable: addTable,
            addField: addField,
            editField: editField,
            addOperation: addOperation,
            removeTable: removeTable,
            removeOperation: removeOperation,
            databaseOptions: databaseOptions,
            adapter: adapter,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            editTable: editTable,
            selected: selected,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAdapter,
                commands: ko.observableArray([
                    {
                        caption: "Connect",
                        icon: "fa fa-exchange",
                        command: connect
                    },
                    {
                        caption: "Publish",
                        icon: "fa fa-sign-in",
                        command: generate,
                        enable: connected
                    }
                ])
            }
        };
        return vm;
    });
