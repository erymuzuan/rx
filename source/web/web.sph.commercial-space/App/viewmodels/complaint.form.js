/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" /> 


define(['services/datacontext', 'services/logger', 'durandal/system', 'config'],
	function (context, logger, system, config) {

	    var template = ko.observable(new bespoke.sphcommercialspace.domain.ComplaintTemplate()),
	        id = ko.observable(),
	        isBusy = ko.observable(false),


	        activate = function (routedata) {

	            vm.stateOptions(config.stateOptions);

	            isBusy(true);
	            id(parseInt(routedata.templateId));
	            var tcs = new $.Deferred();
	            var query = "ComplaintTemplateId eq " + id();
	            context.loadOneAsync("ComplaintTemplate", query).then(function (tpl) {
	                template(tpl);
	                vm.complaint().TemplateId(id());
	                var categories = _(tpl.ComplaintCategoryCollection()).map(function (c) {
	                    return c.Name();
	                });
	                vm.categoryOptions(categories);
	                var fieldToValueMap = function(f) {
	                    var webid = system.guid();
	                    var v = new bespoke.sphcommercialspace.domain.CustomFieldValue(webid);
	                    v.Name(f.Name());
	                    v.Type(f.Type());
	                    return v;
	                },
	                cfs = _(tpl.CustomFieldCollection()).map(fieldToValueMap),
	                cls = _(tpl.CustomListDefinitionCollection()).map(function (v) {
	                    var lt = new bespoke.sphcommercialspace.domain.CustomListValue(system.guid());
	                    lt.Name(v.Name());

	                    var fields = _(v.CustomFieldCollection()).map(fieldToValueMap);
	                    lt.CustomFieldCollection = ko.observableArray(fields);
	                    return lt;
	                });

	                vm.complaint().CustomFieldValueCollection(cfs);
	                vm.complaint().CustomListValueCollection(cls);

	                tcs.resolve(true);
	                isBusy(false);

	            });
	            return tcs.promise();
	        },

            viewAttached = function (view) {

                $(view).find('*[title]').tooltip({ placement: 'right' });
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
	                    vm.complaint().ReferenceNo(result.referenceNo);
	                    $('#complaint-ticket-modal').modal();
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
	        customFields: ko.observable(),
	        stateOptions : ko.observableArray(),

	        template: template,
	        complaint: ko.observable(new bespoke.sphcommercialspace.domain.Complaint()),
	        toolbar: {
	            saveCommand: submit
	        }

	    };

	    vm.complaint().Category.subscribe(function (category) {

	        var cat = _(template().ComplaintCategoryCollection()).find(function (c) {
	            return c.Name() === category;
	        });
	        vm.subCategoryOptions(cat.SubCategoryCollection());
	    });

	    return vm;

	});
