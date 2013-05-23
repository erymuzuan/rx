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
        var pendingTask = context.getCountAsync("RentalApplication", "Status eq 'New'", "Status");
        var aprovedTask = context.getCountAsync("RentalApplication", "Status eq 'Approved'", "Status");
        var waitingTask = context.getCountAsync("RentalApplication", "Status eq 'Waiting'", "Status");
        var declinedTask = context.getCountAsync("RentalApplication", "Status eq 'Declined'", "Status");
        var confirmedTask = context.getCountAsync("RentalApplication", "Status eq 'Confirmed'", "Status");
        var offerRejectedTask = context.getCountAsync("RentalApplication", "Status eq 'OfferRejected'", "Status");
        var waitingConfirmationTask = context.getCountAsync("RentalApplication", "Status eq 'WaitingConfirmation'", "Status");
        
        $.when(pendingTask, aprovedTask,waitingTask,declinedTask,confirmedTask,offerRejectedTask,waitingConfirmationTask)
            .then(function (pending, approved,waiting,declined,confirmed,offerRejected,waitingConfirmation) {
                vm.approved(approved);
                vm.pending(pending);
                vm.waiting(waiting);
                vm.declined(declined);
                vm.confirmed(confirmed);
                vm.offerRejected(offerRejected);
                vm.waitingConfirmation(waitingConfirmation);
                tcs.resolve(true);
            });
            return tcs.promise();
    },
        viewPending = function () {
            var url = '/#/applicationlist/new';
            router.navigateTo(url);
        },
        viewApproved = function () {
            var url = '/#/applicationlist/approved';
            router.navigateTo(url);
        },
        viewWaitingConfirmation = function () {
            var url = '/#/applicationlist/waitingconfirmation';
            router.navigateTo(url);
        },
        viewOfferRejected = function () {
            var url = '/#/applicationlist/offerrejected';
            router.navigateTo(url);
        },
        viewWaitingList = function () {
            var url = '/#/applicationlist/waiting';
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
        waiting:ko.observable(),
        declined:ko.observable(),
        confirmed:ko.observable(),
        waitingConfirmation:ko.observable(),
        offerRejected:ko.observable(),
        allocate: ko.observable(),
        viewPendingCommand: viewPending,
        viewApprovedCommand: viewApproved,
        viewWaitingListCommand: viewWaitingList,
        viewOfferRejectedCommand: viewOfferRejected,
        viewWaitingConfirmationCommand: viewWaitingConfirmation,
        viewDeclinedCommand: viewDeclined
       
    };

    return vm;


});