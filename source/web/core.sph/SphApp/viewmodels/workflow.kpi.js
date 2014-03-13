/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext'],
    function (context) {

        var
            startDate = ko.observable(moment().format('YYYY-MM-DD')),
            endDate = ko.observable(moment().format('YYYY-MM-DD')),
            unit = ko.observable(3600),
            interval = ko.observable(1),
            intervalInSeconds = ko.computed(function () {
                return unit() * interval();
            }),
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
                $('#reportrange').daterangepicker(
                        {
                            ranges: {
                                'Today': [moment(), moment()],
                                'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
                                'Last 7 Days': [moment().subtract('days', 6), moment()],
                                'Last 30 Days': [moment().subtract('days', 29), moment()],
                                'This Month': [moment().startOf('month'), moment().endOf('month')],
                                'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
                            },
                            startDate: moment().subtract('days', 29),
                            endDate: moment()
                        },
                        function (start, end) {
                            startDate(start.format('YYYY-MM-DD'));
                            endDate(end.format('YYYY-MM-DD'));
                        }
                    );
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
                    categories = _(result.elapses.entries).map(function (v) { return v.key; }),
                    unitText = "seconds";
             //   <option value="1">Second</option>
             //   <option value="60">Minute</option>
             //   <option value="3600">Hour</option>
             //   <option value="86400">Day</option>
             //   <option value="604800">Week</option>
                switch (parseInt(unit())) {
                    case 1:
                        unitText = "second";
                        break;
                    case 60:
                        unitText = "minute";
                        break;
                    case 3600:
                        unitText = "hour";
                        break;
                    case 86400:
                        unitText = "day";
                        break;
                    case 604800:
                        unitText = "week";
                        break;
                    default:
                }
                if (parseInt(interval()) > 1) {
                    unitText += "s";
                }
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
                        name: 'Interval ' + interval() + ' ' + unitText,
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
                                   },
                                    {
                                        "range": {
                                            "Initiated": {
                                                "from": startDate,
                                                "to": endDate
                                            }
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
                            "interval": intervalInSeconds
                        }
                    }
                }
            };


        endDate.subscribe(showChartCommand);
        startDate.subscribe(showChartCommand);
        var vm = {
            startDate: startDate,
            endDate: endDate,
            unit: unit,
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
