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
                var raTask = context.loadOneAsync("RentalApplication", "RentalApplicationId eq " + routeData.rentalApplicationId);
                var templateListTask = context.getTuplesAsync("ContractTemplate", "ContractTemplateId gt 0", "ContractTemplateId", "Type");

                return $.when(raTask, templateListTask).then(function (ra, list) {
                    vm.contractTypeOptions(list);
                    rentalApplication = ra;
                });

            },
            viewAttached = function (view) {
                _uiready.init(view);
            },
            contract = {
                ContractId : ko.observable(),
                ReferenceNo : ko.observable(),
                Type : ko.observable(),
                Date : ko.observable(),
                Value : ko.observable(),
                Title : ko.observable(),
                Remarks : ko.observable(),
                Period : ko.observable(),
                PeriodUnit : ko.observable(),
                StartDate : ko.observable(),
                EndDate : ko.observable(),
                DocumentCollection : ko.observableArray([]),
                TopicCollection : ko.observableArray([]),
                Owner:{
                    Name : ko.observable(),
                    TelephoneNo : ko.observable(),
                    FaxNo : ko.observable(),
                    Email : ko.observable(),
                    Address:{
                        State : ko.observable(),
                        City : ko.observable(),
                        Postcode : ko.observable(),
                        Street : ko.observable()
                    }
                },

                ContractingParty:{
                    Name : ko.observable(),
                    RegistrationNo : ko.observable(),

                    Contact:{
                        Name : ko.observable(),
                        Title : ko.observable(),
                        IcNo : ko.observable(),
                        Role : ko.observable(),
                        MobileNo : ko.observable(),
                        OfficeNo : ko.observable(),
                        Email : ko.observable()
                    },
                    Address: {
                        State: ko.observable(),
                        City: ko.observable(),
                        Postcode: ko.observable(),
                        Street: ko.observable()
                    }
                },
                Tenant:{
                    Name : ko.observable(),
                    RegistrationNo : ko.observable(),
                    Id : ko.observable(),
                    Address:{
                        State : ko.observable(),
                        City : ko.observable(),
                        Postcode : ko.observable(),
                        Street : ko.observable()
                    }

                },
                CommercialSpace:{
                    CommercialSpaceId : ko.observable(),
                    BuildingId : ko.observable(),
                    LotName : ko.observable(),
                    FloorName : ko.observable(),
                    Size : ko.observable(),
                    Category : ko.observable(),
                    RentalRate : ko.observable(),
                    RentalType : ko.observable(),
                    IsOnline : ko.observable(),
                    RegistrationNo : ko.observable(),
                    Status : ko.observable(),
                    ContactPerson : ko.observable(),
                    ContactNo : ko.observable(),
                    State : ko.observable(),
                    City : ko.observable(),
                    BuildingName : ko.observable(),
                    BuildingLot : ko.observable(),
                    LotCollection : ko.observableArray([])
            }

        },

        generateContract = function () {
            var json = JSON.stringify({ rentalApplicationId: rentalApplication.RentalApplicationId(), templateId: vm.selectedTemplateId() });
            return context.post(json, "/Contract/Generate")
                .then(function (t) {
                    ko.mapping.fromJS(ko.mapping.toJS(t), {}, vm.contract);
                });
        },
        addAttachment = function () {
            contract.DocumentCollection.push({
                Title: ko.observable(''),
                Extension: ko.observable(''),
                DocumentVersionCollection: ko.observableArray([])

            });
        },
        startAddTopic = function () {
            cachedTopic = null;

            topic.Text('');
            topic.Title('');
            topic.Description('');
            topic.ClauseCollection([]);

        },
        startAddClause = function () {

            cachedClause = null;
            clause.No('');
            clause.Text('');
            clause.Title('');
            clause.Description('');
        },
        addTopic = function () {
            var clone = ko.mapping.fromJSON(ko.mapping.toJSON(topic));
            template.TopicCollection.push(clone);
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
    addClause = function () {
        var clone = ko.mapping.fromJSON(ko.mapping.toJSON(clause));
        topic.ClauseCollection.push(clone);
        clause.No('');
        clause.Title('');
        clause.Text('');
        clause.Description('');
    },
        cachedTopic,
        cachedClause,
        selectTopic = function (tp, ev) {
            if (cachedTopic) { // copy it back
                ko.mapping.fromJS(ko.mapping.toJS(vm.topic), {}, cachedTopic);
            }
            ko.mapping.fromJS(ko.mapping.toJS(tp), {}, vm.topic);
            cachedTopic = tp;
            var element = $(ev.target);
            element.parents("ul").children()
                .removeClass("selected");
            element.parents("li").addClass("selected");
        },
        selectClause = function (cl, ev) {
            if (cachedClause) { // copy it back
                ko.mapping.fromJS(ko.mapping.toJS(vm.clause), {}, cachedClause);
            }
            ko.mapping.fromJS(ko.mapping.toJS(cl), {}, vm.clause);
            cachedClause = cl;


            var element = $(ev.target);
            element.parents("ul").children()
                .removeClass("selected");
            element.parents("li").addClass("selected");
        },
            save=function() {
                var json = ko.mapping.toJSON({ contract: contract });
                return context.post(json, "Contract/Create")
                    .then(function(c) {
                        console.log(c);
                    });
            };

var vm = {
    isBusy: isBusy,
    activate: activate,
    contract: contract,
    topic: topic,
    clause: clause,
    saveCommand: save,
    selectTopicCommand: selectTopic,
    selectClauseCommand: selectClause,
    addTopicCommand: addTopic,
    startAddClauseCommand: startAddClause,
    startAddTopicCommand: startAddTopic,
    addClauseCommand: addClause,
    addAttachmentCommand: addAttachment,
    generateContractCommand: generateContract,
    contractTypeOptions: ko.observableArray(),
    selectedTemplateId: ko.observable(),
    viewAttached: viewAttached
};

return vm;

});
