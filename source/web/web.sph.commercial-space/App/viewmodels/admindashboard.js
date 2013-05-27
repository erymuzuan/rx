/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var activate = function () {
        logger.log('Admin Dashboard Activated', null, 'admindashboard', true);
        var tcs = new $.Deferred();
        var newTask = context.getCountAsync("RentalApplication", "Status eq 'New'", "Status");
        var aprovedTask = context.getCountAsync("RentalApplication", "Status eq 'Approved'", "Status");
        var waitingTask = context.getCountAsync("RentalApplication", "Status eq 'Waiting'", "Status");
        var declinedTask = context.getCountAsync("RentalApplication", "Status eq 'Declined'", "Status");
        var confirmedTask = context.getCountAsync("RentalApplication", "Status eq 'Confirmed'", "Status");
        var offerRejectedTask = context.getCountAsync("RentalApplication", "Status eq 'OfferRejected'", "Status");
        var waitingConfirmationTask = context.getCountAsync("RentalApplication", "Status eq 'WaitingConfirmation'", "Status");
        var returnedTask = context.getCountAsync("RentalApplication", "Status eq 'Returned'", "Status");

        $.when(newTask, aprovedTask, waitingTask, declinedTask, confirmedTask, offerRejectedTask, waitingConfirmationTask, returnedTask)
            .then(function (newCount, approved, waiting, declined, confirmed, offerRejected, waitingConfirmation, returned) {
                vm.approved(approved);
                vm.newCount(newCount);
                vm.waiting(waiting);
                vm.declined(declined);
                vm.confirmed(confirmed);
                vm.offerRejected(offerRejected);
                vm.waitingConfirmation(waitingConfirmation);
                vm.returned(returned);

                tcs.resolve(true);
            });
        return tcs.promise();
    },
        viewList = function (status) {
            /**/
            var tcs = new $.Deferred();
            setTimeout(function () {
                var url = '/#/applicationlist/' + status;
                console.log(status);
                tcs.resolve(status);
                router.navigateTo(url);
            }, 200);
            return tcs.promise();
        };


    var vm = {
        activate: activate,
        title: 'Papan Tugas',
        rentalApplications: ko.observableArray([]),
        newCount: ko.observable(),
        rejected: ko.observable(),
        approved: ko.observable(),
        waiting: ko.observable(),
        declined: ko.observable(),
        confirmed: ko.observable(),
        waitingConfirmation: ko.observable(),
        offerRejected: ko.observable(),
        allocate: ko.observable(),
        returned: ko.observable(0),
        viewCommand: viewList

    };

    return vm;


});