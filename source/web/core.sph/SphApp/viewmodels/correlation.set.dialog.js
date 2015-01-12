﻿/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


define(['plugins/dialog'],
    function(dialog) {

        var typeOptions = ko.observableArray(),
            correlationSet = ko.observable(new bespoke.sph.domain.CorrelationSet()),
            wd = ko.observable(),
            activate = function() {
                typeOptions(wd().CorrelationTypeCollection());
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activate: activate,
            wd: wd,
            typeOptions: typeOptions,
            correlationSet:correlationSet,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
