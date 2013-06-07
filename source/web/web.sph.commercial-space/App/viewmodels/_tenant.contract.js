/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'], 
	function (context) {

	var 
	isBusy = ko.observable(false),
	activate = function() {
	    return true;
	},
	viewAttached = function(view){

	};

	var vm = {
	    isBusy: isBusy,
	    init: function (b) {
	        var query = "ContractId gt 0";
	        var tcs = new $.Deferred();
	        context.loadAsync("Contract", query).done(function (lo) {
	            vm.contractCollection(lo.itemCollection);
	            tcs.resolve(true);
	        });
	        return tcs.promise();
	    },
		activate : activate,
		viewAttached: viewAttached,
		contractCollection: ko.observableArray()
	};

	return vm;

});
