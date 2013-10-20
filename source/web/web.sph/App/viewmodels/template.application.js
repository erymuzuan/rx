/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.3.js" />


define(['services/datacontext', 'durandal/system', './template.base', 'services/jsonimportexport', 'services/logger'],
    function (context, system, templateBase, eximp, logger) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {


                var customElements = [];
                
                var address = new bespoke.sph.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");
                customElements.push(address);
                
                var banks = new bespoke.sph.domain.RentalApplicationBanksElement(system.guid());
                banks.CssClass("icon-table pull-left");
                banks.Name("Banks");
                customElements.push(banks);
                
                var docs = new bespoke.sph.domain.RentalApplicationAttachmentsElement(system.guid());
                docs.CssClass("icon-calendar pull-left");
                docs.Name("Documents");
                customElements.push(docs);

                var contact = new bespoke.sph.domain.RentalApplicationContactElement(system.guid());
                contact.CssClass("icon-user pull-left");
                contact.Name("Contact");
                customElements.push(contact);

                templateBase.activate(customElements);


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("ApplicationTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("ApplicationTemplate", query)
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
                    vm.template(new bespoke.sph.domain.ApplicationTemplate());

                    vm.template().FormDesign().Name("My form 1");
                    vm.template().FormDesign().Description("Do whatever it takes");

                    templateBase.designer(vm.template().FormDesign());
                    return true;
                }


            },
            viewAttached = function(view) {
               
                templateBase.viewAttached(view);
            },
           

            save = function () {
                var tcs = new $.Deferred();

                // get the sorted element
                var elements = _($('#template-form-designer>form>div')).map(function (div) {
                    return ko.dataFor(div);
                });
             
                vm.template().FormDesign().FormElementCollection(elements);
                var data = ko.mapping.toJSON(vm.template());

                context.post(data, "/Template/SaveApplicationTemplate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                        vm.template().ApplicationTemplateId(result);
                        logger.info("Your template has been saved");
                    });
                return tcs.promise();
            },

        exportTemplate = function () {
            return eximp.exportJson("template.application." + vm.template().ApplicationTemplateId() + ".json", ko.mapping.toJSON(vm.template()));
        },

        importTemplateJson = function () {
             return eximp.importJson()
                 .done(function (json) {
                     try {
                         vm.template(ko.mapping.fromJSON(json));
                         vm.template().ApplicationTemplateId(0);
                     } catch(error) {
                         logger.logError('Fail template import tidak sah', error, this, true);
                     }
                 });
         };
        
        var vm = {
            activate: activate,
            viewAttached: viewAttached,
            subCategoryOptions: ko.observableArray(),
            template: ko.observable(new bespoke.sph.domain.ApplicationTemplate()),
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
            addComboBoxOption: templateBase.addComboBoxOption,
            selectPathFromPicker: templateBase.selectPathFromPicker,
            showPathPicker: templateBase.showPathPicker,
            imageStoreId : ko.observable()
        };

        return vm;

    });
