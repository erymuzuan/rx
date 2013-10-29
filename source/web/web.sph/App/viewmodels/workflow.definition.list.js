/// <reference path="../objectbuilders.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="/Scripts/knockout-2.3.0.debug.js" />
/// <reference path="/Scripts/string.js" />


define([objectbuilders.datacontext, objectbuilders.cultures],
    function (context, cultures) {
        var
            activate = function () {
                return true;
            };


        var vm = {
            activate: activate,
            definitions: ko.observableArray(),
            toolbar: {
                addNew: {
                    location: '/#/workflow.definition.detail/0',
                    caption: 'New Workflow Definition'
                }
            },
            cultures: cultures
        };

        return vm;
    });
