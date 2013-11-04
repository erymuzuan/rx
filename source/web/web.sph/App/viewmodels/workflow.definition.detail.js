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


define([objectbuilders.datacontext, objectbuilders.logger],
    function (context, logger) {
        var isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (routeData) {
                id(parseFloat(routeData.definitionId));
                if (!id()) {
                    vm.workflowdefinition(new bespoke.sph.domain.WorkflowDefinition());
                    return true;
                }
                var query = String.format("WorkflowDefinitionId eq {0}", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("WorkflowDefinition", query)
                    .done(function (wf) {
                        vm.workflowdefinition(wf);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            compileAsync = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.workflowdefinition);
                isBusy(true);

                context.post(data, "/WorkflowDefinition/Compile")
                    .then(function (result) {
                        isBusy(false);
                        logger.info("The workflow had been succesfully compiled");

                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            publishAsync = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.workflowdefinition());
                isBusy(true);

                context.post(data, "/WorkflowDefinition/Publish")
                    .then(function (result) {
                        isBusy(false);
                        logger.info("The workflow had been succesfully published");

                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            saveAsync = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.workflowdefinition());
                isBusy(true);

                context.post(data, "/WorkflowDefinition/Save")
                    .then(function (result) {
                        isBusy(false);
                        logger.info("Data have been succesfully save");

                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            workflowdefinition: ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            toolbar: {
                saveCommand: saveAsync,
                commands: ko.observableArray([
                    {
                        command: compileAsync,
                        caption: 'compile',
                        icon: "fa fa-gear"
                    },
                    {
                        command: publishAsync,
                        caption: 'publish',
                        icon: "fa fa-sign-out"
                    }])
            }
        };

        return vm;

    });
