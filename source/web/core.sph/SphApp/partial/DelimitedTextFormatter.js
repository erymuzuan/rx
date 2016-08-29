/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.DelimitedTextFormatterPartial = function () {

    const system = require("durandal/system"),
        parentOptions = ko.observableArray(["[Root Record]"]),
        childNameChanged = function (name) {
            if (name) {
                parentOptions.push(name);
            }
        },
        removeDetailsRow = function (child) {
            var self = this;
            return function () {
                self.DetailRowCollection.remove(child);
                if (child.nameChangedSubscription) {
                    child.nameChangedSubscription.dispose();
                }
            };
        },
        addDetailsRow = function () {
            const child = new bespoke.sph.domain.FlatFileDetailTag(system.guid());
            child.nameChangedSubscription = child.Name.subscribe(childNameChanged);
            this.DetailRowCollection.push(child);
        };

    const vm = {
        addDetailsRow: addDetailsRow,
        removeDetailsRow: removeDetailsRow,
        parentOptions: parentOptions

    };

    return vm;
};
