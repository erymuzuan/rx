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
             if (vm.selectedState() && !vm.selectedBuildingId() && !vm.selectedCategory()) {
                 var query1 = String.format("and State eq '{0}'", vm.selectedState());
                 query = query + query1;
             }
             if (vm.selectedCategory() && !vm.selectedState() && !vm.selectedBuildingId()) {
                 var query2 = String.format("and Category eq '{0}'", vm.selectedCategory());
                 query = query + query2;
             }
             if (vm.selectedState() && vm.selectedBuildingId() && !vm.selectedCategory()) {
                 var query3 = String.format("and State eq '{0}' and BuildingId eq '{1}'", vm.selectedState(), vm.selectedBuildingId());
                 query = query + query3;
             }
             if (vm.selectedState() && vm.selectedBuildingId() && vm.selectedCategory()) {
                 var query4 = String.format("and State eq '{0}' and BuildingId eq '{1}' and Category eq '{2}'", vm.selectedState(), vm.selectedBuildingId(),vm.selectedCategory());
                 query = query + query4;
             }
             if (vm.selectedState() && !vm.selectedBuildingId() && vm.selectedCategory()) {
                 var query5 = String.format("and State eq '{0}' and Category eq '{1}'", vm.selectedState(), vm.selectedCategory());
                 query = query + query5;
             }
             if (!vm.selectedState() && !vm.selectedBuildingId() && !vm.selectedCategory()) {
                 var query6 = String.format("and CommercialSpaceId gt 0");
                 query = query + query6;
             }
             context.loadAsync('CommercialSpace', query).done(function (lo) {
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