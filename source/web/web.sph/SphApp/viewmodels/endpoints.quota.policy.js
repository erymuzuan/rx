/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {

        var isBusy = ko.observable(false),
            edit = function (policy) {
                
                                
                var tcs = new $.Deferred(),
                    clone = context.clone(policy);
                require(["viewmodels/endpoint.quota.policy.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.policy(clone);
                        app2.showDialog(dialog)
                            .done(function (result) {
                                
                                if (!result) {
                                    tcs.resolve(false);
                                    return;
                                }
                                tcs.resolve(result === "OK");
                                if(result === "OK"){
                                    policy.Name(ko.unwrap(clone.Name));
                                    
                                    policy.RateLimit.Calls(ko.unwrap(clone.RateLimit.Calls));
                                    policy.RateLimit.RenewalPeriod(ko.unwrap(clone.RateLimit.RenewalPeriod));
                                    
                                    policy.Quota.Calls(ko.unwrap(clone.Quota.Calls));
                                    policy.Quota.RenewalPeriod(ko.unwrap(clone.Quota.RenewalPeriod));
                                    
                                    policy.Bandwidth.Calls(ko.unwrap(clone.Bandwidth.Calls));
                                    policy.Bandwidth.RenewalPeriod(ko.unwrap(clone.Bandwidth.RenewalPeriod));
                                    
                                } 
                            });
                    });
                    
                    return tcs.promise();
            },
            selectApi = function (policy) {
                require(["viewmodels/endpoints.quota.api.selections.dialog", "durandal/app"],
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
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(10),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    }
                },
                {
                    Name: ko.observable("Bronze"),
                    RateLimit: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(100),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(2000),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    }
                },
                {
                    Name: ko.observable("Silver"),
                    RateLimit: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(500),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(10000),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    }
                },
                {
                    Name: ko.observable("Gold"),
                    RateLimit: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(500),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        Calls: ko.observable(10000),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    }
                },
                {
                    Name: ko.observable("Platinum"),
                    RateLimit: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(5000),
                        RenewalPeriod: ko.observable(600)
                    },
                    Quota: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(100000),
                        RenewalPeriod: ko.observable(604800)
                    },
                    Bandwidth: {
                        IsUnlimited : ko.observable(false),
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    }
                },
                {
                    Name: ko.observable("Unlimited"),
                    RateLimit: {
                        IsUnlimited : ko.observable(true),
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    },
                    Quota: {
                        IsUnlimited : ko.observable(true),
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    },
                    Bandwidth: {
                        IsUnlimited : ko.observable(true),
                        Calls: ko.observable(200),
                        RenewalPeriod: ko.observable(604800)
                    }
                }
            ]),
            selected = ko.observable(false),
            activate = function () {
                _(policies()).each(function(v) {
                    v.selectApi = selectApi;
                    v.edit = edit;
                });
            },
            attached = function (view) {

            },
            addPolicy = function () {
                var pc =   {
                    Name: ko.observable(),
                    RateLimit: {
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    },
                    Quota: {
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    },
                    Bandwidth: {
                        Calls: ko.observable(),
                        RenewalPeriod: ko.observable()
                    },
                    edit: edit,
                    selectApi : selectApi
                };
                edit(pc).done(function(result){
                   if(result){
                       policies.push(pc);
                   } 
                });
            },
            removePolicy = function (policy) {
                app.showMessage("Are you sure you want to completely remove", "Rx Developer", ["Yes", "No"])
                    .done(function(dialogResult) {
                        if (dialogResult === "Yes") {
                            policies.remove(policy);
                        }
                    });
            };

        return {
            policies: policies,
            addPolicy: addPolicy,
            selected: selected,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            removePolicy : removePolicy
        };
    });
