/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var isBusy = ko.observable(false),
        id = ko.observable(),
        registrationNo = ko.observable(),
        activate = function (routeData) {
            id(routeData.id);
            vm.rentalapplication.CommercialSpaceId(routeData.id);
            var bank = {
                Name: ko.observable(''),
                Location: ko.observable(''),
                AccountNo: ko.observable(''),
                AccountType: ko.observable('')
            };
            vm.rentalapplication.BankCollection.push(bank);
            return true;
        },
        viewAttached = function() {
            $('.datepicker').datepicker();
        },
        
        saveApplication = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.rentalapplication);
            isBusy(true);
            context.post(data, "/RentalApplication/Submit").done(function (e) {
                logger.log("Data has been successfully saved ", e, "rentalapplication", true);
                isBusy(false);
                registrationNo(e.registrationNo);
                $('#success-panel').modal({})
                    .on('hidden', function() {
                        router.navigateTo('/#/');
                    });
                tcs.resolve(e.status);
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
        };

    var vm = {
        activate: activate,
        registrationNo: registrationNo,
        viewAttached: viewAttached,
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
        addAttachmentCommand: addAttachment
    };

    return vm;

    function guidGenerator() {
        var buf = new Uint16Array(8);
        window.crypto.getRandomValues(buf);
        var S4 = function (num) {
            var ret = num.toString(16);
            while (ret.length < 4) {
                ret = "0" + ret;
            };
            return ret;
        };
        return (S4(buf[0]) + S4(buf[1]) + "-" + S4(buf[2]) + "-4" + S4(buf[3]).substring(1) + "-y" + S4(buf[4]).substring(1) + "-" + S4(buf[5]) + S4(buf[6]) + S4(buf[7]));
    }
});