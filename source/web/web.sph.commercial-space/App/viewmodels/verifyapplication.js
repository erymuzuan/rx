/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var id = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routedata) {
            id(routedata.applicationId);
            vm.remarks('');
            
            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + id())
                .then(function (r) {

                    ko.mapping.fromJSON(ko.mapping.toJSON(r), {}, vm.rentalapplication);

                    context.loadOneAsync("CommercialSpace", "CommercialSpaceId eq " + vm.rentalapplication.CommercialSpaceId())
                        .then(function (b) {
                            ko.mapping.fromJSON(ko.mapping.toJSON(b), {}, vm.commercialSpace);
                            tcs.resolve(true);
                        });

                    $.get("/Map/CommercialSpaceImage/" + vm.rentalapplication.CommercialSpaceId() + "?width=300&height=200")
                        .then(function (b) {
                            vm.commercialSpace.StaticMap(b);
                        });


                });
            return tcs.promise();
        },
        addAttachment = function () {
            var attachment = {
                Type: ko.observable(''),
                Name: ko.observable(''),
                IsCompleted: ko.observable(false),
                IsRequired: ko.observable(false),
                IsReceived: ko.observable(false)
            };
            vm.rentalapplication.AttachmentCollection.push(attachment);
        },
        showAuditTrail = function () {
            isBusy(true);
            var query = "EntityId eq " + vm.rentalapplication.RentalApplicationId();
            vm.auditTrailCollection.removeAll();

            context.loadAsync("AuditTrail", query)
                .then(function (lo) {
                    vm.auditTrailCollection(lo.itemCollection);
                    isBusy(false);
                });
            $('#audit-trail').modal({});

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
            var attachments = ko.mapping.toJS(vm.rentalapplication.AttachmentCollection);
            var data = JSON.stringify({ id: id(), attachments: attachments });
            context.post(data, "/RentalApplication/Returned").done(function (e) {
                var url = '/#/returnedapplication/' + e;
                router.navigateTo(url);
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
                vm.rentalapplication.Status('Approved');
                tcs.resolve(true);
            });
            return tcs.promise();
        },

        generateOfferLetter = function () {
            var url = '/#/offerdetails/' + vm.rentalapplication.RentalApplicationId() +'/' + vm.rentalapplication.CommercialSpaceId();
            router.navigateTo(url);
        },

        confirmedOffer = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id(), remarks: vm.remarks() });
            context.post(data, "/RentalApplication/ConfirmOffer").done(function (e) {
                logger.log("Offer letter received &amp; Confirmed ", e, "verifyapplication", true);
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
        isBusy: isBusy,
        activate: activate,
        auditTrailCollection: ko.observableArray([]),
        rentalapplication: {
            CommercialSpaceId: ko.observable(),
            RentalApplicationId: ko.observable(0),
            RegistarationNo: ko.observable(''),
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
                Postcode: ko.observable()
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
        commercialSpace: {
            State: ko.observable(''),
            City: ko.observable(''),
            BuildingName: ko.observable(''),
            BuildingLot: ko.observable(''),
            LotName: ko.observable(''),
            FloorName: ko.observable(''),
            Category: ko.observable(''),
            StaticMap: ko.observable('')
        },
        waitingListCommand: waitingList,
        returnedCommand: returned,
        declinedCommand: declined,
        showAuditTrailCommand: showAuditTrail,
        approvedCommand: approved,
        addAttachmentCommand: addAttachment,
        generateOfferLetterCommand: generateOfferLetter,
        confirmOfferCommand: confirmedOffer,
        rejectOfferLetterCommand: rejectOfferLetter,
        contractCommand: function() {
            router.navigateTo("/#/createcontract/" + vm.rentalapplication.RentalApplicationId());
        },
        remarks: ko.observable('')
    };

    return vm;
});