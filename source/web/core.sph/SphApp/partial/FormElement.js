/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />



bespoke.sph.domain.FormElementPartial = function (model) {
    model.Path.subscribe(function(p) {
        if (ko.unwrap(model.Label).indexOf("Label ") > -1) {
            model.Label(p);
        }
    });

    return {
    };
};

