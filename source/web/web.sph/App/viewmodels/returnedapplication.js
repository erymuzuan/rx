/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger'], function (context, logger) {

    var id = ko.observable(),
        activate = function (routedata) {
            logger.log('Application List View Activated', null, 'applicationlist', true);
            id(routedata.id);
            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + id()).done(function (r) {
                vm.application(r);
                var incompleted = $.grep(r.AttachmentCollection(), function (x) { return x.IsCompleted() == false; });
                $(incompleted).each(function (e) {
                    vm.incompleteAttachments.push(e);
                });
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        sendEmail = function () {
            var tcs = new $.Deferred();
            var attachments = ko.mapping.toJS(vm.application.AttachmentCollection);
            var data = JSON.stringify({ id: id(), attachments: attachments, remarks: vm.application.Remarks() });
            context.post(data, "/RentalApplication/SendReturnedEmail")
                .done(function (result) {
                    logger.log("Successfully send the email", result, this, true);
                    tcs.resolve(true);
                });
            return tcs.promise();
        },
        generateLetter = function () {
            var tcs = new $.Deferred();
            var attachments = ko.mapping.toJS(vm.application.AttachmentCollection);
            var data = JSON.stringify({ id: id(), attachments: attachments, remarks: vm.application.Remarks() });
            context.post(data, "/RentalApplication/GenerateReturnedLetter")
                .done(function (result) {
                    window.open("/RentalApplication/Download/" + result);
                    tcs.resolve(true);
                });
            return tcs.promise();
        };

    var vm = {
        activate: activate,
        application: ko.observable(new bespoke.sph.domain.RentalApplication()),
        incompleteAttachments: ko.observableArray([]),
        sendEmailCommand: sendEmail,
        generateLetterCommand: generateLetter
    };
    return vm;
});