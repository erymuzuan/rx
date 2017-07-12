/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../schemassystem.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />


bespoke.sph.domain.XmlTextFormatterPartial = function (model) {
    const system = require("durandal/system"),
        addParentElement = function () {
            this.ParentElementValueCollection.push(new bespoke.sph.domain.XmlElementTextFieldMapping(system.guid()));
        },
        removeParentElement = function (element) {
            var self = this;
            return function () {
                self.ParentElementValueCollection.remove(element);
            };
        },
        addParentAttribute = function () {
            this.ParentAttributeValueCollection.push(new bespoke.sph.domain.XmlAttributeTextFieldMapping({ WebId: system.guid(), TypeName: "System.String, mscorlib"}));
        },
        removeParentAttribute = function (attr) {
            var self = this;
            return function () {
                self.ParentAttributeValueCollection.remove(attr);
            };
        };

    

    return {
        addParentElement: addParentElement,
        removeParentElement: removeParentElement,
        addParentAttribute: addParentAttribute,
        removeParentAttribute: removeParentAttribute
    };
};