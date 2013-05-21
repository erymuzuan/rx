/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger,router) {

    var activate = function() {
        logger.log('Admin Dashboard Activated', null, 'admindashboard', true);
            var tcs = new $.Deferred();
        var pendingTask = context.getCountAsync("RentalApplication", "Status eq 'Pending'", "Status");
        var aprovedTask = context.getCountAsync("RentalApplication", "Status eq 'Approved'", "Status");
        
        $.when(pendingTask, aprovedTask)
            .then(function (pending, approved) {
                vm.approved(approved);
                vm.pending(pending);
                tcs.resolve(true);
            });
            return tcs.promise();
    },
        viewPending = function () {
            var url = '/#/applicationlist/pending';
            router.navigateTo(url);
        },
        viewApproved = function () {
            var url = '/#/applicationlist/approved';
            router.navigateTo(url);
        },
        viewDeclined = function () {
            var url = '/#/applicationlist/declined';
            router.navigateTo(url);
        };
    
   
    var vm = {
        activate: activate,
        title: 'Papan Tugas',
        rentalApplications: ko.observableArray([]),
        pending:ko.observable(),
        rejected:ko.observable(),
        approved:ko.observable(),
        allocate: ko.observable(),
        viewPendingCommand: viewPending,
        viewApprovedCommand: viewApproved,
        viewDeclinedCommand: viewDeclined
       
    };

    return vm;


});