define(["services/datacontext", "services/logger", objectbuilders.config, objectbuilders.router, "services/app", "viewmodels/_custom.forms.routes", "viewmodels/_custom.forms.dialogs", "viewmodels/_custom.forms.partial.views", "viewmodels/_custom.forms.scripts", objectbuilders.app],
    function (context, logger, config, router, app, customForm, customDialog, partialView, customScript,app2) {
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
                                          }).then(function (ed) {
                                              if (ed)
                                                  router.navigate("#trigger.setup/" + ko.unwrap(ed.Id));
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
                };

        var vm = {
            addCustomFormAsync: customForm.addNew,
            addCustomDialogAsync: customDialog.addNewDialog,
            addPartialViewAsync: partialView.addNewPartialView,
            addCustomScriptAsync: customScript.addNew,
            addEntityDefinitionAsync: addEntityDefinitionAsync,
            addAdapterAsync: addAdapterAsync,
            addReportDefinitionAsync: addReportDefinitionAsync,
            addTriggerAsync: addTriggerAsync,
            addTransformDefinitionAsync: addTransformDefinitionAsync,
            addWorkflowDefinitionAsync: addWorkflowDefinitionAsync
        };

        return vm;
    });