define(["services/datacontext", "services/logger", objectbuilders.config, objectbuilders.router, "services/app"],
    function (context, logger, config, router, app) {
    var addEntityDefinitionAsync = function () {

        return app.showDialog("new.entity.definition.dialog")
              .done(function (dialog, result) {
                  if (result === "OK") {
                      router.navigate("#entity.details/" + ko.unwrap(dialog.id));
                  }
              });
    },
            addWorkflowDefinitionAsync = function () {

                return app.showDialog("new.workflow.definition.dialog")
                      .done(function (dialog, result) {
                          if (result === "OK") {
                              router.navigate("#workflow.definition.visual/" + ko.unwrap(dialog.id));
                          }
                      });
            },
            addTransformDefinitionAsync = function () {

                return app.showDialog("new.transform.definition.dialog")
                      .done(function (dialog, result) {
                          if (result === "OK") {
                              router.navigate("#transform.definition.edit/" + ko.unwrap(dialog.id));
                          }
                      });
            },
            addTriggerAsync = function () {

                return app.showDialog("new.trigger.dialog", function (dialog) {
                    dialog.entity(null);
                })
                      .done(function (dialog, result) {
                          if (result === "OK") {
                              router.navigate("#trigger.setup/" + ko.unwrap(dialog.id));
                          }
                      });
            },
            addAdapterAsync = function () {

                return app.showDialog("new.adapter.dialog")
                      .done(function (dialog, result) {
                          if (result === "OK") {
                              router.navigate("#" + ko.unwrap(dialog.url));
                          }
                      });
            },
            addReportDefinitionAsync = function () {

                return app.showDialog("new.report.definition.dialog")
                      .done(function (dialog, result) {
                          if (result === "OK") {
                              router.navigate("#reportdefinition.edit/" + ko.unwrap(dialog.id));
                          }
                      });
            };

    var vm = {
        addEntityDefinitionAsync: addEntityDefinitionAsync,
        addAdapterAsync: addAdapterAsync,
        addReportDefinitionAsync: addReportDefinitionAsync,
        addTriggerAsync: addTriggerAsync,
        addTransformDefinitionAsync: addTransformDefinitionAsync,
        addWorkflowDefinitionAsync: addWorkflowDefinitionAsync
    };

    return vm;
});