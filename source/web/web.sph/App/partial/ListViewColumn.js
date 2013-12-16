/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ListViewColumnPartial = function (model) {

    var icon = ko.observable('/images/form.element.textbox.png');
    model.Input.subscribe(function (c) {
        if (!c.$type) {
            return;
        }

        var pattern = /Bespoke\.Sph\.Domain\.(.*?),/,
            name = pattern.exec(ko.unwrap(c.$type))[1];
        icon('/images/form.element.' + name +'.png');

    });
    return {
        icon: icon
    };
};