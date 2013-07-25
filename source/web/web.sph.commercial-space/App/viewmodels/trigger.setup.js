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
            id = ko.observable(),
            editedField,
            editedEmail,
            activate = function(routeData) {
                id(parseInt(routeData.id));

                var query = String.format("TriggerId eq {0} ", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("Trigger", query)
                    .done(function(t) {
                        if (t) {
                            vm.trigger(t);
                        } else {
                            vm.trigger(new bespoke.sphcommercialspace.domain.Trigger());
                        }
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            viewAttached = function() {
                $('#rules-table').on('click', 'a.dropdown-toggle', function() {
                    $(this).parent().toggleClass("open");
                });

                $('#action-panel').on('click', 'a.dropdown-toggle', function() {
                    $(this).parent().toggleClass("open");
                });

                $('#action-table').on('click', 'a.dropdown-toggle', function() {
                    $(this).parent().toggleClass("open");
                });

            },
            addRule = function() {
                var rule = new bespoke.sphcommercialspace.domain.Rule();
                rule.Left({ Name: ko.observable("+ Field") });
                rule.Right({ Name: ko.observable("+ Field") });
                vm.trigger().RuleCollection.push(rule);
                $('#rules-table .dropdown-toggle').dropdown();
            },
            /* fields */
            startAddDocumentField = function(accessor) {
                editedField = accessor;

                var documentField = new bespoke.sphcommercialspace.domain.DocumentField();
                vm.documentField(documentField);

                $('#document-panel-modal').modal({});
            },
            startAddConstantField = function(accessor) {
                editedField = accessor;

                vm.constantField(new bespoke.sphcommercialspace.domain.ConstantField());
                $('#constant-panel-modal').modal({});
            },
            startAddFunctionField = function(accessor) {
                editedField = accessor;

                vm.functionField(new bespoke.sphcommercialspace.domain.FunctionField());
                $('#function-panel-modal').modal({});
            },
            saveField = function(field) {
                editedField(field);
            },
            addSetterActionChild = function() {
                var child = new bespoke.sphcommercialspace.domain.SetterActionChild();
                child.Field({ Name: ko.observable("") });
                vm.setterAction().SetterActionChildCollection.push(child);
                $('#action-table .dropdown-toggle').dropdown();
            },
           
            addSetterAction = function() {
                var setterAction = new bespoke.sphcommercialspace.domain.SetterAction();
                vm.setterAction(setterAction);
                vm.trigger().ActionCollection.push(setterAction);
            },            
            /* email */
            addEmailAction = function () {
                var emailAction = new bespoke.sphcommercialspace.domain.EmailAction();
                vm.trigger().ActionCollection.push(emailAction);
            },
            startEditEmailAction = function (email) {
                editedEmail = email;
                var clone = ko.mapping.fromJS(ko.mapping.toJS(email));
                vm.emailAction(clone);

                $('#email-action-modal').modal({});
            },
            saveEmail = function () {
                var clone = ko.mapping.fromJS(ko.mapping.toJS(vm.emailAction));
                vm.trigger().ActionCollection.replace(editedEmail, clone);
            },

            updateSetterAction = function () {

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
            activate: activate,
            viewAttached: viewAttached,

            functionField: ko.observable(new bespoke.sphcommercialspace.domain.FunctionField()),
            constantField: ko.observable(new bespoke.sphcommercialspace.domain.ConstantField()),
            documentField: ko.observable(new bespoke.sphcommercialspace.domain.DocumentField()),
            setterAction: ko.observable(new bespoke.sphcommercialspace.domain.SetterAction()),
            trigger: ko.observable(new bespoke.sphcommercialspace.domain.Trigger()),

            addRuleCommand: addRule,
            addEmailActionCommand: addEmailAction,
            addSetterActionCommand: addSetterAction,
            addSetterActionChildCommand: addSetterActionChild,


            /*** FIELD */
            startAddDocumentField: startAddDocumentField,
            startAddFunctionField: startAddFunctionField,
            startAddConstantField: startAddConstantField,
            saveField: saveField,

            /* email */
            startEditEmailAction: startEditEmailAction,
            saveEmail: saveEmail,
            emailAction : ko.observable(new bespoke.sphcommercialspace.domain.EmailAction()),
            updateSetterActionCommand: updateSetterAction,


            toolbar: {
                saveCommand: save,
                reloadCommand: function () { return activate({ id: id() }); }
            }

        };

        return vm;

    });
