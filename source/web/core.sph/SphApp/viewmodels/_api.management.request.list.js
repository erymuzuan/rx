/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../../core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../core.sph/SphApp/schemas/trigger.workflow.g.js" />
/// <reference path="../../../core.sph/SphApp/schemas/form.designer.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app, "knockout"],
    function (context, logger, router, app, ko) {

        const isBusy = ko.observable(false),
            results = ko.observableArray(),
            from = ko.observable(moment().subtract(1, "day").format("YYYY-MM-DD")),
            to = ko.observable(moment().add(1, "day").format("YYYY-MM-DD")),
            query = {
                "query": {
                    "bool": {
                        "must": [
                            {
                                "range": {
                                    "time": {
                                        "gte": from,
                                        "lte": to
                                    }
                                }
                            }
                        ]
                    }
                },
                "aggs": {
                    "path": {
                        "terms": { "field": "request.path.raw" }
                    }
                },
                "fields": [],
                "from": 0,
                "size": 1
            },
            createCharts = function (buckets) {
                const categories = Object.keys(buckets),
                    data = Object.keys(buckets).map(v => { return { category: v, value: buckets[v]}; });
                results(data);
                const chart = $("#request-path-charts").empty().kendoChart({
                    theme: "metro",
                    title: {
                        text: "Request by hour"
                    },
                    legend: {
                        position: "bottom"
                    },
                    seriesDefaults: {
                        labels: {
                            visible: true,
                            format: "{0}"
                        }
                    },
                    series: [
                        {
                            type: "line",
                            data: data
                        }
                    ],
                    categoryAxis: {
                        categories: categories,
                        majorGridLines: {
                            visible: false
                        }
                    },
                    seriesClick: function (e) {

                    }, tooltip: {
                        visible: true,
                        format: "{0}",
                        template: "#= category #: #= value #"
                    }
                }).data("kendoChart");
                console.log(chart);
            },
            search = function () {
                const data = ko.mapping.toJSON(query);
                isBusy(true);

                return context.post(data, `/management-api/request-logs/${ko.unwrap(from)}/${ko.unwrap(to)}`)
                    .then(function (result) {
                        isBusy(false);
                        createCharts(result.Aggregates.path);
                    });
            },
            activate = function () {
            },
            attached = function (view) {
                $(view).find("div.date-range")
                    .daterangepicker(
                        {
                            ranges: {
                                'Today': [moment(), moment()],
                                'Yesterday': [moment().subtract(1, "days"), moment().subtract(1, "days")],
                                'Last 7 Days': [moment().subtract(6, "days"), moment()],
                                'Last 30 Days': [moment().subtract(29, "days"), moment()],
                                'This Month': [moment().startOf("month"), moment().endOf("month")],
                                'Last Month': [
                                    moment().subtract(1, "month").startOf("month"),
                                    moment().subtract(1, "month").endOf("month")
                                ]
                            },
                            startDate: moment().subtract(29, "days"),
                            endDate: moment()
                        },
                        function (start, end) {
                            from(start.format("YYYY-MM-DD"));
                            to(end.format("YYYY-MM-DD"));
                            return search();
                        }
                    );


                return search();
            };

        return {
            from: from,
            to: to,
            results: results,
            isBusy: isBusy,
            search: search,
            activate: activate,
            attached: attached,
            query: query
        };


    });
