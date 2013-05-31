/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function () {
                var tcs = new $.Deferred();
                isBusy(true);

                context.loadAsync("Contract", "ContractId gt 0")
                    .then(function (result) {
                        isBusy(false);
                        vm.contracts(result.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            viewAttached = function (view) {
                _uiready.init(view);
            };

        var vm = {
            isBusy: isBusy,
            contracts: ko.observableArray(),
            activate: activate,
            viewAttached: viewAttached
        };

        return vm;

    });
