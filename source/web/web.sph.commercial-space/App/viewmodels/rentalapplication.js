/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger'], function (context, logger) {
    
    var isBusy = ko.observable(false),
        id = ko.observable(),
        activate = function (routeData) {
            logger.log('Rental Application View Activated', null, 'rentalapplication', true);
            id(routeData.id);
            vm.rentalapplication.CommercialSpaceId(routeData.id);
            return true;
        },
        
        saveApplication = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.rentalapplication);
            isBusy(true);
            context.post(data, "/RentalApplication/SaveRentalApplicationOne").done(function (e) {
                logger.log("Data has been successfully saved ", e, "rentalapplication", true);
                isBusy(false);
                tcs.resolve(true);
            });
            return tcs.promise();
        };

    var vm = {
        activate: activate,
        rentalapplication: {
            CommercialSpaceId: ko.observable(),
            CompanyName: ko.observable(''),
            CompanyRegistrationNo: ko.observable(''),
            DateStart: ko.observable(),
            DateEnd: ko.observable(),
            Purpose: ko.observable(''),
            CompanyType: ko.observable(''),
            Address: {
                Street: ko.observable(),
                City: ko.observable(),
                State: ko.observable(),
                Postcode: ko.observable(),
            },
        },
        saveCommand: saveApplication,
        isBusy: isBusy
    };

    return vm;
});