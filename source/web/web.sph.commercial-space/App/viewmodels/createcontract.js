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


define(['services/datacontext', './_contract.clauses'],
    function (context, clauses) {
        var isBusy = ko.observable(false),
            rentalApplication,
            activate = function (routeData) {
                isBusy(true);
                var raTask = context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + routeData.rentalApplicationId);
                var templateListTask = context.getTuplesAsync("ContractTemplate", "ContractTemplateId gt 0", "ContractTemplateId", "Type");
                var logTask = context.loadAsync("AuditTrail", "EntityId eq " + routeData.rentalApplicationId);

                var tcs = new $.Deferred();
                $.when(raTask, templateListTask, logTask).then(function (ra, list, logs) {
                    vm.contractTypeOptions(list);
                    vm.auditTrailCollection(logs.itemCollection);
                    rentalApplication = ra;
                    tcs.resolve(true);
                });

                return tcs.promise();

            },
            viewAttached = function (view) {
                _uiready.init(view);
                $('#documents').on('click', 'tr', function (e) {
                    e.preventDefault();
                    ko.mapping.fromJS(ko.mapping.toJS(ko.dataFor(this)), {}, vm.selectedDocument);
                });
            },
        contract = new bespoke.sphcommercialspace.domain.Contract(),
        generateContract = function () {
            var json = JSON.stringify({ rentalApplicationId: rentalApplication.RentalApplicationId(), templateId: vm.selectedTemplateId() });
            var tcs = new $.Deferred();
            context.post(json, "/Contract/Generate")
                .then(function (t) {
                    ko.mapping.fromJS(ko.mapping.toJS(t), {}, vm.contract);
                    clauses.init(vm.contract);
                    tcs.resolve(t);
                });

            return tcs.promise();
        },
        addAttachment = function () {
            contract.DocumentCollection.push({
                Title: ko.observable(''),
                Extension: ko.observable(''),
                DocumentVersionCollection: ko.observableArray([])

            });
        },
            generateDocument = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({
                    id: contract.ContractId(),
                    templateId: vm.selectedDocumentTemplate(),
                    remarks: vm.documentRemarks(),
                    title: vm.documentTitle()
                });
                context.post(data, "/Contract/GenerateDocument")
                    .then(function (doc) {
                        vm.contract.DocumentCollection.push(doc);
                        tcs.resolve(doc);
                    });
                return tcs.promise();
            },
            selectedDocument = {
                Title: ko.observable(),
                Extension: ko.observable(),
                DocumentVersionCollection: ko.observableArray([])
            },
            startGenerateDocument = function () {
                var tcs = new $.Deferred();
                vm.documentRemarks("");
                vm.documentTitle("");
                context.loadOneAsync("ContractTemplate", "ContractTemplateId eq " + vm.selectedTemplateId())
                    .then(function (t) {
                        vm.documentTemplateCollection(ko.mapping.toJS(t.DocumentTemplateCollection));
                        tcs.resolve(t);
                    });

                return tcs.promise();

            },

            save = function () {
                var json = ko.mapping.toJSON({ contract: contract, rentalApplicationId: rentalApplication.RentalApplicationId() });
                var tcs = new $.Deferred();
                context.post(json, "Contract/Create")
                    .then(function (c) {
                        ko.mapping.fromJS(ko.mapping.toJS(c), {}, vm.contract);
                        tcs.resolve(c);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            contract: contract,
            saveCommand: save,
            addAttachmentCommand: addAttachment,
            generateContractCommand: generateContract,
            contractTypeOptions: ko.observableArray(),
            selectedTemplateId: ko.observable(),
            documentTemplateCollection: ko.observableArray([]),
            selectedDocumentTemplate: ko.observable(),
            startGenerateDocumentCommand: startGenerateDocument,
            generateDocumentCommand: generateDocument,
            documentTitle: ko.observable(),
            documentRemarks: ko.observable(),
            auditTrailCollection: ko.observableArray(),
            selectedDocument: selectedDocument
        };

        return vm;

    });
