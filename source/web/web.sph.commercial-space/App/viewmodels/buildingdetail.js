/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../services/datacontext.js" />
define(['services/datacontext',
        'durandal/plugins/router',
     'durandal/system',
        'durandal/app',
        'services/logger'],
    function (datacontext, router, system, app, logger) {
        
        var activate = function (routeData) {
            logger.log('Building Details View Activated', null, 'buildingdetail', true);
            var id = parseInt(routeData.id);
            var query = "BuildingId eq " + id;
            datacontext.loadOneAsync("Building", query).done(function(b) {
                ko.mapping.fromJSON(ko.mapping.toJSON(b), {}, vm.building);;
            });
            return true;
        };
      
        var saveAsync = function() {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.building);
            context.post(data, "/Building/SaveBuilding").done(function(e) {
                logger.log("New building has been added", e, "building", true);
            });
            return tcs.promise();
        };
        
        var vm = {
            activate: activate,
            building: {
                Name: ko.observable(''),
                Address: {
                    Street: ko.observable(''),
                    State: ko.observable(''),
                    City: ko.observable(''),
                    Postcode: ko.observable(''),
                },
                LotNo: ko.observable(''),
                Size: ko.observable(''),
                Status: ko.observable(''),
                Block: ko.observable('')
            },
            saveCommand: saveAsync,
            title: 'Building Details'
        };
        
        return vm;
    });