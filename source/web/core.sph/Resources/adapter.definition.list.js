/// <reference path="../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var adapterOptions = ko.observableArray(),
            getAdapterType = function (adapter) {

                return /^.*?,(.*?).adapter/.exec(ko.unwrap(adapter.$type))[1].trim();
            },
            isBusy = ko.observable(false),
            adapters = ko.observableArray(),
            activate = function () {

                var tcs = new $.Deferred();
                $.get("adapter/installed-adapters", function(d) {
                    adapterOptions(d);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            attached = function (view) {

            },
            getDesigner = function ($type) {
                var item = _(adapterOptions()).find(function(v) {
                    return ko.unwrap(v.adapter.$type) === ko.unwrap($type);
                });
                if (item.designer) {
                    return item.designer;
                }
                return {};
            };

        var vm = {
            adapterOptions: adapterOptions,
            getAdapterType: getAdapterType,
            getDesigner: getDesigner,
            adapters : adapters,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
