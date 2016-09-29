﻿define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app, objectbuilders.eximp,
        objectbuilders.dialog, objectbuilders.config, "services/_ko.list",
        "jquery", "ko", "String", "bespoke", "underscore", "Task"],
    function (context, logger, router, system, app, eximp, dialog, config, kolist,$, ko) {

        "use strict";
        var errors = ko.observableArray(),
            warnings = ko.observableArray(),
            originalEntity = "",
            operationOptions = ko.observableArray(),
            layoutOptions = ko.observableArray(),
            collectionMemberOptions = ko.observableArray(),
            formElements = ko.observableArray(),
            wdOptions = ko.observableArray(),
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            schema = ko.observable(),
            form = ko.observable(new bespoke.sph.domain.WorkflowForm({WebId: system.guid()})),
            selectedFormElement = ko.observable(),
            activate = function (wdid, id) {
                kolist.init();
                var query = String.format("Id eq '{0}'", wdid),
                    tcs = new $.Deferred();


                context.getListAsync("WorkflowDefinition", null, "Name")
                    .then(function (entities) {
                        var list = _(entities).map(function (v) {
                            return {
                                text: v,
                                value: v
                            };
                        });
                        wdOptions(list);
                    });

                context.loadOneAsync("WorkflowDefinition", query)
                    .done(function (b) {
                        wd(b);
                        var receives = _(b.ActivityCollection()).chain()
                            .filter(function (v) {
                                return ko.unwrap(v.$type).indexOf("ReceiveActivity") > -1;
                            }).value();
                        operationOptions(receives);

                    });

                $.get("/app/entityformdesigner/layoutoptions").done(function (options) {
                    var layouts = _(options).map(function (v) {
                        return {
                            value: v,
                            text: v
                        };
                    });
                    layouts.splice(0, 0, {value: " ", text: "[Default Layout]"});
                    layoutOptions(layouts);
                });

                context.loadOneAsync("WorkflowForm", "Id eq '" + id + "'")
                    .done(function (f) {
                        _(f.FormDesign().FormElementCollection()).each(function (v) {
                            v.isSelected = ko.observable(false);
                        });
                        var url = "/api/workflow-forms/" + id + "/activities/" + ko.unwrap(f.Operation) + "/schema";
                        $.getJSON(url)
                            .done(function (model) {
                                schema(model);
                            });
                        form(f);
                        originalEntity = ko.toJSON(form);

                        var vodUrl = "/api/workflow-forms/" + ko.unwrap(form().Id) + "/activities/" + ko.unwrap(form().Operation) + "/vod";
                        $.getJSON(vodUrl)
                            .done(function (b) {
                                var vod = context.toObservable(b);
                                // get the AllowMutlipe members
                                var members = [];
                                var getAllowMultipleMembers = function (list) {
                                    _(list).each(function (v) {
                                        if (ko.unwrap(v.AllowMultiple)) {
                                            members.push(v);
                                        }
                                        getAllowMultipleMembers(ko.unwrap(v.MemberCollection));
                                    });
                                };

                                getAllowMultipleMembers(ko.unwrap(vod.MemberCollection));
                                var names = _(members).map(function (v) {
                                    return {
                                        text: ko.unwrap(v.TypeName),
                                        value: "bespoke." + config.applicationName + "." + ko.unwrap(wd().WorkflowTypeName) + ".domain." + ko.unwrap(v.TypeName)
                                    };
                                });
                                collectionMemberOptions(names);
                            });
                        tcs.resolve(true);
                    });


                form().WorkflowDefinitionId(wdid);

                return tcs.promise();

            },
            removeFormElement = function (fe) {
                var fd = ko.unwrap(form().FormDesign);
                fd.FormElementCollection.remove(fe);
            },
            attached = function (view) {
                form().Name.subscribe(function (v) {
                    if (!form().Route()) {
                        form().Route(v.toLowerCase().replace(/\W+/g, "-"));
                    }
                });
                var fd = ko.unwrap(form().FormDesign);

                var dropDown = function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var button = $(this);
                    button.parent().addClass("open")
                        .find("input:first").focus()
                        .select();
                };

                // delete selected element when [delete] key is pressed
                $(view).on("keyup", "div.selected-form-element", function (e) {
                    if (e.keyCode === 46 && typeof selectedFormElement() != "undefined") {
                        removeFormElement(selectedFormElement());
                    }
                });

                // Fix input element click problem
                $(view).on("click mouseup mousedown", ".dropdown-menu input, .dropdown-menu label",
                    function (e) {
                        e.stopPropagation();
                    });
                $("#template-form-designer").on("click", "button.dropdown-toggle", dropDown);


                //toolbox item clicked
                $("#add-field").on("click", "a", function (e) {
                    e.preventDefault();
                    _(fd.FormElementCollection()).each(function (f) {
                        f.isSelected(false);
                    });

                    // clone
                    var el = ko.dataFor(this).element;
                    var fe = context.clone(el);
                    fe.isSelected = ko.observable(true);
                    fe.Label("Label " + fd.FormElementCollection().length);
                    fe.CssClass("");
                    fe.Visible("true");
                    fe.Enable("true");
                    fe.Size("input-large");
                    fe.ElementId(system.guid());
                    fe.WebId(system.guid());

                    fd.FormElementCollection.push(fe);
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


                var receive = function (evt, ui) {
                        $(".selected-form-element").each(function () {
                            var kd = ko.dataFor(this);
                            if (typeof kd.isSelected === "function")
                                kd.isSelected(false);
                        });

                        var elements = _($("#template-form-designer>form>div")).map(function (div) {
                                return ko.dataFor(div);
                            }),
                            fe = context.clone(ko.dataFor(ui.item[0]).element),
                            sortable = $(this),
                            position = 0,
                            children = sortable.children();

                        fe.isSelected = ko.observable(true);
                        fe.Enable("true");
                        fe.Visible("true");
                        fe.Label("Label " + fd.FormElementCollection().length);
                        fe.ElementId(system.guid());
                        fe.WebId(system.guid());

                        for (var i = 0; i < children.length; i++) {
                            if ($(children[i]).position().top > ui.position.top) {
                                position = i;
                                break;
                            }
                        }
                        elements.splice(position, 0, fe);
                        $("#template-form-designer>form").sortable("destroy");
                        //rebuild
                        fd.FormElementCollection(elements);
                        initDesigner();
                        $("#template-form-designer>form li.ui-draggable").remove();
                        selectedFormElement(fe);
                    },
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
                    return "localStorage" in window && window.localStorage !== null;
                } catch (e) {
                    return false;
                }
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {

                    var fd = ko.unwrap(form().FormDesign);
                    // get the sorted element
                    var elements = _($("#template-form-designer>form>div")).map(function (div) {
                        return ko.dataFor(div);
                    });
                    fd.FormElementCollection(elements);
                    dialog.close(this, "OK");
                    if (supportsHtml5Storage()) {
                        localStorage.removeItem(form().WebId());
                    }
                }
            },
            cancelClick = function () {
                if (supportsHtml5Storage()) {
                    localStorage.removeItem(form().WebId());
                }
                dialog.close(this, "Cancel");
            },
            selectFormElement = function (fe, e) {
                $(e.currentTarget).prop("tabindex", 0);
                $(e.currentTarget).focus();
                $(".selected-form-element").each(function () {
                    var kd = ko.dataFor(this);
                    if (typeof kd.isSelected === "function")
                        kd.isSelected(false);
                });

                if (typeof fe.isSelected === "undefined") {
                    fe.isSelected = ko.observable(true);
                }
                fe.isSelected(true);
                selectedFormElement(fe);
                if (supportsHtml5Storage()) {
                    localStorage.setItem(form().WebId(), ko.mapping.toJSON(form));
                }
                $("#form-designer-toolbox .nav-tabs a[href='#fields-settings']").tab("show");

                var elementSelectedPosition = $(".selected-form-element").position();
                var propertiesPaddingTop = (elementSelectedPosition.top < 95) ? 0 : elementSelectedPosition.top - 95;
                $("#fields-settings").css("padding-top", propertiesPaddingTop);
            },
            importCommand = function () {
                return eximp.importJson()
                    .done(function (json) {
                        try {

                            var obj = JSON.parse(json),
                                clone = context.toObservable(obj);

                            form().FormDesign(clone.FormDesign());

                        } catch (error) {
                            logger.logError("Fail template import tidak sah", error, this, true);
                        }
                    });
            },
            publish = function () {
                var fd = ko.unwrap(form().FormDesign);
                // get the sorted element
                var elements = _($("#template-form-designer>form>div")).map(function (div) {
                    return ko.dataFor(div);
                });
                fd.FormElementCollection(elements);


                var data = ko.mapping.toJSON(form);

                return context.post(data, "/api/workflow-forms/publish")
                    .then(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                            form().Id(result.id);
                            errors.removeAll();
                            warnings(result.warnings);
                            originalEntity = ko.toJSON(form);
                        } else {
                            errors(result.Errors);
                            warnings(result.Warnings);
                            logger.error("There are errors in your form, !!!");
                        }
                    });

            },
            save = function () {
                var fd = ko.unwrap(form().FormDesign);
                // get the sorted element
                var elements = _($("#template-form-designer>form>div")).map(function (div) {
                    return ko.dataFor(div);
                });
                fd.FormElementCollection(elements);


                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(form);

                context.post(data, "/api/workflow-forms")
                    .then(function (result) {
                        if (result.success) {
                            form().Id(result.id);
                            logger.info("Your form has been successfully saved.");
                            originalEntity = ko.toJSON(form);
                        } else {
                            errors(result.Errors);
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            canDeactivate = function () {
                var tcs = new $.Deferred();


                if (originalEntity !== ko.toJSON(form)) {
                    app.showMessage("Save change to the item", "Rx Developer", ["Yes", "No", "Cancel"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {
                                save().done(function () {
                                    tcs.resolve(true);
                                });
                            }
                            if (dialogResult === "No") {
                                tcs.resolve(true);
                            }
                            if (dialogResult === "Cancel") {
                                tcs.resolve(false);
                            }

                        });
                } else {
                    return true;
                }
                return tcs.promise();
            },

            removeAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(form);
                app.showMessage("Are you sure you want to permanently remove this form?, this action cannot be undone!!", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.send(data, "/api/workflow-forms/" + form().Id(), "DELETE")
                                .fail(tcs.reject)
                                .then(function (result) {
                                    if (result.success) {
                                        logger.info(result.message);
                                        errors.removeAll();
                                        window.location = "/sph#dev.home/";
                                    } else {
                                        logger.error("There are errors in your form, cannot be removed !!");
                                    }
                                    tcs.resolve(result);
                                });
                        } else {
                            tcs.resolve(false);

                        }
                    });

                return tcs.promise();
            },
            depublishAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(form);

                context.post(data, "/api/workflow-forms/depublish")
                    .then(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {
                            var views = _(result.views).map(function (v) {
                                return {
                                    Message: `${v} view has a link to this form!`,
                                    Code: ""
                                };
                            });
                            errors(views);
                            logger.error("There are errors in your form, depublish those views first to proceed, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            partialEditor = null,
            editCode = function () {
                if (null === partialEditor || partialEditor.closed) {
                    var partial = "partial/" + form().Route();
                    partialEditor = window.open("/sph/editor/file?id=/sphapp/" + partial + ".js", "_blank", "height=600px,width=800px,toolbar=0,location=0");
                    form().Partial(partial);
                } else {
                    partialEditor.focus();
                }

                return Task.fromResult(true);

            },
            layoutEditor = null,
            editLayout = function () {
                if (null === layoutEditor || layoutEditor.closed) {
                    layoutEditor = window.open("/sph/editor/file?id=/views/workflowformrenderer/" + form().Layout() + ".cshtml", "_blank", "height=600px,width=800px,toolbar=0,location=0");

                } else {
                    layoutEditor.focus();
                }

                return Task.fromResult(true);
            },
            viewSchema = function () {
                var tcs = new $.Deferred();
                var url = "/api/workflow-forms/" + ko.unwrap(form().Id) + "/activities/" + ko.unwrap(form().Operation) + "/vod";
                $.getJSON(url)
                    .done(function (vod) {
                        require(["viewmodels/workflow.schema.dialog", "durandal/app"], function (dg, app2) {
                            dg.vod(context.toObservable(vod));
                            app2.showDialog(dg).done(tcs.resolve);
                        });
                    });


                return tcs.promise();
            },
            translateLabels = function () {
                var tcs = new $.Deferred(),
                    fd = ko.unwrap(form().FormDesign),
                    elements = fd.FormElementCollection();
                require(["viewmodels/resource.table.dialog", "durandal/app"], function (dg, app2) {

                    dg.keys(_(elements).map(function (v) {
                        return ko.unwrap(v.Label);
                    }));
                    dg.keys.splice(0, 0, form().Name());
                    if (form().Caption() && form().Caption() !== form().Name()) {
                        dg.keys.splice(0, 0, form().Caption());
                    }
                    dg.resource(form().Route());

                    app2.showDialog(dg)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {

                            }
                            tcs.resolve(result);
                        });

                });

                return tcs.promise();
            };

        var vm = {
            errors: errors,
            warnings: warnings,
            collectionMemberOptions: collectionMemberOptions,
            layoutOptions: layoutOptions,
            operationOptions: operationOptions,
            attached: attached,
            activate: activate,
            canDeactivate: canDeactivate,
            formElements: formElements,
            selectedFormElement: selectedFormElement,
            selectFormElement: selectFormElement,
            removeFormElement: removeFormElement,
            form: form,
            wd: wd,
            schema: schema,
            wdOptions: wdOptions,
            okClick: okClick,
            cancelClick: cancelClick,
            importCommand: importCommand,
            toolbar: {
                commands: ko.observableArray([{
                    caption: "Clone",
                    icon: "fa fa-copy",
                    command: function () {
                        form().Name(form().Name() + " Copy (1)");
                        form().Route("");
                        form().Id("0");
                        form().Partial("");
                        return Task.fromResult(0);
                    }
                },
                    {
                        caption: "Publish",
                        icon: "fa fa-sign-in",
                        command: publish,
                        enable: ko.computed(function () {
                            return form().Id() !== "0";
                        })
                    },
                    {
                        command: depublishAsync,
                        caption: "Depublish",
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return form().Id() !== "0" && form().IsPublished();
                        })
                    },
                    {
                        command: editCode,
                        caption: "Edit Code",
                        icon: "fa fa-code",
                        enable: ko.computed(function () {
                            return form().Route();
                        })
                    },
                    {
                        command: editLayout,
                        caption: "Edit Layout",
                        icon: "fa  fa-file-text-o",
                        enable: ko.computed(function () {
                            if (typeof form().Layout() === "undefined") {
                                return false;
                            }
                            if (!form().Layout()) {
                                return false;
                            }
                            if (form().Layout().trim() === "") {
                                return false;
                            }
                            return form().Route();
                        })
                    },
                    {
                        command: translateLabels,
                        caption: "Translate",
                        icon: "fa  fa-language",
                        enable: ko.computed(function () {
                            return form().Route();
                        })
                    },
                    {
                        command: viewSchema,
                        caption: "View Schema",
                        icon: "fa  fa-clipboard"
                    }
                ]),
                saveCommand: save,
                removeCommand: removeAsync
            }
        };

        return vm;

    });


