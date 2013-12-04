/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            activate = function (routeData) {
                id(parseInt(routeData.id));
                var query = String.format("WorkflowDefinitionId eq {0}", id()),
                    tcs = new $.Deferred(),
                    vt = $.get('/WorkflowMonitor/DeployedVersions/' + id()),
                    wdt =context.loadOneAsync("WorkflowDefinition", query);
                $.when(vt, wdt)
                    .done(function (versions, b) {
                        wd(b);
                        workflows(versions[0]);
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            viewAttached = function (view) {

            },
            workflows = ko.observable();

        var vm = {
            wd: wd,
            isBusy: isBusy,
            workflows: workflows,
            activate: activate,
            viewAttached: viewAttached
        };

        return vm;

    });
