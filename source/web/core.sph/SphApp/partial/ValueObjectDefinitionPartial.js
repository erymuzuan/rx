﻿/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.ValueObjectDefinitionPartial = function () {
    var system = require("durandal/system"),
        app = require("durandal/app"),
        context = require(objectbuilders.datacontext),
        addMember = function () {
            this.MemberCollection.push(new bespoke.sph.domain.Member({
                WebId: system.guid(),
                Boost: 1
            }));
        },
        removeMember = function (floor) {
            var self = this;
            return function () {
                self.MemberCollection.remove(floor);
            };
        },
        editMember = function (member) {
            var self = this;
            return function () {
                require(["viewmodels/member.dialog", "durandal/app"], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                    dialog.member(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.BlockCollection.replace(member, clone);
                            }
                        });
                });
            };
        },
        addBusinessRule = function () {
            var br = new bespoke.sph.domain.BusinessRule({ WebId: system.guid() });
            var self = this;

            require(["viewmodels/business.rule.dialog", "durandal/app"], function (dialog, app) {
                dialog.rule(br);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.BusinessRuleCollection.push(br);
                        }
                    });
            });

        },
        editBusinessRule = function (rule) {
            var self = this;
            return function () {
                require(["viewmodels/business.rule.dialog", "durandal/app"], function (dialog, app) {
                    var clone = context.toObservable(ko.mapping.toJS(rule));
                    dialog.rule(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.BusinessRuleCollection.replace(rule, clone);
                            }
                        });
                });
            };
        },
        removeBusinessRule = function (rule) {
            var self = this;
            return function () {
                self.BusinessRuleCollection.remove(rule);
            };
        },
        editMemberMap = function (member) {
            var building = this;
            return function () {
                console.log("show map ", building);
                console.log(" on member ", member);
                require(["viewmodels/member.map", "durandal/app"], function (dialog, app) {
                    dialog.init(building.BuildingId(), member.MemberPlanStoreId());
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
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
