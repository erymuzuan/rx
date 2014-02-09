/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var
            isBusy = ko.observable(false),
            recentWorkflowDefinitions = ko.observableArray(),
            recentEntityDefinitions = ko.observableArray(),
            activate = function () {
                var tcs = new $.Deferred(),
                    wdTask = context.loadAsync({ entity: "WorkflowDefinition", sort: "ChangedDate desc", size: 5 }),
                    edTask = context.loadAsync({ entity: "EntityDefinition", sort: "ChangedDate desc", size: 5 });

                $.when(wdTask, edTask).then(function (wdLo, edLo) {
                    recentWorkflowDefinitions(wdLo.itemCollection);
                    recentEntityDefinitions(edLo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            attached = function (view) {

            };

        var vm = {
            recentEntityDefinitions: recentEntityDefinitions,
            recentWorkflowDefinitions: recentWorkflowDefinitions,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
