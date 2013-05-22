/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
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
        },
            viewFloorPlan = function (floor) {
                var url = '/#/floorplan/' + vm.building.BuildingId() + '/' + floor.Name();
                router.navigateTo(url);
            };

        var goBack = function () {
            var url = '/#/building';
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
            isBusy(true);
            var pathTask = $.get("/Building/GetEncodedPath/" + vm.building.BuildingId());
            var centerTask = $.get("/Building/GetCenter/" + vm.building.BuildingId());
            $.when(pathTask, centerTask)
            .then(function (path, center) {

                mapvm.init({
                    panel: 'map',
                    draw: true,
                    polygoncomplete: polygoncomplete,
                    zoom: 18,
                    center: new google.maps.LatLng(center[0].Lat,center[0].Lng)
                });
                if (path) {
                    mapvm.add({
                        encoded: path[0],
                        draggable: true,
                        editable: true,
                        zoom: 18
                    });
                }

            });
        },
            polygon = null,
            polygoncomplete = function (shape) {
                polygon = shape;
            },
            saveMap = function () {
                if (!polygon) {
                    logger.log("No shape");
                    return false;
                }
                var tcs = new $.Deferred();
                var data = JSON.stringify({ buildingId: vm.building.BuildingId(), path: mapvm.getEncodedPath(polygon) });
                datacontext
                    .post(data, "/Building/SaveMap")
                    .then(function (e) {
                        logger.log("Map has been successfully saved ", e, "buildingdetail", true);
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
            saveMapCommand: saveMap,
            addFloorCommand: addFloor,
            addLotCommand: addLot,
            viewFloorPlanCommand: viewFloorPlan,
            goBackCommand: goBack,
            isBusy: isBusy,
            removeFloorCommand: removeFloor,
            title: 'Building Details'
        };

        return vm;
    });