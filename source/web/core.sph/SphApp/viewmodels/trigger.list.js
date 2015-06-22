/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define([], function () {

    var activate = function () {
        return true;
    };

    var vm = {
        activate: activate,
        triggerCollection: ko.observableArray([]),
        toolbar: {
            addNew: { location: "#/trigger.setup/0", caption: "Add new trigger" }
        }
    };

    return vm;
});
