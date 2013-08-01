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
            vm.toolbar().auditTrail.id(routedata.applicationId);
            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + id())
                .then(function (r) {
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
                var url = '/#/rentalapplication.verify/' + id();
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

        exportList = function () { }

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
        toolbar: ko.observable({
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
                    icon: "icon-eraser ",
                    command: update,
                    status: 'none'
                },
                {
                    caption: 'Masuk Senarai Menunggu',
                    icon: "icon-reorder ",
                    command: waitingList,
                    status: 'rentalapplication().Status() == "Baru"'
                },
                {
                    caption: 'Kembalikan',
                    icon: "icon-reply ",
                    command: returned,
                    status: 'rentalapplication().Status() == "Baru"'
                },
                {
                    caption: 'Batalkan',
                    icon: "icon-remove-sign ",
                    command: decline,
                    status: 'rentalapplication().Status() == "Baru" || rentalapplication().Status() == "Dikembalikan"'
                },
                {
                    caption: 'Luluskan',
                    icon: "icon-ok ",
                    command: approve,
                    status: 'rentalapplication().Status() == "Menunggu" || rentalapplication().Status() == "Dikembalikan"'
                },
                {
                    caption: '1. Sediakan Tawaran',
                    icon: "icon-file-text ",
                    command: generateOfferLetter,
                    status: 'rentalapplication().Status() == "Diluluskan"'
                },
                {
                    caption: '2. Sediakan Kontrak',
                    icon: "icon-file-text ",
                    command: function () {
                        router.navigateTo("/#/contract.create/" + vm.rentalapplication().RentalApplicationId());
                    },
                    status: 'rentalapplication().Status() == "Diluluskan"'
                },
                {
                    caption: '3. Selesai',
                    icon: "icon-file-text ",
                    command: complete,
                    status: 'rentalapplication().Status() == "Diluluskan"'
                },
                {
                    caption: 'Cetak Surat Pembatalan',
                    icon: "icon-file-text ",
                    command: generateDeclinedOfferLetter,
                    status: 'rentalapplication().Status() == "Ditolak"'
                },
                {
                    caption: 'Sediakan Maklumat Penyewa',
                    icon: "icon-file-text ",
                    command: createTenant,
                    status: 'rentalapplication().Status() == "Selesai"'
                },
                {
                    caption: 'Tidak Berjaya',
                    icon: "icon-file-text ",
                    command: unsucces,
                    status: 'rentalapplication().Status() == "Menunggu"'
                }

            ])
        })
    };

    return vm;
});