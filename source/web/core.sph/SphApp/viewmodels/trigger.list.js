/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(["services/new-item"], function (addItemService) {

    const activate = function () {
        return true;
    };

    const vm = {
        activate: activate,
        triggerCollection: ko.observableArray([]),
        toolbar: {
            addNewCommand: function () {
                return addItemService.addTriggerAsync();
            },
            commands: ko.observableArray()
        }
    };

    return vm;
});
