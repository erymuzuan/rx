/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
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

define(['services/datacontext', 'services/logger', './_space.contract', 'durandal/system'], function (context, logger, contractlistvm,system) {

    var title = ko.observable(),
        selectedBuilding = {},
        isBusy = ko.observable(false),
        activate = function (routeData) {
            vm.space().BuildingId(parseInt(routeData.buildingId));
            var templateId = parseInt(routeData.templateId);
            title('Tambah ruang komersil');

            var tcs = new $.Deferred(),
                templateTask = context.loadOneAsync("SpaceTemplate", "SpaceTemplateId eq " + templateId),
                buildingTask = context.getTuplesAsync("Building", "", "BuildingId", "Name"),
                stateTask = context.loadOneAsync("Setting", "Key eq 'State'"),
                csTask = context.loadOneAsync("Space", "SpaceId eq " + routeData.csId);
         
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
                        cs = new bespoke.sph.domain.Space();
                        cs.TemplateId(templateId);
                        cs.Category(tpl.Name);
                        vm.space(cs);

                        var fieldToValueMap = function (f) {
                            var webid = system.guid();
                            var v = new bespoke.sph.domain.CustomFieldValue(webid);
                            v.Name(f.Name());
                            v.Type(f.Type());
                            return v;
                        },
                           cfs = _(tpl.CustomFieldCollection()).map(fieldToValueMap),
                           cls = _(tpl.CustomListDefinitionCollection()).map(function (v) {
                               var lt = new bespoke.sph.domain.CustomListValue(system.guid());
                               lt.Name(v.Name());

                               var fields = _(v.CustomFieldCollection()).map(fieldToValueMap);
                               lt.CustomFieldCollection = ko.observableArray(fields);

                               return lt;
                           });

                        vm.space().CustomFieldValueCollection(cfs);
                        vm.space().CustomListValueCollection(cls);
                        return;
                    }
                    
                    vm.space(cs);
                    
                    
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
            var data = ko.mapping.toJSON(vm.space());
            isBusy(true);
            context.post(data, "/Space/Save")
                .done(function (e) {
                    logger.log("Data has been successfully saved ", e, "space.detail", true);

                    isBusy(false);
                    tcs.resolve(true);
                });
            return tcs.promise();
        },
        selectLot = function () {
            $('#lot-selection-panel').modal();
        },
        addLots = function () {
            vm.space().LotCollection(vm.selectedLots);
            var lots = ko.mapping.toJS(vm.selectedLots);
            var lotName = _(lots).reduce(function (memo, lot) {
                return memo + lot.Name + ",";
            }, "");
            var size = _(lots).reduce(function (memo, lot) {
                return memo + lot.Size;
            }, 0);
            vm.space().LotName(lotName);
            vm.space().Size(size);
            vm.space().LotCollection(vm.selectedLots);
        };

    var vm = {
        activate: activate,
        title: title,
        viewAttached: viewAttached,
        space: ko.observable(new bespoke.sph.domain.Space()),
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
        vm.space().BuildingId(id);
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
        vm.space().FloorName(floor);
        vm.lotOptions(sf.LotCollection);
    });

    return vm;
});