/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/bootstrap.js" />


bespoke.sph.domain.BusinessRuleBase = function () {
    var addBusinessRule = function () {
        var  system = require(objectbuilders.system),
            br = new bespoke.sph.domain.BusinessRule(system.guid());
        br.Name("<YOUR RULE NAME>");
        this.BusinessRuleCollection.push(br);

    },
    removeBusinessRule = function (br) {
        var self = this;
        return function () {
            self.BusinessRuleCollection.remove(br);
        };
    },
        editBusinessRule = function (br) {
            var self = this;
            return function () {
                self.selectedBusinessRule(br);
            };
        };

    var vm = {
        addBusinessRule: addBusinessRule,
        removeBusinessRule: removeBusinessRule,
        editBusinessRule: editBusinessRule
    };

    return vm;

};
