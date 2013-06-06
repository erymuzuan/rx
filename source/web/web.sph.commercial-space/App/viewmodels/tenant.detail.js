/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/mockTenantContext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/mockTenantContext'],
	function (context) {

	var isBusy = ko.observable(false),
	id = ko.observable(),
	tenant = new bespoke.sphcommercialspace.domain.Tenant(),
	activate = function (routeData) {
	    id(routeData.id);
	    var query = String.format("TenantId eq {0}", id());
	    var tcs = new $.Deferred();
	    context.loadOneAsync("Tenant", query)
	        .done(function(b) {
	            vm.tenant(b);
	            tcs.resolve(true);
	        });

	    return tcs.promise();
	};

	var vm = {
		isBusy : isBusy,
		activate : activate,
		tenant: ko.observable(tenant)
	};

	return vm;

});
