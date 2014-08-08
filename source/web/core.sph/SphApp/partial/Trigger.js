/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.TriggerPartial = function () {

    var system = require('durandal/system'),
        removeAction = function (action) {
            var self = this;
            return function () {
                self.ActionCollection.remove(action);
            };
        },
        addAction = function (type) {
            var self = this;
            return function () {
                var t = type.toLowerCase().replace(", ", ",");
                require(['viewmodels/action.' + t, 'durandal/app'], function (dialog, app2) {
                    if (typeof dialog.action !== "function") {
                        console.error("The dialog for " + t + " do not implement action as observable");
                        return;
                    }if (typeof dialog.trigger === "function") {
                        dialog.trigger(self);
                    }

                    app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {

                            var action = dialog.action();

                            action.WebId(system.guid());
                            action.IsActive(true);
                            self.ActionCollection.push(action);
                        }
                    });

                });

            };
        },
        editAction = function (action) {
            var self = this;
            return function () {
                var type = ko.unwrap(action.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(action));

                require(['viewmodels/action.' + type.toLowerCase().replace(", ", ","), 'durandal/app'], function (dialog, app2) {
                    dialog.action(clone);

                    app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.ActionCollection.replace(action, clone);
                        }
                    });

                });

            };
        },
         addRule = function () {
             var rule = new bespoke.sph.domain.Rule(system.guid());
             rule.Left({ Name: ko.observable("+ Field") });
             rule.Right({ Name: ko.observable("+ Field") });
             this.RuleCollection.push(rule);
         },
        removeRule = function (rule) {
            var self = this;
            return function () {
                self.RuleCollection.remove(rule);
            };
        };

    var vm = {
        addRule: addRule,
        removeRule: removeRule,
        removeAction: removeAction,
        addAction: addAction,
        editAction: editAction

    };

    return vm;
};
