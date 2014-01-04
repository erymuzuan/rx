/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />


var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};

bespoke.sph.domain.StartWorkflowActionPartial = function () {

    var system = require('durandal/system'),
        removeMapping = function (map) {
            var self = this;
            return function() {
                self.WorkflowTriggerMapCollection.remove(map);
            };
        },
        addMapping = function() {
            var child = new bespoke.sph.domain.WorkflowTriggerMap(system.guid());
            child.Field({ Name: ko.observable("+ Field") });
            this.WorkflowTriggerMapCollection.push(child);
        };

    var vm = {
        addMapping: addMapping,
        removeMapping: removeMapping

    };

    return vm;
};
