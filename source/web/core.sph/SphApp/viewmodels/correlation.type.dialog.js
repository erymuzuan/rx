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

        var correlationType = ko.observable(new bespoke.sph.domain.CorrelationType()),
            wd = ko.observable(),
            activate = function () {

            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function(view) {
                setTimeout(function () { $(view).find("#name").focus(); }, 500);
            };

        var vm = {
            attached: attached,
            activate: activate,
            wd: wd,
            correlationType: correlationType,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
