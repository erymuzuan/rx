/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/objectbuilders.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="/Scripts/underscore.js" />
/// <reference path="/Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../scripts/trigger.workflow.g.js" />


define(["plugins/dialog", objectbuilders.system],
    function (dialog, system) {

        var action = ko.observable(new bespoke.sph.domain.SetterAction(system.guid())),
            activate = function () {
                //action(new bespoke.sph.domain.SetterAction(system.guid()));
            },
            attached = function (view) {
                setTimeout(function () {
                    $(view).find("#setter-action-title").focus();
                }, 500);
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
            action: action,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
