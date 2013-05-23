/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var id = ko.observable(),
        activate = function (routedata) {
            logger.log('Application List View Activated', null, 'applicationlist', true);
            id(routedata.applicationId);
            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + id()).done(function (r) {
                ko.mapping.fromJSON(ko.mapping.toJSON(r), {}, vm.rentalapplication);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        addAttachment = function () {
            var attachment = {
                Type: ko.observable(''),
                Name: ko.observable(''),
                IsRequired: ko.observable(false),
                IsReceived: ko.observable(false)
            };
            vm.rentalapplication.AttachmentCollection.push(attachment);
        },
        waitingList = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/WaitingList").done(function (e) {
                logger.log("Application has been insert into waiting list ", e, "verifyapplication", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        returned = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/Returned").done(function (e) {
                logger.log("Application has been returned ", e, "verifyapplication", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        declined = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/Declined").done(function (e) {
                logger.log("Application has been declined ", e, "verifyapplication", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        approved = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/Approved").done(function (e) {
                logger.log("Application has been approved ", e, "verifyapplication", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        generateOfferLetter =function() {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/GenerateOfferLetter").done(function (e) {
                logger.log("Offer letter generated ", e, "verifyapplication", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        confirmedOffer = function() {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/Confirmed").done(function(e) {
                logger.log("Offer letter received & Confirmed ", e, "verifyapplication", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        rejectOfferLetter = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/RejectedOfferLetter").done(function (e) {
                logger.log("Offer letter received & Confirmed ", e, "verifyapplication", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        }
        ;

    var vm = {
        activate: activate,
        rentalapplication: {
            CommercialSpaceId: ko.observable(),
            CompanyName: ko.observable(''),
            Status: ko.observable(''),
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
        waitingListCommand: waitingList,
        returnedCommand: returned,
        declinedCommand: declined,
        approvedCommand: approved,
        addAttachmentCommand: addAttachment,
        generateOfferLetterCommand: generateOfferLetter,
        confirmOfferCommand: confirmedOffer,
        rejectOfferLetterCommand: rejectOfferLetter
    };

    return vm;
});