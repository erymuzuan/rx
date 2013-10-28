/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'durandal/system', './template.base', 'services/jsonimportexport', 'services/logger', 'config', objectbuilders.defaultValueProvider],
    function (context, system, designerHost, eximp, logger, config, defaultValueProvider) {

        var templateId = ko.observable(),
            activate = function (routeData) {


                var customElements = [];

                var address = new bespoke.sph.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");
                customElements.push(address);

                var building = new bespoke.sph.domain.BuildingElement(system.guid());
                building.CssClass("icon-building pull-left");
                building.Name("Building");
                customElements.push(building);

                var map = new bespoke.sph.domain.BuildingMapElement(system.guid());
                map.CssClass("icon-globe pull-left");
                map.Name("Map");
                customElements.push(map);

                var unit = new bespoke.sph.domain.SpaceUnitElement(system.guid());
                unit.CssClass("icon-check-empty pull-left");
                unit.Name("Unit");
                customElements.push(unit);

                var feature = new bespoke.sph.domain.SpaceFeaturesElement(system.guid());
                feature.CssClass("icon-list-ol pull-left");
                feature.Name("Features");
                customElements.push(feature);

                designerHost.activate(customElements);


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("SpaceTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("SpaceTemplate", query)
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
                    vm.template(new bespoke.sph.domain.SpaceTemplate());

                    vm.template().FormDesign().Name("jenis ruang anda");
                    vm.template().FormDesign().Description("Keterangan jenis ruang");

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
                var data = ko.mapping.toJSON(vm.template());

                context.post(data, "/Template/SaveSpaceTemplate")
                    .then(function (result) {
                        if (result.status === "OK") {

                            logger.info("Data has been successfully save");
                            tcs.resolve(result);
                        } else {
                            logger.error(result.message);
                            tcs.resolve(result);
                        }
                    });
                return tcs.promise();
            },

        exportTemplate = function () {
            return eximp.exportJson("template.space." + vm.template().SpaceTemplateId() + ".json", ko.mapping.toJSON(vm.template));
        },

        importTemplateJson = function () {
            return eximp.importJson()
                .done(function (json) {
                    try {
                        var obj = JSON.parse(json),
                            clone = context.toObservable(obj);

                        vm.template(clone);
                        vm.template().SpaceTemplateId(0);
                    } catch (error) {
                        logger.logError('Fail template import tidak sah', error, this, true);
                    }
                });
        },

            loadDefaultValueFields = function () {
                return defaultValueProvider.loadAsync("Space", vm.template());
            };

        var vm = {
            activate: activate,
            template: ko.observable(new bespoke.sph.domain.SpaceTemplate()),
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
            config: config,
            loadDefaultValueFields: loadDefaultValueFields
        };

        return vm;

    });
