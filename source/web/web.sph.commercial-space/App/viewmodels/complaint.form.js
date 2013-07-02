/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/mockComplaintTemplateContext'],
	function (context) {

	    var id = ko.observable(),
	        isBusy = ko.observable(false),
	        activate = function (routedata) {
	            isBusy(true);
	            id(parseInt(routedata.id));
	            var tcs = new $.Deferred();
	            var query = String.format("ComplaintTemplateId eq '{0}'", id());
	            context.loadOneAsync("ComplaintTemplate", query).then(function (ct) {
	                var categories = _(ct.ComplainCategoryCollection()).map(function (c) {
	                    return c.Name();
	                });
	                vm.categoryOptions(categories);
	                tcs.resolve(true);
	                isBusy(false);
	            });
	            tcs.promise();
	        },
	        submit = function () {
	            var tcs = new $.Deferred();
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
	        categoryOptions: ko.observableArray([]),
	        subCategoryOptions: ko.observableArray([]),
	        complaint: ko.observable(new bespoke.sphcommercialspace.domain.Complaint()),
	        submitCommand: submit
	    };



	    return vm;

	});
