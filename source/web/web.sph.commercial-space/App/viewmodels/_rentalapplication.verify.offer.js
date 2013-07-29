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

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger, router) {

    var id = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (r) {
          vm.rentalapplication(r);
        },

        generateOfferLetter = function () {
            var url = '/#/offerdetails/' + vm.rentalapplication().RentalApplicationId() + '/' + vm.rentalapplication().CommercialSpaceId();
            router.navigateTo(url);
        },
        generateDeclinedOfferLetter = function () {
             var tcs = new $.Deferred();
             var data = JSON.stringify({ id: vm.rentalapplication().RentalApplicationId() });
             context.post(data, "/RentalApplication/GenerateDeclinedLetter").done(function (e) {
                 logger.log("Declined letter generated ", e, "rentalapplication.verify", true);
                 window.open("/RentalApplication/Download");
                 tcs.resolve(true);
             });
             return tcs.promise();
         },
        complete = function() {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: vm.rentalapplication().RentalApplicationId() });
            context.post(data, "/RentalApplication/Complete").done(function (e) {
                logger.log("Permohonan selesai ", e, "rentalapplication.verify", true);
                tcs.resolve(true);
                var url = '/#/admindashboard';
                router.navigateTo(url);
            });
            return tcs.promise();
        },
        createTenant = function() {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: vm.rentalapplication().RentalApplicationId() });
            context.post(data, "/Tenant/Create").done(function (e) {
                logger.log("Penyewa dijana ", e, "rentalapplication.verify", true);
                tcs.resolve(true);
                var url = '/#/admindashboard';
                router.navigateTo(url);
            });
            return tcs.promise();
        },
        unsucces = function() {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: vm.rentalapplication().RentalApplicationId() });
            context.post(data, "/RentalApplication/Unsuccess").done(function (e) {
               tcs.resolve(true);
            });
            return tcs.promise();
        };

    var vm = {
        isBusy: isBusy,
        activate: activate,
        rentalapplication: ko.observable(new bespoke.sphcommercialspace.domain.RentalApplication()),
        toolbar : ko.observable({
            commands: ko.observableArray([
                {
                    caption: '1. Sediakan Tawaran',
                    icon: "icon-file-text ",
                    command: generateOfferLetter,
                    status: 'Approved'
                },
                {
                    caption: '2. Sediakan Kontrak',
                    icon: "icon-file-text ",
                    command: function() {
                        router.navigateTo("/#/contract.create/" + vm.rentalapplication().RentalApplicationId());
                    },
                    status: 'Approved'
                },
                {
                    caption: '3. Selesai',
                    icon: "icon-file-text ",
                    command: complete,
                    status: 'Approved'
                },
                {
                    caption: 'Cetak Surat Pembatalan',
                    icon: "icon-file-text ",
                    command: generateDeclinedOfferLetter,
                    status: 'Declined'
                },
                {
                    caption: 'Sediakan Maklumat Penyewa',
                    icon: "icon-file-text ",
                    command: createTenant,
                    status: 'Completed'
                },
                {
                    caption: 'Tidak Berjaya',
                    icon: "icon-file-text ",
                    command: unsucces,
                    status: 'Waiting'
                }
            ])
        })
    };

    return vm;
});