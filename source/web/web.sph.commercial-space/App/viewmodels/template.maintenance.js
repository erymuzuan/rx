/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.3.js" />


define(['services/datacontext', 'durandal/system', './template.base', 'services/logger'],
    function (context, system, templateBase) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {


                var customElements = [];
                var address = new bespoke.sphcommercialspace.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");
                customElements.push(address);

                templateBase.activate(customElements);


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("MaintenanceTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("MaintenanceTemplate", query)
                        .done(function (b) {
                            var fd = b.FormDesign;
                            b.FormDesign = ko.observable(fd);
                            _(b.FormDesign().FormElementCollection()).each(function (fe) {
                                // add isSelected for the designer
                                fe.isSelected = ko.observable(false);
                            });
                            vm.maintenanceTemplate(b);
                            templateBase.designer(vm.maintenanceTemplate().FormDesign());
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.maintenanceTemplate(new bespoke.sphcommercialspace.domain.MaintenanceTemplate());

                    vm.maintenanceTemplate().FormDesign().Name("My form 1");
                    vm.maintenanceTemplate().FormDesign().Description("Do whatever it takes");

                    templateBase.designer(vm.maintenanceTemplate().FormDesign());
                    return true;
                }
            },
            save = function () {
                var tcs = new $.Deferred();

                // get the sorted element
                var elements = _($('#template-form-designer>form>div')).map(function (div) {
                    return ko.dataFor(div);
                });
                vm.maintenanceTemplate().FormDesign().FormElementCollection(elements);
                var data = ko.mapping.toJSON(vm.maintenanceTemplate);

                context.post(data, "/Template/SaveMaintenanceTemplate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };
        
        var vm = {
            activate: activate,
            viewAttached: templateBase.viewAttached,
            maintenanceTemplate: ko.observable(new bespoke.sphcommercialspace.domain.MaintenanceTemplate()),
            toolbar: {
                saveCommand: save
            },
            customFormElements: templateBase.customFormElements,
            formElements: templateBase.formElements,
            selectFormElement: templateBase.selectFormElement,
            selectedFormElement: templateBase.selectedFormElement,
            removeFormElement: templateBase.removeFormElement,
            removeComboBoxOption: templateBase.removeComboBoxOption,
            addComboBoxOption: templateBase.addComboBoxOption
        };

        return vm;

    });
