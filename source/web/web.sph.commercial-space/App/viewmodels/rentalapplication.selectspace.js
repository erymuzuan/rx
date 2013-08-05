/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'durandal/plugins/router'], function (context, router) {

    var buildingOptions = ko.observableArray(),
        activate = function () {
            var tcs = new $.Deferred();
            var stateTask =  context.loadOneAsync("Setting", "Key eq 'State'");
            var categoryTask = context.loadOneAsync("Setting", "Key eq 'Categories'");
            $.when(stateTask,categoryTask).then(function (s,c) {
                    var states = JSON.parse(ko.mapping.toJS(s.Value));
                    var categories = JSON.parse(ko.mapping.toJS(c.Value));
                    vm.stateOptions(states);
                    vm.categoryOptions(categories);
                    tcs.resolve(true);
                });

            return tcs.promise();
        },
        applyCommercialSpace = function (selectedCs) {
            if (selectedCs && selectedCs.CommercialSpaceId()) {
                var url = '/#/rentalapplication/' + selectedCs.CommercialSpaceId();
                router.navigateTo(url);
            }
        },
         search = function () {
             vm.commercialspaces.removeAll();
             var tcs = new $.Deferred();
             var query = "IsAvailable eq 'true'";
             
             context.loadAsync('CommercialSpace', query)
                 .done(function (lo) {
                 vm.commercialspaces(lo.itemCollection);
                 tcs.resolve(true);
             });

             return tcs.promise();

         };

    var vm = {
        activate: activate,
        commercialspaces: ko.observableArray([]),
        applyCommercialSpaceCommand: applyCommercialSpace,
        searchCommand: search,
        selectedBuildingId: ko.observable(),
        buildingOptions: buildingOptions,
        categoryOptions: ko.observableArray([]),
        stateOptions: ko.observableArray([]),
        selectedState: ko.observable(),
        selectedCategory : ko.observable()
    };

    vm.selectedState.subscribe(function (state) {
        var tcs = new $.Deferred();
        context.getTuplesAsync("Building", "State eq " + "'" + state + "'", "BuildingId", "Name")
           .then(function (list) {
               buildingOptions(list);
               tcs.resolve(true);
           });
        return tcs.promise();
    });

    return vm;
});