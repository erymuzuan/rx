/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function(context, logger, router) {

    var lotCollection = ko.observableArray();
    var activate = function (routeData) {
        logger.log('Lot Details View Activated', null, 'lotdetail', true);
        var buildingId = routeData.buildingId;
        var floor = routeData.floorName;

        var tcs = new $.Deferred();
        context.loadOneAsync('Building', 'BuildingId eq ' + buildingId)
            .done(function(b) {
                $.filter(function(f) {
                }, b.FloorCollection);

                tcs.resolve(true);
            });
        
        return tcs.promise();
        

    };
    
    var addNew = function() {
    };
    var save = function() {

    };
    
    var vm = {
        activate: activate,
        title: 'Lot',
        Name: ko.observable(''),
        Size: ko.observable(''),
        addNewLotCommand: addNew,
        saveCommand: save,
        lotCollection : lotCollection
    };

    return vm;
});