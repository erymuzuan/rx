/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            documentTemplates = ko.observableArray(),
            emailTemplates = ko.observableArray(),
            activate = function () {
                return true;

            },
            attached = function () {
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            documentTemplates: documentTemplates,
            emailTemplates: emailTemplates
        };

        return vm;

    });
