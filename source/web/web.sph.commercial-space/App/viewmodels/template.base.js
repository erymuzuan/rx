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


define(['services/datacontext', 'durandal/system'],
    function (context, system) {

        var isBusy = ko.observable(false),
            activate = function (customElements) {
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

                vm.customFormElements(customElements);



            },
            designer = ko.observable(),
            viewAttached = function () {
                $("#files").kendoUpload({
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
                            designer().ImageStoreId(storeId);
                        }

                        if (removed) {
                            designer().ImageStoreId("");
                        }


                    }
                });
                $('#add-field').on("click", 'a', function (e) {
                    e.preventDefault();
                    _(designer().FormElementCollection()).each(function (f) {
                        f.isSelected(false);
                    });

                    // clone
                    var fe = ko.mapping.fromJS(ko.mapping.toJS(ko.dataFor(this)));
                    fe.isSelected = ko.observable(true);
                    fe.Label("Label " + designer().FormElementCollection().length);
                    fe.CssClass("");
                    fe.Visible("true");
                    fe.Size("input-large");
                    fe.ElementId(system.guid());

                    designer().FormElementCollection.push(fe);
                    vm.selectedFormElement(fe);

                });
                $.getScript('/Scripts/jquery-ui-1.10.3.custom.min.js')// only contains UI core and interactions API 
                    .done(function () {

                        var initDesigner = function () {
                            $('#template-form-designer>form').sortable({
                                items: 'div',
                                placeholder: 'ph',
                                helper: 'original',
                                dropOnEmpty: true,
                                forcePlaceholderSize: true,
                                forceHelperSize: false,
                                receive: receive
                            });

                        },
                            receive = function (evt, ui) {
                                var elements = _($('#template-form-designer>form>div')).map(function (div) {
                                    return ko.dataFor(div);
                                });
                                var fe = ko.dataFor(ui.item[0]);
                                fe.isSelected = ko.observable(true);
                                elements.splice(2, 0, fe);
                                $('#template-form-designer>form').sortable("destroy");


                                designer().FormElementCollection(elements);
                            };

                        initDesigner();
                        $('#add-field>ul>li').draggable({
                            helper: 'clone',
                            connectToSortable: "#template-form-designer>form"
                        });
                    });
            },
            selectFormElement = function (fe) {
                _(designer().FormElementCollection()).each(function (f) {
                    f.isSelected(false);
                });
                fe.isSelected(true);
                vm.selectedFormElement(fe);
            },
            removeFormElement = function (fe) {
                designer().FormElementCollection.remove(fe);
            },
            addComboBoxOption = function () {

                vm.selectedFormElement().ComboBoxItemCollection.push(new bespoke.sphcommercialspace.domain.ComboBoxItem);
            },
            removeComboBoxOption = function (option) {
                vm.selectedFormElement().ComboBoxItemCollection.remove(option);
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            customFormElements: ko.observableArray(),
            formElements: ko.observableArray(),
            selectFormElement: selectFormElement,
            selectedFormElement: ko.observable(),
            removeFormElement: removeFormElement,
            removeComboBoxOption: removeComboBoxOption,
            addComboBoxOption: addComboBoxOption,
            designer: designer
        };

        return vm;

    });
