/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
function (context) {

    var
    isBusy = ko.observable(false),
    activate = function () {
        isBusy(true);
        return context.loadAsync("ContractTemplate", "ContractTemplateId gt 0")
            .then(function(lo) {
                vm.contractTemplateCollection(lo.itemCollection);
            });
    };

    var vm = {
        isBusy: isBusy,
        activate: activate,
        contractTemplateCollection : ko.observableArray([])
    };

    return vm;

});
