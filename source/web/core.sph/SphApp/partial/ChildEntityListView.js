/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ChildEntityListViewPartial = function (model) {
    // ViewColumn
    var system = require('durandal/system'),
        addViewColumn = function () {
            this.ViewColumnCollection.push(new bespoke.sph.domain.ViewColumn({ WebId: system.guid(),Header :'<New Column>'}));
        },
        removeViewColumn = function (obj) {
            var self = this;
            return function () {
                self.ViewColumnCollection.remove(obj);
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

        },
    editViewColumn = function (vc) {
        return function () {
            var clone = ko.mapping.fromJS(ko.mapping.toJS(vc));

            require(['viewmodels/view.column.dialog', 'durandal/app'], function (dialog, app2) {
                dialog.column(clone);
                dialog.entity(model.Entity());

                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            for (var g in vc) {
                                if (typeof vc[g] === "function" && vc[g].name === "observable") {
                                    vc[g](ko.unwrap(clone[g]));
                                } else {
                                    vc[g] = clone[g];
                                }
                            }
                        }
                    });

            });

        };
    };
    return {
        addSort: addSort,
        removeSort: removeSort,
        addViewColumn: addViewColumn,
        editViewColumn: editViewColumn,
        removeViewColumn: removeViewColumn
    };
};