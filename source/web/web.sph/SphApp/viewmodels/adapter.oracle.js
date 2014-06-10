/// <reference path="../../Scripts/jquery-2.1.0.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var adapter = ko.observable(
            {
                ConnectionString: ko.observable(),
                Schema: ko.observable(),
                Name: ko.observable(),
                Description: ko.observable(),
                Table: ko.observable()
            }),
            schemaOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function () {
                adapter().ConnectionString.subscribe(function (cs) {
                    isBusy(true);
                    $.get("/oracleadapter/schemas?cs=" + cs)
                        .done(function (result) {
                            isBusy(false);
                            schemaOptions(result);
                        });
                });
                adapter().Schema.subscribe(function (schema) {
                    isBusy(true);
                    var cs = adapter().ConnectionString();
                    $.get("/oracleadapter/tables/" + schema + "?cs=" + cs)
                        .done(function (result) {
                            isBusy(false);
                            tableOptions(result);
                        });
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
