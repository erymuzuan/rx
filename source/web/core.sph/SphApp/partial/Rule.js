﻿/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.RulePartial = function (model) {

    const system = require("durandal/system"),
        showFieldDialog = function (accessor, field, path) {
            require([`viewmodels/${path}`, "durandal/app"], function (dialog, app2) {
                dialog.field(field);


                app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        },
        addField = function (accessor, type) {
            const field = new bespoke.sph.domain[type + "Field"](system.guid());
            showFieldDialog(accessor, field, "field." + type.toLowerCase());
        },
        editField = function (field, accessor) {
            return function () {
                const fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                    type = pattern.exec(fieldType)[1];


                showFieldDialog(accessor, clone, "field." + type.toLowerCase());

            };
        },
        binaryOperator = ko.computed(function() {
            if (ko.unwrap(model.Operator) === "IsNull") {
                return false;
            }
            if (ko.unwrap(model.Operator) === "IsNotNull") {
                return false;
            }

            return true;
        });

    const vm = {
        binaryOperator: binaryOperator,
        addField: addField,
        editField: editField

    };

    return vm;
};
