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
/// <reference path="../viewmodels/_tenant.adhoc.js" />


define(['services/datacontext', './_tenant.rent', './_tenant.contract', './_tenant.adhoc', './_tenant.payment'],
	function (context, rentvm, contractvm, invoicevm, paymentvm) {

	    var isBusy = ko.observable(false),
        id = ko.observable(),
        tenant = new bespoke.sphcommercialspace.domain.Tenant(),
        activate = function (routeData) {
            id(routeData.tenantId);
            var query = String.format("TenantId eq {0}", id());
            var tcs = new $.Deferred();
            var detailLoaded = function (tnt) {
                vm.tenant(tnt);
                var paymentTask = paymentvm.activate(tnt);
               var rentTask = rentvm.activate(tnt);
                var invoiceTask = invoicevm.activate(tnt);
                var contractTask = contractvm.init(tnt);
                    $.when(rentTask,invoiceTask,contractTask,paymentTask).done(function () {
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
