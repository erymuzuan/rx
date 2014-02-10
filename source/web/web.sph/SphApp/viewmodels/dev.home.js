/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', objectbuilders.config],
    function (context, logger, config) {

        var isBusy = ko.observable(false),
            activate = function () {
                var groups2 = _(config.routes).chain()
                   .map(function (v) {
                       return v.groupName;
                   })
                   .uniq()
               .map(function (g) {
                   return {
                       groupName: g,
                       routes: _(config.routes).filter(function (v) { return v.groupName === g && v.isAdminPage; })
                   };
               }).
               filter(function (v) { return v.groupName && v.routes.length; })
                   .value();
                groups(groups2);
                return Task.fromResult(true);
            },
            attached = function (view) {

            },
            groups = ko.observableArray(),
            recentWorkflowDefinitions = ko.observableArray(),
            recentEntityDefinitions = ko.observableArray();

        var vm = {
            recentEntityDefinitions: recentEntityDefinitions,
            recentWorkflowDefinitions: recentWorkflowDefinitions,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            groups: groups
        };

        return vm;

    });
