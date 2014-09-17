﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.app],
    function (context, logger, router, app) {

        var isBusy = ko.observable(false),
            activate = function () {
                var query = "Id ne '0'",
                    tcs = new $.Deferred();

                context.loadAsync("WorkflowDefinition", query)
                    .then(function (lo) {
                        isBusy(false);
                        vm.wdOptions(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();


            },
            attached = function (view) {

            }, search = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.query);
                isBusy(true);

                context.post(data, "/WorkflowMonitor/Search")
                    .then(function (result) {
                        isBusy(false);
                        var items = _(result.$values).map(function (v) {
                            return context.toObservable(v, /Bespoke\.Sph\.Workflows.*\.(.*?),/);
                        });
                        vm.results(items);
                        tcs.resolve(items);
                        vm.selectedItems.removeAll();
                    });
                return tcs.promise();
            },
            terminateItems = function () {

                var tcs = new $.Deferred(),
                    instancesId = _(vm.selectedItems()).map(function (v) {
                        return v.Id();
                    }), terminate = function () {

                        var data = JSON.stringify({ instancesId: instancesId });
                        isBusy(true);

                        context.post(data, "/WorkflowMonitor/Terminate")
                            .then(function (result) {
                                isBusy(false);
                                tcs.resolve(result);
                                logger.info('Your instances has been succesfully terminated');
                                
                                // refresh
                                search();
                            });

                    };
                app.showMessage('Are you sure you want terminate these running instances', 'SPH - Workflow', ['Yes', 'No'])
                    .done(function (dr) {
                        if (dr === 'Yes') {
                            terminate();
                        } else {
                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            query: {
                state: ko.observable(),
                workflowDefinitionId: ko.observable(),
                createdDateFrom: ko.observable(moment().startOf('week').format('DD/MM/YYYY HH:mm')),
                createdDateEnd: ko.observable()

            },
            search: search,
            activate: activate,
            attached: attached,
            wdOptions: ko.observableArray(),
            selectedItems: ko.observableArray(),
            results: ko.observableArray(),
            terminateItems: terminateItems
        };

        return vm;

    });
