/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../objectbuilders.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/app', 'durandal/system', objectbuilders.cultures ], 
    function (context, logger, router, app, system) {

    var title = ko.observable(''),
        buildingId = ko.observable(),
        isBusy = ko.observable(false),
        floorname = ko.observable(),
        blockName = ko.observable(),
        activate = function (routeData) {
            logger.log('Lot Details View Activated', null, 'lotdetail', true);

            buildingId(routeData.buildingId);
            floorname(routeData.floorname);
            blockName(routeData.block);

            title(String.format(cultures.lots.LOT_LIST_TITLE, blockName(), floorname()));
            
            var tcs = new $.Deferred();
            context.loadOneAsync('Building', 'BuildingId eq ' + buildingId())
                .done(function (b) {
                    if (!b) {
                        var lot = new bespoke.sph.domain.Lot();
                        lot.IsSpace(true);
                        vm.floor().LotCollection.push(lot);
                    }
                    if (blockName()) {
                        var block = _(b.BlockCollection()).find(function (k) {
                            return k.Name() == blockName();
                        });
                        
                        if (!block) {
                            log.error("Cannot find block " + blockName());
                            tcs.resolve(false);
                            return;
                        }
                        var floor = _(block.FloorCollection()).find(function (f) {
                            return f.Name() == floorname();
                        });
                        vm.floor(floor);
                        tcs.resolve(true);
                        return;
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
            var lot = new bespoke.sph.domain.Lot(system.guid());
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
            var url = "/#/building.detail/" + buildingId();
            router.navigateTo(url);
        },

       addCs = function (lot) {
           var url = '/#/space.detail/' + buildingId() + '/' + floorname() + '/' + lot.Name();
           router.navigateTo(url);
       };

    var vm = {
        activate: activate,
        title: title,
        floor: ko.observable(new bespoke.sph.domain.Floor()),
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