﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

define(['plugins/dialog', objectbuilders.system],
    function (dialog, system) {

        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            attached = function (view) {
                setTimeout(function () { $(view).find("#email-action-title").focus(); }, 500);
            },
           cancelClick = function () {
               dialog.close(this, "Cancel");
           };

        var vm = {
            attached : attached,
            action: ko.observable(new bespoke.sph.domain.EmailAction(system.guid())),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
