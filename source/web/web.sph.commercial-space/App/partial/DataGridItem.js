/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};

bespoke.sphcommercialspace.domain.DataGridItemPartial = function () {

    var system = require('durandal/system'),
        addColumn = function () {
            var self = this;
            self.DataGridColumnCollection.push(new bespoke.sphcommercialspace.domain.DataGridColumn(system.guid()));
        },
        removeColumn = function (column) {
            var self = this;
            return function () {
                self.DataGridColumnCollection.remove(column);
            };
        };
    return {
        addColumn: addColumn,
        removeColumn: removeColumn
    };
};
