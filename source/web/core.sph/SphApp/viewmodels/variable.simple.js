﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="~/Scripts/_utils.js" />

define(["plugins/dialog"],
    function (dialog) {

        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            attached = function (view) {
                setTimeout(function () { $(view).find("#variable-name").focus(); }, 500);
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            attached: attached,
            variable: ko.observable(new bespoke.sph.domain.SimpleVariable()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
