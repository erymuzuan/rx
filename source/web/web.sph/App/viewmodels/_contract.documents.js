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
    function (context) {
        var isBusy = ko.observable(false),
            document = ko.observable(),
            contractType = ko.observable(),
            init = function (contract) {
                contractType(contract.Type());
                vm.contractId(contract.ContractId());
                contract.Type.subscribe(contractTypeChanged);
                var documents = _(contract.DocumentCollection()).map(function (t) {
                    return t;
                });
                vm.documentCollection(documents);
            },
            activate = function () {

            },
            viewAttached = function () {
                $('#documents').on('click', 'tr', function (e) {
                    e.preventDefault();
                    ko.mapping.fromJS(ko.mapping.toJS(ko.dataFor(this)), {}, vm.selectedDocument);
                });
            },
            addAttachment = function () {
                documentCollection.push(document);
            },
            generateDocument = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({
                    id: vm.contractId(),
                    templateId: vm.selectedDocumentTemplate(),
                    remarks: vm.documentRemarks(),
                    title: vm.documentTitle()
                });
                context.post(data, "/Contract/GenerateDocument")
                    .then(function (doc) {
                        vm.documentCollection.push(doc);
                        tcs.resolve(doc);
                    });
                return tcs.promise();
            },
            selectedDocument = new bespoke.sph.domain.Document(),
            startGenerateDocument = function () {
                var tcs = new $.Deferred();
                vm.documentRemarks("");
                vm.documentTitle("");
                context.loadOneAsync("ContractTemplate", "Type eq '" + contractType() + "'")
                    .then(function (t) {
                        vm.documentTemplateCollection(ko.mapping.toJS(t.DocumentTemplateCollection));
                        $('#documents-template').modal();
                        tcs.resolve(t);
                    });

                return tcs.promise();

            },

            contractTypeChanged = function (type) {
                contractType(type);
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            init: init,
            contractId: ko.observable(),
            startGenerateDocumentCommand: startGenerateDocument,
            generateDocumentCommand: generateDocument,
            documentTemplateCollection: ko.observableArray([]),
            selectedDocumentTemplate: ko.observable(),
            addAttachmentCommand: addAttachment,
            documentCollection: ko.observableArray(),
            documentTitle: ko.observable(),
            documentRemarks: ko.observable(),
            selectedDocument: selectedDocument
        };

        return vm;

    });
