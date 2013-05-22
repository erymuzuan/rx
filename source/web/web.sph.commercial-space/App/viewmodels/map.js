/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/logger.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />


define(
    function () {

        var geocoder2, map;
        var isBusy = ko.observable(false),
            errors = ko.observableArray(),
            messages = ko.observableArray(),
            getEncodedPath = function (poly) {
                var path = poly.getPath();
                return google.maps.geometry.encoding.encodePath(path);

            },
            clear = function (){},
            add = function (shape) {
                if (!shape) {
                    throw "shape is null";
                }
                if (!shape.encoded) {
                    throw "encoded line is null";
                }
                
                var lines = google.maps.geometry.encoding.decodePath(shape.encoded);
                var polygon = new google.maps.Polygon({
                    paths: lines,
                    map: map,
                    clickable: shape.clikable || true,
                    editable: shape.editable || false,
                    draggable: shape.draggable || false,
                    strokeWeight: shape.strokeWeight || 4,
                    strokeColor: shape.strokeColor || "#2C3CEF",
                    fillOpacity: shape.fillOpacity || 0.5,
                    fillColor: shape.fillColor || "#00AEDB"
                });
                polygon.setMap(map);
            }
        ;

        var vm = {
            map: map,
            isBusy: isBusy,
            errors: errors,
            messages: messages,
            fitToBounds: fitToBounds,
            setCenter: setCenter,
            setZoom: setZoom,
            init: init,
            clear: clear,
            add: clear,
            getEncodedPath: getEncodedPath,
            geocode: geocode
        };
        return vm;

        function init(ops) {

            var ops2 = ops || {};
            var panel = ops2.panel || 'map';
            var zoom = ops2.zoom || 11;

            var center = ops2.center || new google.maps.LatLng(3.1282, 101.6441);

            var options = {
                zoom: zoom,
                center: center,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                streetViewControl: false,
                mapTypeControl: false,
                mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
                navigationControlOptions: { style: google.maps.NavigationControlStyle.DEFAULT }
            };
            map = new google.maps.Map(document.getElementById(panel), options);

            if (ops2.draw) {
                var drawingManager = new google.maps.drawing.DrawingManager({
                    drawingControl: true,
                    drawingControlOptions: {
                        position: google.maps.ControlPosition.TOP_CENTER,
                        drawingModes: [
                          google.maps.drawing.OverlayType.MARKER,
                          google.maps.drawing.OverlayType.CIRCLE,
                          google.maps.drawing.OverlayType.POLYGON,
                          google.maps.drawing.OverlayType.POLYLINE,
                          google.maps.drawing.OverlayType.RECTANGLE
                        ]
                    },
                    polygonOptions: {
                        editable: true
                    }
                });
                if (ops.polygoncomplete) {
                    google.maps.event.addListener(drawingManager, 'polygoncomplete', ops2.polygoncomplete);
                }

                drawingManager.setMap(map);

            }


        }

        function setCenter(lat, lng) {
            var p = new google.maps.LatLng(lat, lng);
            map.setCenter(p);

        }
        function setZoom(zoom) {
            map.setZoom(zoom);
        }

        function fitToBounds(points) {
            var bounds = new google.maps.LatLngBounds();
            for (var i = 0; i < points ; i++) {
                bounds.extend(points.getAt(i));
            }
            map.fitBounds(bounds);

        }


        function geocode(lat, lng) {

            var tcs = new $.Deferred();
            if (!geocoder2)
                geocoder2 = new google.maps.Geocoder();

            var request = { region: "my", location: new google.maps.LatLng(lat, lng) };
            geocoder2.geocode(request, function (result, status) {
                var address = {
                    Street: "",
                    Street2: "",
                    District: "",
                    Postcode: "",
                    State: "",
                    Country: "Malaysia"
                };
                if (status == google.maps.GeocoderStatus.OK) {

                    for (var i = 0; i < result[0].address_components.length; i++) {
                        var add = result[0].address_components[i];
                        if (add.types[0] == 'administrative_area_level_1') {
                            address.State = add.long_name;
                        }
                        if (add.types[0] == 'locality') {
                            address.District = add.long_name;
                        }
                        if (add.types[0] == 'route') {
                            address.Street = add.long_name;
                        }
                    }
                    if (!address.Street) address.Street = result[0].address_components[1].long_name;

                    tcs.resolve(address);
                }
            });

            return tcs.promise();
        }



    });