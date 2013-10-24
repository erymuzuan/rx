/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />


var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};

bespoke.sph.domain.SpaceTemplatePartial = function (model) {

    var system = require('durandal/system'),
        context = require(objectbuilders.datacontext),
        logger = require(objectbuilders.logger),
        editBusinessRule = function (br) {
            var self = this;
            return function () {
                self.selectedBusinessRule(br);
            }
        },
        addBusinessRule = function () {
            var self = this,
                br = new bespoke.sph.domain.BusinessRule(system.guid());


            br.Name("<YOUR RULE NAME>");
            self.BusinessRuleCollection.push(br);

        },
        removeBusinessRule = function (br) {
            var self = this;
            return function () {
                self.BusinessRuleCollection.remove(br);
            };
        };



    return {
        selectedBusinessRule: ko.observable(new bespoke.sph.domain.BusinessRule(system.guid())),
        addBusinessRule: addBusinessRule,
        editBusinessRule: editBusinessRule,
        removeBusinessRule: removeBusinessRule
    };
};

