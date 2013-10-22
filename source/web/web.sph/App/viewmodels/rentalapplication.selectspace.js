﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext'], function (context) {

    var activate = function () {
        vm.searchTerm.query('IsAvailable eq 1 and IsOnline eq 1');
        
        var tcs = new $.Deferred(),
            templateTasks = context.loadAsync("ApplicationTemplate", "IsActive eq 1"),
            csTasks = context.loadAsync("Space", vm.searchTerm.query()),
            statesTask = context.getDistinctAsync("Building", "", "State"),
            categoriesTask = context.getDistinctAsync("Space", "", "Category");
        

        $.when(templateTasks, csTasks, statesTask, categoriesTask)
            .done(function (tlo, cslo, states2, categories2) {
                vm.spaces(cslo.itemCollection);
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
                vm.spaces(items);
                vm.searchTerm.stateOptions(states2);
                vm.searchTerm.categoryOptions(categories2);
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
                            if (c.ApplicationTemplateOptions().indexOf(t.ApplicationTemplateId()) > -1) {
                                return c.ApplicationTemplateOptions().indexOf(t.ApplicationTemplateId()) > -1;
                            }
                            return false;
                        });
                        return {
                            name: t.Name(),
                            id: t.ApplicationTemplateId(),
                            spaces: filtered
                        };
                    });
                    vm.spaces(items);
                    tcs.resolve(true);

                    if (!pager) {
                        var options = {
                            element: $('#search-space-pager'),
                            count: spaceLoadOperation.rows,
                            changed: function (p, s) {
                                context.searchAsync({
                                    entity: "Space",
                                    page: p,
                                    size: s
                                }, sq).done(function (lo) {
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
                query: ko.observable(),
                state: ko.observable(),
                stateOptions: ko.observableArray(),
                categoryOptions: ko.observableArray(),
                minValue: ko.observable(),
                maxValue: ko.observable(),
                min: ko.observable(),
                max: ko.observable(),

            };

    var vm = {
        activate: activate,
        spaces : ko.observableArray([]),
        searchTerm: searchTerm,
        searchCommand: search
    };

    return vm;
});