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

        var
            isBusy = ko.observable(false),
            wd = ko.observable(),
            id = ko.observable(),
            activate = function (wdid) {
                id(parseInt(wdid));
                var query1 = String.format("WorkflowDefinitionId eq {0}", wdid),
                    tcs = new $.Deferred();
                context.loadOneAsync("WorkflowDefinition", query1)
                    .done(function (b) {
                        wd(b);
                        tcs.resolve(true);
                    });

                return tcs.promise();

            },
            attached = function (view) {

            },
            query = {
                "facets": {
                    "state": {
                        "terms": {
                            "field": "State"
                        }
                    }
                }
            };

        var vm = {
            wd: wd,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
