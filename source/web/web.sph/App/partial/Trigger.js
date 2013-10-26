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

bespoke.sph.domain.TriggerPartial = function () {

    var system = require('durandal/system'),
        addAction = function (type) {
            var self = this;
            return function () {
                var action = new bespoke.sph.domain[type + 'Action'](system.guid());
                self.ActionCollection.push(action);
            };
        },
        editAction = function (action) {
            var self = this;
            return function () {
                var actionType = ko.unwrap(action.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(action)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Action,/,
                    type = pattern.exec(actionType)[1];

                require(['viewmodels/action.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
                    dialog.action(clone);
                    
                    app2.showModal(dialog)
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
            return function() {
                self.RuleCollection.remove(rule);
            };
        };

    var vm = {
        addRule: addRule,
        removeRule: removeRule,
        addAction: addAction,
        editAction: editAction

    };

    return vm;
};
