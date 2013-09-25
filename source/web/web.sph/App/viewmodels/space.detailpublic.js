/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../objectbuilders.js" />

define(['services/datacontext', 'services/logger', './_space.contract', 'durandal/system', 'config', objectbuilders.cultures, objectbuilders.map],
    function (context, logger, contractlistvm, system, config, cultures, map) {

        var title = ko.observable(),
            activate = function (routeData) {
                var id = parseInt(routeData.id),
                    tcs = new $.Deferred();
                context.loadOneAsync("Space", "SpaceId eq " + id).done(
                    function (space) {
                        vm.space(space);
                        // get the building
                        var buildingId = space.BuildingId();
                        if (buildingId) {
                            context.loadOneAsync("Building", "BuildingId eq " + buildingId)
                                .done(function (b) {
                                    vm.building(b);
                                    tcs.resolve(true);
                                });
                        } else {

                            tcs.resolve(true);
                        }

                        var templates = space.ApplicationTemplateOptions().join(",");
                        context.loadAsync("ApplicationTemplate", String.format("ApplicationTemplateId in ({0})", templates))
                            .done(function(lo) {
                                vm.applicationTemplates(lo.itemCollection);
                            });
                    });



                return tcs.promise();
            },
            viewAttached = function (view) {
                $(view).tooltip({ 'placement': 'right' });
                // load map

                var buildingId = vm.building().BuildingId(),
                    point = new google.maps.LatLng(3.1282, 101.6441),
                    pathTask = $.get("/Building/GetEncodedPath/" + buildingId),
                    centerTask = $.get("/Building/GetCenter/" + buildingId);

                $.when(pathTask, centerTask)
                .then(function (path, center) {
                    if (center[0]) {
                        point = new google.maps.LatLng(center[0].Lat, center[0].Lng);
                    } else {
                        $('#map-space').hide();
                        return;
                    }
                    var staticMap = String.format("http://maps.googleapis.com/maps/api/staticmap?size=640x300&markers="+
                        "icon:{2}&center={0},{1}&sensor=false&zoom=18",
                        point.lat(), point.lng(),"https://s3-ap-southeast-1.amazonaws.com/sph.my/map-icons/office-building.png");
                    $('#map-space').html('<img src="' + staticMap + '" alt="map"/>')
                        .on('click', function () {

                            map.init({
                                panel: 'map-space',
                                zoom: center[0] ? 18 : 12,
                                center: point
                            });
                            if (path[0]) {
                                var shape = map.add({
                                    encoded: path[0],
                                    draggable: true,
                                    editable: true,
                                    zoom: 18,
                                    icon: '/images/maps/office-building.png'
                                });
                            }

                        });


                });
                

            },
            showPhoto = function (photo) {
                vm.photo(photo);
                $('#photo-dialog').modal();
            };

        var vm = {
            activate: activate,
            title: title,
            map: ko.observable(),
            viewAttached: viewAttached,
            space: ko.observable(new bespoke.sph.domain.Space()),
            building: ko.observable(new bespoke.sph.domain.Building()),
            photo : ko.observable(new bespoke.sph.domain.Photo()),
            showPhoto: showPhoto,
            applicationTemplates : ko.observableArray()
        };



        return vm;
    });