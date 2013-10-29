/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../objectbuilders.js" />

define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, 'durandal/system', objectbuilders.config, objectbuilders.validation, objectbuilders.defaultValueProvider], function (context, logger, router, system, config, validation, defaultValueProvider) {

    var isBusy = ko.observable(false),
        spaceId = ko.observable(),
        mTemplate = ko.observable(),
        registrationNo = ko.observable(),
        rentalApplication = ko.observable(new bespoke.sph.domain.RentalApplication()),

        activate = function (routeData) {
            spaceId(parseInt(routeData.spaceId));
            var templateId = parseInt(routeData.templateId),
                tcs = new $.Deferred(),
                csTask = context.loadOneAsync('Space', 'SpaceId eq ' + spaceId()),
                stateTask = context.loadOneAsync("Setting", "Key eq 'State'");

            $.when(csTask, stateTask).done(function (cs, s) {
                var states = JSON.parse(ko.mapping.toJS(s.Value));
                vm.stateOptions(states);
                vm.space(cs);

                var categoryOptions = ko.observableArray();
                _.each(cs.FeatureDefinitionCollection(), function (t) {
                    categoryOptions.push(t.Category());
                });
                var distinctCategories = ko.observableArray();
                distinctCategories(_.uniq(categoryOptions()));
                var features = _(distinctCategories()).map(function (t) {
                    var filtered = _(cs.FeatureDefinitionCollection()).filter(function (c) {
                        return c.Category() === t;
                    });
                    return {
                        category: t,
                        features: filtered
                    };
                });
                vm.facilities(features);
                vm.totalRate(cs.RentalRate());
                tcs.resolve(true);
            });
            vm.rentalapplication().SpaceId(spaceId());


            // build custom fields value
            context.loadOneAsync("ApplicationTemplate", "ApplicationTemplateId eq " + templateId)
                .done(function (template) {
                    mTemplate(template);
                    defaultValueProvider.setDefaultValues(vm.rentalapplication(), template);
                    var cfs = _(template.CustomFieldCollection()).map(function (f) {
                        var webid = system.guid();
                        var v = new bespoke.sph.domain.CustomFieldValue(webid);
                        v.Name(f.Name());
                        v.Type(f.Type());
                        return v;
                    });

                    vm.rentalapplication().CustomFieldValueCollection(cfs);
                    vm.rentalapplication().TemplateName(template.Name());

                });
                  defaultValueProvider.setDefaultValues(vm.building(), template);
            vm.rentalapplication().TemplateId(templateId);

            return tcs.promise();
        },
        viewAttached = function () {
            validation.init($('#application-detail-form'), mTemplate());
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

            if (!validation.valid()) {
                return Task.fromResult(false);
            }
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.rentalapplication());
            isBusy(true);
            context.post(data, "/RentalApplication/Submit")
                .done(function (e) {
                    logger.log("Data has been successfully saved ", e, "rentalapplication", true);
                    isBusy(false);
                    registrationNo(e.registrationNo);
                    vm.rentalapplication(new bespoke.sph.domain.RentalApplication());
                    if (!config.isAuthenticated) {
                        $('#success-panel').modal({})
                            .on('hidden', function () {
                                router.navigateTo('/#/');
                            });
                    }

                    tcs.resolve(e.status);
                });
            return tcs.promise();
        },
        addAttachment = function () {
            var attachment = new bespoke.sph.domain.Attachment(system.guid());
            vm.rentalapplication().AttachmentCollection.push(attachment);
        },
        removeAttachement = function (attachment) {
            vm.rentalapplication().AttachmentCollection.remove(attachment);
        },
        addFeatureToList = function (fd) {
            var feature = new bespoke.sph.domain.Feature();
            feature.Name(fd.Name());
            feature.Occurence(fd.Occurence());
            feature.OccurenceTimeSpan(fd.OccurenceTimeSpan());
            feature.Quantity(fd.AvailableQuantity());
            feature.Charge(fd.AvailableQuantity() * fd.Charge());

            vm.rentalapplication().FeatureCollection.push(feature);
            var rentalRate = vm.space().RentalRate();
            var addedCharge = _(vm.rentalapplication().FeatureCollection()).reduce(function (memo, val) {
                return memo + parseFloat(val.Charge());
            }, 0);
            vm.totalRate(addedCharge + rentalRate);
        },

        removeFeatureFromList = function (feature) {
            vm.rentalapplication().FeatureCollection.remove(feature);
            var rentalRate = vm.totalRate();
            var reduceCharge = parseFloat(feature.Charge());
            vm.totalRate(rentalRate - reduceCharge);
        }
    ;

    var vm = {
        activate: activate,
        registrationNo: registrationNo,
        viewAttached: viewAttached,
        configureUpload: configureUpload,
        stateOptions: ko.observableArray(),
        facilities: ko.observableArray(),
        availableQuantities: ko.observableArray(),
        rentalapplication: ko.observable(new bespoke.sph.domain.RentalApplication()),
        space: ko.observable(new bespoke.sph.domain.Space()),
        totalRate: ko.observable(),
        toolbar: {
            printCommand: ko.observable({
                entity: ko.observable("RentalApplication"),
                id: ko.observable(0),
                item: rentalApplication,
            }),
            commands: ko.observableArray([{
                caption: "Hantar Permohonan",
                icon: 'icon-envelope',
                status: 'none',
                command: saveApplication
            }])
        },
        isBusy: isBusy,
        addAttachmentCommand: addAttachment,
        removeAttachmentCommand: removeAttachement,
        addFeatureToList: addFeatureToList,
        removeFeatureFromList: removeFeatureFromList
    };

    return vm;

});