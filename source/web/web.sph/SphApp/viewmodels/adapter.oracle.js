/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context) {

        var adapter = ko.observable(
            {
                connectionString: ko.observable(),
                schema: ko.observable(),
                name: ko.observable(),
                description: ko.observable(),
                tables: ko.observableArray()
            }),
            schemaOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {
                adapter().connectionString.subscribe(function (cs) {
                    isBusy(true);
                    $.get("/oracleadapter/schemas?cs=" + cs)
                        .done(function (result) {
                            isBusy(false);
                            schemaOptions(result);
                        });
                });
                adapter().schema.subscribe(function (schema) {
                    isBusy(true);
                    var cs = adapter().connectionString();
                    $.get("/oracleadapter/tables/" + schema + "?cs=" + cs)
                        .done(function (result) {
                            isBusy(false);
                            var tables = _(result).map(function (v) {
                                return {
                                    name: v,
                                    parents: ko.observableArray(),
                                    children: ko.observableArray()
                                };
                            });
                            tableOptions(tables);
                        });
                });

                $(view).on('click', 'input[type=checkbox]', function () {
                    var table = ko.dataFor(this);
                    console.log(table.name);
                    var o = {
                        connectionString: adapter().connectionString(),
                        schema: adapter().schema(),
                        table: table.name
                    };
                    var tcs = new $.Deferred();
                    var data = JSON.stringify(o);
                    isBusy(true);

                    context.post(data, "/oracleadapter/relationships")
                        .then(function (result) {
                            isBusy(false);
                            var children = _(result).filter(function (v) {
                                return v.fk_table_name === table.name;
                            });
                            table.children(children);
                            var parents = _(result).filter(function (v) {
                                return v.src_table_name === table.name;
                            });
                            table.parents(parents);

                            tcs.resolve(result);
                        });
                    return tcs.promise();

                });
            },
            generate = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/oracleadapter/generate")
                    .done(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            generate: generate,
            adapter: adapter,
            schemaOptions: schemaOptions,
            tableOptions: tableOptions,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
