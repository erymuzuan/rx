/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext'],
    function (context) {

        var
            interval = ko.observable(5),
            id = ko.observable(),
            selectedActivityName = ko.observable(),
            isBusy = ko.observable(false),
            activities = ko.observableArray(),
            activate = function (wdid) {
                id(parseInt(wdid));
                var query1 = String.format("WorkflowDefinitionId eq {0}", wdid),
                    tcs = new $.Deferred();
                context.loadOneAsync("WorkflowDefinition", query1)
                    .done(function (b) {
                        var list = _(b.ActivityCollection()).filter(function (v) {
                            return v.IsAsync();
                        });
                        activities(list);
                        tcs.resolve(true);
                    });

                return tcs.promise();

            },
            attached = function (view) {

            },
            showChartCommand = function () {
                var tcs = new $.Deferred();
                isBusy(true);

                context.searchAsync({
                    entity: "activity"
                }, ko.mapping.toJS(query))
                    .then(function (result) {
                        isBusy(false);
                        drawChart(result.facets);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            drawChart = function (result) {

                var data = _(result.elapses.entries).map(function (v) { return v.count; }),
                    categories = _(result.elapses.entries).map(function (v) { return v.key; });
                $("#chart-div").kendoChart({
                    title: {
                        text: "KPI for " + selectedActivityName()
                    },
                    legend: {
                        position: "bottom"
                    },
                    chartArea: {
                        background: ""
                    },
                    seriesDefaults: {
                        type: "column"
                    },
                    series: [{
                        name: 'Interval ' + interval() + ' seconds',
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
                        format: "{0}%",
                        template: "#= category #: #= value #"
                    }
                });


            },
            query = {
                "query": {
                    "filtered": {
                        "filter": {
                            "and": {
                                "filters": [
                                   {
                                       "term": {
                                           "Name": selectedActivityName
                                       }
                                   },
                                   {
                                       "term": {
                                           "WorkflowDefinitionId": id
                                       }
                                   }
                                ]
                            }
                        }
                    }
                },
                "facets": {
                    "elapses": {
                        "histogram": {
                            "field": "ElapseSeconds",
                            "interval": interval
                        }
                    }
                }
            };

        var vm = {
            interval: interval,
            selectedActivityName: selectedActivityName,
            showChartCommand: showChartCommand,
            activities: activities,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
