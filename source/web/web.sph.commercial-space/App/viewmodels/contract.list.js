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
            contract = ko.observable(new bespoke.sphcommercialspace.domain.Contract()),
            activate = function() {
                var tcs = new $.Deferred();
                isBusy(true);
                context.loadAsync("Contract", "ContractId gt 0")
                    .then(function(result) {
                        isBusy(false);
                        vm.contracts(result.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            viewAttached = function(view) {
                _uiready.init(view);
            },
            exportList = function (){};

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
            })
        };

        return vm;

    });
