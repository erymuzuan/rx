﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" /> 
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="../objectbuilders.js" />

define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, 'durandal/system', 'services/watcher',objectbuilders.config,objectbuilders.cultures], function (context, logger, router, system, watcher,config,culture) {
    var isBusy = ko.observable(false),
        department = ko.observable(),
        designation = ko.observable(),
        id = ko.observable(),
        templateId = ko.observable(),
        activate = function (routedata) {
            id(parseInt(routedata.id));
            templateId(parseInt(routedata.templateId));
             var  tcs = new $.Deferred();

            if (id) {
                department(config.profile.Department);
                designation(config.profile.Designation);
                var officerQuery = String.format("Department eq '{0}'", department());
                var officersTask = context.getTuplesAsync("UserProfile", officerQuery,"Username","FullName");
                // build custom fields value
                var maintenanceTemplateTask = context.loadOneAsync("MaintenanceTemplate", "MaintenanceTemplateId eq " + templateId());
                
                $.when(officersTask, maintenanceTemplateTask)
                    .done(function (officers,template) {
                        var cfs = _(template.CustomFieldCollection()).map(function (f) {
                            var webid = system.guid();
                            var v = new bespoke.sph.domain.CustomFieldValue(webid);
                            v.Name(f.Name());
                            v.Type(f.Type());
                            return v;
                        });

                        vm.maintenance().CustomFieldValueCollection(cfs);
                        vm.maintenance().WorkOrderType(template.Name());
                        vm.maintenance().MaintenanceId(id());
                        vm.maintenance().TemplateId(templateId());
                        vm.officerOptions(officers);
                        tcs.resolve();

                    });

                vm.maintenance().TemplateId(templateId);
                vm.toolbar().watching(false);
                return tcs.promise();
            }


            var query = "MaintenanceId eq " + id,
                    loadTask = context.loadOneAsync("Maintenance", query),
                    watcherTask = watcher.getIsWatchingAsync("Maintenance", id);

            $.when(loadTask, watcherTask).done(function (b, w) {
                vm.maintenance(b);
                vm.toolbar().watching(w);
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

        addNewComment = function () {
            var comment = new bespoke.sph.domain.Comment();
            vm.maintenance().WorkOrder.CommentCollection.push(comment);
        },

        addNewWarranty = function () {
            var warranty = new bespoke.sph.domain.Warranty();
            vm.maintenance().WorkOrder.WarrantyCollection.push(warranty);
        },

        addNewPartAndLabor = function () {
            var partAndLabor = new bespoke.sph.domain.PartsAndLabor();
            vm.maintenance().WorkOrder.PartsAndLaborCollection.push(partAndLabor);
        },

        addNonCompliance = function () {
            var noncompliance = new bespoke.sph.domain.NonCompliance();
            vm.maintenance().WorkOrder.NonComplianceCollection.push(noncompliance);
        },
        assignToOfficer = function () {
            var tcs = new $.Deferred();
            var data = ko.toJSON(vm.maintenance);
            isBusy(true);
            context.post(data, "/Maintenance/Assign")
                .then(function (result) {
                    isBusy(false);
                    tcs.resolve(result);
                });
            var url = '/#/maintenance.dashboard';
            router.navigateTo(url);
            return tcs.promise();

        },
        closeMaintenance = function () {
            var tcs = new $.Deferred();
            var data = ko.toJSON({id :id()});
            isBusy(true);
            context.post(data, "/Maintenance/Closed")
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
        viewAttached: viewAttached,
        maintenance: ko.observable(new bespoke.sph.domain.Maintenance()),
        template: ko.observable(new bespoke.sph.domain.MaintenanceTemplate()),
        officerOptions: ko.observableArray(),
        addNewCommentCommand: addNewComment,
        addNewWarrantyCommand: addNewWarranty,
        addNewPartAndLaborCommand: addNewPartAndLabor,
        addNonComplianceCommand: addNonCompliance,
        culture : culture,
        toolbar: ko.observable({
            watchCommand: function() { return watcher.watch("Maintenance", vm.maintenance().MaintenanceId()); },
            unwatchCommand: function () { return watcher.unwatch("Maintenance", vm.maintenance().MaintenanceId()); },
            watching: ko.observable(false),
            saveCommand: assignToOfficer,
            commands: ko.observableArray([
                {
                    caption: culture.maintenance_detail.CLOSE_MAINTENANCE_BUTTON_CAPTION,
                    icon: 'icon-thumbs-up',
                    command: closeMaintenance,
                    visible: 'maintenance().Status == "culture.maintenance_dashboard.INPROGRESS_MAINTENANCE_STATUS_CAPTION"'
                },
                {
                    caption: culture.maintenance_detail.ASSIGN_MAINTENANCE_BUTTON_CAPTION,
                    icon: 'icon-thumbs-up',
                    command: assignToOfficer,
                    visible: 'maintenance().Status == "culture.maintenance_dashboard.NEW_MAINTENANCE_STATUS_CAPTION"'
                }])
        })

    };

    return vm;
});