﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define([],
    function () {

        var okClick = function (data, ev) {
            if (ev.target.form.checkValidity()) {
                this.modal.close("OK");
            }

        },
            cancelClick = function () {
                this.modal.close("Cancel");
            };

        var vm = {
            variable: ko.observable(new bespoke.sph.domain.ComplexVariable()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
