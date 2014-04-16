/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.SearchDefinitionPartial = function () {

    var system = require('durandal/system'),
        removeFilter = function (child) {
            var self = this;
            return function() {
                self.FilterCollection.remove(child);
            };
        },
        addFilter = function() {
            var child = new bespoke.sph.domain.Filter(system.guid());
            this.FilterCollection.push(child);
        };

    var vm = {
        addFilter: addFilter,
        removeFilter: removeFilter

    };

    return vm;
};
