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
            vm.applications.removeAll();
            isBusy(true);
            status(routedata.status);
            var tcs = new $.Deferred();
            var query = String.format("Status eq '{0}'", status());
            context.loadAsync("RentalApplication", query).done(function (lo) {
                vm.applications(lo.itemCollection);
                tcs.resolve(true);
                isBusy(false);
            });
            tcs.promise();
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
                var url = '/#/rentalapplication.verify/' + selectedApplication.RentalApplicationId();
                router.navigateTo(url);
            }
        },
        search = function() {
            vm.applications.removeAll();
            var tcs = new $.Deferred();
            var query = "";
            if (!vm.selectedIdSsm() && vm.selectedRegistrationNo()) {
                var query1 = String.format("Status eq '{0}' and RegistrationNo eq '{1}' ",status(), vm.selectedState());
                query = query1;
            }
            if (vm.selectedIdSsm() && !vm.selectedRegistrationNo()) {
                var query2 = String.format("Status eq '{0}' and CompanyRegistrationNo eq '{1}' or ContactIcNo eq '{1}'", status(), vm.selectedIdSsm());
                query = query2;
            }
            if (vm.selectedIdSsm() && vm.selectedRegistrationNo()) {
                var query3 = String.format("Status eq '{0}' and RegistrationNo eq '{1}' and (CompanyRegistrationNo eq '{2}' or ContactIcNo eq '{2}')", status(), vm.selectedRegistrationNo(), vm.selectedIdSsm());
                query = query3;
            }
            if (!vm.selectedIdSsm() && !vm.selectedRegistrationNo()) {
                var query4 = String.format("Status eq '{0}'", status());
                query = query + query4;
            }
            
            context.loadAsync('RentalApplication', query).done(function (lo) {
                vm.applications(lo.itemCollection);
                tcs.resolve(true);
            });

            return tcs.promise();
        };

    var vm = {
        status: status,
        isBusy: isBusy,
        activate: activate,
        title: 'Application List',
        applications: ko.observableArray([]),
        viewAttached: viewAttached,
        selectedBuildingId: ko.observable(),
        buildingOptions: ko.observableArray(),
        selectedIdSsm: ko.observable(),
        selectedRegistrationNo: ko.observable(),
        searchCommand : search
    };
    return vm;
});