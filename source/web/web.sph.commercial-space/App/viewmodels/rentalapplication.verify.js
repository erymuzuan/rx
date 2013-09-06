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
        app = ko.observable(),
        isBusy = ko.observable(false),
        activate = function (routedata) {
            app(routedata);
            id(routedata.applicationId);
            vm.title('Perincian Borang Permohonan');
            vm.toolbar.auditTrail.id(routedata.applicationId);
            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + id())
                .then(function (r) {
                    vm.rentalapplication(r);

                    lulusPermohonanCommandStatus(r.Status() == "Menunggu" || r.Status() == "Dikembalikan");
                    baruCommandStatus(r.Status() == "Baru");
                    dibatalkanCommandStatus(r.Status() == "Baru" || r.Status() == "Dikembalikan");
                    diluluskanCommandStatus(r.Status() == "Diluluskan");
                    ditolakCommandStatus(r.Status() == "Ditolak");
                    selesaiCommandStatus(r.Status() == "Selesai");
                    menungguCommandStatus(r.Status() == "Menunggu");

                    context.loadOneAsync("CommercialSpace", "CommercialSpaceId eq " + vm.rentalapplication().CommercialSpaceId())
                        .then(function (b) {
                            vm.commercialSpace(b);
                            tcs.resolve(true);
                        });

                  
                });

            return tcs.promise();
        },

        addAttachment = function () {
            var attachment = new bespoke.sphcommercialspace.domain.Attachment();
            vm.rentalapplication().AttachmentCollection.push(attachment);
        },

        removeAttachment = function (doc) {
            vm.rentalapplication().AttachmentCollection.remove(doc);
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
                var url = '/#/admindashboard';
                router.navigateTo(url);
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

        update = function () {
            var tcs = new $.Deferred();
            var attachments = ko.mapping.toJS(vm.rentalapplication().AttachmentCollection);
            var note = ko.mapping.toJS(vm.rentalapplication().Remarks());
            var data = JSON.stringify({ id: id(), attachments: attachments, note: note });
            context.post(data, "/RentalApplication/Update").done(function () {
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
                var url = '/#/admindashboard';
                router.navigateTo(url);
            });
            return tcs.promise();
        },

        approve = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id() });
            context.post(data, "/RentalApplication/Approved").done(function (r) {
                logger.log(r.message, r.message, "rentalapplication.verify", true);
                if (r.result) {
                    vm.rentalapplication().Status('Diluluskan');
                }
                tcs.resolve(true);
                if (result) {
                    var url = '/#/admindashboard';
                    router.navigateTo(url);
                }

            });
            return tcs.promise();
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
        },

        printList = function () { },

        exportList = function () { },

        lulusPermohonanCommandStatus = ko.observable(),
        baruCommandStatus = ko.observable(),
        dibatalkanCommandStatus = ko.observable(),
        diluluskanCommandStatus = ko.observable(),
        ditolakCommandStatus = ko.observable(),
        selesaiCommandStatus = ko.observable(),
        menungguCommandStatus = ko.observable()

    ;

    var vm = {
        isBusy: isBusy,
        activate: activate,
        title: ko.observable(),
        rentalapplication: ko.observable(new bespoke.sphcommercialspace.domain.RentalApplication()),
        commercialSpace: ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpace()),
        showDetailsCommand: showDetails,
        addAttachmentCommand: addAttachment,
        removeAttachmentCommand: removeAttachment,
        toolbar: {
            reloadCommand: function () {
                return activate(app());
            },
            auditTrail: {
                entity: "RentalApplication",
                id: ko.observable()
            },
            printCommand: printList,
            exportCommand: exportList,
            commands: ko.observableArray([
                {
                    caption: 'Kemaskini',
                    icon: 'icon-eraser',
                    command: update,
                },
                {
                    caption: 'Masuk Senarai Menunggu',
                    icon: 'icon-reorder',
                    command: waitingList,
                    visible: baruCommandStatus
                },
                {
                    caption: 'Kembalikan',
                    icon: 'icon-reply',
                    command: returned,
                    visible: baruCommandStatus
                },
                {
                    caption: 'Batalkan',
                    icon: 'icon-remove-sign',
                    command: decline,
                    visible: dibatalkanCommandStatus
                },
                {
                    caption: 'Luluskan',
                    icon: 'icon-ok',
                    command: approve,
                    visible: lulusPermohonanCommandStatus
                },
                {

                    caption: '1. Sediakan Tawaran',
                    icon: 'icon-file-text',
                    command: function () {
                        router.navigateTo('/#/offerdetails/' + vm.rentalapplication().RentalApplicationId() + '/' + vm.rentalapplication().CommercialSpaceId());
                        return {
                            then: function () { }
                        };
                    },
                    visible: diluluskanCommandStatus
                },
                {
                    caption: '2. Sediakan Kontrak',
                    icon: 'icon-file-text',
                    command: function () {
                        router.navigateTo("/#/contract.create/" + vm.rentalapplication().RentalApplicationId());
                        return {
                            then: function () { }
                        };
                    },
                    visible: diluluskanCommandStatus
                },
                {
                    caption: '3. Selesai',
                    icon: 'icon-file-text',
                    command: complete,
                    visible: diluluskanCommandStatus
                },
                {
                    caption: 'Cetak Surat Pembatalan',
                    icon: 'icon-file-text',
                    command: generateDeclinedOfferLetter,
                    visible: ditolakCommandStatus
                },
                {
                    caption: 'Sediakan Maklumat Penyewa',
                    icon: 'icon-file-text',
                    command: createTenant,
                    visible: selesaiCommandStatus
                },
                {
                    caption: 'Tidak Berjaya',
                    icon: 'icon-file-text',
                    command: unsucces,
                    visible: menungguCommandStatus
                }

            ])
        }
    };

    return vm;
});