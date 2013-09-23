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
        var csTasks = context.loadAsync("Space", "IsAvailable eq 1 and IsOnline eq 1");
        var statesTask = context.getDistinctAsync("Building", "", "State");
        var categoriesTask = context.getDistinctAsync("Space", "", "Category");

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
                tcs.resolve(true);

            });

        // get max and min
        context.getMaxAsync("Space", "IsAvailable eq 1 and IsOnline eq 1", "RentalRate")
            .done(function (max) {
                vm.searchTerm.max(max);
                vm.searchTerm.maxValue(max);
            });
        context.getMinAsync("Space", "IsAvailable eq 1 and IsOnline eq 1", "RentalRate")
            .done(function (min) {
                vm.searchTerm.min(min);
                vm.searchTerm.minValue(min);
            });

        return tcs.promise();
    },

        search = function () {
            var tcs = new $.Deferred();

            var musts = [{
                "range": {
                    "RentalRate": {
                        "from": searchTerm.minValue(),
                        "to": searchTerm.maxValue()
                    }
                }
            }];
            if (states().length) {
                musts.push({
                    "match": { "Address.State": { "query": states().join(" OR ") } }
                });
            }
            if (categories().length) {
                musts.push({
                    "match": { "Category": { "query": categories().join(" OR ") } }
                });
            }

            var sq = {
                "query": {
                    "bool": {
                        "must": musts
                    }
                }
            };


            var templateTasks = context.loadAsync("ApplicationTemplate", "IsActive eq 1"),
                csTasks = context.searchAsync("Space", sq),
                pager;
            $.when(templateTasks, csTasks)
                .done(function spacesLoaded(templateLoadOperation, spaceLoadOperation) {
                    var items = _(templateLoadOperation.itemCollection).map(function (t) {
                        var filtered = _(spaceLoadOperation.itemCollection).filter(function (c) {
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

                    if (!pager) {
                        var options = {
                            element: $('#search-space-pager'),
                            count: spaceLoadOperation.rows,
                            changed: function(p,s) {
                                context.searchAsync({
                                    entity: "Space",
                                    page: p,
                                    size :s
                                }, sq).done(function(lo) {
                                    spacesLoaded(templateLoadOperation, lo);
                                });
                            }
                        };
                        pager = new bespoke.utils.ServerPager(options);
                    } else {
                        pager.update(spaceLoadOperation.rows);
                    }

                });


            return tcs.promise();

        },
        states = ko.observableArray(),
        categories = ko.observableArray(),
        searchTerm = {
            states: states,
            categories: categories,
            stateOptions: ko.observableArray(),
            categoryOptions: ko.observableArray(),
            minValue: ko.observable(),
            maxValue: ko.observable(),
            min: ko.observable(),
            max: ko.observable(),

        };

    var vm = {
        activate: activate,
        items: ko.observableArray([]),
        searchTerm: searchTerm,
        searchCommand: search
    };

    return vm;
});