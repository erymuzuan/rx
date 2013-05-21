/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var title = ko.observable(),
        buildingId = ko.observable(),
        activate = function (routedata) {
            logger.log('Permohonan View Activated', null, 'commercialspaceforrental', true);
            buildingId(routedata.buildingId);
            title('Ruang Komersial Untuk Disewa');

            var tcs = new $.Deferred();
            context.loadAsync('CommercialSpace', 'BuildingId eq ' + buildingId())
                .done(function (lo) {
                    vm.commercialspaces(lo.itemCollection);
                    tcs.resolve(true);
                });

            return tcs.promise();
        },
        
        viewAttached = function (view) {
            bindEventToList(view, '#div-cs', gotoDetails);
        },
        
        bindEventToList = function (rootSelector, selector, callback, eventName) {
            var eName = eventName || 'click';
            $(rootSelector).on(eName, selector, function () {
                var cs = ko.dataFor(this);
                callback(cs);
                return false;
            });
        },
        gotoDetails = function (selectedCs) {
            if (selectedCs && selectedCs.CommercialSpaceId()) {
                var url = '/#/rentalapplication/' + selectedCs.CommercialSpaceId() + "/0";
                router.navigateTo(url);
            }
        },
        showMap = function () {

        };

    var vm = {
        activate: activate,
        title: title,
        commercialspaces: ko.observableArray([]),
        viewAttached: viewAttached,
        showMapCommand: showMap
    };
    return vm;
});