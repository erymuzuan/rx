/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../services/Contract.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
	function (context, logger, router) {

	    var isBusy = ko.observable(false),
            activate = function () { return true; },
            searchPayment = function () {
                vm.contractCollection.removeAll();
                var tcs = new $.Deferred();
                var query = "";
                if (vm.contractNo() && vm.idSsmNo()) {
                    var query2 = String.format("TenantIdSsmNo eq '{0}' and ReferenceNo eq '{1}'", vm.idSsmNo(), vm.contractNo());
                    query = query + query2;
                };
                if (vm.contractNo() && !vm.idSsmNo()) {
                    var query3 = String.format("ReferenceNo eq '{0}'", vm.contractNo());
                    query = query + query3;
                };
                if (vm.idSsmNo() && !vm.contractNo()) {
                    var query4 = String.format("TenantIdSsmNo eq '{0}'", vm.idSsmNo());
                    query = query + query4;
                };

                context.loadAsync("Contract", query).done(function (lo) {
                    isBusy(false);
                    var contracts = _(lo.itemCollection).map(function (r) {
                        var r2 = _(r).extend(new bespoke.sphcommercialspace.domain.ContractPartial());
                        calculateAccumulatedAccrued(r2);
                        return r2;
                    });
                    vm.contractCollection(contracts);
                    tcs.resolve(true);
                    vm.contractNo('');
                    vm.idSsmNo('');
                });

                return tcs.promise();
            },

             calculateAccumulatedAccrued = function (r) {
                 var queryInvoice = String.format("ContractNo eq '{0}'", r.ReferenceNo());
                 var queryPayment = String.format("ContractNo eq '{0}'", r.ReferenceNo());

                 var totalInvoiceTask = context.getSumAsync("Invoice", queryInvoice,"Amount");
                 var totalPaymentTask = context.getSumAsync("Payment", queryPayment,"Amount");

                 var tcs = new $.Deferred();
                 $.when(totalInvoiceTask, totalPaymentTask)
                      .then(function (totalinvoice, totalpayment) {
                          var accrued = parseFloat(totalinvoice) - parseFloat(totalpayment);
                          r.Accrued(accrued);
                          tcs.resolve(accrued);
                      });
                 return tcs.promise();
             },

	        showDetails = function (data) {
	            vm.payment().ContractNo(data.ReferenceNo());
	            vm.payment().TenantIdSsmNo(data.Tenant.IdSsmNo());
	            $('#set-payment-modal').modal({});
	        },

            save = function () {
                var tcs = new $.Deferred();
                var payment = ko.mapping.toJS(vm.payment());
                var postdata = JSON.stringify({payment: payment });
                context.post(postdata, "/Payment/Save").done(function (e) {
                    logger.log("Payment received", e, "payment", true);
                    tcs.resolve(true);
                });
                return tcs.promise();
            };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        contractNo: ko.observable(),
	        idSsmNo: ko.observable(),
	        searchPaymentCommand: searchPayment,
	        contractCollection: ko.observableArray([]),
	        showDetailsCommand: showDetails,
	        payment : ko.observable (new bespoke.sphcommercialspace.domain.Payment()),
	        saveCommand: save

	    };

	    return vm;


	});
