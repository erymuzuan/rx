﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../partial/ExecutedActivity.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            instance = ko.observable(),
            wd = ko.observable(),
            tracker = ko.observable(),
            loadWd = function (wf) {
                var query = String.format("WorkflowDefinitionId eq {0}", wf.WorkflowDefinitionId()),
                    tcs = new $.Deferred(),
                    wdTask = context.loadOneAsync("WorkflowDefinition", query),
                    trackerTask = context.loadOneAsync("Tracker", "WorkflowId eq " + wf.WorkflowId());
                $.when(wdTask, trackerTask)
                    .done(function (b, t) {
                        wd(b);
                        tracker(t);
                        // sort the
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            activate = function (idArg) {
                id(parseInt(idArg));
                var query = String.format("WorkflowId eq {0}", id()),
                    tcs = new $.Deferred();
                context.loadOneAsync("Workflow", query)
                    .done(function (b) {
                        var wf = context.toObservable(b);
                        instance(wf);
                        loadWd(wf).done(tcs.resolve);
                    });

                return tcs.promise();

            },
            attached = function () {
                
            };

        var vm = {
            instance: instance,
            tracker: tracker,
            isBusy: isBusy,
            wd: wd,
            activate: activate,
            attached: attached
        };

        return vm;

    });
