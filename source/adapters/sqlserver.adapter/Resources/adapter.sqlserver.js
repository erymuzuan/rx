/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var adapter = ko.observable(),
            isBusy = ko.observable(false),
            loadingSchemas = ko.observable(false),
            loadingDatabases = ko.observable(false),
            connected = ko.observable(false),
            databaseOptions = ko.observableArray(),
            schemaOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            sprocOptions = ko.observableArray(),
            activate = function (sid) {
                if (parseInt(sid) === 0) {
                    adapter({
                        $type: "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter",
                        Name: ko.observable(),
                        Description: ko.observable(),
                        Server: ko.observable('(localdb)\\ProjectsV12'),
                        TrustedConnection: ko.observable(true),
                        UserId: ko.observable(),
                        Password: ko.observable(),
                        Database: ko.observable(),
                        Schema: ko.observable()
                    });
                }

            },
            attached = function (view) {
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

                        var tables = _(result.tables).map(function(v) {
                            return {
                                name: v,
                                children: ko.observableArray(),
                                busy : ko.observable(false)
                            };
                        });
                        tableOptions(tables);
                        sprocOptions(result.sprocs);
                        loadingSchemas(false);
                        logger.info("You are now connected, please select your schema");
                    });
                });

                $('#table-options-panel').on('click', 'input[type=checkbox]', function() {
                    var table = ko.dataFor(this);
                    table.busy(true);
                    setTimeout(function() {
                        table.children.push('test');
                        table.children.push('test 2');
                        table.busy(false);
                    }, 5000);
                });
            },
            connect = function () {
                var tcs = new $.Deferred();
                loadingSchemas(true);
                loadingDatabases(true);
                context.post(ko.mapping.toJSON(adapter), "sqlserver-adapter/databases")
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
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            generate = function () {

            },
            save = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/sph/adapter/save")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            }
        ;

        var vm = {
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
