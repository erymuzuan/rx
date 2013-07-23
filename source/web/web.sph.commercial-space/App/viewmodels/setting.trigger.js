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
            activate = function () {
                return true;
            },
           addRule = function () {
                var rule = new bespoke.sphcommercialspace.domain.Rule();
                vm.trigger().RuleCollection.push(rule);
           },
            editedRule = ko.observable(),
            editedFunctionField = ko.observable(),
            addFunctionField = function (rule) {
                isBusy(true);
                var functionField = new bespoke.sphcommercialspace.domain.FunctionField();
                var r1 = rule;
                var clone = r1;
                editedRule(r1);
                
                vm.rule(clone);
                vm.functionField(functionField);
               
                
                $('#function-panel-modal').modal({});
            },
            addDocumentField = function (rule) {
                isBusy(true);
                var documentField = new bespoke.sphcommercialspace.domain.DocumentField();
                var r1 = rule;
                var clone = r1;
                editedRule(r1);
                
                vm.rule(clone);
                vm.documentField(documentField);
                $('#document-panel-modal').modal({});
            }, addConstantField = function (rule) {
                isBusy(true);
                var constantField = new bespoke.sphcommercialspace.domain.ConstantField();
                var r1 = rule;
                var clone = r1;
                editedRule(r1);
                
                vm.rule(clone);
                vm.constantField(constantField);
                $('#constant-panel-modal').modal({});
            },
            addToRule = function () {
                vm.rule().Left(vm.functionField());
                vm.trigger().RuleCollection.replace(editedRule, vm.rule());
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ comp: vm.trigger() });
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
            activate: activate,
            functionField : ko.observable(new bespoke.sphcommercialspace.domain.FunctionField()),
            constantField : ko.observable(new bespoke.sphcommercialspace.domain.ConstantField()),
            documentField: ko.observable(new bespoke.sphcommercialspace.domain.DocumentField()),
            trigger: ko.observable(new bespoke.sphcommercialspace.domain.Trigger()),
            rule : ko.observable(new bespoke.sphcommercialspace.domain.Rule()),
            addRuleCommand: addRule,
            addFunctionFieldCommand: addFunctionField,
            addToRuleCommand: addToRule,
            addDocumentFieldCommand: addDocumentField,
            addConstantFieldCommand: addConstantField,
            saveCommand: save
        };

        return vm;

    });
