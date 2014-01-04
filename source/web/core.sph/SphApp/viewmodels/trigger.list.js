/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'plugins/router'], function (context, logger, router) {

    var activate = function () {
        return true;
    },
    addNew = function () {
        var url = '/#/trigger.setup/0';
        router.navigateTo(url);
        return {
            then: function () { }
        };
    },
     exportList = function () {

     };

    var vm = {
        activate: activate,
        triggerCollection: ko.observableArray([]),
        toolbar: {
            addNewCommand: addNew,
            exportCommand: exportList
        }
    };

    return vm;
});
