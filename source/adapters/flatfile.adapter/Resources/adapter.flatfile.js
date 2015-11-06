/// <reference path="../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />

/*
        public string SampleFile { get; set; }
        public bool IsPositional { get; set; }
        public bool IsTagIdentifier { get; set; }
        public string Delimiter { get; set; }
        public string EscapeCharacted { get; set; }
        public string Tag { get; set; }

*/
var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};
bespoke.sph.domain.FlatFileAdapter = function() {
    var adapter = {
        $type: "Bespoke.Sph.Integrations.Adapters.FlatFileAdapter, http.adapter",
        SampleFile: ko.observable(),
        Id: ko.observable("0"),
        Schema: ko.observable(),
        IsPositional : ko.observable(false),
        IsTagIdentifier: ko.observable(false),
        Delimiter: ko.observable(),
        EscapeCharacted: ko.observable(),
        Tag: ko.observable(),
        Name: ko.observable(),
        Description: ko.observable(),
        OperationDefinitionCollection: ko.observableArray()
    };

    return adapter;
};


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
                        if (b) {
                            adapter(b);
                        } else {
                            adapter(
                                {
                                    $type: "Bespoke.Sph.Integrations.Adapters.FlatFileAdapter, http.adapter",
                                    SampleFile: ko.observable(),
                                    Id: ko.observable("0"),
                                    Schema: ko.observable(),
                                    Name: ko.observable(),
                                    Description: ko.observable(),
                                    OperationDefinitionCollection: ko.observableArray()
                                });
                            adapter().SampleFile.subscribe(function (har) {
                                // call the server to get the list of operations
                                isBusy(true);
                                if (!har) return;

                                require(["viewmodels/flatfile-adapter-wizard", "durandal/app"], function (dialog, app2) {
                                    app2.showDialog(dialog)
                                        .done(function (result) {
                                            if (!result) return;
                                            if (result === "OK") {


                                            }
                                        });
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
