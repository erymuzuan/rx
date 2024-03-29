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


define(["plugins/dialog", "services/datacontext"],
    function (dialog, context) {

        const isBusy = ko.observable(false),
            id = ko.observable(),
            instance = ko.observable(),
            wd = ko.observable(),
            tracker = ko.observable(),
            loadWd = function (wf) {
                const query = String.format("Id eq '{0}'", wf.WorkflowDefinitionId()),
                    tcs = new $.Deferred(),
                    wdTask = context.loadOneAsync("WorkflowDefinition", query),
                    trackerTask = context.loadOneAsync("Tracker", "WorkflowId eq '" + ko.unwrap(wf.Id) + "'");
                $.when(wdTask, trackerTask)
                    .done(function (b, t) {
                        wd(b);
                        tracker(t);
                        // sort the
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            activate = function () {

                const query = String.format("Id eq '{0}'", id()),
                    tcs = new $.Deferred();
                context.loadOneAsync("Workflow", query)
                    .done(function (b) {
                        instance(b);
                        loadWd(b).done(tcs.resolve);
                    });

                return tcs.promise();

            },
            attached = function (view) {
                setTimeout(function () {
                    $(view).find("input.search-query").parent().css("margin-right", "40px");
                }, 100);
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        return {
            instance: instance,
            tracker: tracker,
            isBusy: isBusy,
            wd: wd,
            id: id,
            activate: activate,
            attached: attached,
            cancelClick: cancelClick
        };

    });
