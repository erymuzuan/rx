﻿/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />




bespoke.sph.domain.StartWorkflowActionPartial = function () {

    var system = require("durandal/system"),
        removeMapping = function (map) {
            var self = this;
            return function () {
                self.WorkflowTriggerMapCollection.remove(map);
            };
        },
        addMapping = function () {
            var child = new bespoke.sph.domain.WorkflowTriggerMap(system.guid());
            child.Field({ Name: ko.observable("+ Field") });
            this.WorkflowTriggerMapCollection.push(child);
        },
        editMapping = function (child) {
            this.WorkflowTriggerMapCollection.push(child);
        };

    var vm = {
        editMapping: editMapping,
        addMapping: addMapping,
        removeMapping: removeMapping

    };

    return vm;
};
