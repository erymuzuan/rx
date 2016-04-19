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
                                if(result === "OK"){
                                    policy.Name(ko.unwrap(clone.Name));

                                    policy.RateLimit().Calls(ko.unwrap(clone.RateLimit().Calls));
                                    policy.RateLimit().RenewalPeriod(ko.unwrap(clone.RateLimit().RenewalPeriod));

                                    policy.QuotaLimit().Calls(ko.unwrap(clone.QuotaLimit().Calls));
                                    policy.QuotaLimit().RenewalPeriod(ko.unwrap(clone.QuotaLimit().RenewalPeriod));

                                    policy.BandwidthLimit().Size(ko.unwrap(clone.BandwidthLimit().Size));
                                    policy.BandwidthLimit().RenewalPeriod(ko.unwrap(clone.BandwidthLimit().RenewalPeriod));

                                    context.post(ko.mapping.toJSON(policy), "/api/quota-policies").then(tcs.resolve);

                                }else{
                                  tcs.resolve(false);
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
            policies = ko.observableArray(),
            selected = ko.observable(false),
            activate = function () {

               return context.loadAsync("QuotaPolicy")
               .then(function(lo){
                 policies(lo.itemCollection);
                 _(policies()).each(function(v) {
                     v.selectApi = selectApi;
                     v.edit = edit;
                 });
               });

            },
            attached = function (view) {

            },
            addPolicy = function () {
                var pc =  new bespoke.sph.domain.QuotaPolicy();
                pc.RateLimit().RenewalPeriod = ko.observable(new bespoke.sph.domain.TimePeriod());
                pc.QuotaLimit().RenewalPeriod = ko.observable(new bespoke.sph.domain.TimePeriod());
                pc.BandwidthLimit().RenewalPeriod = ko.observable(new bespoke.sph.domain.TimePeriod());
                pc.selectApi = selectApi;
                pc.edit = edit;

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
