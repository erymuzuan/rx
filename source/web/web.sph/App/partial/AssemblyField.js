/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />



bespoke.sph.domain.AssemblyFieldPartial = function () {
    var system = require(objectbuilders.system),
        addParameter = function () {
            this.ParameterCollection.push(new bespoke.sph.domain.Parameter(system.guid()));
        },
        removeParameter = function(p) {
            var self = this;
            return function() {
                self.ParameterCollection.remove(p);
            };
        };
    return {
        addParameter: addParameter,
        removeParameter: removeParameter
    };
};