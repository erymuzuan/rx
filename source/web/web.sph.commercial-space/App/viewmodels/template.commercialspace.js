/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'durandal/system', './template.base', 'services/jsonimportexport', 'services/logger', 'config'],
    function (context, system, designerHost, eximp, logger, config) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {


                var customElements = [];
                
                var address = new bespoke.sphcommercialspace.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");
                customElements.push(address);
                
                var building = new bespoke.sphcommercialspace.domain.BuildingElement(system.guid());
                building.CssClass("icon-envelope pull-left");
                building.Name("Building");
                customElements.push(building);

                var lot = new bespoke.sphcommercialspace.domain.CommercialSpaceLotsElement(system.guid());
                lot.CssClass("icon-envelope pull-left");
                lot.Name("Lot");
                customElements.push(lot);



                designerHost.activate(customElements);


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("CommercialSpaceTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("CommercialSpaceTemplate", query)
                        .done(function (b) {
                            
                            _(b.FormDesign().FormElementCollection()).each(function (fe) {
                                // add isSelected for the designer
                                fe.isSelected = ko.observable(false);
                            });
                            vm.template(b);
                            designerHost.designer(vm.template().FormDesign());
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.template(new bespoke.sphcommercialspace.domain.CommercialSpaceTemplate());

                    vm.template().FormDesign().Name("My form 1");
                    vm.template().FormDesign().Description("Do whatever it takes");

                    designerHost.designer(vm.template().FormDesign());
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

                context.post(data, "/Template/SaveCommercialSpaceTemplate")
                    .then(function (result) {
                        isBusy(false);
                        logger.info("Data has been successfully save");
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            
        exportTemplate = function () {
            return eximp.exportJson("template.commercialspace." + vm.template().CommercialSpaceTemplateId() + ".json", ko.mapping.toJSON(vm.template));
        },

        importTemplateJson = function () {
             return eximp.importJson()
                 .done(function (json) {
                     try {
                         vm.template(ko.mapping.fromJSON(json));
                         if (typeof vm.template().FormDesign === "object") {
                             vm.template().FormDesign = ko.observable(vm.template().FormDesign);
                         }

                         vm.template().CommercialSpaceTemplateId(0);
                     } catch(error) {
                         logger.logError('Fail template import tidak sah', error, this, true);
                     }
                 });
         };

        var vm = {
            activate: activate,
            template: ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpaceTemplate()),
            toolbar: {
                saveCommand: save,
                exportCommand: exportTemplate,
                importCommand: importTemplateJson
            },

            viewAttached: designerHost.viewAttached,
            customFormElements: designerHost.customFormElements,
            formElements: designerHost.formElements,
            selectFormElement: designerHost.selectFormElement,
            selectedFormElement: designerHost.selectedFormElement,
            removeFormElement: designerHost.removeFormElement,
            removeComboBoxOption: designerHost.removeComboBoxOption,
            selectPathFromPicker: designerHost.selectPathFromPicker,
            showPathPicker: designerHost.showPathPicker,
            addComboBoxOption: designerHost.addComboBoxOption,
            config : config
        };

        return vm;

    });
