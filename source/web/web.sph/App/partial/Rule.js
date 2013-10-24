/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />


var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};

bespoke.sph.domain.RulePartial = function (model) {

    var system = require('durandal/system'),
        context = require(objectbuilders.datacontext),
        logger = require(objectbuilders.logger),
        /* fields */
        startAddDocumentField = function (accessor) {
            require(['viewmodels/field.document', 'durandal/app'], function (dialog, app2) {
                var documentField = new bespoke.sph.domain.DocumentField(system.guid());
                dialog.field(documentField);
               

                app2.showModal(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor( documentField);
                    }
                });

            });
            
        },
        startAddConstantField = function (accessor) {
            editedField = accessor;

            vm.constantField(new bespoke.sph.domain.ConstantField());
            $('#constant-panel-modal').modal({});
        },
        startAddFunctionField = function (accessor) {
            editedField = accessor;

            vm.functionField(new bespoke.sph.domain.FunctionField());
            $('#function-panel-modal').modal({});
        },
        startAddChangedField = function (accessor) {
            editedField = accessor;

            vm.changedField(new bespoke.sph.domain.FieldChangeField());
            $('#changed-panel-modal').modal({});
        },
        startEditField = function (accessor) {
            var self = this;
            return function () {
                if (typeof accessor === "function") {
                    var tf = accessor().$type;
                    var fieldType = typeof tf === "function" ? tf() : tf;


                    editedField = accessor;
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(accessor()));

                    switch (fieldType) {
                        case "Bespoke.Sph.Domain.ConstantField, domain.sph":
                            vm.constantField(clone);
                            $('#constant-panel-modal').modal({});
                            break;
                        case "Bespoke.Sph.Domain.DocumentField, domain.sph":
                            vm.documentField(clone);
                            $('#document-panel-modal').modal({});
                            break;
                        case "Bespoke.Sph.Domain.FunctionField, domain.sph":
                            vm.functionField(clone);
                            $('#function-panel-modal').modal({});
                            break;
                        case "Bespoke.Sph.Domain.FieldChangeField, domain.sph":
                            vm.changedField(clone);
                            $('#changed-panel-modal').modal({});
                            break;
                        default:
                            throw "unrecognized type : " + fieldType;
                    }
                }

                if (typeof accessor === "object") {
                    console.log("this is hard", accessor);
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
