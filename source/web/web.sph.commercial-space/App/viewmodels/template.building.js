﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.3.js" />


define(['services/datacontext', 'durandal/system', './template.base', 'services/jsonimportexport'],
    function (context, system, templateBase, eximp) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {


                var customElements = [];
                var address = new bespoke.sphcommercialspace.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");
                customElements.push(address);

                var map = new bespoke.sphcommercialspace.domain.BuildingMapElement();
                map.CssClass("icon-globe pull-left");
                map.Name("Show map button");
                map.Label("Show map");
                customElements.push(map);

                var floorsElement = new bespoke.sphcommercialspace.domain.BuildingFloorsElement();
                floorsElement.CssClass("icon-table pull-left");
                floorsElement.Name("Floors Table");
                customElements.push(floorsElement);

                templateBase.activate(customElements);


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("BuildingTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("BuildingTemplate", query)
                        .done(function (b) {
                            var fd = b.FormDesign;
                            b.FormDesign = ko.observable(fd);
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
                    vm.template(new bespoke.sphcommercialspace.domain.BuildingTemplate());

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
                vm.template().FormDesign().FormElementCollection(elements);
                var data = ko.mapping.toJSON(vm.template);

                context.post(data, "/Template/SaveBuildingTemplate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            
            exportTemplate = function () {
                return eximp.exportJson("template.building." + vm.template().BuildingTemplateId() + ".json", ko.mapping.toJSON(vm.template));
            };

        var vm = {
            activate: activate,
            viewAttached: templateBase.viewAttached,
            template: ko.observable(new bespoke.sphcommercialspace.domain.BuildingTemplate()),
            toolbar: {
                saveCommand: save,
                exportCommand: exportTemplate
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
