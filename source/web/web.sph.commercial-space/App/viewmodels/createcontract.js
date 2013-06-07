/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="./_contract.clauses.js" />


define(['services/datacontext', './_contract.clauses', './_audittrail.list'],
    function (context, clauses, audittrailvm) {
        var isBusy = ko.observable(false),
            application,
            activate = function (routeData) {
                isBusy(true);
                var raTask = context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + routeData.rentalApplicationId);
                var templateListTask = context.getTuplesAsync("ContractTemplate", "ContractTemplateId gt 0", "ContractTemplateId", "Type");
                var logTask = audittrailvm.activate("RentalApplication", routeData.rentalApplicationId);
                
                var tcs = new $.Deferred();
                $.when(raTask, templateListTask, logTask).then(function (ra, list) {
                    vm.contractTypeOptions(list);
                    application = ra;
                    tcs.resolve(true);
                });

                return tcs.promise();

            },
            viewAttached = function (view) {
                _uiready.init(view);
            
            },
        generateContract = function () {
            var json = JSON.stringify({ rentalApplicationId: application.RentalApplicationId(), templateId: vm.selectedTemplateId() });
            var tcs = new $.Deferred();
            context.post(json, "/Contract/Generate")
                .then(function (t) {
                    vm.contract(t);
                    clauses.init(vm.contract);
                    tcs.resolve(t);
                });

            return tcs.promise();
        },
            save = function () {
                var json = ko.mapping.toJSON({ contract: vm.contract(), rentalApplicationId: application.RentalApplicationId() });
                var tcs = new $.Deferred();
                context.post(json, "Contract/Create")
                    .then(function (c) {
                        vm.contract(c);
                        tcs.resolve(c);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            contract: ko.observable(new bespoke.sphcommercialspace.domain.Contract()),
            selectedTemplateId: ko.observable(),
            saveCommand: save,
            generateContractCommand: generateContract,
            contractTypeOptions: ko.observableArray()
        };

        return vm;

    });
