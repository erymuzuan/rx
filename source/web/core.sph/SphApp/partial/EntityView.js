/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/SphApp/services/system.js" />
/// <reference path="/SphApp/schemas/form.designer.g.js" />


bespoke.sph.domain.EntityViewPartial = function () {

    // Filter
    var system = require("durandal/system"),
        addConditionalFormatting = function () {
            this.ConditionalFormattingCollection.push(new bespoke.sph.domain.ConditionalFormatting(system.guid()));
        },
        removeConditionalFormatting = function (cf) {
            var self = this;
            return function () {
                self.ConditionalFormattingCollection.remove(cf);
            };

        },
        addRouteParameter = function () {
            this.RouteParameterCollection.push(new bespoke.sph.domain.RouteParameter(system.guid()));
        },
        removeRouteParameter = function (obj) {
            var self = this;
            return function () {
                self.RouteParameterCollection.remove(obj);
            };

        },
        addViewColumn = function () {
            this.ViewColumnCollection.push(new bespoke.sph.domain.ViewColumn(system.guid()));
        },
        removeViewColumn = function (obj) {
            var self = this;
            return function () {
                self.ViewColumnCollection.remove(obj);
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
        addConditionalFormatting: addConditionalFormatting,
        removeConditionalFormatting: removeConditionalFormatting,
        addViewColumn: addViewColumn,
        removeViewColumn: removeViewColumn,
        addSort: addSort,
        removeSort: removeSort,
        addFilter: addFilter,
        removeFilter: removeFilter
    };
};