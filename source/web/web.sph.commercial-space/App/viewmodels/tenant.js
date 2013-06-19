﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
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

	};

	var vm = {
		isBusy : isBusy,
		activate : activate,
		viewAttached: viewAttached,
		tenantCollection: ko.observableArray()
	};

	return vm;

});
