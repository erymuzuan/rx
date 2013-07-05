/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../viewmodels/_tenant.rent.js" />
/// <reference path="../viewmodels/_tenant.contract.js" />


define(['services/datacontext', './_tenant.rent', './_tenant.contract','./_tenant.complaint','./complaintform.tenant'],
	function (context, rentvm, contractvm, complaintListvm,cfvm) {

	    var isBusy = ko.observable(false),
        id = ko.observable(),
        tenant = new bespoke.sphcommercialspace.domain.Tenant(),
        activate = function (routeData) {
            //id(parseInt(routeData.tenantId));
            var query = String.format("TenantId eq 4");//{0}", id()); 
            var tcs = new $.Deferred();
            var detailLoaded = function (tnt) {
                vm.tenant(tnt);
                var rentTask = rentvm.activate(tnt);
                var contractTask = contractvm.init(tnt);
                var complaintListTask = complaintListvm.activate(tnt);
                var complaintFormTask = cfvm.activate(tnt);
                $.when(rentTask, contractTask, complaintListTask, complaintFormTask).done(function () {
                    tcs.resolve(true);
                });
            };
            context.loadOneAsync("Tenant", query)
                .then(detailLoaded);

            return tcs.promise();
        };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        tenant: ko.observable(tenant)
	    };

	    return vm;

	});
