﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


define(["services/datacontext", "services/logger", "plugins/dialog"],
    function(context, logger, dialog) {

        var attached = function (view) {
                setTimeout(function () { $(view).find("#route-parameter-field-name").focus(); }, 500);
            }, okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            field: ko.observable(new bespoke.sph.domain.RouteParameterField()),
            attached: attached,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
