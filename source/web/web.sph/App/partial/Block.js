/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="/Scripts/knockout-2.3.0.debug.js" />
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
                    app.showModal(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BlockCollection.replace(block, clone);
                            }
                        });
                });
            };
        };
    return {
        addFloor: addFloor,
        editFloor: editFloor
    };
};