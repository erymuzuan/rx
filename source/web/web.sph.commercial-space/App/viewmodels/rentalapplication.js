﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/system'], function (context, logger, router, system) {

    var isBusy = ko.observable(false),
        id = ko.observable(),
        registrationNo = ko.observable(),
        activate = function (routeData) {
            
            id(routeData.id);
            var tcs = new $.Deferred();
            var csTask = context.loadOneAsync('CommercialSpace', 'CommercialSpaceId eq ' + id());
            var stateTask =  context.loadOneAsync("Setting", "Key eq 'State'");
            $.when(csTask, stateTask).done(function (cs, s) {
                var states = JSON.parse(ko.mapping.toJS(s.Value));
                vm.stateOptions(states);
                vm.commercialSpace(cs);
                tcs.resolve(true);
            });
            vm.rentalapplication().CommercialSpaceId(routeData.id);
            var bank =new bespoke.sphcommercialspace.domain.Bank(system.guid());
            vm.rentalapplication().BankCollection.push(bank);
            return tcs.promise();
        },
        viewAttached = function () {
            
        },
        configureUpload = function (element, index, attachment) {
            
            $(element).find("input[type=file]").kendoUpload({
                async: {
                    saveUrl: "/BinaryStore/Upload",
                    removeUrl: "/BinaryStore/Remove",
                    autoUpload: true
                },
                multiple: false,
                error: function (e) {
                    logger.logError(e, e, this, true);
                },
                success: function (e) {
                    logger.log('Your file has been uploaded', e, "route/create", true);
                    attachment.StoreId(e.response.storeId);
                    attachment.IsReceived(e.operation === "upload");

                }
            });
        },
        
        saveApplication = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.rentalapplication());
            isBusy(true);
            context.post(data, "/RentalApplication/Submit").done(function (e) {
                logger.log("Data has been successfully saved ", e, "rentalapplication", true);
                isBusy(false);
                registrationNo(e.registrationNo);
                vm.rentalapplication(new bespoke.sphcommercialspace.domain.RentalApplication());
                $('#success-panel').modal({})
                    .on('hidden', function() {
                        router.navigateTo('/#/');
                    });
                tcs.resolve(e.status);
            });
            return tcs.promise();
        },
        addBankCollection = function () {
            var bank = new bespoke.sphcommercialspace.domain.Bank(system.guid());
            vm.rentalapplication().BankCollection.push(bank);
        },
        addAttachment = function () {
            var attachment = new bespoke.sphcommercialspace.domain.Attachment(system.guid());
            vm.rentalapplication().AttachmentCollection.push(attachment);
        };

    var vm = {
        activate: activate,
        registrationNo: registrationNo,
        viewAttached: viewAttached,
        configureUpload: configureUpload,
        stateOptions: ko.observableArray(),
        rentalapplication: ko.observable(new bespoke.sphcommercialspace.domain.RentalApplication()),
        commercialSpace : ko.observable (new bespoke.sphcommercialspace.domain.CommercialSpace()),
        saveCommand: saveApplication,
        addBankCommand: addBankCollection,
        isBusy: isBusy,
        addAttachmentCommand: addAttachment
    };

    return vm;

});