/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger'], function (context, logger) {

    var activate = function() {
        logger.log('Admin Dashboard Activated', null, 'admindashboard', true);
            var tcs = new $.Deferred();
            context.getCountAsync("RentalApplication", "Status eq 'Pending'", "Status").done(function (c) {
                vm.pending(c);
                tcs.resolve(true);
            });
            context.getCountAsync("RentalApplication", "Status eq 'Approved'", "Status").done(function (c) {
                vm.approved(c);
                tcs.resolve(true);
            });
            return tcs.promise();
    };
    
   
    var vm = {
        activate: activate,
        title: 'Papan Tugas',
        rentalApplications: ko.observableArray([]),
        pending:ko.observable(),
        rejected:ko.observable(),
        approved:ko.observable(),
        allocate:ko.observable()
       
    };

    return vm;


});