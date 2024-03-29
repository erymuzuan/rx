﻿/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="~/Scripts/durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.SetterActionChildPartial = function () {

    var system = require('durandal/system'),
        showFieldDialog = function (accessor, field, path, entity) {
            require(['viewmodels/' + path, 'durandal/app'], function (dialog, app2) {
                dialog.field(field);
                if (typeof dialog.entity === "function") {
                    dialog.entity(entity);
                    console.log("found the entity dialog :" + dialog.entity.name);
                }

                app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        },
        addField = function (accessor, type, entity) {
            var field = new bespoke.sph.domain[type + 'Field'](system.guid());
            showFieldDialog(accessor, field, 'field.' + type.toLowerCase(), entity);
        },
        editField = function (field) {
            var self = this;
            return function () {
                var fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                    type = pattern.exec(fieldType)[1];


                showFieldDialog(self.Field, clone, 'field.' + type.toLowerCase());
            };
        };

    var vm = {
        addField: addField,
        editField: editField

    };

    return vm;
};
