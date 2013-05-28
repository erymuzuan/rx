/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function(context, logger, router) {

    var rentalId = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routeData) {
            rentalId(routeData.rentalId);
            vm.Offer.CommercialSpaceId(routeData.csId);
            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + rentalId()).done(function(r) {
                ko.mapping.fromJSON(ko.mapping.toJSON(r.Offer), {}, vm.Offer);
                tcs.resolve();
            });
            return tcs.promise();
        },
        addCondition= function() {
            var condition = {
                Title: ko.observable(),
                Description: ko.observable(),
                Note: ko.observable(),
                IsRequired : ko.observable()
            };
            vm.Offer.OfferConditionCollection.push(condition);
        },
        saveOffer = function() {
            var tcs = new $.Deferred();
            var data = JSON.stringify({id: rentalId(), offer: ko.mapping.toJS(vm.Offer) });
            isBusy(true);
            context.post(data, "/RentalApplication/SaveOffer").done(function () {
                logger.log("Offer has been successfully saved ", "offerdetails", true);
                isBusy(false);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        generateOfferLetter = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: rentalId() });
            context.post(data, "/RentalApplication/GenerateOfferLetter").done(function (e) {
                logger.log("Offer letter generated ", e, "offerdetails", true);
                window.open("/RentalApplication/Download");
                tcs.resolve(true);
            });
            return tcs.promise();
        };

    var vm = {
        activate: activate,
        isBusy: isBusy,
        Offer: {
            CommercialSpaceId: ko.observable(),
            Size: ko.observable(),
            Building: ko.observable(),
            Floor:ko.observable(),
            Deposit:ko.observable(),
            Rent:ko.observable(),
            Date:ko.observable(),
            ExpiryDate: ko.observable(),
            OfferConditionCollection:ko.observableArray([])
        },
        addConditionCommand: addCondition,
        saveCommand: saveOffer,
        generateOfferLetterCommand: generateOfferLetter
    };

    return vm;
}
);
