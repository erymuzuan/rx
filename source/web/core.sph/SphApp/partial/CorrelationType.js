/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


bespoke.sph.domain.CorrelationTypePartial = function () {

    var system = require('durandal/system'),
        removeCorrelationProperty = function (prop) {
            var self = this;
            return function () {
                self.CorrelationPropertyCollection.remove(prop);
            };
        },
        addCorrelationProperty = function () {
            this.CorrelationPropertyCollection.push(new bespoke.sph.domain.CorrelationProperty(system.guid()));
        };
    return {
        removeCorrelationProperty: removeCorrelationProperty,
        addCorrelationProperty: addCorrelationProperty

    };
};