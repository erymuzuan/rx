/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/_uiready.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            activate = function (routeData) {
                isBusy(true);
                var id = parseInt(routeData.id);
                template().ContractTemplateId(id);

                // reset
                if (id === 0) {

                    template(new bespoke.sphcommercialspace.domain.ContractTemplate());
                    editedTopic(new bespoke.sphcommercialspace.domain.Topic());
                    editedClaus(new bespoke.sphcommercialspace.domain.Clause());

                }

                return context.loadOneAsync("ContractTemplate", "ContractTemplateId eq " + routeData.id)
                    .then(function (e) {
                        if (e) {
                            ko.mapping.fromJS(ko.mapping.toJS(e), {}, vm.template);
                        }
                        isBusy(false);
                    });
            },
            template = ko.observable(new bespoke.sphcommercialspace.domain.ContractTemplate()),
            configureUpload = function (element, index, attachment) {

                $(element).find("input[type=file]").kendoUpload({
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
                        logger.log('Your file has been uploaded', e, "route/create", true);
                        attachment.StoreId(e.response.storeId);
                    }
                });
            },
            viewAttached = function (view) {
                _uiready.init(view);
            },
            addAttachment = function () {
                template.DocumentTemplateCollection.push({
                    Name: ko.observable(''),
                    StoreId: ko.observable('')
                });
            },
            
            // ============= TOPICS ============================= //
            existingTopic = false,
            editedTopic = ko.observable(new bespoke.sphcommercialspace.domain.Topic()),
            startAddTopic = function () {
                editedTopic(new bespoke.sphcommercialspace.domain.Topic());
                existingTopic = null;
            },
            topicDialogOk = function () {
                if (existingTopic) {
                    template().TopicCollection.replace(existingTopic,editedTopic());
                } else {
                    template().TopicCollection.push(editedTopic());
                }

                editedTopic(new bespoke.sphcommercialspace.domain.Topic());
            },
            editTopic = function (tp) {
                var clone = ko.mapping.fromJS(ko.mapping.toJS(tp));
                editedTopic(clone);
                existingTopic = tp;
            },
            

            // *-*-*-* CLAUSES *^*^*^

        editedClaus = ko.observable(),
            startAddClause = function () {
                if (selectedTopic() === null) {
                    console.log("no selected topic");
                    return;
                }
                editedClaus(new bespoke.sphcommercialspace.domain.Clause());
            },
        addClause = function () {
            var clone = ko.mapping.fromJSON(ko.mapping.toJSON(editedClaus));
            selectedTopic().ClauseCollection.push(clone);
            editedClaus(new bespoke.sphcommercialspace.domain.Clause());
        },
            cachedClause,
            selectedTopic = ko.observable(),
            selectTopic = function (tp, ev) {
                selectedTopic(tp);
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
            save = function () {



                var data = ko.mapping.toJSON({ template: template });
                return context.post(data, "/ContractSetting/Save")
                    .then(function (e) {
                        ko.mapping.fromJS(e, {}, vm.template);
                        isBusy(false);
                    });
            },
            removeDocument = function (doc) {
                template.DocumentTemplateCollection.remove(doc);
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            addAttachmentCommand: addAttachment,
            selectTopicCommand: selectTopic,
            selectClauseCommand: selectClause,
            topicDialogOk: topicDialogOk,
            editTopic: editTopic,
            editedTopic: editedTopic,
            selectedTopic: selectedTopic,
            addClauseCommand: addClause,
            startAddTopicCommand: startAddTopic,
            startAddClauseCommand: startAddClause,
            toolbar: {
                saveCommand: save,
                printCommand: save,
                emailCommand: save,
                removeCommand: save,
                reloadCommand: function () {
                    return activate({ id: vm.template.ContractTemplateId() });
                },
                commands: ko.observableArray([{
                    command: save,
                    icon: "icon-check",
                    caption: "Approve"
                }, {
                    command: save,
                    icon: "icon-bolt",
                    caption: "Reject"
                }
                ])
            },
            statusbar: {
                text: ko.observable(),
                progress: ko.observable()
            },
            configureUpload: configureUpload,
            template: template,
            topic: editedTopic,
            clause: editedClaus,
            contractTypeTemplate: ko.observable(),
            removeDocumentCommand: removeDocument,
            viewAttached: viewAttached
        };

        return vm;

    });
