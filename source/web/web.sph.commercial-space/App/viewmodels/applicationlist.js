/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {
    var status = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routedata) {
            isBusy(true);
            status(routedata.status);
            var tcs = new $.Deferred();
            var query = String.format("Status eq '{0}'", status());
            context.loadAsync("RentalApplication", query).done(function (lo) {
                vm.applications.removeAll();
                vm.applications(lo.itemCollection);
                tcs.resolve(true);
                isBusy(false);
            });
            return tcs.promise();
        };

    var vm = {
        status: status,
        isBusy: isBusy,
        activate: activate,
        title: 'Senarai Permohonan',
        applications: ko.observableArray([]),
        toolbar: ko.observable({
            reloadCommand: function () {
                return activate({ status: status() });
            }
        })
    };
    return vm;
});