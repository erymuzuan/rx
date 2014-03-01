/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.ListViewColumnPartial = function (model) {

    try {

        var pattern1 = /Bespoke\.Sph\.Domain\.(.*?),/,
            input1 = ko.unwrap(model.Input),
            name1 = pattern1.exec(ko.unwrap(input1.$type))[1],
            icon1 = '/images/form.element.' + name1 + '.png';


    } catch (err) {

    }
    icon1 = icon1 || '/images/form.element.textbox.png';
    var icon = ko.observable(icon1);
    model.Input.subscribe(function (c) {
        if (!c.$type) {
            return;
        }

        var pattern = /Bespoke\.Sph\.Domain\.(.*?),/,
            name = pattern.exec(ko.unwrap(c.$type))[1];
        icon('/images/form.element.' + name + '.png');


    });
    return {
        icon: icon
    };
};