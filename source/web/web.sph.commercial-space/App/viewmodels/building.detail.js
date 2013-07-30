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
    function (context, router, system, app, mapvm, logger) {


        var isBusy = ko.observable(false),
            activate = function (routeData) {
                var id = parseInt(routeData.id);
                var templateId = parseInt(routeData.templateId);

                var tcs = new $.Deferred();
                context.loadOneAsync("Setting", "Key eq 'State'")
                    .done(function (s) {
                        s = s || {
                            Value: function () {
                                return '[{ "State": "Kelantan", "Name": "Kelantan" }, { "State": "Selangor", "Name": "Selangor" }, {"State": "Kuala Lumpur" ,"Name": "Kuala Lumpur" }]';
                            }
                        };
                        var states = JSON.parse(s.Value());
                        vm.stateOptions(states);
                        tcs.resolve(true);
                    })
                    .fail(function () {
                        var states = [{ State: 'Kelantan', Name: 'Kelantan' }, { State: 'Selangor', Name: 'Selangor' }, { State: 'Kuala Lumpur', Name: 'Kuala Lumpur' }];
                        vm.stateOptions(states);
                        tcs.resolve(true);
                    });


                if (!id) {

                    // build custom fields value
                    context.loadOneAsync("BuildingTemplate", "BuildingTemplateId eq " + templateId)
                        .done(function (template) {
                            var cfs = _(template.CustomFieldCollection()).map(function (f) {
                                var webid = system.guid();
                                var v = new bespoke.sphcommercialspace.domain.CustomFieldValue(webid);
                                v.Name(f.Name());
                                v.Type(f.Type());
                                return v;
                            });

                            vm.building().CustomFieldValueCollection(cfs);

                        });

                    vm.building().TemplateId(templateId);
                    return true;
                }
                vm.toolbar().auditTrail.id(id);
                var query = "BuildingId eq " + id;
                context.loadOneAsync("Building", query).done(function (b) {
                    vm.building(b);
                    tcs.resolve(true);
                });

                return tcs.promise();
            },
            addLot = function (floor) {
                var url = '/#/lotdetail/' + vm.building().BuildingId() + '/' + floor.Name();
                router.navigateTo(url);
            },
            viewFloorPlan = function (floor) {
                var url = '/#/floorplan/' + vm.building().BuildingId() + '/' + floor.Name();
                router.navigateTo(url);
            },
            goBack = function () {
                var url = '/#/building';
                router.navigateTo(url);
            },
            removeFloor = function (floor) {
                vm.building().FloorCollection.remove(floor);

            },
            addFloor = function () {
                var floor = new bespoke.sphcommercialspace.domain.Floor();
                vm.building().FloorCollection.push(floor);
            };

        var saveAsync = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.building());
            context.post(data, "/Building/SaveBuilding")
                .done(function (e) {
                    if (e.status) {
                        vm.building().BuildingId(e.buildingId);
                        logger.log(e.message, e, "buildingdetail", true);
                    } else {
                        logger.logError(e.message, e, this, true);
                    }
                    tcs.resolve(e);
                });
            return tcs.promise();
        };

        var showMap = function () {
            isBusy(true);
            var point = new google.maps.LatLng(3.1282, 101.6441);
            var buildingId = vm.building().BuildingId();
            if (!buildingId) {
                mapvm.geocode(
                    vm.building().Address.Street() + ","
                   + vm.building().Address.City() + ","
                   + vm.building().Address.Postcode() + ","
                   + vm.building().Address.State() + ","
                   + "Malaysia.")
                .then(function (result) {
                    if (result.status) {
                        mapvm.init({
                            panel: 'map',
                            draw: true,
                            polygoncomplete: polygoncomplete,
                            zoom: 18,
                            center: result.point
                        });
                    } else {
                        mapvm.init({
                            panel: 'map',
                            draw: true,
                            polygoncomplete: polygoncomplete,
                            zoom: center[0] ? 18 : 12,
                            center: point
                        });
                    }
                });
                return;
            }

            var pathTask = $.get("/Building/GetEncodedPath/" + buildingId);
            var centerTask = $.get("/Building/GetCenter/" + buildingId);
            $.when(pathTask, centerTask)
            .then(function (path, center) {
                isBusy(false);
                if (center[0]) {
                    point = new google.maps.LatLng(center[0].Lat, center[0].Lng);
                }
                mapvm.init({
                    panel: 'map',
                    draw: true,
                    polygoncomplete: polygoncomplete,
                    zoom: center[0] ? 18 : 12,
                    center: point
                });
                if (path[0]) {
                    buildingPolygon = mapvm.add({
                        encoded: path[0],
                        draggable: true,
                        editable: true,
                        zoom: 18
                    });
                }

            });
        },
            buildingPolygon = null,
            polygoncomplete = function (shape) {
                buildingPolygon = shape;
            },
            saveMap = function () {
                if (!buildingPolygon) {
                    logger.log("No shape");
                    return false;
                }
                var tcs = new $.Deferred();
                var data = JSON.stringify({ buildingId: vm.building().BuildingId(), path: mapvm.getEncodedPath(buildingPolygon) });
                context
                    .post(data, "/Building/SaveMap")
                    .then(function (e) {
                        logger.log("Map has been successfully saved ", e, "buildingdetail", true);
                    });
                return tcs.promise();

            };
        
        var vm = {
            activate: activate,
            building: ko.observable(new bespoke.sphcommercialspace.domain.Building()),
            template: ko.observable(new bespoke.sphcommercialspace.domain.BuildingTemplate()),
            stateOptions: ko.observableArray(),
            saveCommand: saveAsync,
            showMapCommand: showMap,
            saveMapCommand: saveMap,
            addFloorCommand: addFloor,
            addLotCommand: addLot,
            viewFloorPlanCommand: viewFloorPlan,
            goBackCommand: goBack,
            isBusy: isBusy,
            removeFloorCommand: removeFloor,
            title: 'Building Details',
            toolbar: ko.observable({
                saveCommand: saveAsync,
                auditTrail: {
                    entity: "Building",
                    id: ko.observable()
                }
            })
        };

        return vm;
    });