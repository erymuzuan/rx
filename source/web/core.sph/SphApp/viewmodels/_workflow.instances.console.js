/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            results = ko.observableArray(),
                query = {
                    state: ko.observable(),
                    workflowDefinitionId: id,
                    createdDateFrom: ko.observable(moment().startOf("week").format("DD/MM/YYYY HH:mm")),
                    createdDateEnd: ko.observable(moment().format("DD/MM/YYYY HH:mm"))

                },
            wdOptions = ko.observableArray(),
            selectedItems = ko.observableArray(),
            activate = function (wdid) {
                if (typeof wdid === "object" && typeof wdid.wdid === "string") {
                    id(wdid.wdid);
                } else {
                    id(wdid);
                }
                results.removeAll();
            },
            search = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(query);
                isBusy(true);

                context.post(data, "/WorkflowMonitor/Search")
                    .then(function (result) {
                        isBusy(false);
                        var items = _(result.$values).map(function (v) {
                            return context.toObservable(v, /Bespoke\.Sph\.Workflows.*\.(.*?),/);
                        });
                        results(items);
                        tcs.resolve(items);
                        selectedItems.removeAll();
                    });
                return tcs.promise();
            },
            attached = function () {
                $("#intance-query-date-range").daterangepicker(
                                       {
                                           ranges: {
                                               'Today': [moment(), moment()],
                                               'Yesterday': [moment().subtract(1, "days"), moment().subtract(1, "days")],
                                               'Last 7 Days': [moment().subtract(6, "days"), moment()],
                                               'Last 30 Days': [moment().subtract(29, "days"), moment()],
                                               'This Month': [moment().startOf("month"), moment().endOf("month")],
                                               'Last Month': [moment().subtract(1, "month").startOf("month"), moment().subtract(1, "month").endOf("month")]
                                           },
                                           startDate: moment().subtract(29, "days"),
                                           endDate: moment()
                                       },
                                       function (start, end) {
                                           query.createdDateFrom(start.format("YYYY-MM-DD"));
                                           query.createdDateEnd(end.format("YYYY-MM-DD"));
                                           return search();
                                       }
                                   );
            },
            terminateItems = function () {

                var tcs = new $.Deferred(),
                    instancesId = _(selectedItems()).map(function (v) {
                        return v.WorkflowId();
                    }), terminate = function () {

                        var data = JSON.stringify({ instancesId: instancesId });
                        isBusy(true);

                        context.post(data, "/WorkflowMonitor/Terminate")
                            .then(function (result) {
                                isBusy(false);
                                tcs.resolve(result);
                                logger.info("Your instances has been succesfully terminated");

                                // refresh
                                search();
                            });

                    };
                app.showMessage("Are you sure you want terminate these running instances", "SPH - Workflow", ["Yes", "No"])
                    .done(function (dr) {
                        if (dr === "Yes") {
                            terminate();
                        } else {
                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            id: id,
            search: search,
            activate: activate,
            attached: attached,
            results: results,
            query: query,
            wdOptions: wdOptions,
            selectedItems: selectedItems,
            terminateItems: terminateItems
        };

        return vm;

    });
