﻿/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />

bespoke.sph.domain.RouteParameterFieldPartial = function (model) {
    // TODO : In QueryEndpoint filter, if term were defined, then maybe we could suggest the name and type
    model.IsOptional.subscribe(function(opt) {
        if (!opt) {
            model.DefaultValue("");
        }
    });
    return {};
};
