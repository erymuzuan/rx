/// <reference path="../../Scripts/jquery-2.0.2.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

define(['services/datacontext', 'services/jsonimportexport'],
    function (context, eximp) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            editedField,
            editedEmail,
            activate = function (routeData) {
                id(parseInt(routeData.id));

                var query = String.format("TriggerId eq {0} ", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("Trigger", query)
                    .done(function (t) {
                        if (t) {
                            vm.trigger(t);
                        } else {
                            vm.trigger(new bespoke.sph.domain.Trigger());
                        }
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            viewAttached = function () {


                $('#setter-action-modal').on('click', 'a.btn,button.close', function (e) {
                    e.preventDefault(true);
                    if ($(this).data("dismiss") === "modal") {
                        $('#setter-action-modal').hide();
                    }
                });

            },
            addRule = function () {
                var rule = new bespoke.sph.domain.Rule();
                rule.Left({ Name: ko.observable("+ Field") });
                rule.Right({ Name: ko.observable("+ Field") });
                vm.trigger().RuleCollection.push(rule);
                $('#rules-table .dropdown-toggle').dropdown();
            },

        removeRule = function (rule) {
            vm.trigger().RuleCollection.remove(rule);
        },
            /* fields */
            startAddDocumentField = function (accessor) {
                editedField = accessor;

                var documentField = new bespoke.sph.domain.DocumentField();
                vm.documentField(documentField);

                $('#document-panel-modal').modal({});
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
            },

            saveField = function (field) {
                editedField(field);
            },

            /* email */
            addEmailAction = function () {
                var emailAction = new bespoke.sph.domain.EmailAction();
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
            /* setter action */
            editedSetter,
            addSetterAction = function () {
                var setterAction = new bespoke.sph.domain.SetterAction();
                vm.trigger().ActionCollection.push(setterAction);
            },

             addSetterActionChild = function () {
                 var child = new bespoke.sph.domain.SetterActionChild();
                 child.Field({ Name: ko.observable("+ Field") });
                 vm.setterAction().SetterActionChildCollection.push(child);

             },

            startEditSetterAction = function (setter) {
                editedSetter = setter;
                var clone = ko.mapping.fromJS(ko.mapping.toJS(setter));
                vm.setterAction(clone);

                $('#setter-action-modal').show();
            },

            saveSetter = function () {

                var clone = ko.mapping.fromJS(ko.mapping.toJS(vm.setterAction));
                vm.trigger().ActionCollection.replace(editedSetter, clone);
            },

        removeAction = function (action) {
            vm.trigger().ActionCollection.remove(action);
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
            },

            exportJson = function () {
                return eximp.exportJson("trigger." + vm.trigger().TriggerId() + ".json", ko.mapping.toJSON(vm.trigger));

            },

         importJson = function () {
             return eximp.importJson()
                 .done(function (json) {
                     vm.trigger(ko.mapping.fromJSON(json));
                     vm.trigger().TriggerId(0);

                 });
         };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,

            functionField: ko.observable(new bespoke.sph.domain.FunctionField()),
            constantField: ko.observable(new bespoke.sph.domain.ConstantField()),
            documentField: ko.observable(new bespoke.sph.domain.DocumentField()),
            changedField: ko.observable(new bespoke.sph.domain.FieldChangeField()),
            trigger: ko.observable(new bespoke.sph.domain.Trigger()),

            addRuleCommand: addRule,
            removeRule: removeRule,


            /*** FIELD */
            startAddDocumentField: startAddDocumentField,
            startAddFunctionField: startAddFunctionField,
            startAddConstantField: startAddConstantField,
            startAddChangedField: startAddChangedField,
            saveField: saveField,
            startEditField: startEditField,

            /* email action*/
            addEmailActionCommand: addEmailAction,
            startEditEmailAction: startEditEmailAction,
            saveEmail: saveEmail,
            emailAction: ko.observable(new bespoke.sph.domain.EmailAction()),

            /* setter action */
            setterAction: ko.observable(new bespoke.sph.domain.SetterAction()),
            addSetterActionCommand: addSetterAction,
            addSetterActionChild: addSetterActionChild,
            startEditSetterAction: startEditSetterAction,
            saveSetter: saveSetter,
            removeAction: removeAction,



            toolbar: {
                saveCommand: save,
                reloadCommand: function () { return activate({ id: id() }); },
                exportCommand: exportJson,
                commands: ko.observableArray([
                    {
                        icon: 'icon-upload',
                        caption: 'import',
                        command: importJson
                    }
                ])
            }

        };

        return vm;

    });
