/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            edit = function (policy) {
                require(["viewmodels/endpoint.quota.policy.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.policy(policy);
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {


                                }
                            });
                    });
            },
            policies = ko.observableArray([
                {
                    Name: ko.observable("Free Trial"),
                    RateLimit: {
                        Calls: ko.observable(10),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    },
                    edit: edit
                },
                {
                    Name: ko.observable("Bronze"),
                    RateLimit: {
                        Calls: ko.observable(100),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        Calls: ko.observable(2000),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    },
                    edit: edit
                },
                {
                    Name: ko.observable("Silver"),
                    RateLimit: {
                        Calls: ko.observable(500),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        Calls: ko.observable(10000),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    },
                    edit: edit
                },
                {
                    Name: ko.observable("Gold"),
                    RateLimit: {
                        Calls: ko.observable(500),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        Calls: ko.observable(10000),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    },
                    edit: edit
                },
                {
                    Name: ko.observable("Platinum"),
                    RateLimit: {
                        Calls: ko.observable(5000),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        Calls: ko.observable(100000),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    },
                    edit: edit
                },
                {
                    Name: ko.observable("Unlimited"),
                    RateLimit: {
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    },
                    Quota: {
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    },
                    Bandwidth: {
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    },
                    edit: edit
                }
            ]),
            selected = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {

            };

        return {
            policies: policies,
            selected: selected,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };
    });
