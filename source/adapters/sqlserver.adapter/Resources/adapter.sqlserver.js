﻿/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../domain/domain.sph/Scripts/objectbuilders.js" />

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
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
                context.post(ko.mapping.toJSON(adp), "sqlserver-adapter/databases")
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
            activate = function (sid) {

                var query = String.format("Id eq '{0}'", sid),
                    tcs = new $.Deferred();

                context.loadOneAsync("Adapter", query)
                    .done(function (b) {
                        var initialized = ko.unwrap(b.Schema) && ko.unwrap(b.Database);
                        if (!initialized) {
                            b.TrustedConnection(true);
                            adapter(b);
                            tcs.resolve(true);
                            return;
                        }
                        var loadSchemaTask = context.post(ko.mapping.toJSON(b), "sqlserver-adapter/schema"),
                            loadTablesSprocsTask = context.post(ko.mapping.toJSON(b), "sqlserver-adapter/objects"),
                            loadDatabasesTask = connect(b);


                        $.when(loadDatabasesTask, loadSchemaTask, loadTablesSprocsTask).done(function (databases, schemaResult, result) {
                            schemaOptions(schemaResult[0].schema);
                            databaseOptions(databases.databases);
                            var tables = _(result[0].tables).map(function (v) {
                                return {
                                    name: v.name,
                                    children: ko.observableArray(),
                                    selectedChildren: ko.observableArray(),
                                    busy: ko.observable(false),
                                    checked: ko.observable(false),
                                    versionColumn: ko.observable(),
                                    versionColumnOptions: ko.observableArray(v.rowVersionColumnOptions),
                                    modifiedDateColumn: ko.observable(),
                                    modifiedDateColumnOptions: ko.observableArray(v.modifiedDateColumnOptions)
                                };
                            });
                            tableOptions(tables);
                            var sprocs = _(result[0].sprocs).map(function (v) {
                                var sp = {
                                    $type: "Bespoke.Sph.Integrations.Adapters.SprocOperationDefinition, sqlserver.adapter",
                                    checked: ko.observable(false)
                                };
                                if(v.requestMemberCollection){
                                   var members =  _(v.requestMemberCollection).map(function(k){
                                       var bp = {$type : "Bespoke.Sph.Integrations.Adapters.SprocParameter, sqlserver.adapter"};
                                       return _(bp).extend(k);
                                   });
                                   v.requestMemberCollection = members;
                                }
                                return _(sp).extend(v);
                            });
                            sprocOptions(sprocs);
                            adapter(b);

                            tcs.resolve(true);
                        });
                    });

                return tcs.promise();

            },
            attached = function () {
                adapter().Database.subscribe(function (db) {
                    if (!db) {
                        return;
                    }
                    loadingSchemas(true);
                    context.post(ko.mapping.toJSON(adapter), "sqlserver-adapter/schema").done(function (result) {
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
                    context.post(ko.mapping.toJSON(adapter), "sqlserver-adapter/objects").done(function (result) {

                        var tables = _(result.tables).map(function (v) {
                            return {
                                name: v.name,
                                children: ko.observableArray(),
                                selectedChildren: ko.observableArray(),
                                busy: ko.observable(false),
                                checked: ko.observable(false),
                                versionColumn: ko.observable(),
                                versionColumnOptions: ko.observableArray(v.rowVersionColumnOptions),
                                modifiedDateColumn: ko.observable(),
                                modifiedDateColumnOptions: ko.observableArray(v.modifiedDateColumnOptions)
                            };
                        });
                        tableOptions(tables);

                        var sprocs = _(result.sprocs).map(function (v) {
                            var sp = {
                                $type: "Bespoke.Sph.Integrations.Adapters.SprocOperationDefinition, sqlserver.adapter",
                                checked: ko.observable(false)
                            };
                            if (v.requestMemberCollection) {
                                var members = _(v.requestMemberCollection).map(function (k) {
                                    var bp = { $type: "Bespoke.Sph.Integrations.Adapters.SprocParameter, sqlserver.adapter" };
                                    return _(bp).extend(k);
                                });
                                v.requestMemberCollection = members;
                            }
                            return _(sp).extend(v);
                        });
                        sprocOptions(sprocs);
                        loadingSchemas(false);
                        logger.info("You are now connected, please select your schema");
                    });
                });


                $("#table-options-panel").on("click", "input[type=checkbox]", function () {
                    var table = ko.dataFor(this),
                        checkbox = $(this);

                    // when children is selected
                    if (typeof table.busy !== "function") {
                        table = ko.dataFor(checkbox.parents("ul")[0]);
                        var parent = _(selectedTables()).find(function (v) {
                            return v.Name === table.name;
                        }),
                        child = ko.dataFor(this);
                        if (checkbox.is(":checked")) {
                            parent.ChildRelationCollection.push(child);
                        } else {
                            var ct = _(parent.ChildRelationCollection()).find(function (v) { return v.Table === child.Table; });
                            table.selectedChildren.remove(ct);
                            parent.ChildRelationCollection.remove(ct);
                        }
                        return;
                    }


                    if (!checkbox.is(":checked")) {
                        table.children.removeAll();

                        var at = _(selectedTables()).find(function (v) {
                            return v.Name === table.name;
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
                    context.post(ko.mapping.toJSON(adapter), "sqlserver-adapter/children/" + table.name)
                        .done(function (result) {
                            _(result.children)
                                .each(function (v) {
                                    v.checked = ko.observable(false);
                                });
                            table.children(result.children);
                            table.busy(false);
                        });
                    setTimeout(function () {
                        table.busy(false);
                    }, 5000);
                });



                $("#sproc-option-panel").on("click", "input[type=checkbox]", function () {
                    var sproc = ko.dataFor(this),
                        checkbox = $(this);

                    // remove any matching, duplicate sprocs
                    var sproc2 = _(adapter().OperationDefinitionCollection()).find(function (v) {
                        var name1 = ko.unwrap(v.Name) || ko.unwrap(v.name),
                            name2 = ko.unwrap(sproc.Name) || ko.unwrap(sproc.name);
                        return name1 === name2;
                    });

                    if (checkbox.is(":checked")) {
                        // fix the @return_value
                        var return_value = ko.unwrap(sproc.ResponseMemberCollection || sproc.responseMemberCollection)[0],
                            withType =  _({$type : "Bespoke.Sph.Integrations.Adapters.SprocResultMember, sqlserver.adapter"})
                                    .extend(return_value);
                        if(ko.isObservable(sproc.ResponseMemberCollection)){
                            sproc.ResponseMemberCollection.replace(return_value, withType);
                        }
                        if(sproc.responseMemberCollection){
                            sproc.responseMemberCollection.splice(0,1, withType);
                        }
                        if (!sproc2) {
                            adapter().OperationDefinitionCollection.push(sproc);
                        }
                    } else {
                        if (sproc2) {
                            adapter().OperationDefinitionCollection.remove(sproc2);
                        }
                    }
                });

                // check the sproc
                _(adapter().OperationDefinitionCollection()).each(function (v) {
                    var chb = $("input[name=sproc-" + ko.unwrap(v.Name) + "]"),
                        findOneInOptions = _(sprocOptions()).find(function(o){
                                var name1 = ko.unwrap(v.Name) || ko.unwrap(v.name),
                                    name2 = ko.unwrap(o.Name) || ko.unwrap(o.name);
                                return name1 === name2;

                        });
                    // we have to find one that match this sproc in options and replace it,
                    // just to make sure when user check and uncheck the checkbox, the same object is still there
                    v.checked = ko.observable(true);
                    v.name = v.Name;
                    sprocOptions.replace(findOneInOptions, v);
                    chb.trigger("click");
                });

                // trigger the checks for each selected table
                _(adapter().Tables()).each(function (v) {
                    var chb = $("input[name=table-" + ko.unwrap(v.Name) + "]"),
                        table = ko.dataFor(chb[0]);
                    chb.trigger("click");
                    table.versionColumn(ko.unwrap(v.VersionColumn));
                    table.modifiedDateColumn(ko.unwrap(v.ModifiedDateColumn));

                    // do it for the child
                    var it = setInterval(function () {
                        if (!table.busy()) {
                            _(v.ChildRelationCollection()).each(function (ct) {
                                console.log("child ", ko.unwrap(ct.Table));
                                $("input[name=child-" + ko.unwrap(v.Name) + "-" + ko.unwrap(ct.Table) + "]").trigger("click");
                            });
                            clearInterval(it);
                        }
                    }, 200);

                });
                adapter().Tables = selectedTables();


            },
            generate = function () {

                _(ko.unwrap(selectedTables)).each(function (v) {
                    var opt = _(tableOptions()).find(function (x) {
                        return ko.unwrap(v.Name) === ko.unwrap(x.name);
                    });
                    v.VersionColumn = ko.unwrap(opt.versionColumn);
                    v.ModifiedDateColumn = ko.unwrap(opt.modifiedDateColumn);
                });
                adapter().Tables = selectedTables();

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
                _(ko.unwrap(selectedTables)).each(function (v) {
                    var opt = _(tableOptions()).find(function (x) {
                        return ko.unwrap(v.Name) === ko.unwrap(x.name);
                    });
                    v.VersionColumn = ko.unwrap(opt.versionColumn);
                    v.ModifiedDateColumn = ko.unwrap(opt.modifiedDateColumn);
                });
                adapter().Tables = selectedTables();
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
                console.log(table);
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
            editTable: editTable,
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
