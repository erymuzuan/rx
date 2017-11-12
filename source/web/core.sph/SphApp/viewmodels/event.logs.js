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
    function (context) {
        const list = ko.observableArray(),
            severityOptions = ko.observableArray(),
            selectedSeverities = ko.observableArray(),
            sourceOptions = ko.observableArray(),
            selectedSources = ko.observableArray(),
            logOptions = ko.observableArray(),
            selectedLogs = ko.observableArray(),
            logId = ko.observable(),
            selectedComputers = ko.observableArray(),
            timeFrom = ko.observable(moment().subtract(7, "days").format()),
            timeTo = ko.observable(moment().format()),
            isBusy = ko.observable(false),
            computerOptions = ko.observableArray(),
            openDetails = function (log) {
                require(["viewmodels/log.details.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.log(log);

                    app2.showDialog(dialog)
                        .done(function () { });

                });
            },
            query = ko.observable({
                "Sorts": [
                 {"Path": "time", "Direction" : "desc"}
                ],
                "Aggregates" : [],
                "Fields" :[]
            }),
            getKeysAsync = function (field) {
                const tcs = new $.Deferred(),
                    query2 = ko.toJS(query);

                query2.Aggregates.push({$type:"Bespoke.Sph.Domain.GroupAggregate, domain.sph", "Path" : field, "Name" : "category"});

                query2.Fields.push("computer");

                context.searchAsync("log", `$orderby=time desc&$apply=groupby(${field}, aggregate($count with count as Count))`)
                 .then(function (result) {
                     const buckets = result.aggregations.category.buckets;
                     tcs.resolve(buckets);
                 });

                return tcs.promise();
            },
            searchById = function () {
                return $.getJSON(`/management-api/logs/${ko.unwrap(logId)}`)
                    .done(function (r) {
                        openDetails(r);
                    });
            },
            activate = function () {

                getKeysAsync("severity").done(severityOptions);
                getKeysAsync("source").done(sourceOptions);
                getKeysAsync("log").done(logOptions);

                return getKeysAsync("computer")
                    .then(computerOptions);
            },
            attached = function (view) {
                $(view).find("form#filter-logs-form").one("submit", function (e) {
                    e.preventDefault();
                    searchById();
                });
            },
            executeQuery = function () {
                const q = {
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
                },
                    pushTerms = function (term, options) {
                        const selecteditems = ko.unwrap(options),
                            qt = {
                                "terms": {
                                }
                            };
                        qt.terms[term] = selecteditems;
                        if (selecteditems.length > 0) {
                            q.query.bool.must.push(qt);
                        }
                    };
                pushTerms("computer", selectedComputers);
                pushTerms("severity", selectedSeverities);
                pushTerms("source", selectedSources);
                pushTerms("log", selectedLogs);

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

                query(q);

            };

        selectedComputers.subscribe(executeQuery, null, "arrayChange");
        selectedSeverities.subscribe(executeQuery, null, "arrayChange");
        selectedSources.subscribe(executeQuery, null, "arrayChange");
        selectedLogs.subscribe(executeQuery, null, "arrayChange");
        timeFrom.subscribe(executeQuery);
        timeTo.subscribe(executeQuery);
        query.subscribe(activate);



        const vm = {
            logId: logId,
            searchById: searchById,
            openDetails: openDetails,
            computerOptions: computerOptions,
            severityOptions: severityOptions,
            sourceOptions: sourceOptions,
            selectedComputers: selectedComputers,
            selectedSeverities: selectedSeverities,
            selectedSources: selectedSources,
            logOptions: logOptions,
            selectedLogs: selectedLogs,
            timeFrom: timeFrom,
            timeTo: timeTo,
            query: query,
            list: list,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: function () { },
                removeCommand: function () { },
                commands: ko.observableArray([
                    {
                        command: function () {
                           // return changed(1, 20);
                        },
                        caption: "Reload",
                        icon: "bowtie-icon bowtie-navigate-refresh",
                        id: ""
                    }
                ])
            }
        };

        return vm;

    });
