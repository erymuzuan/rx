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


define(['services/datacontext', 'services/logger'],
	function (context, logger) {

	    var isBusy = ko.observable(false),
            activate = function () { return true; },
            searchPayment = function () {
                vm.contractCollection.removeAll();
                var tcs = new $.Deferred();
                var query = "";
                if (vm.contractNo() && vm.idSsmNo()) {
                    var query2 = String.format("TenantIdSsmNo eq '{0}' and ReferenceNo eq '{1}'", vm.idSsmNo(), vm.contractNo());
                    query = query + query2;
                }
                if (vm.contractNo() && !vm.idSsmNo()) {
                    var query3 = String.format("ReferenceNo eq '{0}'", vm.contractNo());
                    query = query + query3;
                }
                if (vm.idSsmNo() && !vm.contractNo()) {
                    var query4 = String.format("TenantIdSsmNo eq '{0}'", vm.idSsmNo());
                    query = query + query4;
				}
                if (!vm.idSsmNo() && !vm.contractNo()) {
                    var query5 = String.format("ContractId gt 0", vm.idSsmNo());
                    query = query + query5;
                }
				context.loadAsync("Contract", query)
                    .done(function (lo) {
                        isBusy(false);
                        var contracts = _(lo.itemCollection).map(function (r) {
                            return _(r).extend(new bespoke.sphcommercialspace.domain.ContractPartial());
                        });
                        _(contracts).each(function(r2){
                            r2.getAccruedAmount(context)
                                .done(function(amount){
                                    r2.Accrued(amount);
                                });
                        });
                        vm.contractCollection(contracts);
                        tcs.resolve(true);
                        vm.contractNo('');
                        vm.idSsmNo('');
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
                var json = JSON.stringify({payment: payment });
                context.post(json, "/Payment/Save")
					.done(function (e) {
					    var query = String.format("ContractId gt 0");
					    context.loadAsync("Contract", query)
					        .then(function(lo) {
					            isBusy(false);
					            vm.contractCollection(lo.itemCollection);
					        });
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
