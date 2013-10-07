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

define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, 'durandal/app', 'durandal/system', objectbuilders.cultures],
    function (context, logger, router, app, system, cultures) {

        var title = ko.observable(''),
            buildingId = ko.observable(),
            building = ko.observable(),
            isBusy = ko.observable(false),
            floorname = ko.observable(),
            editedUnit = ko.observable(),
            activate = function (routeData) {
                logger.log('Unit Details View Activated', null, 'lotdetail', true);

                buildingId(parseInt(routeData.buildingId));

                var tcs = new $.Deferred();
                context.loadOneAsync('Building', 'BuildingId eq ' + buildingId())
                    .done(function (b) {
                        if (!b) {
                            logger.error("Cannot find building - " + buildingId());
                            return;
                        }
                        title(String.format(cultures.lots.LOT_LIST_TITLE, b.Name()));
                        building(b);


                        var blocks = b.BlockCollection(),
                            placeHolder = {
                                Name: ko.observable('[Semua Block]'),
                                isPlaceHolder: true
                            };

                        placeHolder.Name('[Semua Block]');
                        blocks.splice(0, 0, placeHolder);
                        vm.blockOptions(blocks);


                        var floors = [{
                            Name: ko.observable('[Semua Lantai]'),
                            isPlaceHolder: true
                        }];

                        vm.floorOptions(_(floors).concat(b.FloorCollection()));
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            removeUnit = function (floor) {
                vm.floor().UnitCollection.remove(floor);
            },
            addNew = function () {
                $('#unit-dialog').modal();
                $('#ok-unit-dialog-btn').one('click', function (ev) {
                    ev.preventDefault();
                    if (ev.target.form.checkValidity()) {
                        //if (!vm.block.isPlaceHolder)
                        //    unit.BlockNo(vm.block.Name());
                        //unit.FloorNo(vm.floor.Number());
                        //  vm.unit().BlockNo(vm.unit().BlockNo());
                        vm.unit().BlockNo(vm.blockNo().Name());
                        vm.unit().FloorNo(vm.floorNo().Name());
                        vm.list.push(vm.unit());
                        $('#unit-dialog').modal("hide");
                        return save(vm.unit());
                    }
                });
            },
            editUnit = function (unit) {
                var c1 = ko.mapping.fromJSON(ko.mapping.toJSON(unit));
                var clone = c1;
                editedUnit(unit);
                vm.unit(clone);

                $('#unit-dialog').modal({});
            },
            save = function (unit) {
                var tcs = new $.Deferred();
                var data = JSON.stringify({
                    floor: ko.mapping.toJS(vm.floorNo()),
                    block: ko.mapping.toJS(vm.blockNo()),
                    buildingId: buildingId(),
                    unit: ko.toJS(unit)
                });
                isBusy(true);
                context.post(data, "/Building/AddUnit")
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
            block: ko.observable(new bespoke.sph.domain.Block()),
            unit: ko.observable(new bespoke.sph.domain.Unit()),
            list: ko.observableArray(),
            blockNo: ko.observable(),
            floorNo: ko.observable(),
            addCsCommand: addCs,
            addNewUnitCommand: addNew,
            editUnit: editUnit,
            goBackCommand: goBack,
            searchTerm: {
                block: ko.observable(),
                floor: ko.observable()
            },
            blockOptions: ko.observableArray(),
            floorOptions: ko.observableArray(),
            removeUnitCommand: removeUnit,
            isBusy: isBusy,
            toolbar : {
                clicks: ko.observableArray([{
                    caption: "Unit",
                    command: addNew,
                    icon :"icon-plus-sign"
                }])
            }
        };

        vm.blockNo.subscribe(function (b) {
            if (!b) return;
            var floors = [{
                Number: ko.observable('[Semua Lantai]'),
                isPlaceHolder: true
            }];
            if (!b.isPlaceHolder) {
                vm.floorOptions(_(floors).concat(b.FloorCollection()));
            } else {
                vm.floorOptions(_(floors).concat(building().FloorCollection()));
            }
        });

        vm.searchTerm.block.subscribe(function (b) {
            if (!b) return;
            var floors = [{
                Number: ko.observable('[Semua Lantai]'),
                isPlaceHolder: true
            }];
            if (!b.isPlaceHolder) {
                vm.floorOptions(_(floors).concat(b.FloorCollection()));
            } else {
                vm.floorOptions(_(floors).concat(building().FloorCollection()));
            }
        });
        vm.searchTerm.floor.subscribe(function (floor) {
            if (!floor) return;
            if (!vm.searchTerm.block()) return;


            var units = [];
            if (floor.isPlaceHolder && vm.searchTerm.block().isPlaceHolder) {
                // look for all blocks
                _(building().BlockCollection()).each(function (blk) {
                    if (blk.isPlaceHolder) return;
                    _(blk.FloorCollection()).each(function (flr) {
                        if (flr.isPlaceHolder) return;
                        units = _(flr.UnitCollection()).concat(units);
                    });
                });
                // common floors
                _(building().FloorCollection()).each(function (flr) {
                    if (flr.isPlaceHolder) return;
                    units = _(flr.UnitCollection()).concat(units);
                });
            }
            //
            if (floor.isPlaceHolder && !vm.block().isPlaceHolder) {

                _(vm.block().FloorCollection()).each(function (f) {
                    units = _(f.UnitCollection()).concat(units);
                });


            }
            if (!floor.isPlaceHolder) {
                units = floor.UnitCollection();
            }
            vm.list(units);
        });

        return vm;
    });