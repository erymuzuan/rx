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

bespoke.sph.domain.BusinessRulePartial = function (model) {

    var system = require('durandal/system'),
        context = require(objectbuilders.datacontext),
        logger = require(objectbuilders.logger),
        addRule = function () {
            var self = this,
                br = new bespoke.sph.domain.Rule(system.guid());

            br.Left({ Name: ko.observable("+ Field") });
            br.Right({ Name: ko.observable("+ Field") });
            
            self.RuleCollection.push(br);

        },
        removeRule = function (br) {
            var self = this;
            return function () {
                self.RuleCollection.remove(br);
            };
        };



    return {
        addRule: addRule,
        removeRule: removeRule
    };
};
