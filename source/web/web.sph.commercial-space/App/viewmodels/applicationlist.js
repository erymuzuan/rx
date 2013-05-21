/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {
    var status = ko.observable(),
        activate = function (routedata) {
            logger.log('Application List View Activated', null, 'applicationlist', true);
            status(routedata.status);
            var tcs = new $.Deferred();
            var query = String.format("Status eq '{0}'", status());
            context.loadAsync("RentalApplication", query).done(function (lo) {
                vm.applications(lo.itemCollection);
                tcs.resolve(true);
            });
            tcs.promise();
        },
        viewDetail = function () {
            var url = '/#/rentalapplication/';
            router.navigateTo(url);
        },

        viewAttached = function (view) {
            bindEventToList(view, '#div-application', gotoDetails);
        },
        bindEventToList = function (rootSelector, selector, callback, eventName) {
            var eName = eventName || 'click';
            $(rootSelector).on(eName, selector, function () {
                var application = ko.dataFor(this);
                callback(application);
                return false;
            });
        },
        gotoDetails = function (selectedApplication) {
            if (selectedApplication && selectedApplication.RentalApplicationId()) {
                var url = '/#/rentalapplication/'+ selectedApplication.CommercialSpaceId() +"/" + selectedApplication.RentalApplicationId();
                router.navigateTo(url);
            }
        };

    var vm = {
        activate: activate,
        title: 'Application List',
        applications: ko.observableArray([]),
        viewDetailCommand: viewDetail,
        viewAttached: viewAttached
    };

    return vm;
});