bespoke.sph.domain.ReceiveActivityPartial = function () {
    //
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