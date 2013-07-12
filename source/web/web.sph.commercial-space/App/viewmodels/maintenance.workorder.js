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
            //var query = String.format("WorkOrderNo eq '{0}'", id());
            var query = String.format("ComplaintId eq 20 ");
            var tcs = new $.Deferred();
            context.loadOneAsync("Maintenance", query)
                .then(function(m) {
                    isBusy(false);
                    vm.maintenance(m);
                    tcs.resolve(true);
                });
            return tcs.promise();
        },
        
        addNewComment = function() {
            var comment  = new bespoke.sphcommercialspace.domain.Comment();
            vm.maintenance().WorkOrder.CommentCollection.push(comment);
        },
        
        addNewWarranty = function() {
            var warranty = new bespoke.sphcommercialspace.domain.Warranty();
            vm.maintenance().WorkOrder.WarrantyCollection.push(warranty);
        },
        
        addNewPartAndLabor = function() {
            var partAndLabor = new bespoke.sphcommercialspace.domain.PartsAndLabor();
            vm.maintenance().WorkOrder.PartsAndLaborCollection.push(partAndLabor);
        },
        
        save = function () {
           var tcs = new $.Deferred();
           var data = ko.toJSON(vm.maintenance);
           isBusy(true);
           context.post(data, "/Maintenance/Save")
               .then(function (result) {
                   isBusy(false);
                   tcs.resolve(result);
               });
           return tcs.promise();
        };


    var vm = {
        activate: activate,
        maintenance: ko.observable(new bespoke.sphcommercialspace.domain.Maintenance()),
        addNewCommentCommand: addNewComment,
        addNewWarrantyCommand: addNewWarranty,
        addNewPartAndLaborCommand: addNewPartAndLabor,
        saveCommand: save
    };

    return vm;
});