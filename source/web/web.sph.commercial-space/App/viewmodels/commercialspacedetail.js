/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var title = ko.observable(),
        buildingId = ko.observable(),
        selectedBuilding = {},
        floorname = ko.observable(),
        lotname = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routeData) {

            buildingId(parseInt(routeData.buildingId));
            floorname(routeData.floorname);
            lotname(routeData.lotname);
            title('Tambah ruang komersil');

            vm.commercialSpace.RegistrationNo('');
            vm.commercialSpace.CommercialSpaceId(0);
            vm.commercialSpace.BuildingId(buildingId());
            vm.commercialSpace.FloorName(floorname());
            vm.commercialSpace.LotName(lotname());
            vm.commercialSpace.RentalRate('');
            vm.commercialSpace.Size('');
            vm.commercialSpace.RentalRate('');
            vm.commercialSpace.Category('');
            vm.commercialSpace.IsOnline(false);

            var tcs = new $.Deferred();
            if (buildingId()) {
                var query = String.format("CommercialSpaceId eq {0} ", routeData.commercialspaceid);
                context.loadOneAsync("CommercialSpace", query)
                    .done(function (cs) {
                        if (!cs) {
                            tcs.resolve(true);
                            return;
                        }
                        ko.mapping.fromJSON(ko.mapping.toJSON(cs), {}, vm.commercialSpace);
                        title('Maklumat ruang komersil ' + vm.commercialSpace.RegistrationNo());
                        tcs.resolve(true);
                    });

            } else {
                context.getTuplesAsync("Building", "BuildingId gt 0", "BuildingId", "Name")
                    .then(function (list) {
                        vm.buildingOptions(_(list).sortBy(function (b) {
                            return b.Item2;
                        }));
                        tcs.resolve(true);
                    });

            }


            return tcs.promise();
        },
        saveCs = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.commercialSpace);
            isBusy(true);
            context.post(data, "/CommercialSpace/SaveCommercialSpace")
                .done(function (e) {
                    logger.log("Data has been successfully saved ", e, "commercialspacedetail", true);

                    isBusy(false);
                    tcs.resolve(true);
                });
            return tcs.promise();
        },
        selectLot = function () {
            $('#lot-selection-panel').modal();
        },
        addLots = function () {
            vm.commercialSpace.LotCollection(vm.selectedLots);
            var lots = ko.mapping.toJS(vm.selectedLots);
            var lotName = _(lots).reduce(function (memo, lot) {
                return memo + lot.Name + ",";
            }, "");
            var size = _(lots).reduce(function (memo, lot) {
                return memo + lot.Size;
            }, 0);
            vm.commercialSpace.LotName(lotName);
            vm.commercialSpace.Size(size);
            vm.commercialSpace.LotCollection(vm.selectedLots);
        };

    var vm = {
        activate: activate,
        title: title,
        commercialSpace: {
            CommercialSpaceId: ko.observable(0),
            RegistrationNo: ko.observable(''),
            BuildingId: ko.observable(''),
            FloorName: ko.observable(''),
            LotName: ko.observable(''),
            Size: ko.observable(''),
            Category: ko.observable(''),
            RentalRate: ko.observable(''),
            RentalType: ko.observable(''),
            LotCollection: ko.observableArray([]),
            IsOnline: ko.observable(false)

        },
        buildingOptions: ko.observableArray(),
        floorOptions: ko.observableArray(),
        lotOptions: ko.observableArray(),
        selectedBuilding: ko.observable(),
        selectedFloor: ko.observable(),
        selectedLots: ko.observableArray(),
        saveCsCommand: saveCs,
        selectLotCommand: selectLot,
        addLotsCommand: addLots,
        isBusy: isBusy
    };

    vm.selectedBuilding.subscribe(function (id) {
        vm.isBusy(true);
        vm.commercialSpace.BuildingId(id);
        context.loadOneAsync("Building", "BuildingId eq " + id)
            .then(function (b) {
                var floors = _(b.FloorCollection()).map(function (f) {
                    return f.Name();
                });
                vm.floorOptions(floors);
                vm.isBusy(false);
                selectedBuilding = b;
            });


    });
    vm.selectedFloor.subscribe(function (floor) {
        var building = ko.mapping.toJS(selectedBuilding);
        var sf = _(building.FloorCollection).find(function (f) {
            return f.Name == floor;
        });
        vm.commercialSpace.FloorName(floor);
        vm.lotOptions(sf.LotCollection);
    });

    return vm;
});