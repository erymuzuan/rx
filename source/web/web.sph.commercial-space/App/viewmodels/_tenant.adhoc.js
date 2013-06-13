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

	    var isBusy = ko.observable(false),
	         adhocInvoice = _.extend(new bespoke.sphcommercialspace.domain.Invoice(), new bespoke.sphcommercialspace.domain.AdhocInvoice()),
		     activate = function (tenant) {
		         context.getTuplesAsync("Contract", "TenantIdSsmNo eq '" + tenant.IdSsmNo() + "'", "ContractId", "ReferenceNo")
                     .done(function (list) {
                         vm.contractOptions(list);
                     });

		         var query = String.format("TenantIdSsmNo eq " + "'" + tenant.IdSsmNo() + "'" + " and Type eq 'AdhocInvoice'");
		         var tcs = new $.Deferred();
		         context.loadAsync("Invoice", query)
                     .then(function (lo) {
                         isBusy(false);
                         vm.invoiceCollection(lo.itemCollection);
                         tcs.resolve(true);
                     });
		         vm.invoice().TenantIdSsmNo(tenant.IdSsmNo());
		         return tcs.promise();

		     },
		viewAttached = function (view) {
		    _uiready.init(view);
		},
		getInvoiceNo = function () {
		    var tcs = new $.Deferred();
		    var contractNo = _(vm.contractOptions()).find(function (o) { return o.Item1 == vm.selectedContractId(); }).Item2;
		    var data = JSON.stringify({ contractId: vm.selectedContractId(), type: vm.selectedInvoiceType() });
		    isBusy(true);

		    context.post(data, "/Invoice/GetAddhocInvoiceNo")
		        .then(function (result) {
		            isBusy(false);
		            vm.invoice().No(result);
		            vm.invoice().ContractNo(contractNo);
		            vm.invoice().Type(bespoke.sphcommercialspace.domain.InvoiceType.ADHOC_INVOICE);

		            tcs.resolve(result);
		        });
		    return tcs.promise();
		},

		removeItem = function (item) {
		    vm.invoice().InvoiceItemCollection.remove(item);
		},
	    
        addItem = function () {
            var item = new bespoke.sphcommercialspace.domain.InvoiceItem();
            vm.invoice().InvoiceItemCollection.push(item);
        },
	    showInvoiceDetail = function(data) {
	        isBusy(true);
	        var query = "InvoiceId eq " + data.InvoiceId();
	        context.loadOneAsync("Invoice", query)
	            .then(function(d) {
	                vm.invoice(d);
	                isBusy(false);
	            });
	        $('#add-invoice-modal').modal({});
	    },
	    save = function () {
		    var sum = _(vm.invoice().InvoiceItemCollection()).reduce(function (memo, val) {
		        var s = memo + parseFloat(val.Amount());
		        return s;
		    }, 0);
		    vm.invoice().Amount(sum);
		    var tcs = new $.Deferred();
		    var data = ko.mapping.toJSON({ invoice: vm.invoice });
		    isBusy(true);

		    context.post(data, "/Invoice/SaveAdhocInvoice")
		        .then(function (result) {
		            isBusy(false);
		            tcs.resolve(result);
		            vm.invoice(_.extend(new bespoke.sphcommercialspace.domain.Invoice(), new bespoke.sphcommercialspace.domain.AdhocInvoice()));
		            vm.invoiceCollection.push(result);
		        });
		    return tcs.promise();
		};

	    var vm = {
	        selectedContractId: ko.observable(),
	        selectedInvoiceType: ko.observable(),
	        contractOptions: ko.observableArray(),
	        invoice: ko.observable(adhocInvoice),
	        getInvoiceNoCommand: getInvoiceNo,
	        isBusy: isBusy,
	        activate: activate,
	        addItem: addItem,
	        viewAttached: viewAttached,
	        invoiceCollection: ko.observableArray(),
	        removeItemCommand: removeItem,
	        showInvoiceDetail : showInvoiceDetail,
	        saveCommand: save
	    };

	    return vm;

	});
