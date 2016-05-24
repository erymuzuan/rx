﻿define(["services/datacontext", "services/logger", objectbuilders.config, objectbuilders.router, "services/app", "viewmodels/_custom.forms.routes", "viewmodels/_custom.forms.dialogs", "viewmodels/_custom.forms.partial.views", "viewmodels/_custom.forms.scripts", objectbuilders.app],
    function (context, logger, config, router, app, customForm, customDialog, partialView, customScript, app2) {
        var checkSource = function (type, query) {
            var tcs = new $.Deferred(),
                count = 0,
                checkExist = function () {

                    if (count > 10) {
                        return app2.showMessage("Please make sure your Subscriber Worker is started", "RX Developer", ["OK"])
                        .done(function () {
                            tcs.resolve(false);
                        });
                    }

                    return context.loadOneAsync(type, query)
                        .done(function (ed) {
                            if (ed) {
                                tcs.resolve(ed);
                            } else {
                                setTimeout(checkExist, 250);
                                count++;
                            }
                        });
                };

            checkExist();
            return tcs.promise();
        },
            addEntityDefinitionAsync = function () {

                return app.showDialog("new.entity.definition.dialog")
                      .then(function (dialog, result) {
                          if (result === "OK") {
                              return checkSource("EntityDefinition", "Id eq '" + ko.unwrap(dialog.id) + "'");
                          }
                          return Task.fromResult(0);
                      }).then(function (ed) {
                          if (ed)
                              router.navigate("#entity.details/" + ko.unwrap(ed.Id));
                      });
            },
            addValueObjectDefinitionAsync = function () {
                return app.showDialog("new.value.object.definition.dialog")
                         .then(function (dialog, result) {
                             if (result === "OK") {
                                 return checkSource("ValueObjectDefinition", "Id eq '" + ko.unwrap(dialog.id) + "'");
                             }
                             return Task.fromResult(0);
                         }).then(function (ed) {
                             if (ed)
                                 router.navigate("#value.object.details/" + ko.unwrap(ed.Id));
                         });
            },
            addWorkflowDefinitionAsync = function () {

                return app.showDialog("new.workflow.definition.dialog")
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("WorkflowDefinition", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#workflow.definition.visual/" + ko.unwrap(ed.Id));
                        });
            },
            addTransformDefinitionAsync = function () {
                return app.showDialog("new.transform.definition.dialog")
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("TransformDefinition", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#transform.definition.edit/" + ko.unwrap(ed.Id));
                        });
            },
            addTriggerAsync = function () {

                return app.showDialog("new.trigger.dialog")
                                      .then(function (dialog, result) {
                                          if (result === "OK") {
                                              return checkSource("Trigger", "Id eq '" + ko.unwrap(dialog.id) + "'");
                                          }
                                          return Task.fromResult(0);
                                      }).then(function (trigger) {
                                          if (trigger)
                                              router.navigate("#trigger.setup/" + ko.unwrap(trigger.Id));
                                      });
            },
            addAdapterAsync = function () {

                var url = "";
                return app.showDialog("new.adapter.dialog")
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                url = ko.unwrap(dialog.url);
                                return checkSource("Adapter", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#" + url);
                        });
            },
            addReportDefinitionAsync = function () {

                return app.showDialog("new.report.definition.dialog")
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("ReportDefinition", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#reportdefinition.edit/" + ko.unwrap(ed.Id));
                        });
            },
            addQueryEndpoint = function (ed) {

                return app.showDialog("new.query.endpoint.dialog", function (dialog) {
                    dialog.entity(ed);
                })
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("QueryEndpoint", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#query.endpoint.designer/" + ko.unwrap(ed.Id));
                        });
            },
            addEntityView = function (entityDefinition) {

                return app.showDialog("new.entity.view.dialog", function (dialog) {
                    dialog.entity(entityDefinition);
                })
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("EntityView", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#entity.view.designer/" + ko.unwrap(ed.Id));
                        });
            },
            addWorkflowForm = function (wd) {
                return app.showDialog("new.workflow.form.dialog", function (dialog) {
                    dialog.wd(wd);
                })
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("WorkflowForm", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (wfrm) {
                            if (wfrm)
                                router.navigate("#workflow.form.designer/" + ko.unwrap(wfrm.WorkflowDefinitionId) + "/" + ko.unwrap(wfrm.Id));
                        });
            },
            addEntityForm = function (entityDefinition) {
                return app.showDialog("new.entity.form.dialog", function (dialog) {
                    dialog.entity(entityDefinition);
                })
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("EntityForm", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#entity.form.designer/" + ko.unwrap(ed.EntityDefinitionId) + "/" + ko.unwrap(ed.Id));
                        });
            },
            addOperationEndpoint = function (entityDefinition) {

                return app.showDialog("new.operation.endpoint.dialog", function (dialog) {
                    dialog.entity(entityDefinition);
                })
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("OperationEndpoint", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#operation.endpoint.designer/" + ko.unwrap(ed.Id));
                        });
            },
            addPartialView = function (entityDefinition) {

                return app.showDialog("new.partial.view.dialog", function (dialog) {
                    dialog.entity(entityDefinition);
                })
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("PartialView", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#partial.view.designer/" + ko.unwrap(ed.Entity) + "/" + ko.unwrap(ed.Id));
                        });
            },
            addDashboard = function (entityDefinition) {

                return app.showDialog("new.operation.endpoint.dialog", function (dialog) {
                    dialog.entity(entityDefinition);
                })
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("OperationEndpoint", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#operation.endpoint.designer/" + ko.unwrap(ed.Id));
                        });
            },
            addDialog = function (entityDefinition) {

                return app.showDialog("new.form.dialog.dialog", function (dialog) {
                    dialog.entity(entityDefinition);
                })
                        .then(function (dialog, result) {
                            if (result === "OK") {
                                return checkSource("FormDialog", "Id eq '" + ko.unwrap(dialog.id) + "'");
                            }
                            return Task.fromResult(0);
                        }).then(function (ed) {
                            if (ed)
                                router.navigate("#form.dialog.designer/" + ko.unwrap(ed.Entity) + "/" + ko.unwrap(ed.Id));
                        });
            },
            addDataTransferDefinition = function(dtd){
                return app.showDialog("new.data.transfer.definition.dialog", function (dialog) {
                        dialog.dtd(dtd);
                    })
                    .then(function (dialog, result) {
                        if (result === "OK") {
                            return checkSource("DataTransferDefinition", "Id eq '" + ko.unwrap(dialog.id) + "'");
                        }
                        return Task.fromResult(0);
                    }).then(function (ed) {
                        if (ed)
                            router.navigate("#data.import/" + ko.unwrap(ed.Id));
                    });
            };

        var vm = {
            addCustomFormAsync: customForm.addNew,
            addDataTransferDefinition: addDataTransferDefinition,
            addCustomDialogAsync: customDialog.addNewDialog,
            addPartialViewAsync: partialView.addNewPartialView,
            addCustomScriptAsync: customScript.addNew,
            addValueObjectDefinitionAsync: addValueObjectDefinitionAsync,
            addEntityDefinitionAsync: addEntityDefinitionAsync,
            addAdapterAsync: addAdapterAsync,
            addReportDefinitionAsync: addReportDefinitionAsync,
            addTriggerAsync: addTriggerAsync,
            addTransformDefinitionAsync: addTransformDefinitionAsync,
            addQueryEndpoint: addQueryEndpoint,
            addWorkflowForm: addWorkflowForm,
            addEntityForm: addEntityForm,
            addEntityView: addEntityView,
            addDialog: addDialog,
            addDashboard: addDashboard,
            addPartialView: addPartialView,
            addOperationEndpoint: addOperationEndpoint,
            addWorkflowDefinitionAsync: addWorkflowDefinitionAsync
        };

        return vm;
    });