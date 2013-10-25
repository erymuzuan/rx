/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />


var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};

bespoke.sph.domain.RulePartial = function () {

    var system = require('durandal/system'),
        showFieldDialog = function (accessor, field, path) {
            require(['viewmodels/' + path, 'durandal/app'], function (dialog, app2) {
                dialog.field(field);


                app2.showModal(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        },
        /* fields */
        startAddDocumentField = function (accessor) {
            var documentField = new bespoke.sph.domain.DocumentField(system.guid());
            showFieldDialog(accessor, documentField, 'field.document');
        },
        startAddConstantField = function (accessor) {
            var constantField = new bespoke.sph.domain.ConstantField(system.guid());
            showFieldDialog(accessor, constantField, 'field.constant');

        },
        startAddFunctionField = function (accessor) {

            var functionField = new bespoke.sph.domain.FunctionField(system.guid());
            showFieldDialog(accessor, functionField, 'field.function');

        },
        startAddChangedField = function (accessor) {

            var changedField = new bespoke.sph.domain.FieldChangeField(system.guid());
            showFieldDialog(accessor, changedField, 'field.changed');

        },
        startEditField = function (field, accessor) {
            return function () {
                var fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field));
                
                switch (fieldType) {
                    case "Bespoke.Sph.Domain.ConstantField, domain.sph":
                        showFieldDialog(accessor, clone, 'field.constant');
                        break;
                    case "Bespoke.Sph.Domain.DocumentField, domain.sph":
                        showFieldDialog(accessor, clone, 'field.document');
                        break;
                    case "Bespoke.Sph.Domain.FunctionField, domain.sph":
                        showFieldDialog(accessor, clone, 'field.function');
                        break;
                    case "Bespoke.Sph.Domain.FieldChangeField, domain.sph":
                        showFieldDialog(accessor, clone, 'field.changed');
                        break;
                    default:
                        throw "unrecognized type : " + fieldType;
                }



            };
        };

    var vm = {

        /*** FIELD */
        startAddDocumentField: startAddDocumentField,
        startAddFunctionField: startAddFunctionField,
        startAddConstantField: startAddConstantField,
        startAddChangedField: startAddChangedField,
        startEditField: startEditField

    };

    return vm;
};
