﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

define([],
    function () {

        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                this.modal.close("OK");
            }

        },
            viewAttached = function () {
                $('#line').focus();
            },
            cancelClick = function () {
                this.modal.close("Cancel");
            };

        var vm = {
            line: ko.observable(),
            okClick: okClick,
            viewAttached: viewAttached,
            cancelClick: cancelClick
        };


        return vm;

    });
