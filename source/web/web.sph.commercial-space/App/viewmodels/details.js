/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger'], function (context, logger) {

    var activate = function() {
            logger.log('Admin Dashboard Activated', null, 'details', true);
            var tcs = new $.Deferred();
            context.getCountAsync("RentalApplication", "RentalApplicationId gt 0", "RentalApplicationId").done(function (c) {
                vm.count(c);
                tcs.resolve(true);
            });

            return tcs.promise();
    };
    
   
    var vm = {
        activate: activate,
        title: 'Papan Tugas',
        rentalApplications: ko.observableArray([]),
        count:ko.observable()
       
    };

    return vm;


});