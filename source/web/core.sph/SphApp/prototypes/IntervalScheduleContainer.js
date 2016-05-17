/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.IntervalScheduleContainer = function () {

    // IntervalSchedule
    var system = require("durandal/system"),
        addIntervalSchedule = function (type) {
            var schedule = new bespoke.sph.domain[type + "Schedule"](system.guid());
            var self = this;

            return function () {
                require(["viewmodels/schedule." + type.toLowerCase(), "durandal/app"], function (dialog, app2) {
                    dialog.schedule(schedule);

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.IntervalScheduleCollection.push(schedule);
                            }
                        });

                });
            };
        },        
        editSchedule = function(schedule) {
            return function() {
                var scheduleType = ko.unwrap(schedule.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(schedule)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Schedule,/,
                    type = pattern.exec(scheduleType)[1];

                require(["viewmodels/schedule." + type.toLowerCase(), "durandal/app"], function(dialog, app2) {
                    dialog.schedule(clone);

                    app2.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in schedule) {
                                    if (schedule.hasOwnProperty(g)) {
                                        if (ko.isObservable(g)) {
                                            schedule[g](ko.unwrap(clone[g]));
                                        } else {
                                            schedule[g] = clone[g];
                                        }
                                    }
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
        addIntervalSchedule: addIntervalSchedule,
        editSchedule:editSchedule,
        removeIntervalSchedule: removeIntervalSchedule
    };
};