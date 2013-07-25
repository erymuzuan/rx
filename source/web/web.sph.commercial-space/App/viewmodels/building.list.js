/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var activate = function () {
        var tcs = new $.Deferred();
        context.loadAsync("Building", "BuildingId gt 0").done(function (lo) {
            vm.buildings(lo.itemCollection);
            tcs.resolve(true);
        });
        return tcs.promise();
    },
    addNew = function () {
        var url = '/#/building.detail/0';
        router.navigateTo(url);
        return {
            then: function () { }
        };
    },


            exportList = function () {

            };

    var vm = {
        activate: activate,
        title: 'Building',
        buildings: ko.observableArray([]),
        toolbar: {
            addNewCommand: addNew,
            exportCommand: exportList
        }
    };

    return vm;
});
