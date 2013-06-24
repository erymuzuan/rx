/// <reference path="../../Scripts/jquery-2.0.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../services/datacontext.js" />


define(['services/datacontext'],
    function (context) {

        var buildingOptions = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {
                var query = String.format("RebateId gt 0");
                var tcs = new $.Deferred();
                var rebateTask = context.loadAsync("Rebate", query);
                var buildingTask = context.getTuplesAsync("Building", "BuildingId gt 0", "BuildingId", "Name");
                
                $.when(rebateTask,buildingTask).done(function (lo,list) {
                    vm.rebateCollection(lo.itemCollection);
                    vm.buildingOptions(list);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },

          save = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({ rebates: ko.mapping.toJS(vm.rebateCollection()) });
                isBusy(true);
                context.post(data, "/Rebate/Save")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            rebate: ko.observable(new bespoke.sphcommercialspace.domain.Rebate()),
            rebateCollection: ko.observableArray(),
            buildingOptions: buildingOptions,
            cspaceOptions: ko.observableArray(),
            floorOptions: ko.observableArray(),
            saveCommand: save
        };
    });
