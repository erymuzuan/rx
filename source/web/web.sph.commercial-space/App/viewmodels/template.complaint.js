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


define(['services/datacontext', 'durandal/system', './template.base', 'services/jsonimportexport', 'services/logger'],
    function (context, system, templateBase, eximp, logger) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {


                var customElements = [];
                
                var address = new bespoke.sphcommercialspace.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");
                customElements.push(address);

                var cat = new bespoke.sphcommercialspace.domain.ComplaintCategoryElement(system.guid());
                cat.CssClass("icon-globe pull-left");
                cat.Name("Category");
                customElements.push(cat);

                templateBase.activate(customElements);


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("ComplaintTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("ComplaintTemplate", query)
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
                    vm.template(new bespoke.sphcommercialspace.domain.ComplaintTemplate());

                    vm.template().FormDesign().Name("My form 1");
                    vm.template().FormDesign().Description("Do whatever it takes");

                    templateBase.designer(vm.template().FormDesign());
                    return true;
                }


            },
            viewAttached = function(view) {
                $("#imageStoreId").kendoUpload({
                    async: {
                        saveUrl: "/BinaryStore/Upload",
                        removeUrl: "/BinaryStore/Remove",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function (e) {
                    },
                    success: function (e) {
                        var storeId = e.response.storeId;
                        var uploaded = e.operation === "upload";
                        var removed = e.operation != "upload";
                        // NOTE : the input file name is "files" and the id should equal to the vm.propertyName
                        if (uploaded) {
                            vm.template().FormDesign().ImageStoreId(storeId);
                        }

                        if (removed) {
                            vm.template().FormDesign().ImageStoreId("");
                        }


                    }
                });
                templateBase.viewAttached(view);
            },
            addComplaintCategory = function () {
                var category = new bespoke.sphcommercialspace.domain.ComplaintCategory();
                vm.template().ComplaintCategoryCollection.push(category);
            },
            addSubCategory = function () {
                vm.subCategoryOptions.push({ text: ko.observable() });
            },

            removeSubCategory = function (sub) {
                vm.subCategoryOptions.remove(sub);
            },

            updateCategory = function () {
                vm.template().ComplaintCategoryCollection.push(vm.selectedComplaintCategory());
            },
            removeCategory = function (category) {
                vm.template().ComplaintCategoryCollection.remove(category);
            },
             editCategory = function (category) {
                 vm.selectedComplaintCategory(category);
                 var subs = _(category.SubCategoryCollection()).map(function (s) {
                     return { text: ko.observable(s) };
                 });
                 vm.subCategoryOptions(subs);
                 $('#category-details-modal').modal({});
             },

         saveSubCategory = function () {
             var subs = (vm.subCategoryOptions()).map(function (s) {
                 return s.text();
             });
             vm.selectedComplaintCategory().SubCategoryCollection(subs);
         },
            save = function () {
                var tcs = new $.Deferred();

                // get the sorted element
                var elements = _($('#template-form-designer>form>div')).map(function (div) {
                    return ko.dataFor(div);
                });
                vm.template().FormDesign().FormElementCollection(elements);
                var data = ko.mapping.toJSON(vm.template);

                context.post(data, "/Template/SaveComplaintTemplate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                        vm.template().ComplaintTemplateId(result);
                    });
                return tcs.promise();
            },
            
        exportTemplate = function () {
            return eximp.exportJson("template.complaint." + vm.template().ComplaintTemplateId() + ".json", ko.mapping.toJSON(vm.template));
        },

        importTemplateJson = function () {
            return eximp.importJson()
                .done(function (json) {
                    try {
                        vm.template(ko.mapping.fromJSON(json));
                        vm.template().ComplaintTemplateId(0);
                    } catch (error) {
                        logger.logError('Fail template import tidak sah', error, this, true);
                    }
                });
        };
        
        var vm = {
            activate: activate,
            viewAttached: viewAttached,
            subCategoryOptions: ko.observableArray(),
            template: ko.observable(new bespoke.sphcommercialspace.domain.ComplaintTemplate()),
            selectedComplaintCategory: ko.observable(new bespoke.sphcommercialspace.domain.ComplaintCategory()),
            addComplaintCategory: addComplaintCategory,
            removeCategory: removeCategory,
            addSubCategory: addSubCategory,
            removeSubCategory: removeSubCategory,
            updateCategoryCommand: updateCategory,
            editCategory: editCategory,
            saveSubCategoryCommand: saveSubCategory,
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
