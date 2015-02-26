

define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app, objectbuilders.eximp, objectbuilders.dialog,
     "services/form.designer"],
    function (context, logger, router, system, app, eximp, dialog, designer) {

        var errors = ko.observableArray(),
            layoutOptions = ko.observableArray(),
            entityOptions = ko.observableArray(),
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            form = ko.observable(new bespoke.sph.domain.ScreenActivityForm({ WebId: system.guid() })),
            instance = ko.observable(),
            activate = function (wdid, formid) {

                var query = String.format("Id eq '{0}'", wdid),
                    tcs = new $.Deferred();


                var task1 = context.getListAsync("EntityDefinition", null, "Name"),
                    task2 = context.loadOneAsync("WorkflowDefinition", query),
                    task3 = $.getJSON("/sph/workflowdefinition/GetJavascriptWorkflowInstance/" + wdid);
                $.when(task1, task2, task3).done(function (result1, result2, result3) {
                    var entities = result1,
                        b = result2;
                    var list = _(entities).map(function (v) {
                        return {
                            text: v,
                            value: v
                        };
                    });

                    list.push({ text: "UserProfile*", value: "UserProfile" });
                    list.push({ text: "Designation*", value: "Designation" });
                    list.push({ text: "Department*", value: "Department" });
                    entityOptions(list);

                    wd(b);
                    if (formid === "0") {
                        tcs.resolve(true);
                    }
                    instance(result3[0]);

                });





                $.get("/app/entityformdesigner/layoutoptions").done(function (options) {
                    layoutOptions(options);
                });

                if (formid !== "0") {
                    context.loadOneAsync("ScreenActivityForm", "Id eq '" + formid + "'")
                    .done(function (f) {
                        _(f.FormDesign().FormElementCollection()).each(function (v) {
                            v.isSelected = ko.observable(false);
                            v.hasError = ko.observable(false);
                        });
                        form(f);
                        tcs.resolve(true);
                    });
                } else {
                    form(new bespoke.sph.domain.ScreenActivityForm(system.guid()));
                    setTimeout(function () {
                        tcs.resolve(true);
                    }, 500);
                }
                form().Name.subscribe(function (v) {
                    if (!form().Route()) {
                        form().Route(v.toLowerCase().replace(/\W+/g, "-"));
                    }
                });
                form().WorkflowDefinitionId(wdid);
                return tcs.promise();

            },
            attached = function (view) {
                designer.attached(view, form);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {

                    var fd = ko.unwrap(form().FormDesign);
                    fd.FormElementCollection(designer.getOrderedElements());
                    dialog.close(this, "OK");
                    if (designer.supportsHtml5Storage()) {
                        localStorage.removeItem(form().WebId());
                    }
                }
            },
            cancelClick = function () {
                if (designer.supportsHtml5Storage()) {
                    localStorage.removeItem(form().WebId());
                }
                dialog.close(this, "Cancel");
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
                var fd = ko.unwrap(form().FormDesign),
                    elements = designer.getOrderedElements(),
                    tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(form);

                fd.FormElementCollection(elements);

                context.post(data, "/screen-activity-form/publish")
                    .then(function (result) {
                        _(elements).each(function (f) {
                            f.hasError(false);
                        });

                        if (result.success) {
                            logger.info(result.message);
                            wd().Id(result.id);
                            errors.removeAll();
                        } else {
                            errors(result.Errors);
                            _(result.Errors).each(function (v) {
                                _(elements).each(function (f) {
                                    if (ko.unwrap(f.ElementId) === v.ItemWebId) {
                                        f.hasError(true);
                                    }
                                });
                            });
                            logger.error("There are errors in your form, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();

            },
            save = function () {
                var fd = ko.unwrap(form().FormDesign);
                fd.FormElementCollection(designer.getOrderedElements());


                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(form);

                context.post(data, "/screen-activity-form")
                    .then(function (result) {
                        if (result.success) {
                            form().Id(result.id);
                            logger.info("Your form has been successfully saved.");
                        } else {
                            errors(result.Errors);
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },

        removeAsync = function () {

            var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(form);
            app.showMessage("Are you sure you want to permanently remove this form ? This action cannot be undone !", "Reactive Developer", ["Yes", "No"])
                .done(function (dialogResult) {
                    if (dialogResult === "Yes") {

                        context.send(data, "/screen-activity-form/" + form().Id(), "DELETE")
                            .then(function (result) {
                                if (result.success) {
                                    logger.info(result.message);
                                    errors.removeAll();
                                    window.location = "/sph#workflow.definition.visual/" + wd().Id();
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

            context.post(data, "/screen-activity-form/depublish")
                .then(function (result) {
                    if (result.success) {
                        logger.info(result.message);
                        errors.removeAll();
                    } else {
                        var views = _(result.views).map(function (v) {
                            return {
                                Message: v + " view has a link to this form!",
                                Code: ""
                            }
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
            if (null == partialEditor || partialEditor.closed) {
                var partial = "partial/" + form().Route();
                partialEditor = window.open("/sph/editor/file?id=/sphapp/" + partial + ".js", '_blank', 'height=600px,width=800px,toolbar=0,location=0');
                form().Partial(partial);
            } else {
                partialEditor.focus();
            }

            return Task.fromResult(true);

        },
        layoutEditor = null,
        editLayout = function () {
            if (null == layoutEditor || layoutEditor.closed) {
                layoutEditor = window.open("/sph/editor/file?id=/views/entityformrenderer/" + form().Layout() + ".cshtml", '_blank', 'height=600px,width=800px,toolbar=0,location=0');

            } else {
                layoutEditor.focus();
            }

            return Task.fromResult(true);
        };

        var vm = {
            errors: errors,
            showError: designer.showError,
            layoutOptions: layoutOptions,
            attached: attached,
            activate: activate,
            formElements: designer.formElements,
            selectedFormElement: designer.selectedFormElement,
            selectFormElement: designer.selectFormElement,
            removeFormElement: designer.removeFormElement,
            form: form,
            wd: wd,
            instance: instance,
            entityOptions: entityOptions,
            okClick: okClick,
            cancelClick: cancelClick,
            importCommand: importCommand,
            toolbar: {
                commands: ko.observableArray([{
                    caption: 'Clone',
                    icon: 'fa fa-copy',
                    command: function () {
                        form().Name(form().Name() + ' Copy (1)');
                        form().Route('');
                        form().Id("0");
                        return Task.fromResult(0);
                    }
                },
                {
                    caption: 'Publish',
                    icon: 'fa fa-sign-in',
                    command: publish,
                    enable: ko.computed(function () {
                        return form().Id() !== "0";
                    })
                },
                {
                    command: depublishAsync,
                    caption: 'Depublish',
                    icon: "fa fa-sign-out",
                    enable: ko.computed(function () {
                        return form().Id() !== "0" && form().IsPublished();
                    })
                },
                {
                    command: editCode,
                    caption: 'Edit Code',
                    icon: "fa fa-code",
                    enable: ko.computed(function () {
                        return form().Route();
                    })
                },
                {
                    command: editLayout,
                    caption: 'Edit Layout',
                    icon: "fa  fa-file-text-o",
                    enable: ko.computed(function () {
                        return form().Route();
                    })
                }
                ]),
                saveCommand: save,
                removeCommand: removeAsync
            }
        };

        return vm;

    });


