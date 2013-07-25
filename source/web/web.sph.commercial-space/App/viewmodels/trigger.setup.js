/// <reference path="../../Scripts/jquery-2.0.2.intellisense.js" />
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
            isEmail = ko.observable(false),
            id = ko.observable(),
            activate = function (routeData) {
                id(parseInt(routeData.id));
                if (!id()) {
                    vm.trigger(new bespoke.sphcommercialspace.domain.Trigger());
                    return true;
                }
                var query = String.format("TriggerId eq {0} ", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("Trigger", query)
                    .done(function(t) {
                        vm.trigger(t);
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },

            viewAttached = function () {
            // NOTE: there's a bug someweher that makes bootstrap data-toggle didn't work
            $('#rules-table').on('click', 'a.dropdown-toggle', function () {
                $(this).parent().toggleClass("open");
            });
                
            $('#action-panel').on('click', 'a.dropdown-toggle', function () {
                $(this).parent().toggleClass("open");
            });

            $('#action-table').on('click', 'a.dropdown-toggle', function () {
                $(this).parent().toggleClass("open");
            });

        },
           addRule = function () {
               var rule = new bespoke.sphcommercialspace.domain.Rule();
               rule.Left({ Name: ko.observable("") });
               rule.Right({ Name: ko.observable("") });
               vm.trigger().RuleCollection.push(rule);
               $('#rules-table .dropdown-toggle').dropdown();
           },
            addSetterActionChild = function () {
               var child = new bespoke.sphcommercialspace.domain.SetterActionChild();
               child.Field({ Name: ko.observable("") });
               vm.setterAction().SetterActionChildCollection.push(child);
               $('#action-table .dropdown-toggle').dropdown();
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
            updateDocumentFieldToAction = function () {
                editedField.Right(vm.functionField());
            },
            updateConstantFieldToAction = function () {
                editedField.Right(vm.constantField());
            },
            updateFunctionFieldToAction = function () {
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
            addConstantField = function (field) {
                isBusy(true);
                isRight(false);
                var constantField = new bespoke.sphcommercialspace.domain.ConstantField();
                editedField = field;
                vm.constantField(constantField);
                $('#constant-panel-modal').modal({});
            },
             addFunctionFieldToRight = function (field) {
                 isBusy(true);
                 isRight(true);
                 var functionField = new bespoke.sphcommercialspace.domain.FunctionField();
                 editedField = field;
                 vm.functionField(functionField);

                 $('#function-panel-modal').modal({});
             },
            addDocumentFieldToRight = function (field) {
                isBusy(true);
                isRight(true);
                var documentField = new bespoke.sphcommercialspace.domain.DocumentField();
                editedField = field;
                vm.documentField(documentField);

                $('#document-panel-modal').modal({});
            },
            addConstantFieldToRight = function (field) {
                isBusy(true);
                isRight(true);
                var constantField = new bespoke.sphcommercialspace.domain.ConstantField();
                editedField = field;
                vm.constantField(constantField);
                $('#constant-panel-modal').modal({});
            },
            addEmailAction = function () {
                isEmail(true);
                var emailAction = new bespoke.sphcommercialspace.domain.EmailAction();
                vm.emailAction(emailAction);
                vm.trigger().ActionCollection.push(emailAction);
            },
            addSetterAction = function () {
                isEmail(false);
                var setterAction = new bespoke.sphcommercialspace.domain.SetterAction();
                vm.setterAction(setterAction);
                vm.trigger().ActionCollection.push(setterAction);
            },
            updateEmailAction = function () {
                
            },
            updateSetterAction = function () {
                
            },

             addFunctionFieldToAction = function (field) {
                 isBusy(true);
                 isAction(true);
                 var functionField = new bespoke.sphcommercialspace.domain.FunctionField();
                 editedField = field;
                 vm.functionField(functionField);

                 $('#function-panel-modal').modal({});
             },
            addDocumentFieldToAction = function (field) {
                isBusy(true);
                isAction(true);
                var documentField = new bespoke.sphcommercialspace.domain.DocumentField();
                editedField = field;
                vm.documentField(documentField);

                $('#document-panel-modal').modal({});
            },
            addConstantFieldToAction = function (field) {
                isBusy(true);
                isAction(true);
                var constantField = new bespoke.sphcommercialspace.domain.ConstantField();
                editedField = field;
                vm.constantField(constantField);
                $('#setter-action-modal').modal({});
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.trigger);
                isBusy(true);

                context.post(data, "/Trigger/Save")
                    .then(function (result) {
                        isBusy(false);
                        vm.trigger().TriggerId(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            isRight: isRight,
            isEmail: isEmail,
            activate: activate,
            viewAttached: viewAttached,
            functionField: ko.observable(new bespoke.sphcommercialspace.domain.FunctionField()),
            constantField: ko.observable(new bespoke.sphcommercialspace.domain.ConstantField()),
            documentField: ko.observable(new bespoke.sphcommercialspace.domain.DocumentField()),
            emailAction: ko.observable(new bespoke.sphcommercialspace.domain.EmailAction()),
            setterAction: ko.observable(new bespoke.sphcommercialspace.domain.SetterAction()),
            trigger: ko.observable(new bespoke.sphcommercialspace.domain.Trigger()),
            addRuleCommand: addRule,
            addSetterActionChildCommand: addSetterActionChild,
            
            addFunctionFieldCommand: addFunctionField,
            addDocumentFieldCommand: addDocumentField,
            addConstantFieldCommand: addConstantField,
            
            addFunctionFieldToRightCommand: addFunctionFieldToRight,
            addDocumentFieldToRightCommand: addDocumentFieldToRight,
            addConstantFieldToRightCommand: addConstantFieldToRight,
            
            updateFunctionLeftToRuleCommand: updateFunctionLeftToRule,
            updateDocumentLeftToRuleCommand: updateDocumentLeftToRule,
            updateConstantLeftToRuleCommand: updateConstantLeftToRule,
            
            updateDocumentRightToRuleCommand: updateDocumentRightToRule,
            updateFunctionRightToRuleCommand: updateFunctionRightToRule,
            updateConstantRightToRuleCommand: updateConstantRightToRule,

            updateDocumentFieldToActionCommand: updateDocumentFieldToAction,
            updateFunctionFieldToActionCommand: updateFunctionFieldToAction,
            updateConstantFieldToActionCommand: updateConstantFieldToAction,
            
            addEmailActionCommand: addEmailAction,
            addSetterActionCommand: addSetterAction,
            updateEmailActionCommand: updateEmailAction,
            updateSetterActionCommand: updateSetterAction,
            
            addDocumentFieldToActionCommand: addDocumentFieldToAction,
            addConstantFieldToActionCommand: addConstantFieldToAction,
            addFunctionFieldToActionCommand: addFunctionFieldToAction,
            toolbar : {
                 saveCommand: save
            }
           
        };

        return vm;

    });
