/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />



define(["plugins/dialog"],
    function (dialog) {

        var activity = ko.observable(new bespoke.sph.domain.ListenActivity()),
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
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
                if (ko.unwrap(v.WebId) === ko.unwrap(activity().WebId)) {
                    return false;
                }
                return ko.unwrap(v.IsAsync);
            });
            asyncActivities(asyncs);
        });

        var vm = {
            activity: activity,
            wd: wd,
            asyncActivities: asyncActivities,
            attached: attached,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
