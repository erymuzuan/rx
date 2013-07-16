/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" /> 
/// <reference path="../services/mockComplainTemplateContext.js" />


define(['services/datacontext', 'services/logger'],
	function (context, logger) {

	    var template = ko.observable(new bespoke.sphcommercialspace.domain.ComplaintTemplate()),
	        id = ko.observable(),
	        isBusy = ko.observable(false),


	        activate = function (routedata) {
	            isBusy(true);
	            id(parseInt(routedata.id));
	            var tcs = new $.Deferred();
	            var query = "ComplaintTemplateId eq " + id();
	            context.loadOneAsync("ComplaintTemplate", query).then(function (ct) {
	                template(ct);
	                var categories = _(ct.ComplaintCategoryCollection()).map(function (c) {
	                    return c.Name();
	                });
	                vm.categoryOptions(categories);
	                tcs.resolve(true);
	                isBusy(false);
	                
	                // build custom fields value
	                var cfs = _(template().ComplaintCustomFieldCollection()).map(function(f) {
	                    var v = new bespoke.sphcommercialspace.domain.CustomFieldValue(system.guid.newGuid());
	                    v.Name(f.Name());
	                    v.Type(f.Type());
	                    return v;
	                });

	                vm.complaint().CustomFieldValueCollection(cfs);
	            });
	           return tcs.promise();
	        },

            viewAttached = function (view) {
                
                $('#custom-fields-panel').load("/App/complaint.custom.field.html/" + vm.template().ComplaintTemplateId(),
                function() {
                    ko.applyBindings(vm, document.getElementById('custom-fields-panel'));
                });

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
                            vm.complaint().AttachmentStoreId(storeId);
                        }

                        if (removed) {
                            vm.complaint().AttachmentStoreId("");
                        }


                    }
                });
            },

	        submit = function () {
	            var tcs = new $.Deferred();
	            var templateName = template().Name();
	            vm.complaint().Type(templateName);
	            var data = ko.toJSON(vm.complaint);
	            isBusy(true);

	            context.post(data, "/Complaint/Submit")
	                .then(function (result) {
	                    isBusy(false);
	                    tcs.resolve(result);
	                });
	            return tcs.promise();
	        };

	    var vm = {
	        isBusy: isBusy,
	        
	        activate: activate,
	        viewAttached: viewAttached,
	        
	        categoryOptions: ko.observableArray([]),
	        subCategoryOptions: ko.observableArray([]),
	        locationOptions: ko.observableArray(),
            customFields : ko.observable(),
	        
	        template: template,
	        complaint: ko.observable(new bespoke.sphcommercialspace.domain.Complaint()),
	        
	        submitCommand: submit
	    };

	    vm.complaint().Category.subscribe(function (category) {

	        var cat = _(template().ComplaintCategoryCollection()).find(function (c) {
	            return c.Name() === category;
	        });
	        vm.subCategoryOptions(cat.SubCategoryCollection());
	    });

	    return vm;

	});
