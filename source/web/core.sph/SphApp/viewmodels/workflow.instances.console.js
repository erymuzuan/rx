/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
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
            query = {
                state: ko.observable(),
                workflowDefinitionId: ko.observable(),
                createdDateFrom: ko.observable(moment().startOf("week").format("DD/MM/YYYY HH:mm")),
                createdDateEnd: ko.observable()

            },
            wdOptions = ko.observableArray(),
            results = ko.observableArray(),
            selectedItems = ko.observableArray(),
            activate = function () {
                var q = "Id ne '0'",
                    tcs = new $.Deferred();

                context.loadAsync("WorkflowDefinition", q)
                    .then(function (lo) {
                        isBusy(false);
                        wdOptions(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();


            },
            attached = function (view) {

            }, search = function () {
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
            terminateItems = function () {

                var tcs = new $.Deferred(),
                    instancesId = _(selectedItems()).map(function (v) {
                        return v.Id();
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
            },
        openLogs = function (wf) {
            require(["viewmodels/workflow.activities", "durandal/app"], function (dialog, app2) {
                dialog.id(ko.unwrap(wf.Id));
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {


                        }
                    });
            });
        };

        var vm = {
            isBusy: isBusy,
            openLogs: openLogs,
            query: query,
            search: search,
            activate: activate,
            attached: attached,
            wdOptions: wdOptions,
            selectedItems: selectedItems,
            results: results,
            terminateItems: terminateItems
        };

        return vm;

    });
