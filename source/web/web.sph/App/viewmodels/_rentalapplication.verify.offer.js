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
/// <reference path="../objectbuilders.js" />

define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router], function (context, logger, router) {

    var isBusy = ko.observable(false),
        activate = function (r) {
            vm.rentalapplication(r);
        },

        generateOfferLetter = function () {
            var url = '/#/offerdetails/' + vm.rentalapplication().RentalApplicationId() + '/' + vm.rentalapplication().SpaceId();
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
        complete = function () {
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
        createTenant = function () {
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
        unsucces = function () {
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
        rentalapplication: ko.observable(new bespoke.sph.domain.RentalApplication()),
        toolbar: ko.observable({
            commands: ko.observableArray([
                {
                    caption: '1. Sediakan Tawaran',
                    icon: "icon-file-text ",
                    command: generateOfferLetter,
                    status: 'Diluluskan'
                },
                {
                    caption: '2. Sediakan Kontrak',
                    icon: "icon-file-text ",
                    command: function () {
                        router.navigateTo("/#/contract.create/" + vm.rentalapplication().RentalApplicationId());
                    },
                    status: 'Diluluskan'
                },
                {
                    caption: '3. Selesai',
                    icon: "icon-file-text ",
                    command: complete,
                    status: 'Diluluskan'
                },
                {
                    caption: 'Cetak Surat Pembatalan',
                    icon: "icon-file-text ",
                    command: generateDeclinedOfferLetter,
                    status: 'Ditolak'
                },
                {
                    caption: 'Sediakan Maklumat Penyewa',
                    icon: "icon-file-text ",
                    command: createTenant,
                    status: 'Selesai'
                },
                {
                    caption: 'Tidak Berjaya',
                    icon: "icon-file-text ",
                    command: unsucces,
                    status: 'Menunggu'
                }
            ])
        })
    };

    return vm;
});