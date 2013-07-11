/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" /> 
/// <reference path="../services/datacontext.js" />
define(['services/datacontext'], function (context) {
    var isBusy = ko.observable(false),
        //id = ko.observable(),
        activate = function () {
            //id(routedata.won);
            //var query = String.format("WorkOrderNo eq 'WO201312'"); //'{0}'", id());
            var query = String.format("ComplaintId eq 20 ");
            var tcs = new $.Deferred();
            context.loadOneAsync("Maintenance", query)
                .then(function(wo) {
                    isBusy(false);
                    vm.workOrder(wo);
                    tcs.resolve(true);
                });
            return tcs.promise();
        };


    var vm = {
        activate: activate,
        workOrder: ko.observable(new bespoke.sphcommercialspace.domain.Maintenance())
            
    };

    return vm;
});