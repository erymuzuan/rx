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
        rentalid = ko.observable(),
        activate = function (routeData) {
            logger.log('Rental Application View Activated', null, 'rentalapplication', true);
            id(routeData.id);
            rentalid(routeData.rentalId);
            var tcs = new $.Deferred();
            context.getCountAsync("RentalApplication", "RentalApplicationId eq " + rentalid(), "RentalApplicationId").done(function (c) {
                if (c > 0) {
                    context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + rentalid()).done(function (r) {
                        ko.mapping.fromJSON(ko.mapping.toJSON(r), {}, vm.rentalapplication);

                    });
                };
                tcs.resolve(true);
            });
            vm.rentalapplication.CommercialSpaceId(routeData.id);
            return tcs.promise();
        },
        saveApplication = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.rentalapplication);
            isBusy(true);
            context.post(data, "/RentalApplication/Save").done(function (e) {
                logger.log("Data has been successfully saved ", e, "rentalapplication", true);
                isBusy(false);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        addBankCollection = function () {
            var bank = {
                Name: ko.observable(''),
                Location: ko.observable(''),
                AccountNo: ko.observable(''),
                AccountType: ko.observable('')
            };
            vm.rentalapplication.BankCollection.push(bank);
        },
        addAttachment = function () {
            var guid = guidGenerator();
            var attachment = {
                Type: ko.observable(),
                Name: ko.observable(),
                IsRequired: ko.observable(false),
                IsReceived: ko.observable(false),
                StoreId: ko.observable(guid)
            };
            vm.rentalapplication.AttachmentCollection.push(attachment);
        },
        approvedApplication = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(rentalid());
            isBusy(true);
            context.post(data, "/RentalApplication/Approved").done(function (e) {
                logger.log("Application has been successfully approved ", e, "rentalapplication", true);
                isBusy(false);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        declinedApplication = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(rentalid());
            isBusy(true);
            context.post(data, "/RentalApplication/Declined").done(function (e) {
                logger.log("Application has been declined ", e, "rentalapplication", true);
                isBusy(false);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        returnedApplication = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(rentalid());
            isBusy(true);
            context.post(data, "/RentalApplication/Returned").done(function (e) {
                logger.log("Application has been returned ", e, "rentalapplication", true);
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
            Experience: ko.observable(''),
            IsRecordExist: ko.observable(false),
            PreviousAddress: ko.observable(''),
            IsCompany: ko.observable(false),
            Type: ko.observable(''),
            Address: {
                Street: ko.observable(),
                City: ko.observable(),
                State: ko.observable(),
                Postcode: ko.observable(),
            },
            BankCollection: ko.observableArray([]),
            AttachmentCollection: ko.observableArray([]),
            Contact: {
                Name: ko.observable(''),
                Title: ko.observable(''),
                IcNo: ko.observable(''),
                Role: ko.observable(''),
                MobileNo: ko.observable(''),
                OfficeNo: ko.observable(''),
                Email: ko.observable('')
            },
            CurrentYearSales: ko.observable(),
            LastYearSales: ko.observable(),
            PreviousYearSales: ko.observable()
        },
        saveCommand: saveApplication,
        addBankCommand: addBankCollection,
        isBusy: isBusy,
        declinedCommand: declinedApplication,
        returnedCommand: returnedApplication,
        addAttachmentCommand: addAttachment,
        approvedCommand: approvedApplication

    };

    return vm;
    
    function guidGenerator() {
        var buf = new Uint16Array(8);
        window.crypto.getRandomValues(buf);
        var S4 = function(num) {
            var ret = num.toString(16);
            while(ret.length < 4){
                ret = "0"+ret;
            };
            return ret;
        };
        return (S4(buf[0])+S4(buf[1])+"-"+S4(buf[2])+"-4"+S4(buf[3]).substring(1)+"-y"+S4(buf[4]).substring(1)+"-"+S4(buf[5])+S4(buf[6])+S4(buf[7]));
    }
});