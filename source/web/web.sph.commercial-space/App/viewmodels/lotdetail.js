/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/app', 'durandal/system'], 
    function (context, logger, router, app, system) {

    var title = ko.observable(''),
        buildingId = ko.observable(),
        isBusy = ko.observable(false),
        floorname = ko.observable(),
        activate = function (routeData) {
            logger.log('Lot Details View Activated', null, 'lotdetail', true);

            buildingId(routeData.buildingId);
            floorname(routeData.floorname);

            title('Lot details on ' + floorname());
            var tcs = new $.Deferred();
            context.loadOneAsync('Building', 'BuildingId eq ' + buildingId())
                .done(function (b) {
                    if (!b) {
                        var lot = new bespoke.sphcommercialspace.domain.Lot();
                        lot.IsCommercialSpace(true);
                        vm.floor().LotCollection.push(lot);
                    }
                    var flo = $.grep(b.FloorCollection(), function (x) { return x.Name() === floorname(); })[0];
                    vm.floor(flo);
                    
                    tcs.resolve(true);
                });

            return tcs.promise();
        },
        removeLot = function (floor) {
            vm.floor().LotCollection.remove(floor);
        },
        addNew = function () {
            var lot = new bespoke.sphcommercialspace.domain.Lot(system.guid());
            vm.floor().LotCollection.push(lot);
        },
        save = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({
                floor: ko.mapping.toJS(vm.floor),
                buildingId: buildingId(),
                floorname: floorname()
            });
            isBusy(true);
            context.post(data, "/Building/AddLot")
                .done(function (e) {
                    if (e.status) {
                        logger.log(e.message, e, this, true);
                    } else {
                        logger.logError(e.message, e, this, true);
                    }

                    isBusy(false);
                    tcs.resolve(true);
                });

            return tcs.promise();
        },

        goBack = function () {
            var url = "/#/buildingdetail/" + buildingId();
            router.navigateTo(url);
        },

       addCs = function (lot) {
           var url = '/#/commercialspacedetail/' + buildingId() + '/' + floorname() + '/' + lot.Name();
           router.navigateTo(url);
       };

    var vm = {
        activate: activate,
        title: title,
        floor: ko.observable(new bespoke.sphcommercialspace.domain.Floor()),
        addCsCommand: addCs,
        addNewLotCommand: addNew,
        goBackCommand: goBack,
        
        removeLotCommand: removeLot,
        isBusy: isBusy,
        toolbar : {
            saveCommand: save
        }
    };

    return vm;
});