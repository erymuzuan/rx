/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

   
    var addNew = function() {
        var url = '/#/buildingdetail/0';
        router.navigateTo(url);
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
            var url = '/#/buildingdetail/' + selectedBuilding.BuildingId();
            router.navigateTo(url);
        }
    };
    var activate = function() {
        logger.log('Building View Activated', null, 'building', true);
        loadBuilding();
        return true;
    };

    var loadBuilding = function() {
        context.loadAsync("Building", "BuildingId gt 0").done(function(lo) {
            vm.buildings(lo.itemCollection);
            return true;
        });
    };
    

    var vm = {
        activate: activate,
        title: 'Building',
        buildings: ko.observableArray([]),
        viewAttached: viewAttached,
        addNewCommand: addNew
    };

    return vm;
});
