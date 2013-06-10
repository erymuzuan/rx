/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" /> 
/// <reference path="../services/datacontext.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            activate = function () {
            },

            removeRebate = function (rebate) {
                vm.setting().RebateCollection.remove(rebate);
            },

	        addRebate = function () {
	            var rebates = {
	                Building: ko.observable(),
	                CommercialSpaceCategory: ko.observable(),
	                Floor: ko.observable(),
	                Amount: ko.observable(),
	                StartDate: ko.observable(),
	                EndDate: ko.observable()
	            };
	            vm.setting().RebateCollection.push(rebates);
	        },
            save = function () {
                var tcs = new $.Deferred();
                var rebate = ko.mapping.toJS(vm.setting().RebateCollection());
                var postdata = JSON.stringify({ id: vm.setting().SettingId(), rebates: rebate });
                context.post(postdata, "/Setting/SaveRebate").done(function (e) {
                    logger.log("New Rebate saved", e, "setting", true);
                    tcs.resolve(true);
                });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            init: function (r) {
                var query = "SettingId gt 0 ";
                var tcs = new $.Deferred();
                context.loadAsync("Setting", query).done(function (lo) {
                    vm.setting(lo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            activate: activate,
            setting: ko.observable(new bespoke.sphcommercialspace.domain.Setting()),
            //buildingOptions: ko.observableArray(),
            //selectedBuilding: ko.observable(),
            //floorOptions: ko.observableArray(),
            addRebateCommand: addRebate,
            removeRebateCommand: removeRebate,
            saveCommand : save
        };

        //vm.selectedBuilding.subscribe(function () {
        //    vm.isBusy(true);
        //    context.loadAsync("Building", "BuildingId gt 0")
        //        .then(function (b) {
        //            var floors = _(b.FloorCollection()).map(function (f) {
        //                return f.Name();
        //            });
        //            vm.floorOptions(floors);
        //            vm.isBusy(false);
        //            selectedBuilding = b;
        //        });
        //});

        return vm;

    });
