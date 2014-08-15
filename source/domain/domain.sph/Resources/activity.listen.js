/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />



define(['plugins/dialog'],
    function (dialog) {

        var wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            asyncActivities = ko.observableArray(),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function () {
            };

        wd.subscribe(function (d) {
            var asyncs = _(d.ActivityCollection()).filter(function (v) {
                return v.isAsync;
            });
            asyncActivities(asyncs);
        });

        var vm = {
            activity: ko.observable(new bespoke.sph.domain.ListenActivity()),
            wd: wd,
            asyncActivities: asyncActivities,
            attached: attached,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
