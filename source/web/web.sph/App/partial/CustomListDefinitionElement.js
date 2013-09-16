/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.CustomListDefinitionElementPartial = function () {

    return {
        addCustomField: function() {
            this.CustomFieldCollection.push(new bespoke.sph.domain.CustomField());
        },
        removeCustomField: function(f) {
            this.CustomFieldCollection.remove(f);
        }
    };
};