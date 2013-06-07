/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/mockTenantContext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../viewmodels/_tenant.rent.js" />
/// <reference path="../viewmodels/_tenant.contract.js" />


define(['services/mockTenantContext', './_tenant.rent', './_tenant.contract'],
	function (context,rentvm,contractvm) {

	var isBusy = ko.observable(false),
	id = ko.observable(),
	tenant = new bespoke.sphcommercialspace.domain.Tenant(),
	activate = function (routeData) {
	    id(routeData.id);
	    var query = String.format("TenantId eq {0}", id());
	    var tcs = new $.Deferred();
	    var detailLoaded = function(b) {
	        vm.tenant(b);
	        rentvm.init(b);
	        contractvm.init(b);
	        tcs.resolve(true);
	    };
	    context.loadOneAsync("Tenant", query)
	        .then(detailLoaded);

	    return tcs.promise();
	};

	var vm = {
		isBusy : isBusy,
		activate : activate,
		tenant: ko.observable(tenant)
	};

	return vm;

});
