/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/_uiready.js" />


define(['services/datacontext', 'services/logger', 'services/jsonimportexport'],
    function (context, logger, exim) {

        var isBusy = ko.observable(false),
            activate = function(routeData) {
                isBusy(true);
                var id = parseInt(routeData.id);
                template().ContractTemplateId(id);

                if (id === 0) {

                    template(new bespoke.sphcommercialspace.domain.ContractTemplate());
                    editedTopic(new bespoke.sphcommercialspace.domain.Topic());
                    editedClause(new bespoke.sphcommercialspace.domain.Clause());

                }

                return context.loadOneAsync("ContractTemplate", "ContractTemplateId eq " + routeData.id)
                    .then(function(e) {
                        if (e) {
                            template(e);
                        }
                        isBusy(false);
                    });
            },
            template = ko.observable(new bespoke.sphcommercialspace.domain.ContractTemplate()),
            configureUpload = function(element, index, attachment) {

                $(element).find("input[type=file]").kendoUpload({
                    async: {
                        saveUrl: "/BinaryStore/Upload",
                        removeUrl: "/BinaryStore/Remove",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function(e) {
                        logger.logError(e, e, this, true);
                    },
                    success: function(e) {
                        logger.log('Your file has been uploaded', e, "route/create", true);
                        attachment.StoreId(e.response.storeId);
                    }
                });
            },
            viewAttached = function(view) {
                _uiready.init(view);
            },
            addAttachment = function() {
                template().DocumentTemplateCollection.push(new bespoke.sphcommercialspace.domain.DocumentTemplate());
            },
            // ======================== TOPICS ============================= //
            existingTopic = null,
            editedTopic = ko.observable(new bespoke.sphcommercialspace.domain.Topic()),
            startAddTopic = function() {
                editedTopic(new bespoke.sphcommercialspace.domain.Topic());
                existingTopic = null;
            },
            topicDialogOk = function() {
                if (existingTopic) {
                    template().TopicCollection.replace(existingTopic, editedTopic());
                } else {
                    template().TopicCollection.push(editedTopic());
                }
                editedTopic(new bespoke.sphcommercialspace.domain.Topic());
            },
            editTopic = function(tp) {
                var clone = ko.mapping.fromJS(ko.mapping.toJS(tp));
                editedTopic(clone);
                existingTopic = tp;
            },
            selectedTopic = ko.observable(new bespoke.sphcommercialspace.domain.Topic()),
            selectTopic = function(tp, ev) {
                selectedTopic(tp);
                var element = $(ev.target);
                element.parents("ul").children()
                    .removeClass("selected");
                element.parents("li").addClass("selected");

            },
            // =========================  CLAUSES ======================== //

            existingClause = null,
            editedClause = ko.observable(),
            startAddClause = function() {
                if (selectedTopic() === null) {
                    console.log("no selected topic");
                    return;
                }
                editedClause(new bespoke.sphcommercialspace.domain.Clause());
                existingClause = null;
            },
            clauseDialogButtonOk = function() {

                if (existingClause) {
                    selectedTopic().ClauseCollection.replace(existingClause, editedClause());
                } else {
                    selectedTopic().ClauseCollection.push(editedClause());
                }
                editedClause(new bespoke.sphcommercialspace.domain.Clause());
            },
            editClause = function(cl) {
                var json = ko.mapping.toJSON(cl),
                    clone = ko.mapping.fromJSON(json);

                editedClause(clone);
                existingClause = cl;
            },
            selectClause = function(cl, ev) {
                var element = $(ev.target);
                element.parents("ul").children()
                    .removeClass("selected");
                element.parents("li").addClass("selected");
            },
            save = function() {
                var data = ko.mapping.toJSON({ template: template });
                return context.post(data, "/ContractSetting/Save")
                    .then(function(e) {
                        template().ContractTemplateId(e.ContractTemplateId);
                        isBusy(false);
                        logger.log("Template kontrak sudah di simpan", template(), vm, true);
                    });
            },
            removeDocument = function(doc) {
                template.DocumentTemplateCollection.remove(doc);
            },
            exportJson = function() {
                return exim.exportJson("contract.template." + template().ContractTemplateId() + ".json", ko.mapping.toJSON(template));

            },
            importJson = function() {
                return exim.importJson()
                    .done(function(json) {
                        template(ko.mapping.fromJSON(json));
                        template().ContractTemplateId(0);

                    });
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

            editClause: editClause,
            editedClause: editedClause,
            clauseDialogButtonOk: clauseDialogButtonOk,
            startAddTopicCommand: startAddTopic,
            startAddClauseCommand: startAddClause,
            
            toolbar: {
                saveCommand: save,
                exportCommand: exportJson,
                reloadCommand: function () {
                    return activate({ id: vm.template().ContractTemplateId() });
                },
                commands: ko.observableArray([{
                    icon: 'icon-upload',
                    caption: 'import',
                    command: importJson
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
            clause: editedClause,
            contractTypeTemplate: ko.observable(),
            removeDocumentCommand: removeDocument,
            viewAttached: viewAttached
        };

        return vm;

    });
