/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext',
        'durandal/plugins/router',
        'durandal/system',
        'durandal/app',
        'viewmodels/map',
        'services/logger',
        'services/watcher',
        'config'
],
    function (context, router, system, app, mapvm, logger, watcher, config) {


        var isBusy = ko.observable(false),
            setBuildingToContext = function(building, template) {
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


                _(cfs).each(function(f) {
                    if (!building.CustomField(f.Name())) {
                        building.CustomFieldValueCollection.push(f);
                    }
                });

                _(cls).each(function(f) {
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
                            setBuildingToContext(new bespoke.sph.domain.Building(),template);
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
                var url = '/#/lotdetail/' + vm.building().BuildingId() + '/' + floor.Name();
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
                            logger.info("Bangunan sudah di simpan");
                        } else {
                            logger.logError(e.message, e, this, true);
                        }
                        tcs.resolve(e);
                    });
                return tcs.promise();
            },
            mapInitialized = ko.observable(false),
            geoCode = function (address) {

                var point = new google.maps.LatLng(3.1282, 101.6441);
                
               return mapvm.geocode(address)
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
                 });
            },
            showMap = function () {
                $('#map-panel').modal();
                if (mapInitialized()) {
                    return;
                }
                mapInitialized(true);
                isBusy(true);
                var point = new google.maps.LatLng(3.1282, 101.6441);
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
                    isBusy(false);
                    if (center[0]) {
                        point = new google.maps.LatLng(center[0].Lat, center[0].Lng);
                    } else {
                        geoCode(address);
                        return;
                    }
                    mapvm.init({
                        panel: 'map',
                        draw: true,
                        polygoncomplete: polygoncomplete,
                        zoom: center[0] ? 18 : 12,
                        center: point
                    });
                    if (path[0]) {
                        buildingPolygon = mapvm.add({
                            encoded: path[0],
                            draggable: true,
                            editable: true,
                            zoom: 18
                        });
                    }

                });
            },
            buildingPolygon = null,
            polygoncomplete = function (shape) {
                buildingPolygon = shape;
            },
            saveMap = function () {
                if (!buildingPolygon) {
                    logger.log("No shape");
                    return false;
                }
                var tcs = new $.Deferred();
                var data = JSON.stringify({ buildingId: vm.building().BuildingId(), path: mapvm.getEncodedPath(buildingPolygon) });
                context
                    .post(data, "/Building/SaveMap")
                    .then(function (e) {
                        logger.log("Map has been successfully saved ", e, "buildingdetail", true);
                    });
                return tcs.promise();

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