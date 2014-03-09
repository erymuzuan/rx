/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function(context, logger, router) {

        var
            recentWorkflowDefinitions = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function() {
                var tcs = new $.Deferred(),
                  wdTask = context.loadAsync({ entity: "WorkflowDefinition", sort: "ChangedDate desc", size: 5 });

                $.when(wdTask).then(function (wdLo) {
                    recentWorkflowDefinitions(wdLo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            attached = function(view) {

            };

        var vm = {
            recentWorkflowDefinitions: recentWorkflowDefinitions,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
