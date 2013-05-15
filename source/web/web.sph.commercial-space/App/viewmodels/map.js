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
            messages = ko.observableArray()
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
            geocode: geocode
        };
        return vm;

        function init() {

            var klangValley = new google.maps.LatLng(3.1282, 101.6441);
            var options = {
                zoom: 11,
                center: klangValley,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                streetViewControl: false,
                mapTypeControl: false,
                mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
                navigationControlOptions: { style: google.maps.NavigationControlStyle.DEFAULT }
            };
            map = new google.maps.Map(document.getElementById('map'), options);
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