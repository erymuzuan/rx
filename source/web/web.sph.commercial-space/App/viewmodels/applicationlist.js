/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger'], function (context, logger) {
    var activate = function () {
        logger.log('Application List View Activated', null, 'applicationlist', true);

        var tcs = new $.Deferred();
        context.loadAsync("RentalApplication", "RentalApplicationId gt 0").done(function (lo) {
            vm.applications(lo.itemCollection);
            tcs.resolve(true);
        });
        tcs.promise();
    };


    var vm = {
        activate: activate,
        title: 'Application List',
        applications: ko.observableArray([]),
        viewAttached: viewAttached
    };

    return vm;
});