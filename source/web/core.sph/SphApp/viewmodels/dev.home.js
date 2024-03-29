﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="~/Scripts/_task.js" />
define(["services/datacontext", "services/logger", objectbuilders.config, objectbuilders.router, "services/app", "services/new-item"],
    function (context, logger, config, router, app, newItemService) {

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

            };

        var vm = {
            recentEntityDefinitions: recentEntityDefinitions,
            recentWorkflowDefinitions: recentWorkflowDefinitions,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            openSnippetsDialog: openSnippetsDialog,
            groups: groups,
            newItemService: newItemService
        };

        return vm;

    });
