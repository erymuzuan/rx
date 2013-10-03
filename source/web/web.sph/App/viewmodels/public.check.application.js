/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function () {

            },
            viewAttached = function (view) {

            },
            submit = function () {
                var tcs = new $.Deferred(),
                    query = {
                        "query": {
                            "bool": {
                                "must": [
                                  { "match_phrase": { "RegistrationNo": vm.registrationNo() } }
                                ]

                            }
                        }
                    };

                if (vm.type() === "Syarikat") {
                    query.query.bool.must.push({ "match_phrase": { "CompanyRegistrationNo": vm.id() } });
                } else {
                    query.query.bool.must.push({ "match_phrase": { "Contact.IcNo": vm.id() } });
                }

                context.searchAsync("RentalApplication", query)
                    .then(function (result) {
                        vm.results(result.itemCollection);
                        tcs.resolve(result);
                        if (!result.itemCollection.length) {
                            logger.info("Maaf tiada permohonan dijumpai");
                        }
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            id: ko.observable(),
            registrationNo: ko.observable(),
            type: ko.observable(),
            submit: submit,
            results: ko.observableArray()
        };

        return vm;

    });
