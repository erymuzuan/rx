/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.FixedLengthTextFormatterPartial = function (model) {

    const system = require("durandal/system"),
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
        },
        isWizardOk = ko.computed(function() {
            return ko.unwrap(model.SampleStoreId) && ko.unwrap(model.Delimiter);
        }),
        addField = function() {
            const child = new bespoke.sph.domain.FixedLengthTextFieldMapping(system.guid());
            this.FieldMappingCollection.push(child);
        },
        removeField = function(f) {
            var self = this;
            return function () {
                self.FieldMappingCollection.remove(f);

            };
            
        },
        move = function (array, from, to) {
            if (to === from) return;

            const target = array[from],
                increment = to < from ? -1 : 1;

            for (let k = from; k !== to; k += increment) {
                array[k] = array[k + increment];
            }
            array[to] = target;
        },
        arrange = function (f, step) {
            const list = model.FieldMappingCollection,
                temps = ko.unwrap(list),
                index = temps.indexOf(f);

            move(temps, index, index + step);
            list(temps);
        },
        moveDown = function (f) {
            arrange(f, 1);
        },
        moveUp = function (f) {
            arrange(f, -1);
        };

    const vm = {
        isWizardOk : isWizardOk,
        addDetailsRow: addDetailsRow,
        removeDetailsRow: removeDetailsRow,
        parentOptions: parentOptions,
        moveUp: moveUp,
        moveDown: moveDown,
        addField: addField,
        removeField : removeField

    };

    return vm;
};
