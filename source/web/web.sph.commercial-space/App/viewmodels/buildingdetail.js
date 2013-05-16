/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../services/datacontext.js" />
define(['services/datacontext',
        'durandal/plugins/router',
        'durandal/system',
        'durandal/app',
        'viewmodels/map',
        'services/logger'
],
    function (datacontext, router, system, app, mapvm, logger) {


        var isBusy = ko.observable(false),
            activate = function (routeData) {
                var id = parseInt(routeData.id);
                var query = "BuildingId eq " + id;
                var tcs = new $.Deferred();
                datacontext.loadOneAsync("Building", query)
                    .done(function (b) {
                        ko.mapping.fromJSON(ko.mapping.toJSON(b), {}, vm.building);
                        tcs.resolve(true);
               });

                tcs.promise();
            };

        var addLot = function (floor) {
            var url = '/#/lotdetail/' + vm.building.BuildingId() + '/' + floor.Name();
            router.navigateTo(url);
        };
        
        var goBack = function () {
            var url = '/#/building'  ;
            router.navigateTo(url);
        };

        var removeFloor = function (floor) {
            vm.building.FloorCollection.remove(floor);

        };

        var addFloor = function () {
            var floor = {
                Name: ko.observable(),
                Size: ko.observable()
            };
            vm.building.FloorCollection.push(floor);
        };

        var saveAsync = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.building);
            datacontext.post(data, "/Building/SaveBuilding").done(function (e) {
                logger.log("Data has been successfully saved ", e, "buildingdetail", true);
            });
            return tcs.promise();
        };

        var showMap = function () {

            var tcs = new $.Deferred();
            mapvm.init()
                .done(function () {
                    tcs.resolve(true);
                });

            return tcs.promise();
        };

        var vm = {
            activate: activate,
            building: {
                Name: ko.observable(''),
                Address: {
                    Street: ko.observable(''),
                    State: ko.observable(''),
                    City: ko.observable(''),
                    Postcode: ko.observable(''),
                },
                FloorCollection: ko.observableArray([]),
                LotNo: ko.observable(''),
                Size: ko.observable(''),
                Status: ko.observable(''),
            },
            saveCommand: saveAsync,
            showMapCommand: showMap,
            addFloorCommand: addFloor,
            addLotCommand: addLot,
            goBackCommand: goBack,
            isBusy: isBusy,
            removeFloorCommand: removeFloor,
            title: 'Building Details'
        };

        return vm;
    });