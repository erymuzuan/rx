/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            isRight = ko.observable(false),
            activate = function () {
                return true;
            },
           addRule = function () {
               var rule = new bespoke.sphcommercialspace.domain.Rule();
               rule.Left({ Name: ko.observable("") });
               rule.Right({ Name: ko.observable("") });
               vm.trigger().RuleCollection.push(rule);
           },
            editedField,
            addFunctionField = function (field) {
                isBusy(true);
                isRight(false);
                var functionField = new bespoke.sphcommercialspace.domain.FunctionField();
                editedField = field;
                vm.functionField(functionField);

                $('#function-panel-modal').modal({});
            },
            addFunctionFieldToRight = function (field) {
                isBusy(true);
                isRight(true);
                var functionField = new bespoke.sphcommercialspace.domain.FunctionField();
                editedField = field;
                vm.functionField(functionField);

                $('#function-panel-modal').modal({});
            },

            updateFunctionLeftToRule = function () {
                editedField.Left(vm.functionField());
            },
            updateConstantLeftToRule = function () {
                editedField.Left(vm.constantField());
            },
            updateDocumentLeftToRule = function () {
                editedField.Left(vm.documentField());
            },
            updateFunctionRightToRule = function () {
                editedField.Right(vm.functionField());
            },
            updateConstantRightToRule = function () {
                editedField.Right(vm.constantField());
            },
            updateDocumentRightToRule = function () {
                editedField.Right(vm.documentField());
            },
            addDocumentField = function (field) {
                isBusy(true);
                isRight(false);
                var documentField = new bespoke.sphcommercialspace.domain.DocumentField();
                editedField = field;
                vm.documentField(documentField);
                
                $('#document-panel-modal').modal({});
            },
            addDocumentFieldToRight = function (field) {
                isBusy(true);
                isRight(true);
                var documentField = new bespoke.sphcommercialspace.domain.DocumentField();
                editedField = field;
                vm.documentField(documentField);
                
                $('#document-panel-modal').modal({});
            },
            addConstantField = function (field) {
                isBusy(true);
                isRight(false);
                var constantField = new bespoke.sphcommercialspace.domain.ConstantField();
                editedField = field;
                vm.constantField(constantField);
                $('#constant-panel-modal').modal({});
            },
            addConstantFieldToRight = function (field) {
                isBusy(true);
                isRight(true);
                var constantField = new bespoke.sphcommercialspace.domain.ConstantField();
                editedField = field;
                vm.constantField(constantField);
                $('#constant-panel-modal').modal({});
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ trigger: vm.trigger() });
                isBusy(true);

                context.post(data, "/Trigger/Save") 
                    .then(function (result) {
                        isBusy(false);

                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            isRight: isRight,
            activate: activate,
            functionField: ko.observable(new bespoke.sphcommercialspace.domain.FunctionField()),
            constantField: ko.observable(new bespoke.sphcommercialspace.domain.ConstantField()),
            documentField: ko.observable(new bespoke.sphcommercialspace.domain.DocumentField()),
            trigger: ko.observable(new bespoke.sphcommercialspace.domain.Trigger()),
            addRuleCommand: addRule,
            addFunctionFieldCommand: addFunctionField,
            addFunctionFieldToRightCommand: addFunctionFieldToRight,
            updateFunctionLeftToRuleCommand: updateFunctionLeftToRule,
            updateFunctionRightToRuleCommand: updateFunctionRightToRule,
            addDocumentFieldCommand: addDocumentField,
            addDocumentFieldToRightCommand: addDocumentFieldToRight,
            updateDocumentLeftToRuleCommand: updateDocumentLeftToRule,
            updateDocumentRightToRuleCommand: updateDocumentRightToRule,
            addConstantFieldCommand: addConstantField,
            addConstantFieldToRightCommand: addConstantFieldToRight,
            updateConstantLeftToRuleCommand: updateConstantLeftToRule,
            updateConstantRightToRuleCommand: updateConstantRightToRule,
            saveCommand: save
        };

        return vm;

    });
