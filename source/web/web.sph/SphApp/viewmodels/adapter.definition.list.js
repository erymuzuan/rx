﻿/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var getAdapterType = function(adapter){

                return /^.*?,(.*?).adapter/.exec(ko.unwrap(adapter.$type))[1].trim();
            },
            isBusy = ko.observable(false),
            adapters = ko.observableArray(),
            activate = function () {
               

            },
            attached = function (view) {

            };

        var vm = {
            getAdapterType : getAdapterType,
            adapters : adapters,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
