/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var id = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (r) {
          vm.rentalapplication(r);
        },

        generateOfferLetter = function () {
            var url = '/#/offerdetails/' + vm.rentalapplication().RentalApplicationId() + '/' + vm.rentalapplication().CommercialSpaceId();
            router.navigateTo(url);
        },
         generateDeclinedOfferLetter = function () {
             var tcs = new $.Deferred();
             var data = JSON.stringify({ id: id() });
             context.post(data, "/RentalApplication/GenerateDeclinedLetter").done(function (e) {
                 logger.log("Declined letter generated ", e, "rentalapplication.verify", true);
                 window.open("/RentalApplication/Download");
                 tcs.resolve(true);
             });
             return tcs.promise();
         },
        confirmedOffer = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: vm.rentalapplication().RentalApplicationId()});
            context.post(data, "/RentalApplication/ConfirmOffer").done(function (e) {
                logger.log(e, "rentalapplication.verify", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        rejectOfferLetter = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/RejectedOfferLetter").done(function (e) {
                logger.log("Offer letter received & Confirmed ", e, "rentalapplication.verify", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        }
    ;

    var vm = {
        isBusy: isBusy,
        activate: activate,
        rentalapplication : ko.observable(new bespoke.sphcommercialspace.domain.RentalApplication()),
        generateOfferLetterCommand: generateOfferLetter,
        confirmOfferCommand: confirmedOffer,
        rejectOfferLetterCommand: rejectOfferLetter,
        generateDeclinedLetterCommand: generateDeclinedOfferLetter,
        contractCommand: function () {
            router.navigateTo("/#/createcontract/" + vm.rentalapplication().RentalApplicationId());
        },
    };

    return vm;
});