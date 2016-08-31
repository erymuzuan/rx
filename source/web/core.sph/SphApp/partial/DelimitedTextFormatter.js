/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.DelimitedTextFormatterPartial = function (model) {

    const system = require("durandal/system"),
        selectedRow = ko.observable(new bespoke.sph.domain.FlatFileDetailTag()),
        parentOptions = ko.observableArray(["$record"]),
        childNameChanged = function (name) {
            if (name) {
                if (parentOptions().indexOf(name) < 0) {
                    parentOptions.push(name);
                }
            }
        },
        removeDetailsRow = function (child) {
            var self = this;
            return function () {
                self.DetailRowCollection.remove(child);
                if (child.nameChangedSubscription) {
                    child.nameChangedSubscription.dispose();
                }
            };
        },
        addDetailsRow = function () {
            const child = new bespoke.sph.domain.FlatFileDetailTag(system.guid());
            child.nameChangedSubscription = child.Name.subscribe(childNameChanged);
            this.DetailRowCollection.push(child);
            selectedRow(child);
        },
        selectRow = function(row) {
            selectedRow(row);
        },
        isWizardOk = ko.computed(function() {
            return ko.unwrap(model.SampleStoreId) && ko.unwrap(model.Delimiter);
        });

    const vm = {
        isWizardOk : isWizardOk,
        selectedRow: selectedRow,
        selectRow : selectRow,
        addDetailsRow: addDetailsRow,
        removeDetailsRow: removeDetailsRow,
        parentOptions: parentOptions

    };

    return vm;
};
