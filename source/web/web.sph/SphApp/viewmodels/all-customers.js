/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', 'services/chart', objectbuilders.config],
    function (context, logger, router, chart,config) {

        var isBusy = ko.observable(false),
            view = ko.observable(),
            list = ko.observableArray([]),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            activate = function () {
                var edQuery = String.format("Name eq '{0}'", 'Customer'),
                  tcs = new $.Deferred(),
                  formsQuery = String.format("EntityDefinitionId eq 1 and IsPublished eq 1 and IsAllowedNewItem eq 1"),
                  viewQuery = String.format("EntityDefinitionId eq 1"),
                  edTask = context.loadOneAsync("EntityDefinition", edQuery),
                  formsTask = context.loadAsync("EntityForm", formsQuery),
                  viewTask = context.loadOneAsync("EntityView", viewQuery);


                $.when(edTask, viewTask, formsTask)
                 .done(function (b, vw,formsLo) {
                     entity(b);
                     view(vw);
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
                     vm.toolbar.commands(formsCommands);
                     tcs.resolve(true);
                 });



                return tcs.promise();
            },
            attached = function () {
                chart.init('Customer', query);
            },
            query = {
                "query": {
                    "filtered": {
                        "filter": {
               "bool": {
                  "must": [
                                     {
                     "range":{
                         "Age":{}
                     }
                 }

                  ],
                  "must_not": [
                    
                  ]
               }
           }
                    }
                },
                "sort" : [{"FullName":{"order":"desc"}}]
            };

        var vm = {
            config: config,
            view: view,
            chart: chart,
            isBusy: isBusy,
            entity: entity,
            activate: activate,
            attached: attached,
            list: list,
            query: query,
            toolbar: {
                commands: ko.observableArray([])
            }
        };

        return vm;

    });
