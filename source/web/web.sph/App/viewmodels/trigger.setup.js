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
            trigger: ko.observable(new bespoke.sph.domain.Trigger()),

            addRuleCommand: addRule,
            removeRule: removeRule,


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
