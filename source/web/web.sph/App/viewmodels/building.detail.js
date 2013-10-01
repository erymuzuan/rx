/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="../objectbuilders.js" />

define(['services/datacontext',
        'durandal/plugins/router',
        'durandal/system',
        'durandal/app',
        'viewmodels/map',
        'services/logger',
        'services/watcher',
        'config',
        objectbuilders.cultures
],
    function (context, router, system, app, map, logger, watcher, config, cultures) {


        var isBusy = ko.observable(false),
            setBuildingToContext = function (building, template) {
                var fieldToValueMap = function (f) {
                    var webid = system.guid();
                    var v = new bespoke.sph.domain.CustomFieldValue(webid);
                    v.Name(f.Name());
                    v.Type(f.Type());
                    return v;
                },
                          cfs = _(template.CustomFieldCollection()).map(fieldToValueMap),
                          cls = _(template.CustomListDefinitionCollection()).map(function (v) {
                              var lt = new bespoke.sph.domain.CustomListValue(system.guid());
                              lt.Name(v.Name());

                              var fields = _(v.CustomFieldCollection()).map(fieldToValueMap);
                              lt.CustomFieldCollection = ko.observableArray(fields);

                              return lt;
                          });


                _(cfs).each(function (f) {
                    if (!building.CustomField(f.Name())) {
                        building.CustomFieldValueCollection.push(f);
                    }
                });

                _(cls).each(function (f) {
                    if (!building.CustomList(f.Name())) {
                        building.CustomListValueCollection.push(f);
                    }
                });

                vm.building(building);
                vm.building().TemplateId(template.BuildingTemplateId());
                vm.building().TemplateName(template.Name());

            },
            activate = function (routeData) {
                // NOTE : this is the only way to debug as this file is returned as a result of redirect
                //debugger;
                var id = parseInt(routeData.id),
                    templateId = parseInt(routeData.templateId),
                    tcs = new $.Deferred();

                vm.stateOptions(config.stateOptions);
                mapInitialized(false);

                // build custom fields value
                context.loadOneAsync("BuildingTemplate", "BuildingTemplateId eq " + templateId)
                    .done(function (template) {

                        // new building
                        if (!id) {
                            setBuildingToContext(new bespoke.sph.domain.Building(), template);
                            vm.toolbar.watching(false);
                            tcs.resolve();
                            return;
                        } else {

                            vm.toolbar.auditTrail.id(id);
                            vm.toolbar.printCommand.id(id);

                            var query = "BuildingId eq " + id,
                                loadTask = context.loadOneAsync("Building", query),
                                watcherTask = watcher.getIsWatchingAsync("Building", id);

                            $.when(loadTask, watcherTask).done(function (b, w) {
                                setBuildingToContext(b, template);
                                vm.toolbar.watching(w);

                                tcs.resolve(true);
                            });

                        }

                    });


                return tcs.promise();
            },
            addLot = function (floor) {
                var url = '/#/unit.list/' + vm.building().BuildingId();
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
                var floor = new bespoke.sph.domain.Floor();
                vm.building().FloorCollection.push(floor);
            },

            saveAsync = function () {
                var tcs = new $.Deferred();
                var data = ko.toJSON(vm.building());
                context.post(data, "/Building/Save")
                    .done(function (e) {
                        if (e.status) {
                            vm.building().BuildingId(e.buildingId);
                            logger.info(cultures.building.SAVE_BUILDING_MESSAGE);
                        } else {
                            logger.logError(e.message, e, this, true);
                        }
                        tcs.resolve(e);
                    });
                return tcs.promise();
            },
            mapInitialized = ko.observable(false),
            geoCode = function (address) {


                return map.geocode(address)
                  .then(function (result) {
                      if (result.status) {
                          map.init({
                              panel: 'map',
                              draw: true,
                              polygoncomplete: polygoncomplete,
                              markercomplete: markercomplete,
                              zoom: 18,
                              center: result.point
                          });
                      } else {
                          var point = new google.maps.LatLng(3.1282, 101.6441);
                          map.init({
                              panel: 'map',
                              draw: true,
                              polygoncomplete: polygoncomplete,
                              zoom: center[0] ? 18 : 12,
                              center: point
                          });
                      }
                  });
            },
            showMap = function () {
                $('#map-panel').modal();
                if (mapInitialized()) {
                    return;
                }
                mapInitialized(true);
                var buildingId = vm.building().BuildingId(),
                    address = vm.building().Address().Street() + ","
                        + vm.building().Address().City() + ","
                        + vm.building().Address().Postcode() + ","
                        + vm.building().Address().State() + ","
                        + "Malaysia.";

                if (!buildingId) {
                    geoCode(address);
                    return;
                }

                var pathTask = $.get("/Building/GetEncodedPath/" + buildingId);
                var centerTask = $.get("/Building/GetCenter/" + buildingId);
                $.when(pathTask, centerTask)
                .then(function (path, center) {
                    if (!center[0]) {
                        geoCode(address);
                        return;
                    }
                    map.init({
                        panel: 'map',
                        draw: true,
                        polygoncomplete: polygoncomplete,
                        markercomplete: markercomplete,
                        zoom: center[0] ? 18 : 12
                    }).done(function () {
                        if (center[0]) {
                            map.setCenter(center[0].Lat, center[0].Lng);
                        } else {
                            map.setCenter(3.1282, 101.6441);
                        }
                        if (path[0]) {
                            var shape = map.add({
                                encoded: path[0],
                                draggable: true,
                                editable: true,
                                zoom: 18
                            });
                            if (shape.type === 'marker') {
                                pointMarker = shape;
                            }

                            if (shape.type === 'polygon') {
                                buildingPolygon = shape;
                            }
                        }


                    });

                });
            },
            buildingPolygon = null,
            polygoncomplete = function (shape) {
                buildingPolygon = shape;
            },
            pointMarker = null,
            markercomplete = function (marker) {
                console.log(marker);
                marker.setOptions({ draggable: true });
                if (pointMarker) pointMarker.setMap(null);

                pointMarker = marker;
            },
            saveMap = function () {
                if (!buildingPolygon && !pointMarker) {
                    logger.error("No shape");
                    return false;
                }
                var data = {
                    buildingId: vm.building().BuildingId()
                };
                if (buildingPolygon) {
                    data.path = map.getEncodedPath(buildingPolygon);
                }
                if (pointMarker) {
                    data.point = {
                        lat: pointMarker.getPosition().lat(),
                        lng: pointMarker.getPosition().lng()
                    };
                }
                return vm.building().saveMap(data);

            },
            viewAttached = function () {
                $('*[title]').tooltip({ placement: 'right' });
            },
            remove = function () {
                return app.showMessage("Adakah anda pasti untuk buang bangunan ini dari rekod", "Buang Rekod", ["Ya", "Tidak"])
                      .done(function (result) {
                          if (result === "Ya") {
                              var tcs = new $.Deferred();
                              var data = ko.mapping.toJSON(vm.building);

                              context.post(data, "/Building/Remove")
                                  .then(function (msg) {
                                      tcs.resolve(true);
                                      if (msg.status === "OK") {
                                          logger.info("Bangunan sudah berjaya di buang");
                                          router.navigateTo("/#/building.list");
                                      } else {
                                          logger.error(msg.message);
                                      }
                                  });
                              return tcs.promise();
                          }

                          return Task.fromResult(true);
                      });
            };

        var vm = {
            activate: activate,
            building: ko.observable(new bespoke.sph.domain.Building()),
            template: ko.observable(new bespoke.sph.domain.BuildingTemplate()),
            stateOptions: ko.observableArray(),
            saveCommand: saveAsync,
            showMapCommand: showMap,
            saveMapCommand: saveMap,
            addFloorCommand: addFloor,
            addLotCommand: addLot,
            viewFloorPlanCommand: viewFloorPlan,
            goBackCommand: goBack,
            isBusy: isBusy,
            viewAttached: viewAttached,
            removeFloorCommand: removeFloor,
            title: 'Perincian Bangunan',
            cultures: cultures,
            toolbar: {
                watchCommand: function () { return watcher.watch("Building", vm.building().BuildingId()); },
                unwatchCommand: function () { return watcher.unwatch("Building", vm.building().BuildingId()); },
                watching: ko.observable(false),
                saveCommand: saveAsync,
                auditTrail: {
                    entity: "Building",
                    id: ko.observable()
                },
                printCommand: {
                    entity: "Building",
                    id: ko.observable()
                },
                removeCommand: remove
            }
        };

        return vm;
    });