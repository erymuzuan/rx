/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.2.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.TabControlPartial = function () {
    var system = require("durandal/system"),
        addItem = function () {
            var self = this;
            var item = new bespoke.sph.domain.TabPanel(system.guid());
            self.TabPanelCollection.push(item);

        },
        removeItem = function (item) {
            var self = this;
            return function () {
                self.TabPanelCollection.remove(item);
            };
        };
    return {
        addItem: addItem,
        removeItem: removeItem
    };
};