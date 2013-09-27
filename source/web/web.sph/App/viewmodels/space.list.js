﻿/// <reference path="../services/cultures.my.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/loadoperation.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/App/schemas/sph.domain.g.js" />

define([objectbuilders.datacontext, objectbuilders.router, objectbuilders.cultures],
    function (context, router, cultures) {

        var title = ko.observable(cultures.space.title),
            isBusy = ko.observable(false),
            activate = function () {
                var tcs = new $.Deferred();
                var templateTask = context.loadAsync("SpaceTemplate", "IsActive eq 1");
                var csTask = context.loadAsync("Space", "SpaceId gt 0");
                var statesTask = context.getDistinctAsync("Space", "", "State");
                var categoriesTask = context.getDistinctAsync("Space", "", "Category");

                $.when(templateTask, csTask, statesTask, categoriesTask).done(function (tlo, lo, states, categories) {

                    vm.searchTerm.stateOptions(states);
                    vm.searchTerm.categoryOptions(categories);
                    var commands = _(tlo.itemCollection).map(function (t) {
                        return {
                            caption: ko.observable("" + t.Name()),
                            icon: "icon-plus",
                            command: function () {
                                var url = '/#/space.detail-templateid.' + t.SpaceTemplateId() + "/" + t.SpaceTemplateId() + "/0/-/0";
                                router.navigateTo(url);
                                return {
                                    then: function () { }
                                };
                            }
                        };
                    });
                    vm.toolbar.groupCommands([ko.observable(
                    {
                        caption: ko.observable(cultures.space.toolbar.ADD_NEW_SPACE),
                        commands: ko.observableArray(commands)
                    })
                    ]);
                    vm.spaces(lo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            searchState = ko.observable(),
            searchKeyword = ko.observable(),
            query = {
                "query": {
                    "query_string": {
                        "fields": ["Name", "Address.State", "TemplateName", "BuildingName"],
                        "query": searchKeyword
                    }
                }
            },
            search = function () {
                var tcs = new $.Deferred();
                var spaceQuery = {
                    "query": {
                        "query_string": {
                            "fields": ["Name", "Address.State", "TemplateName", "BuildingName"],
                            "query": vm.searchTerm.keyword()
                        }
                    }
                };
                console.log(spaceQuery);
                var csTasks = context.searchAsync("Space", spaceQuery);
                $.when(csTasks)
                    .done(function (lo) {
                        vm.spaces(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();
            };

        var vm = {
            title: title,
            activate: activate,
            isBusy: isBusy,
            spaces: ko.observableArray([]),
            toolbar: {
                groupCommands: ko.observableArray()
            },
            cultures: cultures,
            query : query,
            searchTerm: {
                state: searchState,
                category: ko.observable(),
                stateOptions: ko.observableArray(),
                categoryOptions: ko.observableArray(),
                keyword: searchKeyword
            },
            searchCommand: search
        };

        return vm;
    });