/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/app'], function (context, logger, router, app) {

    var title = ko.observable(),
        buildingId = ko.observable(),
        floorname = ko.observable(),
        lotname = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routeData) {
            logger.log('Commercial Details View Activated', null, 'commercialspacedetail', true);
            buildingId(routeData.buildingId);
            floorname(routeData.floorname);
            lotname(routeData.lotname);
            title('Commercial Space on ' + lotname());
            var tcs = new $.Deferred();
            var query = String.format("BuildingId eq {0} and FloorName eq '{1}'  and LotName eq '{2}'", buildingId(), floorname(), lotname());
            context.loadOneAsync("CommercialSpace", query)
                .done(function (cs) {
                    if (!cs) {
                        vm.commercialSpace.CommercialSpaceId(0);
                        vm.commercialSpace.BuildingId(buildingId());
                        vm.commercialSpace.FloorName(floorname());
                        vm.commercialSpace.LotName(lotname());
                        vm.commercialSpace.RentalRate('');
                        vm.commercialSpace.Size('');
                        vm.commercialSpace.RentalRate('');
                        vm.commercialSpace.Category('');
                        vm.commercialSpace.IsOnline(false);
                        tcs.resolve(true);
                        return;
                    }
                    ko.mapping.fromJSON(ko.mapping.toJSON(cs), {}, vm.commercialSpace);
                    tcs.resolve(true);
                });

            return tcs.promise();
        },
        saveCs = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.commercialSpace);
            isBusy(true);
            context.post(data, "/CommercialSpace/SaveCommercialSpace").done(function (e) {
                logger.log("Data has been successfully saved ", e, "commercialspacedetail", true);
                var url = '/#/lotdetail/' + vm.commercialSpace.BuildingId() + '/' + vm.commercialSpace.FloorName();
                router.navigateTo(url);
                isBusy(false);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        cancelCs = function () {
            var url = '/#/lotdetail/' + vm.commercialSpace.BuildingId() + '/' + vm.commercialSpace.FloorName();
            router.navigateTo(url);
        };
    var vm = {
        activate: activate,
        title: title,
        commercialSpace: {
            CommercialSpaceId : ko.observable(0),
            BuildingId: ko.observable(''),
            FloorName: ko.observable(''),
            LotName: ko.observable(''),
            Size: ko.observable(''),
            Category: ko.observable(''),
            RentalRate: ko.observable(''),
            RentalType: ko.observable(''),
            IsOnline: ko.observable(false),
            
        },

        saveCsCommand: saveCs,
        cancelCsCommand: cancelCs,
        isBusy: isBusy
    };

    return vm;
});