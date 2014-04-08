///#source 1 1 /SphApp/prototypes/BusinessRuleBase.js
/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/bootstrap.js" />


bespoke.sph.domain.BusinessRuleBase = function () {
    var addBusinessRule = function () {
        var  system = require(objectbuilders.system),
            br = new bespoke.sph.domain.BusinessRule(system.guid());
        br.Name("<YOUR RULE NAME>");
        this.BusinessRuleCollection.push(br);

    },
    removeBusinessRule = function (br) {
        var self = this;
        return function () {
            self.BusinessRuleCollection.remove(br);
        };
    },
        editBusinessRule = function (br) {
            var self = this;
            return function () {
                self.selectedBusinessRule(br);
            };
        };

    var vm = {
        addBusinessRule: addBusinessRule,
        removeBusinessRule: removeBusinessRule,
        editBusinessRule: editBusinessRule
    };

    return vm;

};

///#source 1 1 /SphApp/prototypes/FieldContainer.js
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

bespoke.sph.domain.FieldContainer = function () {

    var showFieldDialog = function (accessor, field, path) {
            require(['viewmodels/' + path, 'durandal/app'], function (dialog, app2) {
                dialog.field(field);

                app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        },
        addField = function (accessor, type) {
            var system = require('durandal/system'),
                field = new bespoke.sph.domain[type + 'Field'](system.guid());
            showFieldDialog(accessor, field, 'field.' + type.toLowerCase());
        },
        editField = function (field) {
            var self = this;
            return function () {
                var fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                    type = pattern.exec(fieldType)[1];


                showFieldDialog(self.Field, clone, 'field.' + type.toLowerCase());
            };
        };

    var vm = {
        addField: addField,
        editField: editField

    };

    return vm;
};

///#source 1 1 /SphApp/prototypes/IntervalScheduleContainer.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.IntervalScheduleContainer = function () {

    // IntervalSchedule
    var system = require('durandal/system'),
        addIntervalSchedule = function (type) {
            var schedule = new bespoke.sph.domain[type + 'Schedule'](system.guid());
            var self = this;

            return function () {
                require(['viewmodels/schedule.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
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

                require(['viewmodels/schedule.' + type.toLowerCase(), 'durandal/app'], function(dialog, app2) {
                    dialog.schedule(clone);

                    app2.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in schedule) {
                                    if (typeof schedule[g] === "function" && schedule[g].name === "observable") {
                                        schedule[g](ko.unwrap(clone[g]));
                                    } else {
                                        schedule[g] = clone[g];
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
