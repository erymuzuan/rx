﻿/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function(context, logger, router) {

        var recentEntityDefinitions = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function() {
                var tcs = new $.Deferred(),
                  edTask = context.loadAsync({ entity: "EntityDefinition", sort: "ChangedDate desc", size: 5 });

                $.when(edTask).then(function (edLo) {
                    recentEntityDefinitions(edLo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            attached = function(view) {

            };

        var vm = {
            recentEntityDefinitions: recentEntityDefinitions,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
