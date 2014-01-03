/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ContractTemplatePartial = function () {
    var system = require('durandal/system'),
        addCustomField = function() {
            this.CustomFieldCollection.push(new bespoke.sph.domain.CustomField(system.guid()))
        },
        removeCustomField = function (field) {
            var self = this;
            return function() {
                self.CustomFieldCollection.remove(field);
            };

        };
    return {
        CustomFieldCollection: ko.observableArray(),
        addCustomField: addCustomField,
        removeCustomField : removeCustomField
    };
};