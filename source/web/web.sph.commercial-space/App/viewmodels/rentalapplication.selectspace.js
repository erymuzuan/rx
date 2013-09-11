/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext'], function (context) {

    var activate = function () {
        var tcs = new $.Deferred();
        var templateTasks = context.loadAsync("ApplicationTemplate", "IsActive eq 1");
        var csTasks = context.loadAsync("CommercialSpace", "IsAvailable eq 1 and IsOnline eq 1");
        var statesTask = context.getDistinctAsync("Building", "", "State");
        var categoriesTask = context.getDistinctAsync("CommercialSpace", "", "Category");

        $.when(templateTasks, csTasks, statesTask, categoriesTask)
            .done(function (tlo, cslo, states, categories) {
                var items = _(tlo.itemCollection).map(function (t) {
                    var filtered = _(cslo.itemCollection).filter(function (c) {
                        return c.ApplicationTemplateOptions().indexOf(t.ApplicationTemplateId()) > -1;
                    });
                    return {
                        name: t.Name(),
                        id: t.ApplicationTemplateId(),
                        spaces: filtered
                    };
                });
                vm.items(items);
                vm.searchTerm.stateOptions(states);
                vm.searchTerm.categoryOptions(categories);
                vm.searchTerm.states(states);
                vm.searchTerm.categories(categories);
                tcs.resolve(true);

            });


        return tcs.promise();
    },

        search = function () {
            var tcs = new $.Deferred();

            var states = JSON.stringify(vm.searchTerm.states())
                .replace(/\"/g, "'")
                .replace(/\[/g, "")
                .replace(/\]/g, ""),
                buildingQuery = String.format("State in ({0})", states);
            
            console.log(states);
            if (!states) {
                buildingQuery = "BuildingId gt 0";
            }

            context.getListAsync("Building", buildingQuery, "BuildingId")
               .then(function (ids) {

                   var categories = JSON.stringify(vm.searchTerm.categories())
                       .replace(/\"/g, "'")
                       .replace(/\[/g, "")
                       .replace(/\]/g, ""),
                       buildings = JSON.stringify(ids)
                       .replace(/\[/g, "")
                       .replace(/\]/g, "");

                   console.log("Building", buildings);
                   console.log("categories", categories);
                   var query = "IsAvailable eq 1 and IsOnline eq 1";
                   if (categories) {
                       query += String.format(" and Category in ({0})", categories);
                   }
                   if (buildings) {
                       query += String.format(" and BuildingId in ({0})", buildings);
                   }


                   var templateTasks = context.loadAsync("ApplicationTemplate", "IsActive eq 1");
                   var csTasks = context.loadAsync("CommercialSpace", query);
                   $.when(templateTasks, csTasks)
                       .done(function (tlo, cslo) {
                           var items = _(tlo.itemCollection).map(function (t) {
                               var filtered = _(cslo.itemCollection).filter(function (c) {
                                   return c.ApplicationTemplateOptions().indexOf(t.ApplicationTemplateId()) > -1;
                               });
                               return {
                                   name: t.Name(),
                                   id: t.ApplicationTemplateId(),
                                   spaces: filtered
                               };
                           });
                           vm.items(items);
                           tcs.resolve(true);

                       });


               });
            return tcs.promise();

        };

    var vm = {
        activate: activate,
        items: ko.observableArray([]),
        searchTerm: {
            states: ko.observableArray(),
            categories: ko.observableArray(),
            stateOptions: ko.observableArray(),
            categoryOptions: ko.observableArray()
        },
        searchCommand: search
    };

    return vm;
});