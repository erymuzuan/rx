/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            activate = function () {
                var tcs = new $.Deferred();
                var loadContractTemplateTask = context.loadAsync("ContractTemplate", "ContractTemplateId gt 0");
                var loadContractTypeOptionsTask = context.getTuplesAsync("ContractTemplate", "ContractTemplateId gt 0", "ContractTemplateId", "Type");

                $.when(loadContractTemplateTask, loadContractTypeOptionsTask).then(function(ctlo, list) {
                    vm.contractTemplateTypes(ctlo.itemCollection);
                    vm.contractTypeOptions(_(list).sortBy(function (b) {
                        return b.Item2;
                    }));

                    tcs.resolve();
                });
                return tcs.promise();
            },
            viewAttached = function () {

                $("#tabstrip").kendoTabStrip({
                    animation: {
                        open: {
                            effects: "fadeIn"
                        }
                    }
                });

                $("#returnLetterTemplate").kendoUpload({
                    async: {
                        saveUrl: "/BinaryStore/Upload",
                        removeUrl: "/BinaryStore/Remove",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function (e) {
                        logger.logError(e, e, this, true);
                    },
                    success: function (e) {
                        logger.log('Your file has been ' + e.operation, e, this, true);
                        var storeId = e.response.storeId;
                        var uploaded = e.operation === "upload";
                        var removed = e.operation != "upload";

                        if (uploaded) {
                           vm.storeId(storeId);
                        }

                        if (removed) {
                            vm.storeId('');
                        }

                    }
                });

            },
            
            addContractType = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({contractType: vm.contractTemplate.Type()});
                context.post(data, "/ContractSetting/SaveContractType")
                    .done(function (e) {
                        logger.log("Saved", "contractsetup", true);
                        vm.contractTemplate.Type('');
                        var type = {
                            Type: ko.observable(e)
                        };
                        vm.contractTemplateTypes.push(type);
                        tcs.resolve(true);
                    });
                return tcs.promise();
            },
            
            addDocument = function () {
                var documentName = {
                    Name: ko.observable(vm.fileName()),
                    StoreId : ko.observable(vm.storeId())
                };
                vm.contractTemplate.DocumentTemplateCollection.push(documentName);
                var tcs = new $.Deferred();
                var data = JSON.stringify({id: vm.contractTemplate.ContractTemplateId() , documentTemplates: ko.mapping.toJS(vm.contractTemplate.DocumentTemplateCollection)});
                context.post(data, "/ContractSetting/SaveDocumentTemplate")
                    .done(function () {
                        logger.log("Saved", "contractsetup", true);
                        vm.fileName('');
                        vm.storeId('');
                        tcs.resolve(true);
                    });
                return tcs.promise();
            },

            saveDocumentTemplate = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({
                    settings: [{
                        Key: "Template.Returned.Letter",
                        Value: vm.returnLetterTemplate()
                    },
                        {
                            Key: "Template.Offer.Letter",
                            Value: vm.offerLetterTemplate()
                        }]
                });
                isBusy(true);

                context.post(data, "/Setting/Save")
                    .then(function (result) {
                        isBusy(false);
                        logger.log('All the setting has been saved', result, this, true);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            activate: activate,
            isBusy: isBusy,
            viewAttached: viewAttached,
            contractTemplateTypes: ko.observableArray([]),
            contractTypeOptions: ko.observableArray([]),
            contractTemplate: {
                ContractTemplateId: ko.observable(''),
                Type: ko.observable(''),
                DocumentTemplateCollection: ko.observableArray([])
            },
            fileName: ko.observable(''),
            storeId: ko.observable(''),
            saveCommand: saveDocumentTemplate,
            addDocumentCommand: addDocument,
            addContractTypeCommand: addContractType

        };

        return vm;
    });