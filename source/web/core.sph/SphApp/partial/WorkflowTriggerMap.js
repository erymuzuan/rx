/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />


bespoke.sph.domain.WorkflowTriggerMapPartial = function () {
    var self = this,
        system = require('durandal/system'),
        showFieldDialog = function (accessor, field, path) {
            require(['viewmodels/' + path, 'durandal/app'], function (dialog, app2) {
                dialog.field(field);


                app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        };

    self.addField = function(accessor, type) {
        var field = new bespoke.sph.domain[type + 'Field'](system.guid());
            showFieldDialog(accessor, field, 'field.' + type.toLowerCase());
    };

    self.editField = function(field, accessor) {
        return function() {
            var fieldType = ko.unwrap(field.$type),
                clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                type = pattern.exec(fieldType)[1];


            showFieldDialog(accessor, clone, 'field.' + type.toLowerCase());

        };
    };

    return self;
};

bespoke.sph.domain.WorkflowTriggerMapPartial.prototype = new bespoke.sph.domain.FieldContainer();
