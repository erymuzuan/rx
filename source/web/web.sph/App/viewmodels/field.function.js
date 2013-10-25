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
            },
            viewAttached = function (view) {

                $('#script-help-buttton').popover({
                    title: 'C# scripting help',
                    content: $('#script-help-content').html(),
                    html: true
                });


            };

        var vm = {
            field: ko.observable(new bespoke.sph.domain.FunctionField()),
            okClick: okClick,
            cancelClick: cancelClick,
            viewAttached: viewAttached
        };


        return vm;

    });
