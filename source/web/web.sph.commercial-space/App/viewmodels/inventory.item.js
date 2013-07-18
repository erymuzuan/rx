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
        activate = function () {
            return true;
        },
	     saveInventory = function() {
	         var tcs = new $.Deferred();
	         var data = ko.mapping.toJSON({inventory : vm.inventory()});
	         isBusy(true);

	         context.post(data, "/Inventory/Save")
	             .then(function(result) {
	                 isBusy(false);

	                 
	                 tcs.resolve(result);
	             });
	         return tcs.promise();
	     };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        inventory : ko.observable(new bespoke.sphcommercialspace.domain.Inventory()),
	        saveCommand: saveInventory
	    };

	    return vm;

	});
