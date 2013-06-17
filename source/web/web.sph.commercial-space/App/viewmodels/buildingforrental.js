/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'durandal/plugins/router'], function (context, router) {

    var activate = function () {
        var tcs = new $.Deferred();
        context.loadAsync('CommercialSpace', 'IsAvailable eq 1 ').done(function (lo) {
            vm.commercialspaces(lo.itemCollection);
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
             var query = "";
             if (vm.selectedState() && !vm.selectedBuildingId() && !vm.selectedCategory()) {
                 var query1 = String.format("State eq '{0}'", vm.selectedState());
                 query = query1;
             }
             if (vm.selectedCategory() && !vm.selectedState() && !vm.selectedBuildingId()) {
                 var query2 = String.format("Category eq '{0}'", vm.selectedCategory());
                 query = query2;
             }
             if (vm.selectedState() && vm.selectedBuildingId() && !vm.selectedCategory()) {
                 var query3 = String.format("State eq '{0}' and BuildingId eq '{1}'", vm.selectedState(), vm.selectedBuildingId());
                 query = query + query3;
             }
             if (vm.selectedState() && vm.selectedBuildingId() && vm.selectedCategory()) {
                 var query4 = String.format("State eq '{0}' and BuildingId eq '{1}' and Category eq '{2}'", vm.selectedState(), vm.selectedBuildingId(),vm.selectedCategory());
                 query = query + query4;
             }
             if (vm.selectedState() && !vm.selectedBuildingId() && vm.selectedCategory()) {
                 var query5 = String.format("State eq '{0}' and Category eq '{1}'", vm.selectedState(), vm.selectedCategory());
                 query = query + query5;
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
        buildingOptions: ko.observableArray(),
        selectedState: ko.observable(),
        selectedCategory : ko.observable()
    };

    vm.selectedState.subscribe(function (state) {
        var tcs = new $.Deferred();
        context.getTuplesAsync("Building", "State eq " + "'" + state + "'", "BuildingId", "Name")
           .then(function (list) {
               vm.buildingOptions(_(list).sortBy(function (b) {
                   return b.Item2;
               }));
               tcs.resolve(true);
           });
        return tcs.promise();
    });

    return vm;
});