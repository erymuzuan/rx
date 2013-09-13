/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />

var bespoke = bespoke || {};
 
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.BlockPartial = function () {
    var system = require('durandal/system'),
        editFloor = function (block) {
            var self = this;
            return function() {
                require(['viewmodels/block.dialog', 'durandal/app'], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(block));
                    dialog.block(clone);
                    app.showModal(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BlockCollection.replace(block, clone);
                            }
                        });
                });
            };
        },
        editBlockMap = function(block) {
            var self = this;
            return function () {
                require(['viewmodels/block.floormap.dialog', 'durandal/app'], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(block));
                    dialog.block(clone);
                    app.showModal(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result == "OK") {
                                
                            }
                        });
                });
            };
        };
    return {
        editFloor: editFloor,
        editBlockMap: editBlockMap
    };
};