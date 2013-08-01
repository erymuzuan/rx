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


define(['services/datacontext', 'durandal/system'],
    function (context, system) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {
                /*//build form element  
                */
                var elements = [];
                var textbox = new bespoke.sphcommercialspace.domain.TextBox(system.guid());
                textbox.CssClass("icon-text-width pull-left");
                textbox.Name("Single line text");
                textbox.IsRequired(true);

                var textarea = new bespoke.sphcommercialspace.domain.TextAreaElement(system.guid());
                textarea.CssClass("icon-desktop pull-left");
                textarea.Name("Paragrapah text");
                textarea.Rows(5);
                textarea.IsRequired(true);

                var checkbox = new bespoke.sphcommercialspace.domain.CheckBox(system.guid());
                checkbox.CssClass("icon-check pull-left");
                checkbox.Name("Checkboxes");

                var cbb = new bespoke.sphcommercialspace.domain.ComboBox(system.guid());
                cbb.CssClass("icon-chevron-down pull-left");
                cbb.Name("Select list");
                cbb.ComboBoxItemCollection.push(new bespoke.sphcommercialspace.domain.ComboBoxItem());
                cbb.ComboBoxItemCollection.push(new bespoke.sphcommercialspace.domain.ComboBoxItem());
                cbb.ComboBoxItemCollection.push(new bespoke.sphcommercialspace.domain.ComboBoxItem());
                cbb.IsRequired(true);

                var datepicker = new bespoke.sphcommercialspace.domain.DatePicker(system.guid());
                datepicker.CssClass("icon-calendar pull-left");
                datepicker.Name("Tarikh");
                datepicker.IsRequired(true);

                var number = new bespoke.sphcommercialspace.domain.NumberTextBox(system.guid());
                number.CssClass("icon-html5 pull-left");
                number.Name("Nombor");
                number.Step(1);
                number.IsRequired(true);

                var email = new bespoke.sphcommercialspace.domain.EmailFormElement(system.guid());
                email.CssClass("icon-envelope pull-left");
                email.Name("Emel");
                email.IsRequired(true);

                var section = new bespoke.sphcommercialspace.domain.SectionFormElement(system.guid());
                section.CssClass("icon-group pull-left");
                section.Name("Section");

                var address = new bespoke.sphcommercialspace.domain.AddressElement(system.guid());
                address.CssClass("icon-envelope pull-left");
                address.Name("Address");

                elements.push(textbox);
                elements.push(textarea);
                elements.push(checkbox);
                elements.push(cbb);
                elements.push(datepicker);
                elements.push(number);
                elements.push(email);
                elements.push(section);
                elements.push(address);

                vm.formElements(elements);

                /* */
                var customElements = [];
                var map = new bespoke.sphcommercialspace.domain.BuildingMapElement();
                map.CssClass("icon-globe pull-left");
                map.Name("Show map button");
                map.Label("Show map");
                customElements.push(map);

                var floorsElement = new bespoke.sphcommercialspace.domain.BuildingFloorsElement();
                floorsElement.CssClass("icon-table pull-left");
                floorsElement.Name("Floors Table");
                customElements.push(floorsElement);

                vm.customFormElements(customElements);


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
                            vm.buildingTemplate(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.buildingTemplate(new bespoke.sphcommercialspace.domain.BuildingTemplate());

                    vm.buildingTemplate().FormDesign().Name("My form 1");
                    vm.buildingTemplate().FormDesign().Description("Do whatever it takes");

                    return true;
                }


            },
            viewAttached = function () {

                $('#add-field').on("click", 'a', function (e) {
                    e.preventDefault();
                    var designer = vm.buildingTemplate().FormDesign();
                    _(designer.FormElementCollection()).each(function (f) {
                        f.isSelected(false);
                    });

                    // clone
                    var fe = ko.mapping.fromJS(ko.mapping.toJS(ko.dataFor(this)));
                    fe.isSelected = ko.observable(true);
                    fe.Label("Label " + designer.FormElementCollection().length);
                    fe.CssClass("");
                    fe.Visible("true");
                    fe.Size("input-large");
                    fe.ElementId(system.guid());

                    designer.FormElementCollection.push(fe);
                    vm.selectedFormElement(fe);

                });
                $.getScript('/Scripts/jquery-ui-1.10.3.custom.min.js')// only contains UI core and interactions API 
                    .done(function () {
                        $('#building-template-form-designer>form').sortable();
                    });
            },
            selectFormElement = function (fe) {
                _(vm.buildingTemplate().FormDesign().FormElementCollection()).each(function (f) {
                    f.isSelected(false);
                });
                fe.isSelected(true);
                vm.selectedFormElement(fe);
            },
            removeFormElement = function (fe) {
                vm.buildingTemplate().FormDesign().FormElementCollection.remove(fe);
            },
            addComboBoxOption = function () {

                vm.selectedFormElement().ComboBoxItemCollection.push(new bespoke.sphcommercialspace.domain.ComboBoxItem);
            },
            removeComboBoxOption = function (option) {
                vm.selectedFormElement().ComboBoxItemCollection.remove(option);
            },
            addCustomField = function () {
                var customfield = new bespoke.sphcommercialspace.domain.CustomField();
                vm.buildingTemplate().CustomFieldCollection.push(customfield);
            },
            removeCustomField = function (customfield) {
                vm.buildingTemplate().CustomFieldCollection.remove(customfield);
            },
            save = function () {
                var tcs = new $.Deferred();

                // get the sorted element
                var elements = _($('#building-template-form-designer>form>div')).map(function (div) {
                    return ko.dataFor(div);
                });
                vm.buildingTemplate().FormDesign().FormElementCollection(elements);
                var data = ko.mapping.toJSON(vm.buildingTemplate);

                context.post(data, "/Template/SaveBuildingTemplate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            buildingTemplate: ko.observable(new bespoke.sphcommercialspace.domain.BuildingTemplate()),
            addCustomField: addCustomField,
            removeCustomField: removeCustomField,
            toolbar: {
                saveCommand: save
            },
            customFormElements: ko.observableArray(),
            formElements: ko.observableArray(),
            selectFormElement: selectFormElement,
            selectedFormElement: ko.observable(),
            removeFormElement: removeFormElement,
            removeComboBoxOption: removeComboBoxOption,
            addComboBoxOption: addComboBoxOption
        };

        return vm;

    });
