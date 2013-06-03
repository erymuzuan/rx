/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function () {
                vm.topicCollection.push({ Title: "test", Description: "tsts", Text: "tststs" });
                console.log(vm.topicCollection);
            },
            viewAttached = function (view) {

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
                vm.topicCollection.push(clone);
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

        clauseDetailsCollapsed = false,
        collapseClauseDetails = function (d, ev) {
            $(ev.target).text(clauseDetailsCollapsed ? "collapse" : "expand");
            if (clauseDetailsCollapsed) {
                $('#clauses textarea').slideDown();
            } else {
                $('#clauses textarea').slideUp();
            }
            clauseDetailsCollapsed = !clauseDetailsCollapsed;
        };

        var vm = {
            init: function (ctr) {
                var topics = _(ctr.TopicCollection()).map(function(t) {
                    return t;
                }); 
                vm.topicCollection(topics);
                console.log(topics.length);
                console.log(vm.topicCollection().length);
            },
            topicCollection: ko.observableArray([]),
            topic: topic,
            clause: clause,
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            addTopicCommand: addTopic,
            startAddClauseCommand: startAddClause,
            startAddTopicCommand: startAddTopic,
            addClauseCommand: addClause,
            removeClauseCommand: removeClause,
            collapseDetailsCommand: collapseClauseDetails
        };

        return vm;

    });
