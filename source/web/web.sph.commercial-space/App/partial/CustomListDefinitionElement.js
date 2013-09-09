/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.CustomListDefinitionElementPartial = function () {

    return {
        addCustomField: function() {
            this.CustomFieldCollection.push(new bespoke.sphcommercialspace.domain.CustomField());
        },
        removeCustomField: function(f) {
            this.CustomFieldCollection.remove(f);
        }
    };
};