/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />

define(['services/datacontext'],
	function (context) {
	    var
        isBusy = ko.observable(false),
	    inventory = ko.observable(new bespoke.sph.domain.Inventory()),
        activate = function () {
            return true;
        },
	    exportList = function (){};

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        inventoryCollection: ko.observableArray(),
	        inventory : inventory,
	        toolbar: ko.observable({
	            reloadCommand: function () {
	                return activate();
	            },
	            addNew : {
	                location: '/#/inventory.detail/0',
	                caption : 'Tambah baru'
	            },
	            printCommand: ko.observable({
	                entity: ko.observable("Inventory"),
	                id: ko.observable(0),
	                item: inventory,
	            }),
	            exportCommand: exportList,
	        })
	    };

	    return vm;

	});
