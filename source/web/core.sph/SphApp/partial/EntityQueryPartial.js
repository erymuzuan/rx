/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/SphApp/services/system.js" />
/// <reference path="/SphApp/schemas/form.designer.g.js" />


bespoke.sph.domain.EntityQueryPartial = function () {

    // Filter
    var system = require("durandal/system"),
        addRouteParameter = function () {
            this.RouteParameterCollection.push(new bespoke.sph.domain.RouteParameter(system.guid()));
        },
        removeRouteParameter = function (obj) {
            var self = this;
            return function () {
                self.RouteParameterCollection.remove(obj);
            };

        },
        addFilter = function () {
            this.FilterCollection.push(new bespoke.sph.domain.Filter(system.guid()));
        },
        removeFilter = function (obj) {
            var self = this;
            return function () {
                self.FilterCollection.remove(obj);
            };

        },
        addSort = function () {
            this.SortCollection.push(new bespoke.sph.domain.Sort(system.guid()));
        },
        removeSort = function (obj) {
            var self = this;
            return function () {
                self.SortCollection.remove(obj);
            };

        };
    return {
        addRouteParameter: addRouteParameter,
        removeRouteParameter: removeRouteParameter,
        addSort: addSort,
        removeSort: removeSort,
        addFilter: addFilter,
        removeFilter: removeFilter
    };
};