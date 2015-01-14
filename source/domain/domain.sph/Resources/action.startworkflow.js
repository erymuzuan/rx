﻿/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/objectbuilders.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />



define(['services/datacontext', 'services/logger', 'plugins/dialog', objectbuilders.system],
    function (context, logger, dialog, system) {

        var action = ko.observable(new bespoke.sph.domain.StartWorkflowAction(system.guid())),
            wdOptions = ko.observableArray(),
            activate = function () {
                var query = "IsActive eq 1";
                var tcs = new $.Deferred();

                context.loadAsync("WorkflowDefinition", query)
                    .then(function (lo) {
                        wdOptions(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    var wd = _(wdOptions()).find(function(v) {
                        return ko.unwrap(v.Id) == ko.unwrap(action().WorkflowDefinitionId);
                    });
                    action().Title(ko.unwrap(wd.Name));
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            action: action,
            activate: activate,
            wdOptions: wdOptions,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });