/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger'], function (context, logger) {
    var vm = {
        activate: activate,
        title: 'Building',
        building: {
            Name:ko.observable(''),
            Address: {
                Street:ko.observable(''),
                State:ko.observable(''),
                City:ko.observable(''),
                Postcode:ko.observable(''),
            },
            LotNo:ko.observable(''),
            Size:ko.observable(''),
            Status:ko.observable(''),
            Block:ko.observable('')
        },
        saveCommand : saveAsync
    };

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log('Building View Activated', null, 'building', true);
        return true;
    }

    function saveAsync() {
        var tcs = new $.Deferred();
        var data = ko.mapping.toJSON(vm.building);
        context.post(data, "/Building/SaveBuilding").done(function(e) {
            logger.log("New building has been added", e, "building", true);
        });

        return tcs.promise();
    }
    //#endregion
});