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
                    Name: ko.observable("Very short"),
                    Location : ko.observable("Memory"),
                    MaxSize : ko.observable(50000),
                    Duration : ko.observable(300)
                },
                {
                    Name: ko.observable("Short"),
                    Location : ko.observable("Memory"),
                    MaxSize : ko.observable(50000),
                    Duration : ko.observable(300)
                },
                {
                    Name: ko.observable("Medium"),
                    Location : ko.observable("Redis"),
                    MaxSize : ko.observable(50000),
                    Duration : ko.observable(300)
                },
                {
                    Name: ko.observable("Long"),
                    Location : ko.observable("Elasticsearch"),
                    MaxSize : ko.observable(50000),
                    Duration : ko.observable(30000)
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
