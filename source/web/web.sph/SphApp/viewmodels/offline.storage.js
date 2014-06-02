/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var items = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {
                if(Modernizr.localstorage && navigator.onLine){
                    var text = localStorage.getItem("offline-post"),
                        index = [];
                    if (text) {
                        index = JSON.parse(text);
                        items(index);
                    }
                }
            },
            attached = function (view) {

            };

        var vm = {
            isBusy: isBusy,
            items : items,
            activate: activate,
            attached: attached
        };

        return vm;

    });
