/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger) {

        var adapter = ko.observable(
            {
                server: ko.observable("i90009638.cloudapp.net"),
                userId: ko.observable("system"),
                password: ko.observable("gsxr750wt"),
                sid: ko.observable("XE"),
                port: ko.observable(1521),
                schema: ko.observable(),
                name: ko.observable(),
                description: ko.observable(),
                tables: ko.observableArray()
            }),
            loadingSchemas = ko.observable(),
            connected = ko.observable(false),
            loadingTables = ko.observable(),
            errors = ko.observableArray(),
            schemaOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {

                adapter().schema.subscribe(function () {
                    loadingTables(true);
                    context.post(ko.mapping.toJSON(adapter), "/oracleadapter/tables/")
                        .done(function (result) {
                            loadingTables(false);
                            var tables = _(result.tables).each(function (v) {
                                // extend it
                                v.parents = ko.observableArray();
                                v.children = ko.observableArray();
                            });
                            tableOptions(tables);
                        });
                });

                $(view).on('click', 'input.table-checkbox', function () {
                    var table = ko.dataFor(this);
                    if ($(this).is(':checked')) {
                        adapter().tables.push(table);
                    } else {
                        adapter().tables.remove(table);
                    }
                });
            },
            generate = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/oracleadapter/generate")
                    .done(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            logger.info(result.result.Output);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your adapter setting, !!!");
                        }
                        tcs.resolve(result);

                    });
                return tcs.promise();
            },
            connect = function () {
                var tcs = new $.Deferred();
                loadingSchemas(true);
                context.post(ko.mapping.toJSON(adapter), "/oracleadapter/schemas")
                    .done(function (result) {
                        loadingSchemas(false);
                        if (result.success) {
                            connected(true);
                            schemaOptions(result.schemas);
                            logger.info("You are now connected, please select your schema");
                        } else {
                            connected(false);
                            logger.error(result.message);
                        }
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/oracleadapter/save")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            errors: errors,
            loadingSchemas: loadingSchemas,
            loadingTables: loadingTables,
            connected: connected,
            generate: generate,
            adapter: adapter,
            schemaOptions: schemaOptions,
            tableOptions: tableOptions,
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
