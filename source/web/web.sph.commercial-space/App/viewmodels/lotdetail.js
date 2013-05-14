/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function(context, logger, router) {

    var activate = function (routeData) {
        logger.log('Lot Details View Activated', null, 'lotdetail', true);
        var id = routeData.id;
        return true;
    };
    
    var addNew = function() {
    };
    
    var vm = {
        activate: activate,
        title: 'Lot',
        Name: ko.observable(''),
        Size: ko.observable(''),
        addNewLotCommand: addNew
    };

    return vm;
});