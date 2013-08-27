﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />

define(['services/datacontext', 'services/logger', './_commercialspace.contract', 'durandal/system'], function (context, logger, contractlistvm,system) {

    var title = ko.observable(),
        selectedBuilding = {},
        isBusy = ko.observable(false),
        activate = function (routeData) {
            vm.commercialSpace().BuildingId(parseInt(routeData.buildingId));
            var templateId = parseInt(routeData.templateId);
            title('Tambah ruang komersil');

            var tcs = new $.Deferred(),
                templateTask = context.loadOneAsync("CommercialSpaceTemplate", "CommercialSpaceTemplateId eq " + templateId),
                buildingTask = context.getTuplesAsync("Building", "BuildingId gt 0", "BuildingId", "Name"),
                stateTask = context.loadOneAsync("Setting", "Key eq 'State'"),
                csTask = context.loadOneAsync("CommercialSpace", "CommercialSpaceId eq " + routeData.csId),
                buildCustomFieldValue = function (tpl) {
                    var cfs = _(tpl.CustomFieldCollection()).map(function (f) {
                        var webid = system.guid();
                        var v = new bespoke.sphcommercialspace.domain.CustomFieldValue(webid);
                        v.Name(f.Name());
                        v.Type(f.Type());
                        v.Value('');
                        return v;
                    });

                    vm.commercialSpace().CustomFieldValueCollection(cfs);
                    vm.commercialSpace().Category(tpl.Name());
                };


            $.when(templateTask, buildingTask, csTask, stateTask)
                .done(function (a, list) {
                    vm.buildingOptions(_(list).sortBy(function (bd) {
                        return bd.Item2;
                    }));
                })
                .done(function (tpl, b, cs, s) {
                    
                    var states = JSON.parse(ko.mapping.toJS(s.Value));
                    vm.stateOptions(states);
                    if (!cs) {
                        cs = new bespoke.sphcommercialspace.domain.CommercialSpace();
                        cs.TemplateId(templateId);
                        cs.Category(tpl.Name);
                        vm.commercialSpace(cs);

                        buildCustomFieldValue(tpl);
                        
                        return;
                    }
                    var templates = _(cs.ApplicationTemplateOptions()).map(function (v) {
                        return v.toString();
                    });
                    vm.commercialSpace(cs);
                    vm.commercialSpace().ApplicationTemplateOptions(templates);
                    
                    title('Maklumat ruang komersil');
                    contractlistvm.activate(routeData)
                        .then(function () {
                            tcs.resolve(true);
                        });
                })
                .done(function () {
                    tcs.resolve();
                });
            
            return tcs.promise();
        },
        viewAttached = function (view) {
            $(view).tooltip({ 'placement': 'right' });
        },
        saveCs = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.commercialSpace());
            isBusy(true);
            context.post(data, "/CommercialSpace/SaveCommercialSpace")
                .done(function (e) {
                    logger.log("Data has been successfully saved ", e, "commercialspacedetail", true);

                    isBusy(false);
                    tcs.resolve(true);
                });
            return tcs.promise();
        },
        selectLot = function () {
            $('#lot-selection-panel').modal();
        },
        addLots = function () {
            vm.commercialSpace().LotCollection(vm.selectedLots);
            var lots = ko.mapping.toJS(vm.selectedLots);
            var lotName = _(lots).reduce(function (memo, lot) {
                return memo + lot.Name + ",";
            }, "");
            var size = _(lots).reduce(function (memo, lot) {
                return memo + lot.Size;
            }, 0);
            vm.commercialSpace().LotName(lotName);
            vm.commercialSpace().Size(size);
            vm.commercialSpace().LotCollection(vm.selectedLots);
        };

    var vm = {
        activate: activate,
        title: title,
        viewAttached: viewAttached,
        commercialSpace: ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpace()),
        buildingOptions: ko.observableArray(),
        floorOptions: ko.observableArray(),
        lotOptions: ko.observableArray(),
        stateOptions: ko.observableArray(),
        selectedBuilding: ko.observable(),
        selectedFloor: ko.observable(),
        selectedLots: ko.observableArray(),
        toolbar: {
            saveCommand: saveCs
        },
        selectLotCommand: selectLot,
        addLotsCommand: addLots,
        isBusy: isBusy
    };

    vm.selectedBuilding.subscribe(function (id) {
        vm.isBusy(true);
        vm.commercialSpace().BuildingId(id);
        context.loadOneAsync("Building", "BuildingId eq " + id)
            .then(function (b) {
                var floors = _(b.FloorCollection()).map(function (f) {
                    return f.Name();
                });
                vm.floorOptions(floors);
                vm.isBusy(false);
                selectedBuilding = b;
            });
    });
    vm.selectedFloor.subscribe(function (floor) {
        var building = ko.mapping.toJS(selectedBuilding);
        var sf = _(building.FloorCollection).find(function (f) {
            return f.Name == floor;
        });
        vm.commercialSpace().FloorName(floor);
        vm.lotOptions(sf.LotCollection);
    });

    return vm;
});