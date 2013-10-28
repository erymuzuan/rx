﻿/// <reference path="../objectbuilders.js" />
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

bespoke.sph.domain.BuildingTemplatePartial = function () {
    var self = this,
        system = require(objectbuilders.system);
    
    return {
        selectedBusinessRule: ko.observable(new bespoke.sph.domain.BusinessRule(system.guid())),
        addBusinessRule: self.addBusinessRule,
        removeBusinessRule: self.removeBusinessRule,
        editBusinessRule: self.editBusinessRule
    };
};

bespoke.sph.domain.BuildingTemplatePartial.prototype = new bespoke.sph.domain.BusinessRuleBase();
