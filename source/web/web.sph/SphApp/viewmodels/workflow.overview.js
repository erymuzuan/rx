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
                    _(deployments).each(function (vr) {
                        if (!vr) {
                            return;
                        }
                        if (!vr[0]) {
                            return;
                        }
                        if (!vr[0].Version) {
                            return;
                        }
                        context.post(ko.mapping.toJSON(query), "workflow_" + wdid + "_" + ko.unwrap(vr[0].Version) + "/search")
                            .then(function (result) {
                                versions.push({
                                    version: vr[0].Version,
                                    states: result.facets.state.terms
                                });
                            });


                    });

                    tcs.resolve(true);

                });

                return tcs.promise();

            },
            attached = function (view) {

                getExecutionHistogram(id(), _(versions()).last().version)
                    .done(drawExecutionChart);
            },
            query = {
                "facets": {
                    "state": {
                        "terms": {
                            "field": "State"
                        }
                    }
                }
            },
            histogramInterval = ko.observable("day"),
            drawExecutionChart = function (result) {

                var data = _(result.aggregations.execution_histogram).map(function (v) { return v.doc_count; }),
                    categories = _(result.aggregations.execution_histogram).map(function (v) { return moment(v.key).format('DD/MM/YY'); });
                $("#chart-div").kendoChart({
                    title: {
                        text: "Ecution by date interval "
                    },
                    legend: {
                        position: "bottom"
                    },
                    chartArea: {
                        background: ""
                    },
                    seriesDefaults: {
                        type: "line"
                    },
                    series: [{
                        name: 'Interval ' + histogramInterval(),
                        data: data
                    }
                    ],
                    valueAxis: {
                        labels: {
                            format: ''
                        },
                        line: {
                            visible: false
                        }
                    },
                    categoryAxis: {
                        categories: categories,
                        majorGridLines: {
                            visible: false
                        }
                    },
                    tooltip: {
                        visible: true,
                        format: "{0}",
                        template: "#= category #: #= value #"
                    }
                });
            },
            getExecutionHistogram = function (wdid, version) {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(createdDateHistogramQuery);
                console.log(data);

                context.post(data, "/workflow_" + wdid + "_" + version + "/search")
                    .then(function (result) {

                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            createdDateHistogramQuery = {
                "aggs": {
                    "execution_histogram": {
                        "date_histogram": {
                            "field": "CreatedDate",
                            "interval": histogramInterval
                        }
                    }
                }
            };

        var vm = {
            id: id,
            histogramInterval: histogramInterval,
            versions: versions,
            wd: wd,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
