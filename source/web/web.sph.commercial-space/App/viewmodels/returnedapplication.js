/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger'], function(context, logger) {

    var id = ko.observable(),
        activate = function(routedata) {
            logger.log('Application List View Activated', null, 'applicationlist', true);
            id(routedata.id);
            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + id()).done(function (r) {
                ko.mapping.fromJSON(ko.mapping.toJSON(r), {}, vm.rentalapplication);
                var incompleted = $.grep(r.AttachmentCollection(), function (x) { return x.IsCompleted() == false; });
                $(incompleted).each(function(e) {
                    vm.incompleteAttachments.push(e);
                });
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        sendEmail = function () {
            var tcs = new $.Deferred();
            var attachments = ko.mapping.toJS(vm.rentalapplication.AttachmentCollection);
            var data = JSON.stringify({ id: id(), attachments: attachments });
            context.post(data, "/RentalApplication/SendEmail").done(function () {
                logger.log("Successfully send the email");
                tcs.resolve(true);
            });
            return tcs.promise();
        };

    var vm = {
        activate: activate,
        rentalapplication: {
            Remarks: ko.observable(''),
            AttachmentCollection: ko.observableArray([])
        },
        incompleteAttachments: ko.observableArray([]),
        sendEmailCommand: sendEmail
    };
    return vm;
});