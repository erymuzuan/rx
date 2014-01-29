/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.EntityDefinitionPartial = function () {
    var system = require('durandal/system'),
        addMember = function() {
            this.MemberCollection.push(new bespoke.sph.domain.Member(system.guid()));
        },
        editMember = function(member) {
            var self = this;
            return function() {
                require(['viewmodels/member.dialog', 'durandal/app'], function(dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                    dialog.member(clone);
                    app.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BlockCollection.replace(member, clone);
                            }
                        });
                });
            };
        },
        removeMember = function (floor) {
            var self = this;
            return function () {
                self.MemberCollection.remove(floor);
            };
        },
        addBusinessRule = function() {
            this.BusinessRuleCollection.push(new bespoke.sph.domain.BusinessRule(system.guid()));
        },
        editBusinessRule = function(rule) {
            var self = this;
            return function() {
                require(['viewmodels/rule.dialog', 'durandal/app'], function(dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(rule));
                    dialog.rule(clone);
                    app.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BlockCollection.replace(rule, clone);
                            }
                        });
                });
            };
        },
        removeBusinessRule = function (floor) {
            var self = this;
            return function () {
                self.BusinessRuleCollection.remove(floor);
            };
        },
        editMemberMap = function (member) {
            var building = this;
            return function () {
                console.log("show map ", building);
                console.log(" on member ", member);
                require(['viewmodels/member.map', 'durandal/app'], function (dialog, app) {
                    dialog.init(building.BuildingId(), member.MemberPlanStoreId());
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (result == "OK") {
                                member.MemberPlanStoreId(dialog.spatialStoreId());
                            }
                        });

                });

            };
        };
    return {
        addMember: addMember,
        editMember: editMember,
        removeMember: removeMember,
        addBusinessRule: addBusinessRule,
        editBusinessRule: editBusinessRule,
        removeBusinessRule: removeBusinessRule,
        editMemberMap: editMemberMap
    };
};