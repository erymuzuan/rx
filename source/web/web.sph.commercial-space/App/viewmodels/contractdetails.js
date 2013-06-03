/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            rentalApplication,
            activate = function (routeData) {
                isBusy(true);
                var tcs = new $.Deferred();

                var contractLoaded = function (ctr) {
                    ko.mapping.fromJS(ko.mapping.toJS(ctr), {}, vm.contract);
                    //load audit trails
                    var query = _("EntityId eq <%= contractId %> AND Type eq 'Contract'").template({ contractId: ctr.ContractId()});
                    var query2 = _("EntityId eq <%= rentalApplicationId %> AND Type eq 'RentalApplication'").template({ rentalApplicationId: ctr.RentalApplicationId() });

                    var t1 = context.loadAsync("AuditTrail", query);
                    var t2 = context.loadAsync("AuditTrail", query2);
                    $.when(t1, t2)
                        .then(function (lo,lo2) {

                            var logs = _(lo.itemCollection).union(lo2.itemCollection);

                            vm.auditTrailCollection(logs);
                            isBusy(false);
                            tcs.resolve(true);
                        });
                };

                context.loadOneAsync("Contract", "ContractId eq " + routeData.id)
                    .then(contractLoaded);

                return tcs.promise();

            },
            viewAttached = function (view) {
                _uiready.init(view);
                $('#documents').on('click', 'tr', function (e) {
                    e.preventDefault();
                    ko.mapping.fromJS(ko.mapping.toJS(ko.dataFor(this)), {}, vm.selectedDocument);
                });
            },
            contract = {
                ContractId: ko.observable(),
                ReferenceNo: ko.observable(),
                Type: ko.observable(),
                Date: ko.observable(),
                Value: ko.observable(),
                Title: ko.observable(),
                Remarks: ko.observable(),
                Period: ko.observable(),
                PeriodUnit: ko.observable(),
                StartDate: ko.observable(),
                EndDate: ko.observable(),
                DocumentCollection: ko.observableArray([]),
                TopicCollection: ko.observableArray([]),
                Owner: {
                    Name: ko.observable(),
                    TelephoneNo: ko.observable(),
                    FaxNo: ko.observable(),
                    Email: ko.observable(),
                    Address: {
                        State: ko.observable(),
                        City: ko.observable(),
                        Postcode: ko.observable(),
                        Street: ko.observable()
                    }
                },

                ContractingParty: {
                    Name: ko.observable(),
                    RegistrationNo: ko.observable(),

                    Contact: {
                        Name: ko.observable(),
                        Title: ko.observable(),
                        IcNo: ko.observable(),
                        Role: ko.observable(),
                        MobileNo: ko.observable(),
                        OfficeNo: ko.observable(),
                        Email: ko.observable()
                    },
                    Address: {
                        State: ko.observable(),
                        City: ko.observable(),
                        Postcode: ko.observable(),
                        Street: ko.observable()
                    }
                },
                Tenant: {
                    Name: ko.observable(),
                    RegistrationNo: ko.observable(),
                    Id: ko.observable(),
                    Address: {
                        State: ko.observable(),
                        City: ko.observable(),
                        Postcode: ko.observable(),
                        Street: ko.observable()
                    }

                },
                CommercialSpace: {
                    CommercialSpaceId: ko.observable(),
                    BuildingId: ko.observable(),
                    LotName: ko.observable(),
                    FloorName: ko.observable(),
                    Size: ko.observable(),
                    Category: ko.observable(),
                    RentalRate: ko.observable(),
                    RentalType: ko.observable(),
                    IsOnline: ko.observable(),
                    RegistrationNo: ko.observable(),
                    Status: ko.observable(),
                    ContactPerson: ko.observable(),
                    ContactNo: ko.observable(),
                    State: ko.observable(),
                    City: ko.observable(),
                    BuildingName: ko.observable(),
                    BuildingLot: ko.observable(),
                    LotCollection: ko.observableArray([])
                }

            },


        addAttachment = function () {
            contract.DocumentCollection.push({
                Title: ko.observable(''),
                Extension: ko.observable(''),
                DocumentVersionCollection: ko.observableArray([])

            });
        },
        startAddTopic = function () {
            topic.Text('');
            topic.Title('');
            topic.Description('');
            topic.ClauseCollection([]);

        },
        editedTopic,
        startAddClause = function (tpc) {
            editedTopic = tpc;

            clause.No('');
            clause.Text('');
            clause.Title('');
            clause.Description('');
        },
    addClause = function () {
        var clone = ko.mapping.fromJSON(ko.mapping.toJSON(clause));
        editedTopic.ClauseCollection.push(clone);
        clause.No('');
        clause.Title('');
        clause.Text('');
        clause.Description('');
    },
        addTopic = function () {
            var clone = ko.mapping.fromJSON(ko.mapping.toJSON(topic));
            contract.TopicCollection.push(clone);
            topic.Title('');
            topic.Text('');
            topic.Description('');
            topic.ClauseCollection([]);
        },
    topic = {
        Title: ko.observable(''),
        Text: ko.observable(''),
        Description: ko.observable(''),
        ClauseCollection: ko.observableArray()
    },
    clause = {
        Title: ko.observable(''),
        No: ko.observable(''),
        Text: ko.observable(''),
        Description: ko.observable('')
    },
    removeClause = function (tpc, cls) {
        tpc.ClauseCollection.remove(cls);
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
    clauseDetailsCollapsed = false,
    collapseClauseDetails = function (d, ev) {
        $(ev.target).text(clauseDetailsCollapsed ? "collapse" : "expand");
        if (clauseDetailsCollapsed) {
            $('#clauses textarea').slideDown();
        } else {
            $('#clauses textarea').slideUp();
        }
        clauseDetailsCollapsed = !clauseDetailsCollapsed;
    },
            save = function () {
                var json = ko.mapping.toJSON({ contract: contract });
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
            contract: contract,
            topic: topic,
            clause: clause,
            saveCommand: save,
            addTopicCommand: addTopic,
            startAddClauseCommand: startAddClause,
            startAddTopicCommand: startAddTopic,
            addClauseCommand: addClause,
            removeClauseCommand: removeClause,
            addAttachmentCommand: addAttachment,
            contractTypeOptions: ko.observableArray(),
            selectedTemplateId: ko.observable(),
            viewAttached: viewAttached,
            documentTemplateCollection: ko.observableArray([]),
            selectedDocumentTemplate: ko.observable(),
            startGenerateDocumentCommand: startGenerateDocument,
            generateDocumentCommand: generateDocument,
            documentTitle: ko.observable(),
            documentRemarks: ko.observable(),
            selectedDocument: selectedDocument,
            auditTrailCollection: ko.observableArray(),
            collapseDetailsCommand: collapseClauseDetails
        };

        return vm;

    });
