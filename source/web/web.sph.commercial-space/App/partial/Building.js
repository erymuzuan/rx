/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.BuildingPartial = function () {
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
                var row = new bespoke.sphcommercialspace.domain.CustomListRow(system.guid());
                var columns = _(list.CustomFieldCollection()).map(function (f) {
                    var webid = system.guid();
                    var v = new bespoke.sphcommercialspace.domain.CustomFieldValue(webid);
                    v.Name(f.Name());
                    v.Type(f.Type());
                    return v;
                });

                row.CustomFieldValueCollection(columns);
                list.CustomListRowCollection.push(row);

            };
        },
        addBlock = function() {
            this.BlockCollection.push(new bespoke.sphcommercialspace.domain.Block(system.guid()));
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
        addBlock: addBlock,
        removeBlock : removeBlock
    };
};