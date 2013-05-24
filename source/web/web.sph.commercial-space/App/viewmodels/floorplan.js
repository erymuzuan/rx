/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="map.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/app', 'viewmodels/map'],
    function (context, logger, router, app, map) {

        var title = ko.observable(''),
            buildingId = ko.observable(),
            activeLot = ko.observable(),
            fillColor = ko.observable("#00AEDB"),
            fillOpacity = ko.observable(0.5),
            mapLoaded = false,
            isBusy = ko.observable(false),
            floorname = ko.observable(),
            activate = function (routeData) {

                buildingId(routeData.buildingId);
                floorname(routeData.floorname);


                title('Pelan lantai : ' + floorname());
                var tcs = new $.Deferred();
                var buildingTask = context.loadOneAsync('Building', 'BuildingId eq ' + buildingId());
                var pathTask = $.get('/Building/GetEncodedPath/' + routeData.buildingId);
                var centerTask = $.get('/Building/GetCenter/' + routeData.buildingId);
                $.when(buildingTask, centerTask, pathTask)
                    .then(function (b, center, path) {
                        if (!b) {
                            var lot = {
                                Name: ko.observable(''),
                                Size: ko.observable(''),
                                IsCommercialSpace: ko.observable(true)
                            };
                            vm.floor.LotCollection.push(lot);
                        }
                        var flo = $.grep(b.FloorCollection(), function (x) { return x.Name() === floorname(); })[0];
                        vm.floor.LotCollection(flo.LotCollection());
                        vm.floor.Name(flo.Name());
                        vm.floor.Size(flo.Size());
                        tcs.resolve(true);

                        setTimeout(function () {
                            var panel = document.getElementById('floorplanmap');
                            if (panel && !mapLoaded) {
                                map.init({
                                    panel: 'floorplanmap',
                                    draw: true,
                                    polygoncomplete: polygoncomplete,
                                    center: new google.maps.LatLng(center[0].Lat, center[0].Lng),
                                    zoom: 19
                                });
                                mapLoaded = true;

                                if (path[0]) {
                                    map.add({
                                        encoded: path[0],
                                        draggable: false,
                                        editable: false,
                                        clickable: false,
                                        fillOpacity: 0.8,
                                        strokeWeight: 0.5,
                                        strokeColor: "#FFFFCC",
                                        fillColor: "#FFFFCC"
                                    });
                                }

                            }
                        }, 3000);
                    });

                isBusy(false);
                return tcs.promise();
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({
                    path: map.getEncodedPath(lotPolygon),
                    fillColor: fillColor(),
                    fillOpacity: fillOpacity(),
                    lot: activeLot(),
                    id: buildingId(),
                    floorname: floorname()
                });
                isBusy(true);
                context.post(data, "/Building/AddLotFloorPlan")
                    .then(function (e) {
                        logger.log("Data has been successfully saved ", e, "buildingdetail", true);

                        isBusy(false);
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            goBack = function () {
                var url = "/#/buildingdetail/" + buildingId();
                router.navigateTo(url);
            },
            lotPolygon = null,
            polygoncomplete = function (shape) {
                lotPolygon = shape;
            },
            viewAttached = function() {
                $("#lotlist").on('click', 'a.btn', function () {
                    $('#lotlist a.btn-warning').removeClass('btn-warning')
                        .addClass('btn-success');
                    $(this).removeClass('btn-success').addClass('btn-warning');
                });
            },
            select = function (lot,event,element) {
                isBusy(true);
                activeLot(lot.Name());
                $.get("/Building/GetFloorPlan/" + buildingId() + "?floor=" + floorname() + "&lot=" + lot.Name())
                    .done(function (shape) {
                        if (lotPolygon) {
                            map.remove(lotPolygon);
                        }
                        lotPolygon = null;
                        if (shape.EncodedPolygon) {
                            lotPolygon = map.add({
                                encoded: shape.EncodedPolygon,
                                draggable: true,
                                editable: true,
                                zIndex : 1,
                                fillColor: shape.FillColor,
                                fillOpacity: shape.FillOpacity
                            });
                        }
                        isBusy(false);

                    });
                

                $('#lotlist a.btn').removeClass('btn-warning')
                    .addClass('btn-success');
                $(element).removeClass('btn-success').addClass('btn-warning');
            };

        var vm = {
            activate: activate,
            title: title,
            floor: {
                Name: ko.observable(''),
                Size: ko.observable(''),
                LotCollection: ko.observableArray([])
            },
            goBackCommand: goBack,
            saveCommand: save,
            selectCommand: select,
            isBusy: isBusy,
            viewAttached : viewAttached
        };

        return vm;
    });