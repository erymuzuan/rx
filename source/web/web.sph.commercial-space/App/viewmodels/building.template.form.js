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
                
                var textarea = new bespoke.sphcommercialspace.domain.TextAreaElement(system.guid());
                textarea.CssClass("icon-desktop pull-left");
                textarea.Name("Paragrapah text");
                textarea.Rows(5);

                var checkbox = new bespoke.sphcommercialspace.domain.CheckBox(system.guid());
                checkbox.CssClass("icon-check pull-left");
                checkbox.Name("Checkboxes");

                var cbb = new bespoke.sphcommercialspace.domain.ComboBox(system.guid());
                cbb.CssClass("icon-chevron-down pull-left");
                cbb.Name("Select list");
                cbb.OptionCollection(['Option 1', 'Option 2', 'Option 3']);

                var datepicker = new bespoke.sphcommercialspace.domain.DatePicker(system.guid());
                datepicker.CssClass("icon-calendar pull-left");
                datepicker.Name("Tarikh");

                var number = new bespoke.sphcommercialspace.domain.NumberTextBox(system.guid());
                number.CssClass("icon-html5 pull-left");
                number.Name("Nombor");
                number.Step(1);
                
                var email = new bespoke.sphcommercialspace.domain.EmailFormElement(system.guid());
                email.CssClass("icon-envelope pull-left");
                email.Name("Emel");

                elements.push(textbox);
                elements.push(textarea);
                elements.push(checkbox);
                elements.push(cbb);
                elements.push(datepicker);
                elements.push(number);
                elements.push(email);

                vm.formElements(elements);
                vm.formDesign().Name("My form 1");
                vm.formDesign().Description("Do whatever it takes");


                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("BuildingTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("BuildingTemplate", query)
                        .done(function (b) {
                            vm.buildingTemplate(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.buildingTemplate(new bespoke.sphcommercialspace.domain.BuildingTemplate());
                    return true;
                }


            },
            viewAttached = function () {

                $('#add-field').on("click", 'a', function (e) {
                    e.preventDefault();

                    _(vm.formDesign().FormElementCollection()).each(function (f) {
                        f.isSelected(false);
                    });

                    var fe = ko.mapping.fromJS(ko.mapping.toJS(ko.dataFor(this)));
                    fe.isSelected = ko.observable(true);
                    fe.Label("Label " + vm.formDesign().FormElementCollection().length);
                    fe.CssClass("input-large");
                    
                    vm.formDesign().FormElementCollection.push(fe);

                });
                $.getScript('/Scripts/jquery-ui-1.10.3.js')
                    .done(function () {
                        $('#building-template-form-designer').sortable();
                    });
            },
            selectFormElement = function (fe) {
                _(vm.formDesign().FormElementCollection()).each(function (f) {
                    f.isSelected(false);
                });
                fe.isSelected(true);
                vm.selectedFormElement(fe);
            },            
            addComboBoxOption = function () {
                vm.selectedFormElement().OptionCollection.push('');
            },          
            removeComboBoxOption = function (option) {
                vm.selectedFormElement().OptionCollection.remove(option);
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
                var data = ko.mapping.toJSON({ buildingTemplate: vm.buildingTemplate });
                isBusy(true);

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
            formDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
            addCustomField: addCustomField,
            removeCustomField: removeCustomField,
            toolbar: {
                saveCommand: save
            },
            formElements: ko.observableArray(),
            selectFormElement: selectFormElement,
            selectedFormElement: ko.observable(),
            removeComboBoxOption: removeComboBoxOption,
            addComboBoxOption: addComboBoxOption
        };

        return vm;

    });
