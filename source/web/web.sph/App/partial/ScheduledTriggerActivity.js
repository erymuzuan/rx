/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ScheduledTriggerActivityPartial = function () {

    // IntervalSchedule
    var system = require('durandal/system'),
        addIntervalSchedule = function (type) {
            var schedule = new bespoke.sph.domain[type + 'Schedule'](system.guid());
            var self = this;

            return function () {

                require(['viewmodels/schedule.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
                    dialog.schedule(schedule);

                    app2.showModal(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                if (typeof self.IntervalSchedule === "function") {
                                    self.IntervalSchedule(schedule);
                                } else {
                                    self.IntervalSchedule = ko.observable(schedule);
                                }
                            }
                        });

                });
            };
        },
        removeIntervalSchedule = function (obj) {
            var self = this;
            return function () {
                self.IntervalScheduleCollection.remove(obj);
            };

        };
    return {
        addSchedule: addIntervalSchedule,
        removeIntervalSchedule: removeIntervalSchedule
    };
};