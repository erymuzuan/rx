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
            versions = ko.observableArray(),
            wd = ko.observable(),
            id = ko.observable(),
            activate = function (wdid) {
                id(parseInt(wdid));
                var query1 = String.format("WorkflowDefinitionId eq {0}", wdid),
                    vt = $.get('/WorkflowMonitor/DeployedVersions/' + id()),
                    tcs = new $.Deferred(),
                    wdTask = context.loadOneAsync("WorkflowDefinition", query1);

                versions.removeAll();
                $.when(vt, wdTask).done(function (deployments, b) {
                    wd(b);
                    _(deployments).each(function (v) {
                        if (!v[0]) {
                            return;
                        }
                        context.post(ko.mapping.toJSON(query), "workflow_" + wdid + "_" + ko.unwrap(v[0].Version) + "/search")
                            .then(function (result) {
                                versions.push({
                                    version: v[0].Version,
                                    states: result.facets.state.terms
                                });
                            });

                    });

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
            id: id,
            versions: versions,
            wd: wd,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
