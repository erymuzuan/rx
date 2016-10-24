/// <reference path="../objectbuilders.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/string.js" />


define([objectbuilders.datacontext, objectbuilders.cultures, objectbuilders.logger, "services/new-item"],
    function (context, cultures, logger, addItemService) {
        const definitions = ko.observableArray(),
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
                        const uploaded = e.operation === "upload";
                        if (uploaded) {
                            const wd = e.response.wd,
                                o = context.toObservable(wd);
                            definitions.push(o);
                        }
                    }
                });
            };


        const vm = {
            activate: activate,
            attached: attached,
            definitions:definitions,
            toolbar: {
                addNewCommand: function () {
                    return addItemService.addWorkflowDefinitionAsync();
                },
                commands: ko.observableArray()
            },
            cultures: cultures
        };

        return vm;
    });
