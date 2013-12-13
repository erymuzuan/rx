/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />

bespoke.sph.domain.ComboBoxPartial = function () {
    var system = require('durandal/system'),
        addItem = function () {
            var self = this;
            var item = new bespoke.sph.domain.ComboBoxItem(system.guid());
            self.ComboBoxItemCollection.push(item);

        },
        removeItem = function (item) {
            var self = this;
            return function () {
                self.ComboBoxItemCollection.remove(item);
            };
        };
    return {
        addItem: addItem,
        removeItem: removeItem
    };
};