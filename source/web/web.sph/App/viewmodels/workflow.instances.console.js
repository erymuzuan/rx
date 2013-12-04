/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function () {
                var query = "WorkflowDefinitionId gt 0";
                var tcs = new $.Deferred();

                context.loadAsync("WorkflowDefinition", query)
                    .then(function (lo) {
                        isBusy(false);
                        vm.wdOptions(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();


            },
            viewAttached = function (view) {

            }, search = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.query);
                isBusy(true);

                context.post(data, "/WorkflowMonitor/Search")
                    .then(function (result) {
                        isBusy(false);
                        var items = _(result.$values).map(function (v) {
                            //
                            return context.toObservable(v,/Bespoke\.Sph\.Workflows.*\.(.*?),/);
                        });
                        vm.results(items);
                        tcs.resolve(items);
                    });
                return tcs.promise();
            },
            terminateItems = function(items) {
                
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
            viewAttached: viewAttached,
            wdOptions: ko.observableArray(),
            selectedItems: ko.observableArray(),
            results: ko.observableArray(),
            terminateItems: terminateItems
        };

        return vm;

    });
