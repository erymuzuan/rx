/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../../core.sph/Scripts/r.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {
        var list = ko.observableArray(),
            severityOptions = ko.observableArray(),
            severity = ko.observable(),
            computer = ko.observable(false),
            timeFrom = ko.observable(moment().subtract(7, "days").format()),
            timeTo = ko.observable(moment().format()),
            isBusy = ko.observable(false),
            computerOptions = ko.observableArray(),
            getKeysAsync = function (field) {
                var tcs = new $.Deferred(),
                    agg = {
                        "aggs": {
                            "category": {
                                "terms": {
                                    "field": field,
                                    "size": 0
                                }
                            }
                        },
                        "fields": [
                            "computer"
                        ]
                    };
                context.searchAsync("log", agg)
                 .then(function (result) {
                     var keys = _(result.aggregations.category.buckets).map(function (v) {
                         return v.key;
                     });
                     tcs.resolve(keys);
                 });

                return tcs.promise();
            },
            activate = function () {

                getKeysAsync("severity").done(severityOptions);

                return getKeysAsync("computer")
                    .then(computerOptions);
            },
            attached = function (view) {

            },
            openDetails = function (log) {
                require(["viewmodels/log.details.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.log(log);

                    app2.showDialog(dialog)
                        .done(function () { });

                });
            },
            query = {
                "sort": [
                 {
                     "time": {
                         "order": "desc"
                     }
                 }
                ]
            },
            executeQuery = function () {
                var q = {
                    "query": {
                        "bool": {
                            "must": []
                        }
                    },
                    "sort": [
                     {
                         "time": {
                             "order": "desc"
                         }
                     }
                    ]
                };

                if (ko.unwrap(computer)) {
                    q.query.bool.must.push({
                        "term": {
                            "computer": {
                                "value": ko.unwrap(computer)
                            }
                        }
                    });
                }
                if (ko.unwrap(severity)) {
                    q.query.bool.must.push({
                        "term": {
                            "severity": {
                                "value": ko.unwrap(severity)
                            }
                        }
                    });
                }
                if (ko.unwrap(timeFrom) && ko.unwrap(timeTo)) {
                    q.query.bool.must.push({
                        "range": {
                            "time": {
                                "from": ko.unwrap(timeFrom),
                                "to": ko.unwrap(timeTo)
                            }
                        }
                    });
                }
                return context.searchAsync("log", q)
                    .then(function(result) {
                        list(result.itemCollection);
                    });
            };

        computer.subscribe(executeQuery);
        severity.subscribe(executeQuery);
        timeFrom.subscribe(executeQuery);
        timeTo.subscribe(executeQuery);


        var vm = {
            openDetails: openDetails,
            computer: computer,
            severity: severity,
            timeFrom: timeFrom,
            timeTo: timeTo,
            computerOptions: computerOptions,
            severityOptions: severityOptions,
            query: query,
            list: list,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
