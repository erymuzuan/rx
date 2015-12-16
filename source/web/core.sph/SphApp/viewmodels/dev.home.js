/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="~/Scripts/_task.js" />
define(["services/datacontext", "services/logger", objectbuilders.config, objectbuilders.router, "services/app"],
    function (context, logger, config, router, app) {

        var isBusy = ko.observable(false),
            groups = ko.observableArray(),
            recentWorkflowDefinitions = ko.observableArray(),
            recentEntityDefinitions = ko.observableArray(),
            activate = function () {
                var groups2 = _(config.routes).chain()
                   .map(function (v) {
                       return v.groupName;
                   })
                   .uniq()
               .map(function (g) {
                   return {
                       groupName: g,
                       routes: _(config.routes).filter(function (v) { return v.groupName === g && v.isAdminPage && v.startPageRoute; })
                   };
               }).
               filter(function (v) { return v.groupName && v.routes.length; })
                   .value();
                groups(groups2);
                return Task.fromResult(true);
            },
            attached = function () {

            },
            openSnippetsDialog = function () {

            },
            addEntityDefinitionAsync = function () {

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

              return app.showDialog("new.trigger.dialog")
                    .done(function (dialog, result) {
                        if (result === "OK") {
                            router.navigate("#trigger.setup/" + ko.unwrap(dialog.id));
                        }
                    });
            };

        var vm = {
            recentEntityDefinitions: recentEntityDefinitions,
            recentWorkflowDefinitions: recentWorkflowDefinitions,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            openSnippetsDialog: openSnippetsDialog,
            groups: groups,
            addEntityDefinitionAsync: addEntityDefinitionAsync,
            addTriggerAsync: addTriggerAsync,
            addTransformDefinitionAsync: addTransformDefinitionAsync,
            addWorkflowDefinitionAsync: addWorkflowDefinitionAsync
        };

        return vm;

    });
