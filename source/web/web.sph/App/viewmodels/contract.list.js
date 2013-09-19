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
            contract = ko.observable(new bespoke.sph.domain.Contract()),
            activate = function() {
                return true;
            },
            viewAttached = function(view) {
                _uiready.init(view);
            },
            exportList = function (){},
        search = function () {
            var tcs = new $.Deferred();
            var contractQuery = String.format("ContractId gt 0");

            if (vm.searchTerm.contractNo()) {
                contractQuery = String.format("ReferenceNo eq '{0}'", vm.searchTerm.contractNo());
            }
            if (vm.searchTerm.keyword()) {
                contractQuery += String.format(" or TenantName like '%{0}%'", vm.searchTerm.keyword());
            }
            console.log(contractQuery);
            var contractTask = context.loadAsync("Contract", contractQuery);
            $.when(contractTask)
                .done(function (lo) {
                    vm.contracts(lo.itemCollection);
                    tcs.resolve(true);
                });
            return tcs.promise();
        };

        var vm = {
            isBusy: isBusy,
            contract:contract,
            contracts: ko.observableArray(),
            activate: activate,
            viewAttached: viewAttached,
            idSsmNo: ko.observable(),
            contractNo: ko.observable(),
            toolbar : ko.observable({
                reloadCommand: function () {
                    return activate();
                },
                printCommand: ko.observable({
                    entity: ko.observable("RentalApplication"),
                    id: ko.observable(0),
                    item: contract,
                }),
                exportCommand: exportList,
            }),
            searchTerm: {
                contractNo: ko.observable(),
                keyword: ko.observable()
            },
            searchCommand: search
        };

        return vm;

    });
