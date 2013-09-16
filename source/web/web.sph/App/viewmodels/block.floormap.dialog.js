/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="map.js" />

define(['durandal/system', 'services/datacontext', 'durandal/plugins/router', 'viewmodels/map'],
    function (system, context, router, map) {
        
        var isBusy = ko.observable(false),
            activate = function (routeData) {
                
                var tcs = new $.Deferred();
                //get address or building location
                var geocode = $.get('http://maps.googleapis.com/maps/api/geocode/json?address=36+Taman+Dato+Demang,+Seri+Kembangan,+Selangor&sensor=false');
                $.when(geocode)
                    .then(function (gc) {
                        if (gc.status == 'OK') {
                            var loc = gc.results[0].geometry.location;
                            setTimeout(function () {
                                var panel = document.getElementById('blockfloormap');
                                if (panel) {
                                    map.init({
                                        panel: 'blockfloormap',
                                        draw: true,
                                        polygoncomplete: polygoncomplete,
                                        center: new google.maps.LatLng(loc.lat, loc.lng),
                                        zoom: 17
                                    });

                                    //if (path[0]) {
                                    //    map.add({
                                    //        encoded: path[0],
                                    //        draggable: false,
                                    //        editable: false,
                                    //        clickable: false,
                                    //        fillOpacity: 0.8,
                                    //        strokeWeight: 0.5,
                                    //        strokeColor: "#FFFFCC",
                                    //        fillColor: "#FFFFCC"
                                    //    });
                                    //}

                                }
                            }, 2000);
                            tcs.resolve(true);
                        }
                    });
                isBusy(false);
                return tcs.promise();
            },
            okClick = function () {
                this.modal.close("OK");
            },
            cancelClick = function () {
                this.modal.close("Cancel");
            },
            lotPolygon = null,
            polygoncomplete = function (shape) {
                lotPolygon = shape;
            };
        var vm = {
            isBusy: isBusy,
            block: ko.observable(new bespoke.sphcommercialspace.domain.Block()),
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick
        };
        return vm;
    });