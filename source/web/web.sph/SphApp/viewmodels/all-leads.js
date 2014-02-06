/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
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
            view = ko.observable(),
            list = ko.observableArray([]),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            activate = function () {
                var edQuery = String.format("Name eq '{0}'", 'Lead'),
                  tcs = new $.Deferred(),
                  viewQuery = String.format("EntityDefinitionId eq 1"),
                  edTask = context.loadOneAsync("EntityDefinition", edQuery),
                  viewTask = context.loadOneAsync("EntityView", viewQuery);


                $.when(edTask, viewTask)
                 .done(function (b, vw) {
                     entity(b);
                     view(vw);
                     
                     tcs.resolve(true);
                 });



                return tcs.promise();
            },
            attached = function () {

            },
            query = {

                "sort": [
                 {
                     "ChangedDate": {
                         "order": "desc"
                     }
                 }
                ]
            };

        var vm = {
            view: view,
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
