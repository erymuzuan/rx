/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../../Scripts/_constants.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.3.js" />
/// <reference path="../objectbuilders.js" />


define([objectbuilders.datacontext, 'durandal/system', './template.base', 'services/jsonimportexport', objectbuilders.logger, 'durandal/app', objectbuilders.router,objectbuilders.cultures,objectbuilders.defaultValueProvider],
    function (context, system, templateBase, eximp, logger, app, router, culture, defaultValueProvider) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {


                var customElements = [];
                var address = new bespoke.sph.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");
                customElements.push(address);

                var map = new bespoke.sph.domain.BuildingMapElement();
                map.CssClass("icon-globe pull-left");
                map.Name("Show map button");
                map.Label("Show map");
                customElements.push(map);

                var floorsElement = new bespoke.sph.domain.BuildingFloorsElement();
                floorsElement.CssClass("icon-table pull-left");
                floorsElement.Name("Floors Table");
                customElements.push(floorsElement);
                
                var blocksElement = new bespoke.sph.domain.BuildingBlocksElement();
                blocksElement.CssClass("icon-building pull-left");
                blocksElement.Name("Blocks Table");
                customElements.push(blocksElement);

                templateBase.activate(customElements);


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("BuildingTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("BuildingTemplate", query)
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
                    vm.template(new bespoke.sph.domain.BuildingTemplate());

                    vm.template().FormDesign().Name("My form 1");
                    vm.template().FormDesign().Description("Do whatever it takes");

                    templateBase.designer(vm.template().FormDesign());
                    return true;
                }


            },
            viewAttached = function (view) {
                $("#imageStoreId").kendoUpload({
                    async: {
                        saveUrl: "/BinaryStore/Upload",
                        removeUrl: "/BinaryStore/Remove",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function () {
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
            save = function () {
                var tcs = new $.Deferred();

                // get the sorted element
                var elements = _($('#template-form-designer>form>div')).map(function (div) {
                    return ko.dataFor(div);
                });
                vm.template().FormDesign().FormElementCollection(elements);
                var data = ko.mapping.toJSON(vm.template());

                context.post(data, "/Template/SaveBuildingTemplate")
                    .then(function (msg) {
                        isBusy(false);
                        if (msg.status === bespoke.ServerOperationStatus.OK) {
                            vm.template().BuildingTemplateId(msg.id);
                            logger.info(String.format(culture.messages.SAVE_SUCCESS,culture.building.TEMPLATE_NAME));
                        } else {
                            var message = _(msg.errors).reduce(
                            function(memo ,v) {
                                return memo + "<br/>" + v.Message + "(" + v.PropertyName + ")";

                            }, msg.message);
                            logger.error(message);
                        }
                        tcs.resolve(true);
                    });
                return tcs.promise();
            },
            remove = function() {
                return app.showMessage("Are you sure you want to remove this template", "Remove Template", ["Ya", "Tidak"])
                    .done(function(result) {
                        if (result === "Ya") {

                            var tcs = new $.Deferred();
                            var data = ko.mapping.toJSON(vm.template);
                            var templateName = vm.template().Name();
                            isBusy(true);

                            context.post(data, "/Template/RemoveBuildingTemplate")
                                .then(function(msg) {
                                    if (msg.status === bespoke.ServerOperationStatus.ERROR) {
                                        logger.error(msg.message);
                                    } else {
                                        logger.info(String.format(culture.messages.DELETE_SUCCESS, templateName));
                                        router.navigateTo("/#/building.template.list");
                                    }                                    
                                    tcs.resolve(result);
                                });
                            return tcs.promise();


                        } else {
                            return Task.fromResult(true);
                        }
                    });

            },

            exportTemplate = function () {
                return eximp.exportJson("template.building." + vm.template().BuildingTemplateId() + ".json", ko.mapping.toJSON(vm.template));
            },

            importTemplateJson = function () {
                return eximp.importJson()
                    .done(function (json) {
                        try {

                            var obj = JSON.parse(json),
                                clone = context.toObservable(obj);
                            
                            clone.BuildingTemplateId(0);
                            vm.template(clone);
                            
                        } catch (error) {
                            logger.logError('Fail template import tidak sah', error, this, true);
                        }
                    });
            }, loadDefaultValueFields = function () {
                return defaultValueProvider.loadAsync("Building", vm.template());
            };

        var vm = {
            activate: activate,
            viewAttached: viewAttached,
            template: ko.observable(new bespoke.sph.domain.BuildingTemplate()),
            toolbar: {
                saveCommand: save,
                exportCommand: exportTemplate,
                importCommand: importTemplateJson,
                removeCommand : remove
            },
            customFormElements: templateBase.customFormElements,
            formElements: templateBase.formElements,
            selectFormElement: templateBase.selectFormElement,
            selectedFormElement: templateBase.selectedFormElement,
            removeFormElement: templateBase.removeFormElement,
            removeComboBoxOption: templateBase.removeComboBoxOption,
            selectPathFromPicker: templateBase.selectPathFromPicker,
            showPathPicker: templateBase.showPathPicker,
            addComboBoxOption: templateBase.addComboBoxOption,
            loadDefaultValueFields: loadDefaultValueFields
        };

        return vm;

    });
