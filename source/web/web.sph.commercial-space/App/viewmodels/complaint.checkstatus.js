/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function(context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function() {

            },
            viewAttached = function(view) {

            },
            search = function() {
                var query = String.format("ReferenceNo eq '{0}'", vm.ticketNo());
                var tcs = new $.Deferred();

                context.loadAsync("Complaint", query)
                    .then(function(lo) {
                        vm.results(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            ticketNo: ko.observable(),
            search: search,
            results: ko.observableArray(),
            selectedItem : ko.observable(new bespoke.sphcommercialspace.domain.Complaint())
        };

        return vm;

    });
