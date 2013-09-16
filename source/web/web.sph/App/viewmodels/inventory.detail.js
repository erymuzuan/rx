/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'durandal/plugins/router'],
	function (context, router) {
	    var
        isBusy = ko.observable(false),
	    activate = function (routeData) {
	        var id = parseInt(routeData.id);
	        if (id === 0) {
	            vm.inventory(new bespoke.sph.domain.Inventory());
	            return true;
	        }

	        var query = String.format("InventoryId eq {0}", id);
	        var tcs = new $.Deferred();
	        context.loadOneAsync("Inventory", query)
	            .done(function (b) {
	                vm.inventory(b);
	                tcs.resolve(true);
	            });

	        return tcs.promise();
	    },
	     saveInventory = function () {
	         var tcs = new $.Deferred();
	         var data = ko.toJSON({ inventory: vm.inventory() });
	         isBusy(true);

	         context.post(data, "/Inventory/Save")
	             .then(function (result) {
	                 isBusy(false);
	                 vm.inventory(new bespoke.sph.domain.Inventory());
	                 router.navigateTo("/#/inventory.list");
	                 tcs.resolve(result);
	             });
	         return tcs.promise();
	     };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        inventory: ko.observable(new bespoke.sph.domain.Inventory()),
	        toolbar: {
	            saveCommand: saveInventory
	        }
	    };

	    return vm;

	});
