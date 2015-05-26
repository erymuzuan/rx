/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.app],
    function (context, logger, router, app) {

        var adapter = ko.observable(),
            isBusy = ko.observable(false),
            loadingSchemas = ko.observable(false),
            loadingDatabases = ko.observable(false),
            connected = ko.observable(false),
            errors = ko.observableArray(),
            databaseOptions = ko.observableArray(),
            schemaOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            sprocOptions = ko.observableArray(),
            selectedTables = ko.observableArray(),
            activate = function (sid) {
                if (!sid || sid === "0") {
                    adapter({
                        $type: "Bespoke.Sph.Integrations.Adapters.MySqlAdapter, mysql.adapter",
                        Id: ko.observable("0"),
                        Name: ko.observable(),
                        Description: ko.observable(),
                        Server: ko.observable('localhost'),
                        UserId: ko.observable('root'),
                        Password: ko.observable(),
                        Database: ko.observable(),
                        Schema: ko.observable(),
                        OperationDefinitionCollection: ko.observableArray(),
                        Tables: selectedTables
                    });

                    return null;
                }

                var query = String.format("Id eq '{0}'", sid),
                    tcs = new $.Deferred();

                context.loadOneAsync("Adapter", query)
                    .done(function (b) {

                        var loadSchemaTask = context.post(ko.mapping.toJSON(b), "mysql-adapter/schemas"),
                            loadTablesSprocsTask = context.post(ko.mapping.toJSON(b), "mysql-adapter/objects"),
                            loadDatabasesTask = connect(b);


                        $.when(loadDatabasesTask, loadSchemaTask, loadTablesSprocsTask).done(function (databases, schemaResult, result) {
                            schemaOptions(schemaResult[0].schema);
                            databaseOptions(databases.databases);
                            var objects = context.toObservable(result[0]),
                                tables = _(objects.tables()).map(function (v) {
                                return {
                                    name: v,
                                    children: ko.observableArray(),
                                    selectedChildren: ko.observableArray(),
                                    busy: ko.observable(false)
                                };
                            });
                            tableOptions(tables);
                            sprocOptions(objects.sprocs());

                            adapter(b);

                            tcs.resolve(true);
                        });
                    });

                return tcs.promise();

            },
            attached = function (view) {
                adapter().Database.subscribe(function (db) {
                    if (!db) {
                        return;
                    }
                    loadingSchemas(true);
                    context.post(ko.mapping.toJSON(adapter), "mysql-adapter/schemas").done(function (result) {
                        schemaOptions(result.schema);
                        loadingSchemas(false);
                        logger.info("You are now connected, please select your schema");
                    });
                });
                adapter().Schema.subscribe(function (db) {
                    if (!db) {
                        return;
                    }
                    loadingSchemas(true);
                    context.post(ko.mapping.toJSON(adapter), "mysql-adapter/objects").done(function (result) {

                        var tables = _(result.tables.$values).map(function (v) {
                            return {
                                name: v,
                                children: ko.observableArray(),
                                selectedChildren: ko.observableArray(),
                                busy: ko.observable(false)
                            };
                        });

                        tableOptions(tables);

                        if (result.sprocs.length > 1) {
                            sprocOptions(result.sprocs);
                        }
                        

                        loadingSchemas(false);
                        logger.info("You are now connected, please select your schema");
                    });
                });


                $('#table-options-panel').on('click', 'input[type=checkbox]', function () {
                    var table = ko.dataFor(this),
                        checkbox = $(this);

                    // when children is selected
                    if (typeof table.busy !== "function") {
                        table = ko.dataFor(checkbox.parents('ul')[0]);
                        var at0 = _(selectedTables()).find(function (v) {
                            return v.Name == table.name;
                        }),
                            child = ko.dataFor(this);
                        if (checkbox.is(':checked')) {
                            at0.ChildRelationCollection.push(child);
                        } else {
                            var ct = _(at0.ChildRelationCollection()).find(function (v) { return v.Name === child.Name; });
                            table.selectedChildren.remove(ct);
                        }
                        return;
                    }


                    if (!checkbox.is(':checked')) {
                        table.children.removeAll();

                        var at = _(selectedTables()).find(function (v) {
                            return v.Name == table.name;
                        });

                        selectedTables.remove(at);
                        return;
                    } else {
                        var adapterTable = {
                            Name: table.name,
                            ChildRelationCollection: ko.observableArray()
                        }
                        var existingTable = _(selectedTables()).find(function (v) { return v.Name === table.name; });
                        if (!existingTable) {
                            selectedTables.push(adapterTable);
                        }
                    }

                    table.busy(true);
                    context.post(ko.mapping.toJSON(adapter), "mysql-adapter/children/" + table.name)
                        .done(function (result) {
                            table.children(result.children);
                            table.busy(false);
                        });
                    setTimeout(function () {
                        table.busy(false);
                    }, 5000);
                });



                $('#sproc-option-panel').on('click', 'input[type=checkbox]', function () {
                    var o = { $type: "Bespoke.Sph.Integrations.Adapters.SprocOperationDefinition, mysql.adapter" },
                        sproc = _(o).extend(ko.dataFor(this)),
                        checkbox = $(this);

                    if (checkbox.is(':checked')) {
                        adapter().OperationDefinitionCollection.push(sproc);
                    } else {

                        var sproc2 = _(adapter().OperationDefinitionCollection()).find(function (v) {
                            return ko.unwrap(v.Name) === ko.unwrap(sproc.Name);
                        });
                        if (sproc2) {
                            adapter().OperationDefinitionCollection.remove(sproc2);
                        }
                    }

                });



                if (ko.unwrap(adapter().Id) && ko.unwrap(adapter().Id) !== "0") {
                    // check the sproc
                    _(adapter().OperationDefinitionCollection()).each(function (v) {
                        var chb = $('input[name=sproc-' + ko.unwrap(v.Name) + ']');
                        chb.prop('checked', true);
                    });

                    // trigger the checks for each selected table
                    _(adapter().Tables()).each(function (v) {
                        var chb = $('input[name=table-' + ko.unwrap(v.Name) + ']'),
                            table = ko.dataFor(chb[0]);
                        chb.trigger('click');
                        // do it for the child
                        var it = setInterval(function () {
                            if (!table.busy()) {
                                _(v.ChildRelationCollection()).each(function (ct) {
                                    console.log("child ", ko.unwrap(ct.Table));
                                    $('input[name=child-' + ko.unwrap(v.Name) + '-' + ko.unwrap(ct.Table) + ']').trigger('click');
                                });
                                clearInterval(it);
                            }
                        }, 200);

                    });
                    adapter().Tables = selectedTables();
                }

            },
            connect = function (adp) {
                if (!adp) {
                    adp = adapter;
                }
                if (typeof adp.Database !== "function") {
                    adp = adapter;
                }
                var tcs = new $.Deferred();
                loadingSchemas(true);
                loadingDatabases(true);
                context.post(ko.mapping.toJSON(adp), "mysql-adapter/databases")
                    .done(function (result) {
                        loadingSchemas(false);
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
                    });

                return tcs.promise();
            },
            generate = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/mysql-adapter/generate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();

            },
            save = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/adapter")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            adapter().Id(result.id);
                            errors.removeAll();

                        } else {
                            errors(result.errors);
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            removeAdapter = function () {

                return app.showMessage("Are you sure you want to permanently remove this adapter", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            return context.send(ko.mapping.toJSON(adapter), "adapter", "DELETE");
                        }
                    });
            }
        ;

        var vm = {
            errors: errors,
            schemaOptions: schemaOptions,
            databaseOptions: databaseOptions,
            tableOptions: tableOptions,
            sprocOptions: sprocOptions,
            adapter: adapter,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAdapter,
                commands: ko.observableArray([
                    {
                        caption: 'Connect',
                        icon: 'fa fa-exchange',
                        command: connect
                    },
                    {
                        caption: 'Publish',
                        icon: 'fa fa-sign-in',
                        command: generate,
                        enable: connected
                    }
                ])
            }
        };

        return vm;

    });
