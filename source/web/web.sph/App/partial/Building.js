/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.BuildingPartial = function () {
    var system = require('durandal/system'),
        getCustomField = function (name) {
        var cs = _(this.CustomFieldValueCollection()).find(function (v) {
            return v.Name() === name;
        });
        if (!cs) {
            throw "Cannot find custom field for " + name + " in Building";
        }
        return cs.Value;
    },
        getCustomList = function (name) {
            var cs = _(this.CustomListValueCollection()).find(function (v) {
                return v.Name() === name;
            });
            if (!cs) {
                throw "Cannot find custom list named " + name + " in Building";
            }
            return cs;
        },
        addCustomListItem = function (name) {

            return function () {
                var list = _(this.CustomListValueCollection()).find(function (v) {
                    return v.Name() === name;
                });
                var row = new bespoke.sph.domain.CustomListRow(system.guid());
                var columns = _(list.CustomFieldCollection()).map(function (f) {
                    var webid = system.guid();
                    var v = new bespoke.sph.domain.CustomFieldValue(webid);
                    v.Name(f.Name());
                    v.Type(f.Type());
                    return v;
                });

                row.CustomFieldValueCollection(columns);
                list.CustomListRowCollection.push(row);

            };
        },
        removeCustomListItem = function (name, row) {
            var self = this,
                list = _(self.CustomListValueCollection()).find(function (v) {
                return v.Name() === name;
            });
            return function() {
                list.CustomListRowCollection.remove(row);
            };
        },
        addBlock = function() {
            this.BlockCollection.push(new bespoke.sph.domain.Block(system.guid()));
        },
        removeBlock = function (block) {
            var self = this;
            return function() {
                self.BlockCollection.remove(block);
            };
        };
    return {
        CustomField: getCustomField,
        CustomList: getCustomList,
        addCustomListItem: addCustomListItem,
        removeCustomListItem: removeCustomListItem,
        addBlock: addBlock,
        removeBlock : removeBlock
    };
};