/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var building = ko.observable(new bespoke.sphcommercialspace.domain.Building()),
        activate = function () {
        var tcs = new $.Deferred();
        var templateTask = context.loadAsync("BuildingTemplate", "IsActive eq 1");
      

        $.when(templateTask).done(function (tlo) {
            vm.templates(tlo.itemCollection);

            var commands = _(tlo.itemCollection).map(function (t) {
                return {
                    caption: ko.observable(t.Name()),
                    icon: "icon-building",
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
        exportList = function () {};

    var vm = {
        activate: activate,
        title: 'Building',
        building:building,
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
            }),
            exportCommand: exportList
        }
    };

    return vm;
});
