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


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
	function (context, logger, router) {

	    var template = ko.observable(new bespoke.sph.domain.ComplaintTemplate()),
	        isBusy = ko.observable(false),
	        tenantId = ko.observable(),
	        complaintNo = ko.observable(),

            activate = function (tenant) {
                tenantId(parseInt(tenant.TenantId()));
                vm.complaint().TenantId(tenantId);
                var query = "TenantIdSsmNo eq '" + tenant.IdSsmNo() + "'";
                var query2 = "TenantId eq " + tenant.TenantId();
                isBusy(true);
                var tcs = new $.Deferred();

                var getContractTask = context.loadAsync("Contract", query);
                var getTenantInfoTask = context.loadOneAsync("Tenant", query2);
                var getComplaintTemplateTask = context.getTuplesAsync("ComplaintTemplate", "ComplaintTemplateId gt 0", "ComplaintTemplateId", "Name");

                $.when(getTenantInfoTask, getContractTask, getComplaintTemplateTask)
                    .then(function (t, lo, list) {
                        vm.complaint().Tenant(t);
                        vm.locationOptions.removeAll();
                        _.each(lo.itemCollection, function (cs) {
                            var list2 =
                                {
                                    Name: ko.observable()
                                };
                            list2.Name(cs.Space().BuildingName() + " , " + cs.Space().FloorName() + " , " + cs.Space().LotName());
                            vm.locationOptions.push(list2);
                        });
                        vm.typeOptions(_(list).sortBy(function (b) {
                            return b.Item2;
                        }));
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
	            vm.complaint().TenantId(tenantId);
	            var data = ko.toJSON(vm.complaint);
	            isBusy(true);

	            context.post(data, "/Complaint/Submit")
	                .done(function (e) {
	                    logger.log("Data has been successfully saved ", e, "complaint", true);
	                    isBusy(false);
	                    complaintNo(e.referenceNo);
	                    isBusy(false);
	                    $('#success-panel').modal({})
                         .on('hidden', function () {
                             vm.complaint(new bespoke.sph.domain.Complaint());
                             vm.activate(vm.tenantInfo);
                         });
	                    tcs.resolve(e);
	                });
	            return tcs.promise();
	        };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        viewAttached: viewAttached,
	        complaintNo: complaintNo,
	        locationOptions: ko.observableArray(),
	        typeOptions: ko.observableArray(),
	        categoryOptions: ko.observableArray([]),
	        subCategoryOptions: ko.observableArray([]),
	        selectedType: ko.observable(),
	        selectedCategory: ko.observable(),
	        template: template,
	        complaint: ko.observable(new bespoke.sph.domain.Complaint()),
	        submitCommand: submit
	    };

	    vm.complaint().Type.subscribe(function (type) {
	        vm.isBusy(true);
	        if (type) {
	            context.loadOneAsync("ComplaintTemplate", "Name eq '" + type + "'")
	            .then(function (t) {
	                vm.template(t);
	                var categories = _(t.ComplaintCategoryCollection()).map(function (c) {
	                    return c.Name();
	                });

	                vm.categoryOptions(categories);
	                vm.isBusy(false);
	                selectedType = t;
	            });
	        }

	    });

	    vm.complaint().Category.subscribe(function (category) {
	        if (category) {
	            var cat = _(template().ComplaintCategoryCollection()).find(function (c) {
	                return c.Name() === category;
	            });

	            vm.subCategoryOptions(cat.SubCategoryCollection());
	        }

	    });

	    return vm;

	});
