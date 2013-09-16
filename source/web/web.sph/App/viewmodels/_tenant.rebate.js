/// <reference path="../../Scripts/jquery-2.0.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../services/datacontext.js" />


define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            activate = function () {
                var query = String.format("RebateId gt 0"),
                    tcs = new $.Deferred();
                
                    context.loadAsync("Rebate", query).done(function (lo) {
                    vm.rebateCollection(lo.itemCollection);
                    tcs.resolve(true);
                });
                   
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            rebate: ko.observable(new bespoke.sph.domain.Rebate()),
            rebateCollection: ko.observableArray()
        };

        return vm;
    });
