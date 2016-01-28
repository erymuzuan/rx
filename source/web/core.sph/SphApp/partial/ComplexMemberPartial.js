/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.ComplexMemberPartial = function(model) {
    model.Name.subscribe(function(name) {
        if (!ko.unwrap(model.TypeName)) {
            $.get("/entity-definition/singular/" + name).done(model.TypeName);
        }
    });

}