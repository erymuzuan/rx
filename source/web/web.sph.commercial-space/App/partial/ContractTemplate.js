/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.ContractTemplatePartial = function () {
    var system = require('durandal/system'),
        addCustomField = function() {
            this.CustomFieldCollection.push(new bespoke.sphcommercialspace.domain.CustomField(system.guid()))
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