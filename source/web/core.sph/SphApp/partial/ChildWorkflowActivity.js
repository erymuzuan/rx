/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />




bespoke.sph.domain.ChildWorkflowActivityPartial = function () {

    var system = require("durandal/system"),
        removeMapping = function (map) {
            var self = this;
            return function () {
                self.PropertyMappingCollection.remove(map);
            };
        },
        addMapping = function () {
            var map = new bespoke.sph.domain.PropertyMapping(system.guid());
            this.PropertyMappingCollection.push(map);
        };

    var vm = {
        addMapping: addMapping,
        removeMapping: removeMapping

    };

    return vm;
};
