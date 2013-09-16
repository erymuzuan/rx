/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />


define(['services/datacontext', 'config', 'viewmodels/map', 'services/logger'],
    function (context, config, mapvm, logger) {

        var isBusy = ko.observable(false),
            activate = function (routeData) {
                var id = parseInt(routeData.id);
                if (id === 0) {
                    vm.land(new bespoke.sph.domain.Land());
                    return true;
                }
                var query = "LandId eq " + id;
                var tcs = new $.Deferred();

                context.loadOneAsync("Land", query)
                    .then(function (land) {
                        vm.land(land);
                        tcs.resolve(true);
                    });
                return tcs.promise();


            },
            viewAttached = function (view) {

            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.land);

                context.post(data, "/Land/Save")
                    .then(function (result) {
                        vm.land().LandId(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
             showMap = function () {

                 var tcs = new $.Deferred();
                 $('#land-map-panel').modal();
                 var point = new google.maps.LatLng(3.1282, 101.6441),
                     landId = vm.land().LandId();
                 if (!landId) {
                     mapvm.geocode(
                          vm.land().Address().Street() + ","
                        + vm.land().Address().City() + ","
                        + vm.land().Address().Postcode() + ","
                        + vm.land().Address().State() + ","
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

                         tcs.resolve(true);
                     });
                     return tcs.promise();
                 }

                 var pathTask = $.get("/Land/GetEncodedPath/" + landId),
                     centerTask = $.get("/Land/GetCenter/" + landId);
                 $.when(pathTask, centerTask)
                 .then(function (path, center) {
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
                         landPolygon = mapvm.add({
                             encoded: path[0],
                             draggable: true,
                             editable: true,
                             zoom: 18
                         });
                     }
                     tcs.resolve(true);

                 });

                 return tcs.promise();
             },
            landPolygon = null,
            polygoncomplete = function (shape) {
                landPolygon = shape;
            },
            saveMap = function () {
                if (!landPolygon) {
                    logger.log("No shape");
                    return false;
                }
                var tcs = new $.Deferred();
                var data = JSON.stringify({ landId: vm.land().LandId(), path: mapvm.getEncodedPath(landPolygon) });
                context
                    .post(data, "/Land/SaveMap")
                    .then(function (e) {
                        logger.log("Map has been successfully saved ", e, "land.detail", true);
                    });
                return tcs.promise();

            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            land: ko.observable(),
            saveMapCommand: saveMap,
            showMapCommand: showMap,
            stateOptions : ko.observableArray([{Name : 'Kelantan'}]),
            toolbar: {
                saveCommand: save
            }
        };

        return vm;

    });
