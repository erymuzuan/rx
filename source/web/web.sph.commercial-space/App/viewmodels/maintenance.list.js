﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext'], function (context) {
    var isBusy = ko.observable(false),
        status = ko.observable(),
        activate = function (routedata) {
            isBusy(true);
            status(routedata.status);
            var header = 'Senarai Rekod Senggara (Status : ' + routedata.status + ')';
            vm.title(header);
                
            var tcs = new $.Deferred();
            var query = String.format("Status eq '{0}'", status());
            context.loadAsync("Maintenance", query).done(function(lo) {
                vm.maintenances(lo.itemCollection);
            tcs.resolve(true);
            isBusy(false);
            });
            
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
        maintenances: ko.observable(new bespoke.sphcommercialspace.domain.Maintenance()),
        toolbar: ko.observable({
            reloadCommand: function () {
                return activate({ status: status() });
            },
            printCommand: printList
        })
    };
    return vm;
});

    
