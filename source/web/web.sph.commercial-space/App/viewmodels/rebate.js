/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function(context) {

        var isBusy = ko.observable(false),
            rebateprint = ko.observable(new bespoke.sphcommercialspace.domain.Rebate()),
            activate = function() {
                var tcs = new $.Deferred();
                var rebateTask = context.loadAsync("Rebate", "RebateId gt 0");
                var contractTask = context.getTuplesAsync("Contract", "ContractId gt 0", "ReferenceNo", "ReferenceNo");
                $.when(rebateTask,contractTask).done(function(lo,list) {
                    vm.rebateCollection(lo.itemCollection);
                    vm.contracts(list);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            save = function() {
                var tcs = new $.Deferred();
                var rebate = ko.mapping.toJS(vm.rebate());
                var data = JSON.stringify({rebate : rebate});
                isBusy(true);
                context.post(data, "/Rebate/Save")
                    .then(function(result) {
                        isBusy(false);
                        vm.rebateCollection.push(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            exportList = function (){};

        var vm = {
            isBusy: isBusy,
            activate: activate,
            rebateCollection: ko.observableArray(),
            contracts: ko.observableArray(),
            rebate: ko.observable(new bespoke.sphcommercialspace.domain.Rebate()),
            rebateprint : rebateprint,
            contractTitle: ko.observable(''),
            saveCommand:save,
            toolbar: ko.observable({
                reloadCommand: function () {
                    return activate();
                },
                printCommand: ko.observable({
                    entity: ko.observable("Rebate"),
                    id: ko.observable(0),
                    item: rebateprint,
                }),
                exportCommand: exportList
                })
        };

        vm.rebate().ContractNo.subscribe(function (contractNo) {
            var query = String.format("ReferenceNo eq '{0}'", contractNo);
            var tcs = new $.Deferred();
            context.loadOneAsync("Contract", query)
                .done(function (b) {
                    if (b) {
                       vm.contractTitle(b.Title()); 
                    }
                    
                    tcs.resolve(true);
                });

            return tcs.promise();
        });
        return vm;

    });
