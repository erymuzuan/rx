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

        self = this,
            selectedActivities = ko.observableArray(),
            activities = ko.observableArray(),
            activate = function () {
                activities = (wd().ActivityCollection());
            },
            attached = function (view) {
                $(view).on('click', 'input[type=checkbox]', function () {
                    console.log("clicking");
                    console.log(this);
                    var dll = ko.dataFor(this);
                    console.log(dll);
                    if ($(this).is(':checked')) {
                        selectedActivities.push(dll);
                        //console.log("selectedActivities" + selectedActivities);
                        console.log(this);
                        console.log($(this));
                        console.log(dll);
                        console.log(dll.WebId());
                        $("#" + dll.WebId()).css("background-color", "red");
                    } else {
                        selectedActivities.remove(dll);
                        console.log("selectedActivities" + selectedActivities);
                    }
                });


            }

        var tryScope = ko.observable(new bespoke.sph.domain.TryScope()),
            wd = ko.observable(),
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
            activities: activities,
            attached: attached
        };


        return vm;

    });
