/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
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
        return true;
    };

    var vm = {
        isBusy: isBusy,
        activate: activate,
        contractTemplateCollection: ko.observableArray([]),
        toolbar : {
            addNew : {
                location: '/#/contract.template/0',
                caption : 'Tambah jenis kontrak'
            }
        }
    };

    return vm;

});
