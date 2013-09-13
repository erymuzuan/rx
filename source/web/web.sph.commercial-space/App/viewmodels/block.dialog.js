/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['durandal/system', 'services/datacontext'],
    function (system, context) {
        
        var isBusy = ko.observable(false),
            activate = function () {

            },
            okClick = function () {
                this.modal.close("OK");
            },
            cancelClick = function () {
                this.modal.close("Cancel");
            },
            addFloor = function (block) {
                var floor = new bespoke.sphcommercialspace.domain.Floor(system.guid());
                block.FloorCollection.push(floor);
            },
            removeFloor = function (floor) {
                vm.block().FloorCollection.remove(floor);
            };
        var vm = {
            isBusy: isBusy,
            block: ko.observable(new bespoke.sphcommercialspace.domain.Block()),
            activate: activate,
            addFloor: addFloor,
            removeFloor: removeFloor,
            okClick: okClick,
            cancelClick: cancelClick
        };
        return vm;
    });