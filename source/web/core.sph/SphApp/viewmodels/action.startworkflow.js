/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />



define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var action = ko.observable(new bespoke.sph.domain.StartWorkflowAction()),
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
                        return ko.unwrap(v.WorkflowDefinitionId) == ko.unwrap(action().WorkflowDefinitionId);
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
