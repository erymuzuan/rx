/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/logger.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/_task.js" />


define([],
    function () {

        var geocoder2, map;
        var overlays = [],
            isBusy = ko.observable(false),
            errors = ko.observableArray(),
            messages = ko.observableArray(),
            getEncodedPath = function (poly) {
                if (poly) {
                    var path = poly.getPath();
                    return google.maps.geometry.encoding.encodePath(path);
                }
                throw "The arguement cannot be null, for getEncodedPath, please get a polygon maybe";

            },
            remove = function (overlay) {
                overlay.setMap(null);
                var list = _.without(overlays, overlay);
                overlays.length = 0;
                overlays = list;
            },
            clear = function () {
                $.each(overlays, function (i, v) {
                    v.setMap(null);
                });
                overlays.length = 0;
            },
            add = function (shape) {
                if (!shape) {
                    throw "shape is null";
                }
                if (!shape.encoded) {
                    throw "encoded line is null";
                }

                var lines = google.maps.geometry.encoding.decodePath(shape.encoded);
                if (lines.length === 1) {// point
                    var marker = new google.maps.Marker({
                        position: lines[0],
                        map: map,
                        zIndex: shape.zIndex || 0,
                        clickable: shape.clikable || true,
                        draggable: shape.draggable || false,
                        title: shape.title,
                        icon: shape.icon
                    });
                    marker.setMap(map);
                    marker.type = 'marker';
                    overlays.push(marker);

                    google.maps.event.addListener(marker, 'dragend', function () {
                        console.log(marker.getPosition());
                    });

                    return marker;
                }
                var polygon = new google.maps.Polygon({
                    paths: lines,
                    map: map,
                    zIndex: shape.zIndex || 0,
                    clickable: shape.clikable || true,
                    editable: shape.editable || false,
                    draggable: shape.draggable || false,
                    strokeWeight: shape.strokeWeight || 4,
                    strokeColor: shape.strokeColor || "#2C3CEF",
                    fillOpacity: shape.fillOpacity || 0.5,
                    fillColor: shape.fillColor || "#00AEDB"
                });
                polygon.setMap(map);
                polygon.type = 'polygon';
                overlays.push(polygon);

                return polygon;
            },
            setupAutocomplete = function (input, placeChanged) {

                var options = { componentRestrictions: { country: 'my' } };
                var autocomplete = new google.maps.places.Autocomplete(input, options);
                if (map) {
                    autocomplete.bindTo('bounds', map);
                }
                google.maps.event.addListener(autocomplete, 'place_changed', function () {
                    var place = autocomplete.getPlace();

                    if (map) {
                        if (place.geometry.viewport) {
                            map.fitBounds(place.geometry.viewport);
                        } else {
                            map.setCenter(place.geometry.location);
                            map.setZoom(17);
                        }

                    }

                    var address = '';
                    if (place.address_components) {
                        address = [
                          (place.address_components[0] && place.address_components[0].short_name || ''),
                          (place.address_components[1] && place.address_components[1].short_name || ''),
                          (place.address_components[2] && place.address_components[2].short_name || '')
                        ].join(' ');
                    }
                    if (placeChanged)
                        placeChanged(address);
                });
            },
            loadGoogleMapLibs = function () {
                if (typeof google.maps === "undefined") {
                    var tcs1 = new $.Deferred();
                    google.load('maps', '3.6', {
                        other_params: 'region=my&sensor=false&libraries=geometry,drawing,places',
                        callback: function () {
                            tcs1.resolve(true);
                        }
                    });

                    return tcs1.promise();
                }

                return Task.fromResult(true);
            };


        var vm = {
            map: map,
            isBusy: isBusy,
            errors: errors,
            messages: messages,
            fitToBounds: fitToBounds,
            setCenter: setCenter,
            getCenter: getCenter,
            getMapCenter: function () {
                return map.getCenter();
            },
            loadGoogleMapLibs: loadGoogleMapLibs,
            setZoom: setZoom,
            init: init,
            clear: clear,
            remove: remove,
            add: add,
            getEncodedPath: getEncodedPath,
            getBounds: getBound,
            geocode: geocode,
            reverseGeocode: reverseGeocode,
            setupAutocomplete: setupAutocomplete
        };
        return vm;



        function init(ops) {

            if (typeof google.maps === "undefined") {
                var tcs1 = new $.Deferred();
                loadGoogleMapLibs().done(function () {
                    init(ops).done(function (p) {
                        tcs1.resolve(p);
                    });
                });

                return tcs1.promise();
            }


            var ops2 = ops || {},
                panel = ops2.panel || 'map',
                zoom = ops2.zoom || 11,
                center = ops2.center || new google.maps.LatLng(3.1282, 101.6441);

            google.maps.visualRefresh = true;

            var options = {
                zoom: zoom,
                center: center,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                streetViewControl: false,
                scaleControl: true,
                mapTypeControl: false,
                mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
                navigationControlOptions: { style: google.maps.NavigationControlStyle.DEFAULT }
            };
            map = new google.maps.Map(document.getElementById(panel), options);

            if (ops.idle) {
                google.maps.event.addListener(map, 'idle', function (e) {
                    ops.idle(e);
                });
            }

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
                    google.maps.event.addListener(drawingManager, 'polygoncomplete', function (pg) {
                        pg.type = 'polygon';
                        overlays.push(pg);
                        ops2.polygoncomplete(pg);
                    });
                }
                if (ops.circlecomplete) {
                    google.maps.event.addListener(drawingManager, 'circlecomplete', function (circle) {
                        overlays.push(circle);
                        ops2.circlecomplete(circle);
                    });
                }
                if (ops.polylinecomplete) {
                    google.maps.event.addListener(drawingManager, 'polylinecomplete', function (polyline) {
                        overlays.push(polyline);
                        ops2.polylinecomplete(polyline);
                    });
                }
                if (ops.rectanglecomplete) {
                    google.maps.event.addListener(drawingManager, 'rectanglecomplete', function (rectangle) {
                        overlays.push(rectangle);
                        ops2.rectanglecomplete(rectangle);
                    });
                }
                if (ops.markercomplete) {
                    google.maps.event.addListener(drawingManager, 'markercomplete', function (marker) {
                        overlays.push(marker);
                        marker.type = 'marker';
                        ops2.markercomplete(marker);
                    });
                }

                drawingManager.setMap(map);

            }

            return Task.fromResult(true);
        }


        function getCenter(polygon) {
            var bounds = new google.maps.LatLngBounds();
            var i;
            var polygonCoords = polygon.getPath().getArray();

            for (i = 0; i < polygonCoords.length; i++) {
                bounds.extend(polygonCoords[i]);
            }

            return bounds.getCenter();
        }


        function setCenter(latOrPoint, lng) {
            var lat = 0;
            if (typeof latOrPoint === "object") {
                lat = latOrPoint.lat;
                lng = latOrPoint.lng;
            }
            var p = new google.maps.LatLng(lat, lng);
            map.setCenter(p);

        }
        function setZoom(zoom) {
            map.setZoom(zoom);
        }

        function getBound() {
            var bound = map.getBounds();
            return bound.getSouthWest().lat() + ',' + bound.getSouthWest().lng() + '|' + bound.getNorthEast().lat() + ',' + bound.getNorthEast().lng();

        }

        function fitToBounds(points) {
            var bounds = new google.maps.LatLngBounds();
            for (var i = 0; i < points ; i++) {
                bounds.extend(points.getAt(i));
            }
            map.fitBounds(bounds);

        }

        function geocode(address) {
            if (typeof google.maps === "undefined") {
                var tcs1 = new $.Deferred();
                google.load('maps', '3.6', {
                    other_params: 'region=my&sensor=false&libraries=geometry,drawing,places',
                    callback: function () {
                        geocode(address)
                            .done(function (p) {
                                tcs1.resolve(p);
                            });
                    }
                });

                return tcs1.promise();
            }

            var tcs = new $.Deferred();
            if (!geocoder2)
                geocoder2 = new google.maps.Geocoder();
            geocoder2.geocode({ 'address': address, region: "my" }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    tcs.resolve({
                        status: true,
                        point: results[0].geometry.location
                    });
                } else {
                    tcs.resolve({
                        status: false,
                        message: "Geocode was not successful for the following reason: " + status
                    });
                }
            });

            return tcs.promise();

        }

        function reverseGeocode(point) {

            var tcs = new $.Deferred();
            if (!geocoder2)
                geocoder2 = new google.maps.Geocoder();

            var request = { region: "my", location: point };
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
                        var add1 = result[0].address_components[i];
                        if (add1.types[0] == 'administrative_area_level_1') {
                            address.State = add1.long_name;
                        }
                        if (add1.types[0] == 'locality') {
                            address.District = add1.long_name;
                        }
                        if (add1.types[0] == 'route') {
                            address.Street = add1.long_name;
                        }
                    }
                    if (!address.Street) address.Street = result[0].address_components[1].long_name;

                    tcs.resolve(address);
                }
            });

            return tcs.promise();
        }



    });