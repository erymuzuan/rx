/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" /> 
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router'], function (context, logger,router) {
    var isBusy = ko.observable(false),
        id = ko.observable(),
        activate = function (routedata) {
            id(parseInt(routedata.id));
            var query = String.format("MaintenanceId eq {0}", id());
            var tcs = new $.Deferred();
            context.loadOneAsync("Maintenance", query)
                .then(function(m) {
                    isBusy(false);
                    vm.maintenance(m);
                    tcs.resolve(true);
                });
            return tcs.promise();
        },
        viewAttached = function (view) {
            $("#AttachmentStoreId").kendoUpload({
                async: {
                    saveUrl: "/BinaryStore/Upload",
                    removeUrl: "/BinaryStore/Remove",
                    autoUpload: true
                },
                multiple: false,
                error: function (e) {
                    logger.logError(e, e, this, true);
                },
                success: function (e) {
                    logger.log('Your file has been ' + e.operation, e, this, true);
                    var storeId = e.response.storeId;
                    var uploaded = e.operation === "upload";
                    var removed = e.operation != "upload";
                    // NOTE : the input file name is "files" and the id should equal to the vm.propertyName
                    if (uploaded) {
                        vm.maintenance().AttachmentStoreId(storeId);
                    }

                    if (removed) {
                        vm.maintenance().AttachmentStoreId("");
                        vm.maintenance().AttachmentName("");
                    }
                }
            });
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
        
        addNonCompliance = function() {
            var noncompliance = new bespoke.sphcommercialspace.domain.NonCompliance();
            vm.maintenance().WorkOrder.NonComplianceCollection.push(noncompliance);
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
           var url = '/#/maintenance.dashboard';
           router.navigateTo(url);
           return tcs.promise();
           
        };


    var vm = {
        activate: activate,
        viewAttached : viewAttached,
        maintenance: ko.observable(new bespoke.sphcommercialspace.domain.Maintenance()),
        addNewCommentCommand: addNewComment,
        addNewWarrantyCommand: addNewWarranty,
        addNewPartAndLaborCommand: addNewPartAndLabor,
        addNonComplianceCommand : addNonCompliance,
        saveCommand: save
        
    };

    return vm;
});