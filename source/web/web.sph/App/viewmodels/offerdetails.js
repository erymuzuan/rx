/// <reference path="../../Scripts/jquery-2.0.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var rentalId = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routeData) {
            rentalId(parseInt(routeData.rentalId));
            vm.offer().SpaceId(routeData.csId);
            var tcs = new $.Deferred();
            var raTask = context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + rentalId());
            var csTask = context.loadOneAsync("Space", "SpaceId eq " + routeData.csId);
            $.when(raTask, csTask).done(function (r, cs) {
                vm.offer(r.Offer());
                vm.space(cs);

                if (!vm.offer().SpaceId()) {
                    vm.offer().SpaceId(cs.SpaceId());
                    vm.offer().Rent(cs.RentalRate());
                    vm.offer().Building(cs.BuildingName());
                    vm.offer().Floor(cs.FloorName());
                    vm.offer().Size(cs.Size());

                }
                tcs.resolve();
            });
            return tcs.promise();
        },
        viewAttached = function (view) {
            _uiready.init(view);
        },
        addCondition = function () {
            var condition = new bespoke.sph.domain.OfferCondition();
            vm.offer().OfferConditionCollection.push(condition);
        },
        saveOffer = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON({ id: rentalId, offer: vm.offer });

            isBusy(true);
            context.post(data, "/RentalApplication/SaveOffer").done(function () {
                logger.log("Offer has been successfully saved ", "offerdetails", true);
                isBusy(false);
                tcs.resolve(true);
                var url = '/#/rentalapplication.verify/' + rentalId();
                router.navigateTo(url);
            });
            return tcs.promise();
        },
        generateOfferLetter = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: ko.toJS(rentalId()) });
            context.post(data, "/RentalApplication/GenerateOfferLetter").done(function (e) {
                logger.log("Offer letter generated ", e, "offerdetails", true);
                window.open("/RentalApplication/Download");
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        removeOfferCondition = function (condition) {
            vm.offer().OfferConditionCollection.remove(condition);
        };

    var vm = {
        activate: activate,
        isBusy: isBusy,
        viewAttached: viewAttached,
        space: ko.observable(new bespoke.sph.domain.Space()),
        offer: ko.observable(new bespoke.sph.domain.Offer()),
        toolbar: {
            commands: ko.observableArray([
                {
                    caption: "Simpan",
                    icon: "icon-envelop",
                    command: saveOffer,
                    status: "none"
                },
                {
                    caption: "Cetak Surat Tawaran",
                    icon: "icon-download-alt",
                    command: generateOfferLetter,
                    status: "none"
                }
            ])
        },
        addConditionCommand: addCondition,
        removeOfferCondition: removeOfferCondition
    };

    return vm;
}
);
