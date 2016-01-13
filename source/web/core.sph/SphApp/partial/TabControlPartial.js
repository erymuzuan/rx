/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.2.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.TabControlPartial = function (tab) {
    var system = require("durandal/system"),
        addItem = function () {
            var item = new bespoke.sph.domain.TabPanel(system.guid());
            tab.TabPanelCollection.push(item);

        },
        removeItem = function (item) {
            return function () {
                tab.TabPanelCollection.remove(item);
            };
        };
    if (tab.TabPanelCollection().length === 0) {
        tab.TabPanelCollection.push(new bespoke.sph.domain.TabPanel({ WebId: system.guid() , Header : "Tab 1"}));
        tab.TabPanelCollection.push(new bespoke.sph.domain.TabPanel({ WebId: system.guid() , Header : "Tab 2"}));
    }
    return {
        addItem: addItem,
        removeItem: removeItem
    };
};