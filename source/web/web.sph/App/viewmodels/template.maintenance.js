/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.3.js" />

define([objectbuilders.datacontext, 'durandal/system', './template.base', 'services/jsonimportexport', objectbuilders.logger, objectbuilders.cultures],
    function (context, system, templateBase, eximp, logger, culture) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {

                var customElements = [];
                var address = new bespoke.sph.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");
                customElements.push(address);

                var officers = new bespoke.sph.domain.MaintenanceOfficerElement(system.guid());
                officers.CssClass("icon-user pull-left");
                officers.Name("Officers");
                customElements.push(officers);

                templateBase.activate(customElements);


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("MaintenanceTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("MaintenanceTemplate", query)
                        .done(function (b) {
                            _(b.FormDesign().FormElementCollection()).each(function (fe) {
                                // add isSelected for the designer
                                fe.isSelected = ko.observable(false);
                            });
                            vm.template(b);
                            templateBase.designer(vm.template().FormDesign());
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.template(new bespoke.sph.domain.MaintenanceTemplate());

                    vm.template().FormDesign().Name("My form 1");
                    vm.template().FormDesign().Description("Do whatever it takes");

                    templateBase.designer(vm.template().FormDesign());
                    return true;
                }
            },
            save = function () {
                var tcs = new $.Deferred();

                // get the sorted element
                var elements = _($('#template-form-designer>form>div')).map(function (div) {
                    return ko.dataFor(div);
                });
                vm.template().FormDesign.FormElementCollection(elements);
                var data = ko.mapping.toJSON(vm.template());

                context.post(data, "/Template/SaveMaintenanceTemplate")
                    .then(function (result) {
                        isBusy(false);
                        logger.info(culture.maintenance.SAVE_TEMPLATE);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },

        exportTemplate = function () {
            return eximp.exportJson("template.maintenance." + vm.template().MaintenanceTemplateId() + ".json", ko.mapping.toJSON(vm.template));
        },

        importTemplateJson = function () {
            return eximp.importJson()
                .done(function (json) {
                    try {

                        var obj = JSON.parse(json),
                            clone = context.toObservable(obj);
                        vm.template(clone);
                        vm.template().MaintenanceTemplateId(0);
                    } catch (error) {
                        logger.logError('Fail template import tidak sah', error, this, true);
                    }
                });
        };

        var vm = {
            activate: activate,
            viewAttached: templateBase.viewAttached,
            template: ko.observable(new bespoke.sph.domain.MaintenanceTemplate()),
            toolbar: {
                saveCommand: save,
                exportCommand: exportTemplate,
                importCommand: importTemplateJson
            },
            customFormElements: templateBase.customFormElements,
            formElements: templateBase.formElements,
            selectFormElement: templateBase.selectFormElement,
            selectedFormElement: templateBase.selectedFormElement,
            removeFormElement: templateBase.removeFormElement,
            removeComboBoxOption: templateBase.removeComboBoxOption,
            selectPathFromPicker: templateBase.selectPathFromPicker,
            showPathPicker: templateBase.showPathPicker,
            addComboBoxOption: templateBase.addComboBoxOption
        };

        return vm;

    });
