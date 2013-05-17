/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {
    
    var activate = function () {
        logger.log('Building View Activated', null, 'building', true);

        var tcs = new $.Deferred();
        context.loadAsync("Building", "BuildingId gt 0").done(function (lo) {
            vm.buildings(lo.itemCollection);
            tcs.resolve(true);
        });
        tcs.promise();
    };

    var viewAttached = function (view) {
        bindEventToList(view, '#div-building', gotoDetails);
    };

    var bindEventToList = function (rootSelector, selector, callback, eventName) {
        var eName = eventName || 'click';
        $(rootSelector).on(eName, selector, function () {
            var building = ko.dataFor(this);
            callback(building);
            return false;
        });
    };
    var gotoDetails = function (selectedBuilding) {
        if (selectedBuilding && selectedBuilding.BuildingId()) {
            var url = '/#/csforrental/' + selectedBuilding.BuildingId();
            router.navigateTo(url);
        }
    };
    var vm = {
        activate: activate,
        title: 'Building',
        buildings: ko.observableArray([]),
        viewAttached: viewAttached
    };

    return vm;
});