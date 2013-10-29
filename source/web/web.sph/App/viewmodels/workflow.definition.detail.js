/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define([objectbuilders.datacontext],
    function(context) {
        var isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (routeData) {
                id(routeData.definitionId);
                var query = String.format("WorkflowDefinitionId eq {0}", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("WorkflowDefinition", query)
                    .done(function(b) {
                        vm.workflowdefinition(b);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            workflowdefinition: ko.observable(new bespoke.sph.domain.WorkflowDefinition())
        };

        return vm;

    });
