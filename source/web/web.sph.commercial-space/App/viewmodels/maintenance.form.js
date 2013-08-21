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

	    var template = ko.observable(new bespoke.sphcommercialspace.domain.MaintenanceTemplate()),
	        id = ko.observable(),
	        isBusy = ko.observable(false),


	        activate = function (routedata) {
	            isBusy(true);
	            id(parseInt(routedata.templateId));
	            var tcs = new $.Deferred();
	            var query = "MaintenanceTemplateId eq " + id();
	            context.loadOneAsync("MaintenanceTemplate", query).then(function (ct) {
	                template(ct);
	                tcs.resolve(true);
	                isBusy(false);
	                
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
                        }


                    }
                });
            },

	        submit = function () {
	            var tcs = new $.Deferred();
	            var templateName = template().Name();
	            vm.maintenance().Type(templateName);
	            var data = ko.toJSON(vm.maintenance);
	            isBusy(true);

	            context.post(data, "/Maintenance/Submit")
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
	        locationOptions: ko.observableArray(),
	        template: template,
	        maintenance: ko.observable(new bespoke.sphcommercialspace.domain.Maintenance()),
	        toolbar : {
	             submitCommand: submit
	        }
	       
	    };
	    return vm;

	});
