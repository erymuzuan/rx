/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" /> 
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
	function (context) {

		var
		isBusy = ko.observable(false),
		activate = function (tenant) {
			var query = String.format("TenantId eq " + tenant.TenantId());
			var tcs = new $.Deferred();
			context.loadAsync("Invoice", query)
				.then(function (lo) {
					isBusy(false);
					vm.adhocInvoiceCollection(lo.itemCollection);
					tcs.resolve(true);
				});
			return tcs.promise();

		},
		viewAttached = function (view) {
			_uiready.init(view);
		},
		addAdhoc = function () {
			var adhoc = {
			    ContractNo : ko.observable(),
			    Date : ko.observable(),
			    Category : ko.observable(),
			    Amount : ko.observable()
			};
			vm.adhocInvoiceCollection.push(adhoc);
		},

		removeAdhoc = function () {
			vm.adhocInvoiceCollection.remove(this);
		},
		save = function () {
		    var tcs = new $.Deferred();
		    var data = JSON.stringify({ adhocInvoices: ko.mapping.toJS(vm.adhocInvoiceCollection) });
		    isBusy(true);

		    context.post(data, "/Invoice/SaveAdhoc")
		        .then(function(result) {
		            isBusy(false);

		            tcs.resolve(result);
		        });
		    return tcs.promise();
		};

		var vm = {
			isBusy: isBusy,
			activate: activate,
			viewAttached: viewAttached,
			adhocInvoiceCollection: ko.observableArray(),
			addAdhocCommand: addAdhoc,
			removeAdhocCommand: removeAdhoc,
			saveCommand: save
		};

		return vm;

	});
