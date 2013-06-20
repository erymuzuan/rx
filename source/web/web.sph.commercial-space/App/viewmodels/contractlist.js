/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function (context) {

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
            },
            search = function () {
                vm.contracts.removeAll();
                var tcs = new $.Deferred();
                var query = "";
                if (vm.idSsmNo() && vm.contractNo()) {
                    var query1 = String.format("TenantIdSsmNo eq '{0}' and ReferenceNo eq '{1}'", vm.idSsmNo(), vm.contractNo());
                    query = query1;
                }
                if (vm.idSsmNo() && !vm.contractNo()) {
                    var query2 = String.format("TenantIdSsmNo eq '{0}'", vm.idSsmNo());
                    query = query2;
                }
                if (!vm.idSsmNo() && vm.contractNo()) {
                    var query3 = String.format("ReferenceNo eq '{0}'", vm.contractNo());
                    query = query + query3;
                }
                context.loadAsync('Contract', query).done(function (lo) {
                    vm.contracts(lo.itemCollection);
                    tcs.resolve(true);
                });

                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            contracts: ko.observableArray(),
            activate: activate,
            viewAttached: viewAttached,
            idSsmNo: ko.observable(),
            contractNo: ko.observable(),
            searchCommand: search
        };

        return vm;

    });
