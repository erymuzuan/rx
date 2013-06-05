/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/_kendo-knockoutbindings.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var rentalId = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routeData) {
            rentalId(routeData.rentalId);
            vm.offer.CommercialSpaceId(routeData.csId);
            var tcs = new $.Deferred();
            var raTask = context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + rentalId());
            var csTask = context.loadOneAsync("CommercialSpace", "CommercialSpaceId eq " + routeData.csId);
            $.when(raTask, csTask).done(function (r, cs) {
                ko.mapping.fromJSON(ko.mapping.toJSON(r.Offer), {}, vm.offer);
                ko.mapping.fromJSON(ko.mapping.toJSON(cs), {}, vm.commercialSpace);

                if (!vm.offer.CommercialSpaceId()) {
                    vm.offer.CommercialSpaceId(cs.CommercialSpaceId());
                    vm.offer.CommercialSpaceCategory(cs.Category());
                    vm.offer.Rent(cs.RentalRate());
                    vm.offer.Building(cs.BuildingName());
                    vm.offer.Floor(cs.FloorName());
                    vm.offer.Size(cs.Size());

                }
                tcs.resolve();
            });
            return tcs.promise();
        },
        viewAttached = function (view) {
            _uiready.init(view);
        },
        addCondition = function () {
            var condition = {
                Title: ko.observable(),
                Description: ko.observable(),
                Note: ko.observable(),
                IsRequired: ko.observable()
            };
            vm.offer.OfferConditionCollection.push(condition);
        },
        saveOffer = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON({ id: rentalId, offer: vm.offer });
            
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
        viewAttached: viewAttached,
        commercialSpace: {
            CommercialSpaceId: ko.observable(),
            Category : ko.observable(),
            RegistrationNo: ko.observable(),
            Size: ko.observable(),
            BuildingName: ko.observable(),
            LotCollection: ko.observableArray([]),
            Address: {
                Street: ko.observable(),
                State: ko.observable(),
                Postcode: ko.observable(),
                City: ko.observable()
            }
        },
        offer: {
            CommercialSpaceId: ko.observable(),
            CommercialSpaceCategory : ko.observable(),
            Size: ko.observable(),
            Building: ko.observable(),
            Floor: ko.observable(),
            Deposit: ko.observable(),
            Rent: ko.observable(),
            Date: ko.observable(),
            ExpiryDate: ko.observable(),
            Period: ko.observable(),
            PeriodUnit: ko.observable(),
            Option: ko.observable(),
            OfferConditionCollection: ko.observableArray([])
        },
        addConditionCommand: addCondition,
        saveCommand: saveOffer,
        generateOfferLetterCommand: generateOfferLetter
    };

    return vm;
}
);
