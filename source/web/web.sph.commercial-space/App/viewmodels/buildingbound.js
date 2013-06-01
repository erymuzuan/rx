/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="map.js" />
/// 
define(['services/datacontext',
        'durandal/plugins/router',
        'durandal/system',
        'durandal/app',
        'viewmodels/map',
        'services/logger'
],
    function (datacontext, router, system, app, mapvm, logger) {


        var isBusy = ko.observable(false),
            isZoom = false,
            buildingCollection = ko.observableArray(),
            activate = function () {
                return true;
            },
            viewAttached = function () {
                mapvm.setupAutocomplete(document.getElementById('search'));
                $('form.form-search').on('click', 'a', function () {
                    $('form.form-search')
                        .find('input')
                        .val("")
                        .focus();
                });
                createMap();
            },
            highlight = function (b) {
                _.each(buildingCollection(), function (g) {
                    g.polygon.setOptions({ fillColor: "white" });
                });
                b.polygon.setOptions({ fillColor: "#FF2800" });
                /* 
                isZoom = true;
                mapvm.setZoom(16);
                mapvm.setCenter(mapvm.getCenter(b.polygon));
                */
                isZoom = false;
                ko.mapping.fromJS(ko.mapping.toJS(b.building), {}, vm.selectedBuilding);
                vm.selectedBuilding.DetailsUrl("/#/buildingdetail/" + b.building.BuildingId());
            };

        var vm = {
            activate: activate,
            viewAttached: viewAttached,
            highlightCommand: highlight,
            buildingCollection: buildingCollection,
            selectedBuilding: {
                Name: ko.observable(''),
                Floors: ko.observable(1),
                BuildingId: ko.observable(0),
                Address: {
                    Street: ko.observable(''),
                    State: ko.observable(''),
                    City: ko.observable(''),
                    Postcode: ko.observable(''),
                },
                LotNo: ko.observable(''),
                Size: ko.observable(''),
                Status: ko.observable(''),
                DetailsUrl : ko.observable()
            },
            isBusy: isBusy
        };

        return vm;


        function createMap() {
            var point = new google.maps.LatLng(3.1282, 101.6441);
            mapvm.init({
                panel: 'map-buildingbound',
                zoom: 13,
                center: point,
                idle: idle
            });
        }

        function idle() {
            if (isZoom) {
                return false;
            }
            var tcs = new $.Deferred();
            var bound = mapvm.getBounds();
            if (bound.indexOf('NaN') > -1) {
                return false;
            }
            $.get("/Map/Get/" + bound)
                .then(function (list) {

                    mapvm.clear();
                    var buildingPolygons = _.map(list, function (b) {
                        return {
                            building: ko.mapping.fromJS(b),
                            polygon:
                                mapvm.add({
                                    encoded: b.EncodedWkt,
                                    draggable: false,
                                    editable: false,
                                    clickable: false,
                                    fillOpacity: 0.8,
                                    strokeWeight: 0.5,
                                    strokeColor: "#FF2800"
                                })
                        };
                    });
                    buildingCollection(buildingPolygons);
                    tcs.resolve(list);
                });

            return tcs.promise();
        }
    });