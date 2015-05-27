/// <reference path="../objectbuilders.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.2.0.debug.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/string.js" />


define([objectbuilders.datacontext, objectbuilders.cultures, objectbuilders.logger],
    function (context, cultures, logger) {
        var
            activate = function () {
                return true;
            },
            attached = function () {
                $("#import").kendoUpload({
                    async: {
                        saveUrl: "/sph/WorkflowDefinition/Import",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function (e) {
                        logger.logError(e, e, this, true);
                    },
                    success: function (e) {
                        if (!e.response.success) {
                            logger.error(e.response.message);
                            return;
                        }
                        var uploaded = e.operation === "upload";
                        if (uploaded) {
                            var wd = e.response.wd,
                                o = context.toObservable(wd);
                            vm.definitions.push(o);
                        }
                    }
                });
            };


        var vm = {
            activate: activate,
            attached: attached,
            definitions: ko.observableArray(),
            toolbar: {
                addNew: {
                    location: '#/workflow.definition.visual/0',
                    caption: 'New Workflow Definition'
                }
            },
            cultures: cultures
        };

        return vm;
    });
