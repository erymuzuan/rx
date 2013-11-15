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
            state = ko.observable(false),
            activate = function (routeData) {
                id(parseFloat(routeData.definitionId));
                if (!id()) {
                    vm.workflowdefinition(new bespoke.sph.domain.WorkflowDefinition());
                    return true;
                }
                var query = String.format("WorkflowDefinitionId eq {0}", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("WorkflowDefinition", query)
                    .done(function (wd) {
                        vm.workflowdefinition(wd);
                        wd.loadSchema();

                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            viewAttached = function () {
                $('table#activities-table').on('mouseenter', 'tr', function (e) {
                    e.preventDefault();
                    var act = ko.dataFor(this),
                        wid = ko.unwrap(act.WebId);
                    console.log(wid);
                });
            },
            compileAsync = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.workflowdefinition);

                context.post(data, "/WorkflowDefinition/Compile")
                    .then(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                            state(true);
                        } else {
                            vm.errors(result.Errors);
                            state(false);
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
                
            },
            
            publishAsync = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.workflowdefinition());

                context.post(data, "/WorkflowDefinition/Publish")
                    .then(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                            vm.workflowdefinition().Version(result.version);
                            //state(true);
                        } else {
                            logger.error(result);
                        }
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
            },
            openVisualDesigner = function () {
                var designer = window.open('/workflowdefinition/visual', "_blank");
                designer.wd = vm.workflowdefinition();
                designer.saved = function (wd) {
                    vm.workflowdefinition(wd);
                };
            };

        var vm = {
            isBusy: isBusy,
            state : state,
            activate: activate,
            viewAttached: viewAttached,
            openVisualDesigner: openVisualDesigner,
            errors : ko.observableArray(),
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
                        visible: state,
                        caption: 'publish',
                        icon: "fa fa-sign-out"
                    }])
            }
        };

        return vm;

    });
