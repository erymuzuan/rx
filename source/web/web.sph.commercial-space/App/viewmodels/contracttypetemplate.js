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

            template.ContractTemplateId(routeData.id);

            // reset
            template.Type('');
            template.Description(''),
            template.DocumentTemplateCollection([]);
            template.TopicCollection([]);
            topic.Text('');
            topic.Title('');
            topic.Description('');
            topic.ClauseCollection([]);
            clause.No('');
            clause.Text('');
            clause.Title('');
            clause.Description('');

            return context.loadOneAsync("ContractTemplate", "ContractTemplateId eq " + routeData.id)
                .then(function (e) {
                    if (e) {
                        ko.mapping.fromJS(ko.mapping.toJS(e), {}, vm.template);
                    }
                    isBusy(false);
                });
        },
        template = {
            ContractTemplateId: ko.observable(0),
            Type: ko.observable(),
            Description: ko.observable(),
            DocumentTemplateCollection: ko.observableArray([]),
            TopicCollection: ko.observableArray([])
        },
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
            selectTopic = function (tp,ev) {
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
            selectClause = function (cl,ev) {
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

                if (cachedTopic) { // copy it back
                    ko.mapping.fromJS(ko.mapping.toJS(vm.topic), {}, cachedTopic);
                }
                if (cachedClause) { // copy it back
                    ko.mapping.fromJS(ko.mapping.toJS(vm.clause), {}, cachedClause);
                }

                var data = ko.mapping.toJSON({ template: template });
                return context.post(data, "/ContractSetting/Save")
                    .then(function (e) {
                        ko.mapping.fromJS(e, {}, vm.template);
                        isBusy(false);
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
            removeDocument = function(doc) {
                template.DocumentTemplateCollection.remove(doc);
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            addAttachmentCommand: addAttachment,
            selectTopicCommand: selectTopic,
            selectClauseCommand: selectClause,
            addTopicCommand: addTopic,
            addClauseCommand: addClause,
            startAddTopicCommand: startAddTopic,
            startAddClauseCommand: startAddClause,
            saveCommand: save,
            configureUpload: configureUpload,
            template: template,
            topic: topic,
            clause: clause,
            contractTypeTemplate: ko.observable(),
            removeDocumentCommand: removeDocument,
            viewAttached: viewAttached
        };

        return vm;

    });
