/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
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
        rentalApplication = ko.observable(new bespoke.sph.domain.RentalApplication()),

        activate = function (routeData) {

            id(parseInt(routeData.id));
            var tcs = new $.Deferred();
            var csTask = context.loadOneAsync('Space', 'SpaceId eq ' + id());
            var stateTask = context.loadOneAsync("Setting", "Key eq 'State'");
            $.when(csTask, stateTask).done(function (cs, s) {
                var states = JSON.parse(ko.mapping.toJS(s.Value));
                vm.stateOptions(states);
                vm.space(cs);
                tcs.resolve(true);
            });
            vm.rentalapplication().SpaceId(routeData.id);
            if (!id) {

                // build custom fields value
                context.loadOneAsync("ApplicationTemplate", "ApplicationTemplateId eq 1" )
                    .done(function(template) {
                        var cfs = _(template.CustomFieldCollection()).map(function(f) {
                            var webid = system.guid();
                            var v = new bespoke.sph.domain.CustomFieldValue(webid);
                            v.Name(f.Name());
                            v.Type(f.Type());
                            return v;
                        });

                        vm.rentalapplication().CustomFieldValueCollection(cfs);

                    });

                vm.rentalapplication().TemplateId(1);
            }
            var bank = new bespoke.sph.domain.Bank(system.guid());
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
            context.post(data, "/RentalApplication/Submit")
                .done(function (e) {
                logger.log("Data has been successfully saved ", e, "rentalapplication", true);
                isBusy(false);
                registrationNo(e.registrationNo);
                vm.rentalapplication(new bespoke.sph.domain.RentalApplication());
                $('#success-panel').modal({})
                    .on('hidden', function () {
                        router.navigateTo('/#/');
                    });
                tcs.resolve(e.status);
            });
            return tcs.promise();
        },
        addBankCollection = function () {
            var bank = new bespoke.sph.domain.Bank(system.guid());
            vm.rentalapplication().BankCollection.push(bank);
        },
        addAttachment = function () {
            var attachment = new bespoke.sph.domain.Attachment(system.guid());
            vm.rentalapplication().AttachmentCollection.push(attachment);
        },
        removeAttachement = function (attachment) {
            vm.rentalapplication().AttachmentCollection.remove(attachment);
        }
    ;

    var vm = {
        activate: activate,
        registrationNo: registrationNo,
        viewAttached: viewAttached,
        configureUpload: configureUpload,
        stateOptions: ko.observableArray(),
        rentalapplication: ko.observable(new bespoke.sph.domain.RentalApplication()),
        space: ko.observable(new bespoke.sph.domain.Space()),
        toolbar: {
            reloadCommand: function () {
                return activate({ status: status() });
            },
            printCommand: ko.observable({
                entity: ko.observable("RentalApplication"),
                id: ko.observable(0),
                item: rentalApplication,
            }),
            commands: ko.observableArray([{
                caption: "Hantar Permohonan",
                icon: 'icon-envelop',
                status: 'none',
                command: saveApplication
            }])
        },
        addBankCommand: addBankCollection,
        isBusy: isBusy,
        addAttachmentCommand: addAttachment,
        removeAttachmentCommand: removeAttachement
    };

    return vm;

});