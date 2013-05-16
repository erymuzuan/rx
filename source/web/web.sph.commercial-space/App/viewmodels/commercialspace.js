
define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/app'], function (context, logger, router, app) {

    var floorname = ko.observable(),
        lotname = ko.observable(),
        buildingId = ko.observable(),
        title = ko.observable(),
        activate = function(routeData) {
            logger.log('Lot Details View Activated', null, 'lotdetail', true);
            buildingId(routeData.buildingId);
            floorname(routeData.floorname);
            lotname(routeData.lotname);

            title('Commercial Space on ' + lotname());
          
        },
        saveCs = function () {

        };
    var vm = {
        activate: activate,
        title: title,
        commercialSpace: {
            FloorName: ko.observable(floorname()),
            LotName: ko.observable(lotname()),
            Name: ko.observable(''),
            Size: ko.observable(''),
            Category: ko.observable(''),
            RentalRate: ko.observable(''),
            RentalType: ko.observable(''),
            IsOnline: ko.observable(false),
        },
       
        saveCsCommand: saveCs
    };

    return vm;
});