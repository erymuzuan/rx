/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
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
                var query = String.format("Name eq '{0}'", '@Model.Definition.Name'),
                  tcs = new $.Deferred(),
                  viewsQuery = String.format("EntityDefinitionId eq '@Model.Definition.EntityDefinitionId' and IsPublished eq 1"),
                  formsQuery = String.format("EntityDefinitionId eq '@Model.Definition.EntityDefinitionId' and IsPublished eq 1 and IsAllowedNewItem eq 1"),
                  edTask = context.loadOneAsync("EntityDefinition", query),
                  formsTask = context.loadAsync("EntityForm", formsQuery),
                  reportTask = context.loadAsync("ReportDefinition", "[DataSource.EntityName] eq '@Model.Definition.Name'"),
                  viewsTask = context.loadAsync("EntityView", viewsQuery);


                $.when(edTask, formsTask, viewsTask,reportTask )
                 .done(function (b, formsLo, viewsLo,reportsLo) {
                     entity(b);
                     var formsCommands = _(formsLo.itemCollection).map(function (v) {
                         return {
                             caption: v.Name(),
                             command: function () {
                                 window.location = '#' + v.Route() + '/0';
                                 return Task.fromResult(0);
                             },
                             icon: v.IconClass()
                         };
                     });
                     reports(reportsLo.itemCollection);
                     views(viewsLo.itemCollection);
                     // get counts
                     _(views()).each(function (v) {
                         v.CountMessage("....");
                         var tm = setInterval(function () {
                             v.CountMessage(v.CountMessage() == "...." ? "..." : "....");
                         }, 250);
                         $.get("/Sph/EntityView/Count/" + v.EntityViewId())
                             .done(function(c) {
                                 clearInterval(tm);
                                 v.CountMessage(c.hits.total);
                             });
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
            addForm = function () {

            },
            addView = function () {

            },
            recentItemsQuery = {
                "sort": [
                 {
                     "ChangedDate": {
                         "order": "desc"
                     }
                 }
                ]
            };

        var vm = {
            isBusy: isBusy,
            views: views,
            entity: entity,
            activate: activate,
            attached: attached,
            reports: reports,
            tools: tools,
            recentItems: recentItems,
            addForm: addForm,
            addView: addView,
            recentItemsQuery: recentItemsQuery,
            toolbar: {
                commands: ko.observableArray([])
            }
        };

        return vm;

    });
