/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />

var bespoke = bespoke || {};
 
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.BlockPartial = function () {
    var system = require('durandal/system'),
        addFloor = function() {
            this.FloorCollection.push(new bespoke.sph.domain.Floor(system.guid()));
        },
        editFloor = function(block) {
            var self = this;
            return function() {
                require(['viewmodels/block.dialog', 'durandal/app'], function(dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(block));
                    dialog.block(clone);
                    app.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BlockCollection.replace(block, clone);
                            }
                        });
                });
            };
        },
        removeFloor = function (floor) {
            var self = this;
            return function () {
                self.FloorCollection.remove(floor);
            };
        },
        editFloorMap = function (block) {
            var building = this;
            return function () {
                console.log("show map ", building);
                console.log(" on block ", block);
                require(['viewmodels/block.map', 'durandal/app'], function (dialog, app) {
                    dialog.init(building.BuildingId(), block.FloorPlanStoreId());
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (result == "OK") {
                                block.FloorPlanStoreId(dialog.spatialStoreId());
                            }
                        });

                });

            };
        };
    return {
        addFloor: addFloor,
        editFloor: editFloor,
        editFloorMap: editFloorMap,
        removeFloor: removeFloor
    };
};