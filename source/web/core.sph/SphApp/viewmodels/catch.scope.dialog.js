/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


define(['plugins/dialog'],
    function (dialog) {

        var tryOptions = ko.observableArray(),
            catchScope = ko.observable(new bespoke.sph.domain.CatchScope()),
            wd = ko.observable(),
            activities = ko.observableArray(),
            activate = function () {
                var list = _(wd().ActivityCollection())
                    .filter(function (v) {
                        if (v.TryScope()) return false;
                        return v.CatchScope() === catchScope().Id() || !v.CatchScope();
                    });
                activities(list);
            },
            attached = function (view) {
                $(view).on('click', 'input.catch-activities', function () {
                    var act = ko.dataFor(this);
                    if (catchScope().Id() === "") {
                        alert("Please enter name for Catch Scope before proceed");
                        return false;
                    }
                    if ($(this).is(':checked')) {
                        act.CatchScope(catchScope().Id());
                    } else {
                        act.CatchScope(null);
                    }
                });

                // set check for existing activities to which refer to this scope
                _(wd().ActivityCollection())
                   .each(function (v) {
                       if (v.CatchScope() === catchScope().Id()) {
                           $("#csd" + v.WebId()).prop('checked', true);
                       }
                   });
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activate: activate,
            attached: attached,
            wd: wd,
            tryOptions: tryOptions,
            okClick: okClick,
            cancelClick: cancelClick,
            catchScope: catchScope,
            activities: activities
        };


        return vm;

    });
