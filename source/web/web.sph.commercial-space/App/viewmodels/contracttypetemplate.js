/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/_uiready.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
        activate = function (routeData) {
            isBusy(true);
            return context.loadOneAsync("ContractTemplate", "ContractTemplateId eq " + routeData.id)
                .then(function(e) {
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
                    attachment.IsReceived(e.operation === "upload");

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
        addClause = function () {

        },
            selectTopic = function (tp) {
                ko.mapping.fromJS(ko.mapping.toJS(tp), {}, vm.topic);
            },
            save = function () {
                var data = ko.mapping.toJSON({ template: template });
                return context.post(data, "/ContractSetting/Save")
                    .then(function (e) {
                        ko.mapping.fromJS(e, {}, vm.template);
                        isBusy(false);
                    });
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            addAttachmentCommand: addAttachment,
            selectTopicCommand: selectTopic,
            addTopicCommand: addTopic,
            addClauseCommand: addClause,
            saveCommand: save,
            configureUpload: configureUpload,
            template: template,
            topic: topic,
            contractTypeTemplate: ko.observable(),
            viewAttached: viewAttached
        };

        return vm;

    });
