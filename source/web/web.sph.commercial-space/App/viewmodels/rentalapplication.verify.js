/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'viewmodels/_rentalapplication.verify.offer'], function (context, logger, router, offervm) {

    var id = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routedata) {
            id(routedata.applicationId);
            vm.remark('');

            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + id())
                .then(function (r) {

                    offervm.activate(r);
                    vm.rentalapplication(r);
                    context.loadOneAsync("CommercialSpace", "CommercialSpaceId eq " + vm.rentalapplication().CommercialSpaceId())
                        .then(function (b) {
                            vm.commercialSpace(b);
                            tcs.resolve(true);
                        });

                    $.get("/Map/CommercialSpaceImage/" + vm.rentalapplication().CommercialSpaceId() + "?width=300&height=200")
                        .then(function (b) {
                            vm.commercialSpace().StaticMap(b);
                        });


                });
            return tcs.promise();
        },
        
        addAttachment = function () {
            var attachment = new bespoke.sphcommercialspace.domain.Attachment();
            vm.rentalapplication().AttachmentCollection.push(attachment);
        },
        
        showAuditTrail = function () {
            isBusy(true);
            var query = "EntityId eq " + vm.rentalapplication().RentalApplicationId();
            vm.auditTrailCollection.removeAll();

            context.loadAsync("AuditTrail", query)
                .then(function (lo) {
                    vm.auditTrailCollection(lo.itemCollection);
                    isBusy(false);
                });
            $('#audit-trail').modal({});

        },
        
        showDetails = function () {
            $('#details-panel').modal({});
        },
        
        waitingList = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/WaitingList").done(function (e) {
                logger.log("Application has been insert into waiting list ", e, "rentalapplication.verify", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        
        returned = function () {
            var tcs = new $.Deferred();
            var attachments = ko.mapping.toJS(vm.rentalapplication().AttachmentCollection);
            var data = JSON.stringify({ id: id(), attachments: attachments });
            context.post(data, "/RentalApplication/Returned").done(function (e) {
                var url = '/#/returnedapplication/' + e;
                router.navigateTo(url);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        
        decline = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/Declined").done(function (e) {
                logger.log("Application has been declined ", e, "rentalapplication.verify", true);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        
        approve = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/Approved").done(function (r) {
                logger.log(r.message, r.message, "rentalapplication.verify", true);
                if (r.result) {
                     vm.rentalapplication().Status('Approved');
                }
               
                tcs.resolve(true);
            });
            return tcs.promise();
        }

    ;

    var vm = {
        isBusy: isBusy,
        activate: activate,
        auditTrailCollection: ko.observableArray([]),
        rentalapplication: ko.observable(new bespoke.sphcommercialspace.domain.RentalApplication()),
        commercialSpace: ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpace()),
        waitingListCommand: waitingList,
        returnedCommand: returned,
        declinedCommand: decline,
        showAuditTrailCommand: showAuditTrail,
        showDetailsCommand: showDetails,
        approvedCommand: approve,
        addAttachmentCommand: addAttachment,
        remark: ko.observable('')
    };

    return vm;
});