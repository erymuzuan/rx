/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
function (context, logger, router) {

    var
    isBusy = ko.observable(false),
    activate = function () {
        isBusy(true);
        return context.getTuplesAsync("ContractTemplate", "ContractTemplateId gt 0", "ContractTemplateId", "Type")
            .then(function(list) {
                vm.contractTemplateCollection(list);
            });
    },
    edit = function(template) {
        router.navigateTo('/#/contracttypetemplate/' + template.Item1);
    },
    viewAttached = function (view) {

    };

    var vm = {
        isBusy: isBusy,
        activate: activate,
        viewAttached: viewAttached,
        editCommand: edit,
        contractTemplateCollection : ko.observableArray([])
    };

    return vm;

});
