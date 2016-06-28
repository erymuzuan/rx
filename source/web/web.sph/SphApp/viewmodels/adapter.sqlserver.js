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
    function (context, logger, router, app, system) {

        var adapter = ko.observable(),
            selected = ko.observable(),
            isBusy = ko.observable(false),
            loadingDatabases = ko.observable(false),
            connected = ko.observable(false),
            errors = ko.observableArray(),
            databaseOptions = ko.observableArray(),
            tableNameOptions = ko.observableArray(),
            lookupColumnOptions = ko.observableArray(),
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
                    .then(function (result) {
                        loadingDatabases(false);
                        if (result.success) {
                            connected(true);
                            databaseOptions(result.databases);

                            if (ko.isObservable(adapter().Version))
                                adapter().Version(result.version);
                            else
                                adapter().Version = ko.observable(result.version);


                            logger.info("You are now connected, please select your database");
                        } else {
                            connected(false);
                            logger.error(result.message);
                        }
                        tcs.resolve(result);
                        isBusy(false);
                    }).fail(function (e) {
                    if (e.status === 502) {

                        connected(false);
                        logger.error(e.responseJSON.Message|| e.responseText);
                        tcs.resolve(false);
                        isBusy(false);
                    }
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

                // get the options for lookup table
                $.getJSON("/sqlserver-adapter/table-options?server=" + server + "&database=" + database + "&trusted=" + trusted + "&userid=" + userid + "&password=" + password)
                    .done(function (result) {
                        var options = _(result).map(function (v) {
                            return "[" + v.Schema + "].[" + v.Name + "]";
                        });

                        tableNameOptions(options);
                    });

                return tcs.promise();
            },
            activate = function (sid) {

                errors.removeAll();
                var query = String.format("Id eq '{0}'", sid);
                return context.loadOneAsync("Adapter", query)
                    .then(function (result) {
                        var initialized = ko.unwrap(result.Database) && ko.unwrap(result.Server);
                        if (!initialized) {
                            result.TrustedConnection(true);
                            return Task.fromResult(0);
                        }
                        // get optiosn for lookup from existing lookup
                        var lookupOptions = [];
                        _(result.TableDefinitionCollection()).each(function (t) {
                            _(t.ColumnCollection()).each(function (c) {
                                var table = ko.unwrap(c.LookupColumnTable().Table);
                                if (table)
                                    lookupOptions.push(table);
                            });
                        });
                        tableNameOptions(lookupOptions);
                        databaseOptions.push(ko.unwrap(result.Database));
                        adapter(result);
                    });


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

                var loadLookupColumnOptions = function (table, lookup) {
                    var splits = table.split("].["),
                        schema = splits[0].replace(/\[/g, "").replace(/]/g, ""),
                        name = splits[1].replace(/\[/g, "").replace(/]/g, ""),
                        adp = ko.unwrap(adapter),
                        server = ko.unwrap(adp.Server),
                        database = ko.unwrap(adp.Database),
                        trusted = ko.unwrap(adp.TrustedConnection),
                        userid = ko.unwrap(adp.UserId),
                        password = ko.unwrap(adp.Password),
                        showLoading = function () {
                            var html =
                                ' <div>' +
                                ' <select class="form-control" style="display: inline"  data-bind="enable:IsEnabled">' +
                                ' </select>' +
                                ' <i class="fa fa-spinner fa-spin fa-fw pull-left" style="margin-left: 5px;margin-top: -20px" data-bind="visible:IsEnabled"></i>' +
                                ' </div>';
                            $("div#value-column-form-group").html('<label>Value Column</label>' + html);
                            $("div#key-column-form-group").html('<label>Key Column</label>' + html);
                        },
                        hideLoading = function () {
                        };

                    showLoading();


                    // get the options for lookup table
                    return $.getJSON("/sqlserver-adapter/table-options/" + schema + "/" + name + "/?server=" + server + "&database=" + database + "&trusted=" + trusted + "&userid=" + userid + "&password=" + password)
                        .then(function (result) {
                            var td = context.toObservable(result),
                                options = [""];

                            _(td.ColumnCollection()).each(function (v) {
                                options.push(ko.unwrap(v.Name));
                            });

                            var valueList = _(options).map(function (v) {
                                    var selected = ko.unwrap(lookup.ValueColumn) === v ? " selected" : "";
                                    return '<option value="' + v + '"' + selected + '>' + v + "</option>";
                                }),
                                keyList = _(options).map(function (v) {
                                    var selected = ko.unwrap(lookup.KeyColumn) === v ? " selected" : "";
                                    return '<option value="' + v + '"' + selected + '>' + v + "</option>";
                                });

                            lookupColumnOptions(options);
                            $("div#key-column-form-group")
                                .html('<label class="control-label" for="key-column-table">Key Column</label><select class="form-control key-column">' + keyList + '</select>');
                            $("div#value-column-form-group")
                                .html('<label class="control-label" for="value-column-table">Value Column</label><select class="form-control value-column">' + valueList + '</select>');
                        }).fail(function (e) {
                            if (e.responseJSON) {
                                logger.error(e.responseJSON.Message, e.responseJSON);
                            } else {
                                logger.error(e.responseText);
                            }

                        }).done(hideLoading);
                };

                selected.subscribe(function (column) {
                    if (!column) return;
                    if (!ko.isObservable(column.LookupColumnTable)) return;
                    if (!ko.unwrap(column.LookupColumnTable().IsEnabled)) return;
                    if (!ko.unwrap(connected)){
                        var value = column.LookupColumnTable().ValueColumn(),
                            valueList = '<option value="' + value + '"' + selected + '>' + value + "</option>",
                            key = column.LookupColumnTable().KeyColumn(),
                            keyList =  '<option value="' + key + '" selected >' + key + "</option>";
                        $("div#key-column-form-group")
                            .html('<label class="control-label" for="key-column-table">Key Column</label><select class="form-control key-column">' + keyList + '</select>');
                        $("div#value-column-form-group")
                            .html('<label class="control-label" for="value-column-table">Value Column</label><select class="form-control value-column">' + valueList + '</select>');
                        return;
                    }


                    var table = ko.unwrap(column.LookupColumnTable().Table),
                        plain = ko.toJS(column.LookupColumnTable);
                    if (!table)return;
                    lookupColumnOptions([]);
                    console.log("Looup table changes %s", table);
                    loadLookupColumnOptions(table, column.LookupColumnTable())
                        .done(function () {
                            column.LookupColumnTable().KeyColumn(plain.KeyColumn);
                            column.LookupColumnTable().ValueColumn(plain.ValueColumn);

                        });


                });
                $(view).on("change", "#lookup-table", function () {
                    var table = $(this).val();
                    if (!table)return;
                    console.log("Lookup table changes %s", table);
                    loadLookupColumnOptions(table, ko.dataFor(this));

                });


                $(view).on("change", "select.key-column", function () {

                    selected().LookupColumnTable().KeyColumn($(this).val());

                });

                $(view).on("change", "select.value-column", function () {
                    selected().LookupColumnTable().ValueColumn($(this).val());
                });


            },
            publishAsync = function () {

                var data = ko.mapping.toJSON(adapter);
                isBusy(true);

                return context.post(data, "/sqlserver-adapter/publish")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {
                            logger.error(result.message);
                            errors(result.errors);
                        }
                    });


            },
            viewFile = function (e) {
                var file = e.FileName || e,
                    line = e.Line || 1;
                var params = [
                        "height=" + screen.height,
                        "width=" + screen.width,
                        "toolbar=0",
                        "location=0",
                        "fullscreen=yes"
                    ].join(","),
                    editor = window.open("/sph/editor/file?id=" + file.replace(/\\/g, "/") + "&line=" + line, "_blank", params);
                editor.moveTo(0, 0);
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
                return function () {
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
            tableNameOptions: tableNameOptions,
            lookupColumnOptions: lookupColumnOptions,
            adapter: adapter,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            viewFile: viewFile,
            editTable: editTable,
            selected: selected,
            connected :connected,
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
                        caption: "Refresh",
                        tooltip: "Refresh adapter objects metada from the database",
                        icon: "fa fa-refresh",
                        command: connect,
                        enable: connected
                    },
                    {
                        caption: "Publish",
                        icon: "fa fa-sign-in",
                        command: publishAsync,
                        tooltip: "Compile the adapter",
                        enable: ko.computed(function () {
                            if (!ko.unwrap(adapter))return false;
                            return adapter().TableDefinitionCollection().length > 0 || adapter().OperationDefinitionCollection().length > 0;
                        })
                    }
                ])
            }
        };
        return vm;
    });
