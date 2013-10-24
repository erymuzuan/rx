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
/// <reference path="../objectbuilders.js" />
/// <reference path="../schemas/sph.domain.g.js" />

define([objectbuilders.datacontext, objectbuilders.logger, './_space.contract', 'durandal/system', objectbuilders.config, objectbuilders.cultures, objectbuilders.map, objectbuilders.validation],
    function (context, logger, contractlistvm, system, config, cultures, map, validation) {

        var title = ko.observable(),
            isBusy = ko.observable(false),
            m_template = ko.observable(),
            activate = function (routeData) {
                vm.selectedBuildingId(0);

                vm.space().BuildingId(parseInt(routeData.buildingId));
                var templateId = parseInt(routeData.templateId);

                var tcs = new $.Deferred(),
                    templateTask = context.loadOneAsync("SpaceTemplate", "SpaceTemplateId eq " + templateId),
                    buildingTask = context.getTuplesAsync("Building", "", "BuildingId", "Name"),
                    getSpaceTask = context.loadOneAsync("Space", "SpaceId eq " + routeData.csId);

                $.when(templateTask, buildingTask, getSpaceTask)
                    .done(function (a, buildingNameList) {
                        vm.buildingOptions(_(buildingNameList).sortBy(function (building) {
                            return building.Item2;
                        }));
                        vm.buildingOptions.push({ Item1: 0, Item2: cultures.space.ADD_NEW_BUILDING });
                    })
                    .done(function (template, b, space) {

                        m_template(template);
                        vm.stateOptions(config.stateOptions);
                        if (!space) {
                            space = new bespoke.sph.domain.Space();
                            space.TemplateName(template.Name());
                            space.TemplateId(templateId);
                            title(template.Name());

                            // default values
                            _(template.DefaultValueCollection()).each(function (v) {
                                if (v.Value()) {
                                    var props = v.PropertyName().split(".");
                                    if (props.length === 1) {
                                        space[props[0]](v.Value());
                                        return;
                                    }
                                    var k = null;
                                    for (var i = 0; i < props.length - 1; i++) {
                                        if (typeof k === "function") {
                                            k = space[props[i]]();
                                        } else {
                                            k = space[props[i]];
                                        }

                                    }
                                    if (typeof k === "function") {
                                        k()[props[props.length - 1]](v.Value());
                                    } else {
                                        throw "What the fuck is wrong with k,, !! ima kata : astrairllah";
                                    }
                                }
                            });

                            vm.space(space);

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

                            vm.space().CustomFieldValueCollection(cfs);
                            vm.space().CustomListValueCollection(cls);
                            return;
                        }

                        space.TemplateName(template.Name());
                        space.TemplateId(templateId);
                        if (!space.MapIcon()) space.MapIcon(template.DefaultMapIcon());
                        if (!space.SmallIcon()) space.SmallIcon(template.DefaultSmallIcon());
                        if (!space.Icon()) space.Icon(template.DefaultIcon());
                        // NOTE : the browser keeps the value as string
                        space.ApplicationTemplateOptions(_(space.ApplicationTemplateOptions()).map(function (v) {
                            return v.toString();
                        }));
                        vm.selectedBuildingId(space.BuildingId());
                        vm.space(space);


                        title('Maklumat ruang komersil');
                        contractlistvm.activate(routeData)
                            .then(function () {
                                tcs.resolve(true);
                            });
                    })
                    .done(function () {
                        tcs.resolve();
                    });

                return tcs.promise();
            },
            viewAttached = function (view) {
                validation.init($('#space-detail-form'), m_template());
                $(view).tooltip({ 'placement': 'right' });
            },
            saveCs = function () {
                if (!validation.valid()) {
                    return Task.fromResult(false);
                }
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.space());
                isBusy(true);
                context.post(data, "/Space/Save")
                    .done(function (e) {
                        logger.log("Data has been successfully saved ", e, "space.detail", true);

                        isBusy(false);
                        tcs.resolve(true);
                    });
                return tcs.promise();
            },
            selectUnit = function () {
                $('#lot-selection-panel').modal();
            },
            addUnits = function () {
                vm.space().UnitCollection(vm.selectedUnits);
                var units = ko.mapping.toJS(vm.selectedUnits),
                    unitNo = _(units).map(function (u) { return u.No; }).join(","),
                    size = _(units).reduce(function (memo, lot) { return memo + lot.Size; }, 0);

                vm.space().UnitNo(unitNo);
                vm.space().Size(size);
                vm.space().UnitCollection(vm.selectedUnits);
                // load the building address
                var query = String.format("BuildingId eq {0}", vm.selectedBuildingId()),
                    tcs = new $.Deferred();
                context.loadOneAsync("Building", query)
                    .done(function (b) {
                        vm.space().Address(b.Address());
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            addFeatures = function () {
                var feature = bespoke.sph.domain.FeatureDefinition();
                vm.space().FeatureDefinitionCollection.push(feature);
            },
            removeFeatures = function (feature) {
                vm.space().FeatureDefinitionCollection.remove(feature);
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

                if (vm.space().SpaceId() === 0) {
                    logger.info("Sila simpan dulu ruang ini");
                    return;
                }

                $('#map-panel').modal();
                if (mapInitialized()) {
                    return;
                }
                mapInitialized(true);
                var spaceId = vm.space().SpaceId(),
                    address = vm.space().Address().Street() + ","
                        + vm.space().Address().City() + ","
                        + vm.space().Address().Postcode() + ","
                        + vm.space().Address().State() + ","
                        + "Malaysia.";

                if (!spaceId) {
                    geoCode(address);
                    return;
                }

                var pathTask = $.get("/Space/GetEncodedPath/" + spaceId);
                var centerTask = $.get("/Space/GetCenter/" + spaceId);
                $.when(pathTask, centerTask)
                    .then(function (path, center) {
                        if (center[0]) {
                            //var point = new google.maps.LatLng(center[0].Lat, center[0].Lng);
                        } else {
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
                            map.setCenter(center[0].Lat, center[0].Lng);
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
                    spaceId: vm.space().SpaceId()
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
                return vm.space().saveMap(data);

            },
            selectedBuilding = ko.observable(new bespoke.sph.domain.Building());

        var vm = {
            activate: activate,
            title: title,
            viewAttached: viewAttached,
            space: ko.observable(new bespoke.sph.domain.Space()),
            buildingOptions: ko.observableArray(),
            validationOptions: ko.observable(),
            blockOptions: ko.observableArray(),
            floorOptions: ko.observableArray(),
            unitOptions: ko.observableArray(),
            stateOptions: ko.observableArray(),
            selectedBuildingId: ko.observable(),
            selectedBlock: ko.observable(),
            selectedFloor: ko.observable(),
            selectedUnits: ko.observableArray(),
            toolbar: {
                saveCommand: saveCs
            },
            showMapCommand: showMap,
            saveMapCommand: saveMap,
            selectUnitCommand: selectUnit,
            addUnitsCommand: addUnits,
            addFeaturesCommand: addFeatures,
            removeFeaturesCommand: removeFeatures,
            isBusy: isBusy
        };

        selectedBuilding.subscribe(function (b) {
            console.log(b);
        });

        vm.selectedBuildingId.subscribe(function (id) {
            vm.space().BuildingId(id);
            if (!id) return;
            context.loadOneAsync("Building", "BuildingId eq " + id)
                .then(function (b) {
                    if (!b) return;
                    var floors = b.FloorCollection(),
                        blocks = b.BlockCollection();

                    selectedBuilding(b);
                    vm.floorOptions(floors);
                    vm.blockOptions(blocks);
                    vm.space().Address(b.Address());
                });
        });

        vm.selectedBlock.subscribe(function (block) {
            if (block) {
                vm.floorOptions(block.FloorCollection());
            } else {
                vm.floorOptions(selectedBuilding().FloorCollection());
            }
        });

        vm.selectedFloor.subscribe(function (floor) {
            if (!floor) return;
            if (!floor.Name) return;
            vm.space().FloorName(floor.Name());
            vm.unitOptions(floor.UnitCollection());
        });

        return vm;
    });