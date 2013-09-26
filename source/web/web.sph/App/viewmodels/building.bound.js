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
            center =  new google.maps.LatLng(3.1282, 101.6441),
            activate = function () {
                logger.info("Loading map....");
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
                isZoom = false;
                vm.selectedBuilding(b.building);
            },
             deactivate = function() {
                 center = mapvm.getCenter();
             };

        var vm = {
            activate: activate,
            deactivate : deactivate,
            viewAttached: viewAttached,
            highlightCommand: highlight,
            buildingCollection: buildingCollection,
            selectedBuilding: ko.observable(new bespoke.sph.domain.Building()),
            isBusy: isBusy
        };

        return vm;


        function createMap() {
            $('#map-buildingbound').html('<img alt="loading" src="/Images/spinner-md.gif">');
            buildingCollection.removeAll();
            mapvm.init({
                panel: 'map-buildingbound',
                zoom: 13,
                center: center,
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

                    var markers = _(list).chain().map(function (b) {

                        var exist = _(buildingCollection()).find(function (v) {
                            return v.building.BuildingId() === b.BuildingId;
                        });
                        if (exist) return null;

                        var marker = mapvm.add({
                            encoded: b.EncodedWkt,
                            draggable: false,
                            editable: false,
                            clickable: false,
                            fillOpacity: 0.8,
                            strokeWeight: 0.5,
                            strokeColor: "#FF2800",
                            title: b.Name,
                            icon: '/images/maps/office-building.png'
                        }),
                            info = new google.maps.InfoWindow({
                                content: '<img alt="loading" src="/Images/spinner-md.gif">',
                                maxWidth: 400
                            });
                        google.maps.event.addListener(marker, 'click', function () {
                            vm.selectedBuilding(b);
                            info.open(marker.getMap(), marker);
                            info.setContent($('#selected-building-panel').html());
                        });

                        return {
                            building: ko.mapping.fromJS(b),
                            polygon: marker
                        };
                    })
                        .each(function (m) {
                            if (null !== m) {
                                buildingCollection.push(m);
                            }
                        })
                        .value();

                    tcs.resolve(markers);
                });

            return tcs.promise();
        }
    });