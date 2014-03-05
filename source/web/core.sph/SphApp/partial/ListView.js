/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.ListViewPartial = function (model) {

    // ListViewColumn
    var system = require('durandal/system'),
        addListViewColumn = function (type) {
            var self = this;
            return function () {
                var column = new bespoke.sph.domain.ListViewColumn(system.guid()),
                    input = bespoke.sph.domain[type](system.guid());


                column.Path.subscribe(function(v) {
                    input.Path(v);
                });
                column.Label.subscribe(function(v) {
                    input.Label(v);
                });

                column.Input(input);
                input.isSelected = ko.observable(false);
                self.ListViewColumnCollection.push(column);
            };
        },
        removeListViewColumn = function (obj) {
            var self = this;
            return function () {
                self.ListViewColumnCollection.remove(obj);
            };

        };

    _(model.ListViewColumnCollection()).each(function(v) {
        v.Input().isSelected = ko.observable(false);
    });

    return {
        addListViewColumn: addListViewColumn,
        removeListViewColumn: removeListViewColumn
    };
};