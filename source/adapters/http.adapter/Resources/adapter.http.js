/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var adapter = ko.observable(),
            errors = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function (id) {
                var query = String.format("Id eq '{0}'", id),
                    tcs = new $.Deferred();
                context.loadOneAsync("Adapter", query)
                    .done(function (b) {
                        b.Har.subscribe(function (har) {
                            // call the server to get the list of operations
                            isBusy(true);
                            if (!har) return;
                            $.get("/httpadapter/operations/" + har)
                                .done(function (results) {
                                    adapter().OperationDefinitionCollection(results);
                                    isBusy(false);
                                }).fail(function (error) {

                                    var rs = error.responseJSON || {};
                                    var message = rs.ExceptionMessage || "";
                                    logger.error(message);
                                });
                        });
                        adapter(b);

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
            errors: errors,
            remove: remove,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                    {
                        caption: "Publish",
                        icon: "fa fa-sign-in",
                        command: publish,
                    }
                ])
            }
        };

        return vm;

    });
