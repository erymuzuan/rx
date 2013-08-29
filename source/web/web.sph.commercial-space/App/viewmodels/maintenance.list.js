/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext'], function () {
    var isBusy = ko.observable(false),
        status = ko.observable(),
        activate = function (routedata) {
            isBusy(true);
            status(routedata.status);
            var header = 'Senarai Rekod Senggara (Status : ' + routedata.status + ')';
            vm.title(header);
                
            var tcs = new $.Deferred();
                isBusy(false);
                tcs.resolve(true);
            return tcs.promise();
        },
        
        viewAttached = function() {
        },
        
        printList = function () { };
    
    var vm = {
        status: status,
        isBusy: isBusy,
        activate: activate,
        viewAttached: viewAttached,
        title: ko.observable(),
        maintenances: ko.observableArray([]),
        toolbar: ko.observable({
            reloadCommand: function () {
                return activate({ status: status() });
            },
            printCommand: printList
        })
    };
    return vm;
});

    
