/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
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
                number.CssClass("icon-xing-sign pull-left");
                number.Name("Nombor");
                number.Step(1);
                number.IsRequired(true);

                var email = new bespoke.sphcommercialspace.domain.EmailFormElement(system.guid());
                email.CssClass("icon-envelope pull-left");
                email.Name("Emel");
                email.IsRequired(true);

                var web = new bespoke.sphcommercialspace.domain.WebsiteFormElement(system.guid());
                web.CssClass("icon-link pull-left");
                web.Name("Website");
                web.IsRequired(true);

                var html = new bespoke.sphcommercialspace.domain.HtmlElement(system.guid());
                html.CssClass("icon-html5 pull-left");
                html.Name("HTML");
                html.IsRequired(false);
                html.Tooltip("Allows you to create custom HTML markup");
                
                var list = new bespoke.sphcommercialspace.domain.CustomListDefinitionElement(system.guid());
                list.CssClass("icon-th-list pull-left");
                list.Name("List");
                list.IsRequired(false);
                list.Tooltip("Creates custom list");

                var section = new bespoke.sphcommercialspace.domain.SectionFormElement(system.guid());
                section.CssClass("icon-reorder pull-left");
                section.Name("Section");

                elements.push(textbox);
                elements.push(textarea);
                elements.push(checkbox);
                elements.push(cbb);
                elements.push(datepicker);
                elements.push(number);
                elements.push(email);
                elements.push(web);
                elements.push(html);
                elements.push(list);
                elements.push(section);

                vm.formElements(elements);

                /* */

                vm.customFormElements(customElements);



            },
            designer = ko.observable(),
            viewAttached = function (view) {

                var dropDown = function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var button = $(this);
                    button.parent().addClass("open")
                        .find("input:first").focus()
                        .select();
                };

                // Fix input element click problem
                $(view).on('click mouseup mousedown', '.dropdown-menu input, .dropdown-menu label',
                    function (e) {
                        e.stopPropagation();
                    });
                $('#template-form-designer').on('click', 'button.dropdown-toggle', dropDown);


                //toolbox item clicked
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
                
                // kendoEditor
                $('#template-form-designer').on('click', 'textarea', function () {
                    var $editor = $(this);
                    var kendoEditor = $editor.data("kendoEditor");
                    if (!kendoEditor) {
                       var htmlElement = ko.dataFor(this);
                       $editor.kendoEditor({
                           change:function() {
                               htmlElement.Text(this.value());
                           }
                       });
                        
                    }
                }
                );
                $.getScript('/Scripts/jquery-ui-1.10.3.custom.min.js')// only contains UI core and interactions API 
                    .done(function () {

                        var initDesigner = function () {
                            $('#template-form-designer>form').sortable({
                                items: '>div',
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


                // get the position
                $('#template-form-designer').on('click', 'form>div', function (e) {
                    e.preventDefault();
                    var top = $(this).position().top;
                    $('#form-designer-toolbox').animate({ "margin-top": top - 140 }, 300);

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
            },
            tree,
            selectedPath = ko.observable(),
            showPathPicker = function (type) {
                console.log(type);
                if (!tree) {
                    $.get("/App/TriggerPathPickerHtml/" + type)
                        .done(function (html) {
                            $('#path-picker-treepanel').html(html);
                            tree = $('#path-picker-treepanel>ul').kendoTreeView({
                                select: function (e) {
                                    selectedPath($(e.node).find("span.k-sprite").data("path"));
                                }
                            });
                            $('#path-picker-panel').modal({});
                        });
                } else {

                    $('#path-picker-panel').modal({});
                }
            },
            selectPathFromPicker = function () {
                vm.selectedFormElement().Path(selectedPath());
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
            designer: designer,
            showPathPicker: showPathPicker,
            selectPathFromPicker: selectPathFromPicker
        };

        return vm;

    });
