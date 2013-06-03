﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />


define([],
    function () {
        var isBusy = ko.observable(false),
        activate = function () {
            vm.topicCollection.push({ Title: "test", Description: "tsts", Text: "tststs" });
            console.log(vm.topicCollection);
        },
        startAddTopic = function () {
            topic(new bespoke.sphcommercialspace.domain.Topic());
        },
        addTopic = function () {
            vm.topicCollection.push(topic);
        },
        editedTopic,
        startAddClause = function (tpc) {
            editedTopic = tpc;
            clause(new bespoke.sphcommercialspace.domain.Clause());
        },
        addClause = function () {
            editedTopic.ClauseCollection.push(clause);
        },
        topic = ko.observable(new bespoke.sphcommercialspace.domain.Topic()),
        clause = ko.observable(new bespoke.sphcommercialspace.domain.Clause()),
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
                var topics = _(ctr.TopicCollection()).map(function (t) {
                    return t;
                });
                vm.topicCollection(topics);

            },
            topicCollection: ko.observableArray([]),
            topic: topic,
            clause: clause,
            isBusy: isBusy,
            activate: activate,
            addTopicCommand: addTopic,
            startAddClauseCommand: startAddClause,
            startAddTopicCommand: startAddTopic,
            addClauseCommand: addClause,
            removeClauseCommand: removeClause,
            collapseDetailsCommand: collapseClauseDetails
        };

        return vm;

    });
