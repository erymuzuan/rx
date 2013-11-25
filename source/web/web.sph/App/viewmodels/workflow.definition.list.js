/// <reference path="../objectbuilders.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
/// <reference path="/Scripts/string.js" />


define([objectbuilders.datacontext, objectbuilders.cultures],
    function (context, cultures) {
        var
            activate = function () {
                return true;
            },
            viewAttached = function() {
                $("#import").kendoUpload({
                    async: {
                        saveUrl: "/WorkflowDefinition/Import",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function(e) {
                        logger.logError(e, e, this, true);
                    },
                    success: function(e) {
                        console.log(e.response);
                        var wd = e.response.wd;
                        console.log(wd);
                        var uploaded = e.operation === "upload";
                        if (uploaded) {
                            vm.definitions.push(context.toObservable(wd));
                        }
                    }
                });
            };


        var vm = {
            activate: activate,
            viewAttached: viewAttached,
            definitions: ko.observableArray(),
            toolbar: {
                addNew: {
                    location: '/#/workflow.definition.visual/0',
                    caption: 'New Workflow Definition'
                }
            },
            cultures: cultures
        };

        return vm;
    });
