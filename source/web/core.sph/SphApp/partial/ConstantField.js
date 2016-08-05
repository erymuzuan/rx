﻿/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />

bespoke.sph.domain.ConstantFieldPartial = function (model, option) {

    var vm = {
        Type: ko.observable(),
        Value: ko.observable(option.Value)
    };

    vm.Value.subscribe(function(v) {
        if (!ko.unwrap(model.Name)) {
            model.Name(v);
        }
    });
    return vm;
};