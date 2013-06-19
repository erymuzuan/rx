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

        vm.rebate().Building.subscribe(function (id) {
            vm.isBusy(true);
            var tcs = new $.Deferred();
            context.loadOneAsync("Building", "BuildingId eq " + id)
                .then(function (b) {
                    var floors = _(b.FloorCollection()).map(function (f) {
                        return f.Name();
                    });
                    vm.floorOptions(floors);
                    vm.isBusy(false);
                    tcs.resolve(true);
                });
            return tcs.promise();
        });

        vm.rebate().Floor.subscribe(function (floorname) {
            vm.isBusy(true);
            var tcs = new $.Deferred();
            context.getTuplesAsync("CommercialSpace", "BuildingId eq " + vm.rebate.Building() + "and FloorName eq '" + floorname + "'", "CommercialSpaceId", "Category")
                .then(function (b) {
                    vm.cspaceOptions(b);
                    vm.isBusy(false);
                    tcs.resolve(true);
                });
            return tcs.promise();
        });

        return vm;

    });
