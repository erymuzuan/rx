/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../durandal/app.js" />


define(['services/datacontext', 'durandal/app'],
    function (context,app) {

        var isBusy = ko.observable(false),
            setting = new bespoke.sphcommercialspace.domain.Setting(),
            activate = function () {
                
            },
            addInterest = function () {
                var tcs = new $.Deferred();
                context.getTuplesAsync("Building", "BuildingId gt 0", "BuildingId", "Name").done(function (list) {
                    vm.buildingOptions(list);
                    tcs.resolve(true);
                });
                var interest = new bespoke.sphcommercialspace.domain.Interest();
                vm.interestCollection.push(interest);
                return tcs.promise();
            },
            save = function() {
                var tcs = new $.Deferred();
                vm.setting().Key('Interest.Collection');
                vm.setting().Value(ko.mapping.toJSON(vm.interestCollection()));
                var data = JSON.stringify({settings: [ko.mapping.toJS(vm.setting())]});
                isBusy(true);

                context.post(data, "/Setting/Save")
                    .then(function(result) {
                        isBusy(false);
                        app.showMessage('setting saved');
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            setting: ko.observable(setting),
            buildingOptions : ko.observableArray(),
            floorOptions : ko.observableArray(),
            cspaceOptions : ko.observableArray(),
            selectedBuildingId : ko.observable(),
            selectedFloor : ko.observable(),
            interestCollection: ko.observableArray(),
            addInterestCommand: addInterest,
            saveCommand: save
        };

        vm.selectedBuildingId.subscribe(function (id) {
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

        vm.selectedFloor.subscribe(function (floorname) {
            vm.isBusy(true);
            var tcs = new $.Deferred();
            context.getTuplesAsync("CommercialSpace", "BuildingId eq " + vm.selectedBuildingId() + "and FloorName eq '" + floorname + "'", "CommercialSpaceId", "Category")
                .then(function (b) {
                    vm.cspaceOptions(b);
                    vm.isBusy(false);
                    tcs.resolve(true);
                });
            return tcs.promise();
        });
        return vm;

    });
