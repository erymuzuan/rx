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
            activate = function (id) {
                var query = String.format("AdapterId eq {0}", id),
                    tcs = new $.Deferred();
                context.loadOneAsync("Adapter", query)
                    .done(function (b) {
                        if (b) {
                            adapter(b);
                        } else {
                            adapter(
                                {
                                    $type: "Bespoke.Sph.Integrations.Adapters.HttpAdapter, http.adapter",
                                    Har: ko.observable(),
                                    AdapterId: ko.observable(0),
                                    BaseAddress: ko.observable(""),
                                    AuthenticationMode: ko.observable("Form"),
                                    Schema: ko.observable(),
                                    Name: ko.observable(),
                                    Description: ko.observable(),
                                    OperationDefinitionCollection: ko.observableArray(),
                                    Timeout: ko.observable(),
                                    TimeoutInterval: ko.observable()
                                });
                            adapter().Har.subscribe(function (har) {
                                // call the server to get the list of operations
                                isBusy(true);
                                if (!har)return;
                                $.get("/httpadapter/operations/" + har).done(function (results) {
                                    adapter().OperationDefinitionCollection(results);
                                    isBusy(false);
                                });
                            });
                        }
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            attached = function (view) {

            },
            remove = function (op) {
                adapter().OperationDefinitionCollection.remove(op);
            },
            save = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/sph/adapter/save")
                    .then(function (result) {
                        isBusy(false);
                        adapter().AdapterId(result.id);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            publish = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/httpadapter/publish")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            adapter: adapter,
            remove: remove,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                    {
                        caption: 'Publish',
                        icon: 'fa fa-sign-in',
                        command: publish,
                    }
                ])
            }
        };

        return vm;

    });
