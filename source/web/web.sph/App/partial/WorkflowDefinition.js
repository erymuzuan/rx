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

bespoke.sph.domain.WorkflowDefinitionPartial = function () {

    var system = require('durandal/system'),
        removeActivity = function(activity) {
            var self = this;
            return function() {
                self.ActivityCollection.remove(activity);
            };
        },
        addActivity = function(type) {
            var self = this;
            return function() {
                var activity = new bespoke.sph.domain[type + 'Activity'](system.guid());

                require(['viewmodels/activity.' + type.toLowerCase(), 'durandal/app'], function(dialog, app2) {
                    dialog.activity(activity);
                    app2.showModal(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.ActivityCollection.push(activity);
                            }
                        });

                });

            };
        },
        editActivity = function(activity) {
            var self = this;
            return function() {
                var activityType = ko.unwrap(activity.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(activity)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Activity,/,
                    type = pattern.exec(activityType)[1];

                require(['viewmodels/activity.' + type.toLowerCase(), 'durandal/app'], function(dialog, app2) {
                    dialog.activity(clone);

                    app2.showModal(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.ActivityCollection.replace(activity, clone);
                            }
                        });

                });

            };
        };

    var vm = {
        removeActivity: removeActivity,
        addActivity: addActivity,
        editActivity: editActivity

    };

    return vm;
};
