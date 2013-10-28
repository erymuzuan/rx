/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../objectbuilders.js" />

define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router], function (context, logger, router) {

    var building = ko.observable(new bespoke.sph.domain.Building()),
        activate = function () {
            var tcs = new $.Deferred();
            var templateTask = context.loadAsync("BuildingTemplate", "IsActive eq 1");
            var statesTask = context.getDistinctAsync("Building", "", "State");

            $.when(templateTask,statesTask).done(function (tlo,states) {
                vm.templates(tlo.itemCollection);
                vm.searchTerm.stateOptions(states);

                var commands = _(tlo.itemCollection).map(function (t) {
                    return {
                        caption: ko.observable(t.Name()),
                        icon: "fa fa-building",
                        command: function () {
                            var url = '/#/building.detail-templateid.' + t.BuildingTemplateId() + "/" + t.BuildingTemplateId() + "/0";
                            router.navigateTo(url);
                            return {
                                then: function () { }
                            };
                        }
                    };
                });

                vm.toolbar.groupCommands([ko.observable(
                    {
                        caption: ko.observable("+ Bangunan Baru"),
                        commands: ko.observableArray(commands)
                    })
                ]);

                tcs.resolve(true);
            });

            return tcs.promise();
        },
        search = function () {
            var tcs = new $.Deferred();
          
            var buildingTask = context.searchAsync("Building", ko.toJSON(vm.query));
            $.when(buildingTask)
                .done(function (lo) {
                    vm.buildings(lo.itemCollection);
                    tcs.resolve(true);
                });
            return tcs.promise();
        },
        searchState = ko.observable(),
        searchKeyword = ko.observable();

    var vm = {
        activate: activate,
        title: 'Building',
        building: building,
        address: ko.observable(),
        buildings: ko.observableArray([]),
        templates: ko.observableArray([]),
        toolbar: {
            groupCommands: ko.observableArray(),
            reloadCommand: function () {
                return activate();
            },
            printCommand: ko.observable({
                entity: ko.observable("Building"),
                id: ko.observable(0),
                item: building,
            })
        },
        searchTerm: {
            state: searchState,
            stateOptions: ko.observableArray(),
            keyword: searchKeyword,
            initalQuery : {},
            query: {
                "query": {
                    "query_string": {
                        "query": searchKeyword
                    }
                }
            }
        },
        searchCommand: search
    };

    return vm;
});
