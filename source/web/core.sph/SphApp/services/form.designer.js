

define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.system],
    function (context, logger, system) {

        var selectedFormElement = ko.observable(),
            formElements = ko.observableArray(),
            form = ko.observable({
                Id: ko.observable(),
                Name: ko.observable(),
                Route: ko.observable(),
                WebId: ko.observable(),
                FormDesign: ko.observable({
                    FormElementCollection: ko.observableArray()
                })
            }),
            createElement = function (el) {
                var fe = context.clone(el);
                fe.isSelected = ko.observable(true);
                fe.hasError = ko.observable(false);
                fe.Label("Label " + (form().FormDesign().FormElementCollection().length + 1));
                fe.CssClass("");
                fe.Visible("true");
                fe.Enable("true");
                fe.Size("input-large");
                fe.ElementId(ko.unwrap(fe.TypeName) + "_" + (form().FormDesign().FormElementCollection().length + 1));
                fe.WebId(system.guid());

                return fe;
            },
            attached = function (view, form1) {

                form = form1;
                var dropDown = function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var button = $(this);
                    button.parent().addClass("open")
                        .find("input:first").focus()
                        .select();
                };

                // Fix input element click problem
                $(view).on("click mouseup mousedown", ".dropdown-menu input, .dropdown-menu label",
                    function (e) {
                        e.stopPropagation();
                    });
                $("#template-form-designer").on("click", "button.dropdown-toggle", dropDown);


                //toolbox item clicked
                $("#add-field").on("click", "a", function (e) {
                    e.preventDefault();
                    _(form().FormDesign().FormElementCollection()).each(function (f) {
                        f.isSelected(false);
                    });

                    var el = ko.dataFor(this).element;
                    var fe = createElement(el);

                    form().FormDesign().FormElementCollection.push(fe);
                    selectedFormElement(fe);


                });

                // kendoEditor
                $("#template-form-designer").on("click", "textarea", function () {
                    var $editor = $(this),
                        kendoEditor = $editor.data("kendoEditor");
                    if (!kendoEditor) {
                        var htmlElement = ko.dataFor(this),
                            editor = $editor.kendoEditor({
                                change: function () {
                                    htmlElement.Text(this.value());
                                }
                            }).data("kendoEditor");

                        htmlElement.Text.subscribe(function (t) {
                            editor.value(ko.unwrap(t));
                        });

                    }
                }
                );


                var receive = {},
                    initDesigner = function () {
                        $("#template-form-designer>form").sortable({
                            items: ">div",
                            placeholder: "ph",
                            helper: "original",
                            dropOnEmpty: true,
                            forcePlaceholderSize: true,
                            forceHelperSize: false,
                            receive: receive
                        });
                    };

                receive = function (evt, ui) {
                    $(".selected-form-element").each(function () {
                        var kd = ko.dataFor(this);
                        if (typeof kd.isSelected === "function")
                            kd.isSelected(false);
                    });

                    var elements = _($("#template-form-designer>form>div")).map(function (div) {
                        return ko.dataFor(div);
                    }),
                    fe = createElement(ko.dataFor(ui.item[0]).element),
                    sortable = $(this),
                    position = 0,
                    children = sortable.children();

                    for (var i = 0; i < children.length; i++) {
                        if ($(children[i]).position().top > ui.position.top) {
                            position = i;
                            break;
                        }
                    }
                    elements.splice(position, 0, fe);
                    $("#template-form-designer>form").sortable("destroy");
                    //rebuild
                    form().FormDesign().FormElementCollection(elements);
                    initDesigner();
                    $("#template-form-designer>form li.ui-draggable").remove();
                    selectedFormElement(fe);
                };

                initDesigner();

                $.get("form-designer/toolbox-items", function (elements) {
                    formElements(elements);
                    $("#add-field>ul>li").draggable({
                        helper: "clone",
                        connectToSortable: "#template-form-designer>form"
                    });
                });


                $("div.context-action-panel").on("click", "buton.close", function () {
                    $(this).parents("div.context-action").hide();
                });

                selectedFormElement.subscribe(function (model) {
                    model.Path.subscribe(function (p) {
                        if (ko.unwrap(model.Label).indexOf("Label ") > -1) {
                            model.Label(p.replace(".", " ")
                                .replace(/([A-Z])/g, " $1").trim());
                        }
                    });
                });


            },
            supportsHtml5Storage = function () {
                try {
                    return "localStorage" in window && window["localStorage"] !== null;
                } catch (e) {
                    return false;
                }
            },
            getOrderedElements = function () {
                var elements = _($("#template-form-designer>form>div")).map(function (div) {
                    return ko.dataFor(div);
                });
                return elements;
            },

            selectFormElement = function (fe) {

                $(".selected-form-element").each(function () {
                    var kd = ko.dataFor(this);
                    if (typeof kd.isSelected === "function")
                        kd.isSelected(false);
                });

                if (typeof fe.isSelected === "undefined") {
                    fe.isSelected = ko.observable(true);
                }
                if (typeof fe.hasError === "undefined") {
                    fe.hasError = ko.observable(true);
                }
                fe.isSelected(true);
                selectedFormElement(fe);
                if (supportsHtml5Storage()) {
                    localStorage.setItem(form().WebId(), ko.mapping.toJSON(form));
                }
            },
            removeFormElement = function (fe) {
                form().FormDesign().FormElementCollection.remove(fe);
            },
            showError = function (error) {
                var fe = _(form().FormDesign().FormElementCollection())
                    .find(function (f) {
                        return ko.unwrap(f.ElementId) === error.ItemWebId;
                    });
                if (fe) {
                    $(".selected-form-element").each(function () {
                        var kd = ko.dataFor(this);
                        if (typeof kd.isSelected === "function")
                            kd.isSelected(false);
                    });
                    fe.isSelected(true);
                    selectedFormElement(fe);
                }
            };

        var vm = {
            showError: showError,
            attached: attached,
            formElements: formElements,
            selectedFormElement: selectedFormElement,
            selectFormElement: selectFormElement,
            removeFormElement: removeFormElement,
            form: form,
            getOrderedElements: getOrderedElements,
            supportsHtml5Storage: supportsHtml5Storage
        };

        return vm;

    });


