/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
	function (context) {

	var 
	isBusy = ko.observable(false),
	tenant = ko.observable(new bespoke.sph.domain.Tenant()),
	activate = function () {
	    var query = String.format("TenantId gt 0");
	    var tcs = new $.Deferred();

	    context.loadAsync("Tenant", query)
	        .then(function(lo) {
	            isBusy(false);
	            vm.tenantCollection(lo.itemCollection);
	            
	            tcs.resolve(true);
	        });
	    return tcs.promise();

		
	},
	viewAttached = function(view){

	},
	
	exportList = function () { },
        search = function () {
            var tcs = new $.Deferred();
            var tenantQuery = String.format("TenantId gt 0");

            if (vm.searchTerm.tenantIdSsmNo()) {
                tenantQuery = String.format("IdSsmNo eq '{0}'", vm.searchTerm.tenantIdSsmNo());
            }
            if (vm.searchTerm.keyword()) {
                tenantQuery += String.format(" or Name like '%{0}%'", vm.searchTerm.keyword());
            }
            console.log(tenantQuery);
            var tenantTask = context.loadAsync("Tenant", tenantQuery);
            $.when(tenantTask)
                .done(function (lo) {
                    vm.tenantCollection(lo.itemCollection);
                    tcs.resolve(true);
                });
            return tcs.promise();
        };

	var vm = {
		isBusy : isBusy,
		activate : activate,
		viewAttached: viewAttached,
		tenantCollection: ko.observableArray(),
		toolbar: ko.observable({
		    reloadCommand: function () {
		        return activate();
		    },
		    printCommand: ko.observable({
		        entity: ko.observable("Tenant"),
		        id: ko.observable(0),
		        item: tenant,
		    }),
		    exportCommand: exportList
		}),
		searchTerm: {
		    tenantIdSsmNo: ko.observable(),
		    keyword: ko.observable()
		},
		searchCommand: search
	};

	return vm;

});
