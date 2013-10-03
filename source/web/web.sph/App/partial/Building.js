/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.BuildingPartial = function (model) {
    var system = require('durandal/system'),
        context = require(objectbuilders.datacontext),
        logger = require(objectbuilders.logger),
        getCustomField = function (name) {
            var cs = _(this.CustomFieldValueCollection()).find(function (v) {
                return v.Name() === name;
            });
            if (!cs) {
                console.log("Cannot find custom field for " + name + " in Building", "CustomField");
                return null;
            }
            return cs.Value;
        },
        getCustomList = function (name) {
            var cs = _(this.CustomListValueCollection()).find(function (v) {
                return v.Name() === name;
            });
            if (!cs) {
                console.log("Cannot find custom list named " + name + " in Building");
                return null;
            }
            return cs;
        },
        addCustomListItem = function (name) {

            return function () {
                var list = _(this.CustomListValueCollection()).find(function (v) {
                    return v.Name() === name;
                });
                var row = new bespoke.sph.domain.CustomListRow(system.guid());
                var columns = _(list.CustomFieldCollection()).map(function (f) {
                    var webid = system.guid();
                    var v = new bespoke.sph.domain.CustomFieldValue(webid);
                    v.Name(f.Name());
                    v.Type(f.Type());
                    return v;
                });

                row.CustomFieldValueCollection(columns);
                list.CustomListRowCollection.push(row);

            };
        },
        removeCustomListItem = function (name, row) {
            var self = this,
                list = _(self.CustomListValueCollection()).find(function (v) {
                    return v.Name() === name;
                });
            return function () {
                list.CustomListRowCollection.remove(row);
            };
        },
        addBlock = function () {
            this.BlockCollection.push(new bespoke.sph.domain.Block(system.guid()));
        },
        removeBlock = function (block) {
            var self = this;
            return function () {
                self.BlockCollection.remove(block);
            };
        },
        editBlockMap = function (block) {
            var building = this;
            return function () {
                console.log("show map ", building);
                console.log(" on block ", block);

                require(['viewmodels/block.map', 'durandal/app'], function (dialog, app) {
                    dialog.init(building.BuildingId(), block.FloorPlanStoreId());
                    app.showModal(dialog)
                        .done(function (result) {
                            if (result == "OK") {
                                block.FloorPlanStoreId(dialog.spatialStoreId());
                            }
                        });

                });

            };
        },
        editBlockFloor = function (block) {
            var building = this;
            return function () {
                console.log("edit floors ", building);
                console.log(" on block ", block);
                if (block.FloorCollection().length === 0) {
                    block.FloorCollection.push(new bespoke.sph.domain.Floor(system.guid()));
                }

                require(['viewmodels/block.floors', 'durandal/app'], function (dialog, app) {
                    dialog.block(block);
                    app.showModal(dialog)
                        .done(function (result) {
                            if (result == "OK") {

                            }
                        });

                });
            };
        },
        staticMap = ko.observable(),
        mapChanged = function (id) {

            if (!id) return;
            var pathTask = $.get("/Building/GetEncodedPath/" + id),
                centerTask = $.get("/Building/GetCenter/" + id);
            $.when(pathTask, centerTask)
                .then(function (path, center) {
                    if (center[0]) {
                        var location = center[0],
                            url =String.format("http://maps.google.com/maps/api/staticmap?center={0},{1}&"
                                + "size=640x300&markers={0},{1}&sensor=false",location.Lat,location.Lng);
                        staticMap(url);
                    }
                });
        },
        saveMap = function (map) {
            var tcs = new $.Deferred();
            context
                 .post(JSON.stringify(map), "/Building/SaveMap")
                 .then(function (e) {
                     tcs.resolve(true);
                     logger.log("Map has been successfully saved ", e, "buildingdetail", true);
                     mapChanged(map.buildingId);

                 });
            return tcs.promise();
        };

   
    if (typeof model.BuildingId === "number") {
        mapChanged(model.BuildingId);
    }
    if (typeof model.BuildingId === "function") {
        mapChanged(model.BuildingId());
        model.BuildingId.subscribe(mapChanged);
    }


    return {
        CustomField: getCustomField,
        CustomList: getCustomList,
        addCustomListItem: addCustomListItem,
        removeCustomListItem: removeCustomListItem,
        addBlock: addBlock,
        editBlockMap: editBlockMap,
        editBlockFloor: editBlockFloor,
        removeBlock: removeBlock,
        staticMap: staticMap,
        saveMap : saveMap
    };
};