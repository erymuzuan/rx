/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['plugins/dialog'],
    function (dialog) {
        console.log("try.scope.dialog.js");

        var activities = ko.observableArray(),
            tryScope = ko.observable(new bespoke.sph.domain.TryScope()),
            wd = ko.observable(),
            activate = function () {
                var list = _(wd().ActivityCollection())
                        .filter(function (v) {
                            if (v.CatchScope()) return false;
                            return v.TryScope() === tryScope().Id() || !v.TryScope();
                        });
                activities(list);
            },
            attached = function (view) {
                $(view).on('click', 'input[type=checkbox]', function () {

                    var act = ko.dataFor(this);

                    if ($(this).is(':checked')) {
                        act.TryScope(tryScope().Id());
                        $("#" + act.WebId()).css("background-color", "red");
                    } else {
                        act.TryScope(null);
                        $("#" + act.WebId()).css("background-color", "");
                    }
                });

                // set check for existing activities to which refer to this scope
                var init = function () {
                    _(wd().ActivityCollection())
                       .each(function (v) {
                           if (v.TryScope() === tryScope().Id()) {
                               $("#tsd" + v.WebId()).prop('checked', true);
                           }
                       });
                };
                setTimeout(init, 500);
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
            wd: wd,
            tryScope: tryScope,
            okClick: okClick,
            cancelClick: cancelClick,
            activate: activate,
            activities2: activities,
            attached: attached
        };


        return vm;

    });
