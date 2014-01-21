﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            id = ko.observable([]),
            tools = ko.observableArray([]),
            reports = ko.observableArray([]),
            recentItems = ko.observableArray([]),
            views = ko.observableArray([]),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            activate = function () {
                var query = String.format("Name eq '{0}'", '@Model.Name'),
                  tcs = new $.Deferred(),
                  formsQuery = String.format("EntityDefinitionId eq @Model.EntityDefinitionId"),
                  edTask = context.loadOneAsync("EntityDefinition", query),
                  formsTask = context.loadAsync("EntityForm", formsQuery);


                $.when(edTask, formsTask)
                 .done(function (b, formsLo) {
                     entity(b);
                     var formsCommands = _(formsLo.itemCollection).map(function (v) {
                         return {
                             caption: v.Name(),
                             command: function () {
                                 window.location = '#' + v.Route();
                                 return Task.fromResult(0);
                             },
                             icon: "@Model.IconClass"
                         };
                     });
                     vm.toolbar.commands(formsCommands);
                     tcs.resolve(true);
                 });

                // TODO : get views

                // TODO: get recent items

                //TODO : reports

                // TODO : tools
                //http://i.imgur.com/OZ6mSFq.png

                return tcs.promise();
            },
            attached = function (view) {

            },
            addForm = function() {
                
            },
            addView = function() {
                
            };

        var vm = {
            isBusy: isBusy,
            views: views,
            entity: entity,
            activate: activate,
            attached: attached,
            reports: reports,
            tools: tools,
            recentItems : recentItems,
            addForm : addForm,
            addView : addView
        };

        return vm;

    });
