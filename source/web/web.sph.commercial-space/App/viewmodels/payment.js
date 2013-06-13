/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
	function (context, logger, router) {

	    var isBusy = ko.observable(false),
            activate = function () { return true; },
            searchPayment = function () {
                vm.paymentCollection.removeAll();
                var tcs = new $.Deferred();
                var query = "";
                if (vm.contractNo() && vm.idSsmNo()) {
                    var query2 = String.format("TenantIdSsmNo eq '{0}' and ContractNo eq '{1}'", vm.idSsmNo(), vm.contractNo());
                    query = query + query2;
                };
                if (vm.contractNo() && !vm.idSsmNo() ) {
                    var query3 = String.format("ContractNo eq '{0}'" ,vm.contractNo());
                    query = query + query3;
                };
                if (vm.idSsmNo() && !vm.contractNo()) {
                    var query4 = String.format("TenantIdSsmNo eq '{0}'" , vm.idSsmNo());
                    query = query + query4;
                };
                context.getCountAsync("Payment", query, "PaymentId").done(function(c) {
                    if (!c) {
                        context.loadAsync("Invoice", query).done(function(lo) {
                            isBusy(false);
                            _.each(lo.itemCollection, function(item) {
                                var payment = new bespoke.sphcommercialspace.domain.Payment();
                                payment.Date(item.Date());
                                payment.Amount(item.Amount());
                                payment.ContractNo(item.ContractNo());
                                payment.TenantIdSsmNo(item.TenantIdSsmNo());
                                vm.paymentCollection.push(payment);
                            });
                            tcs.resolve(true);
                            vm.contractNo('');
                            vm.idSsmNo('');
                        });
                    }
                    return tcs.promise();
                });
            },

	        showDetails = function () {
	            isBusy(true);

	            $('#set-payment-modal').modal({});
	        },


            addPayment = function () {
                var payment = {
                    ReceiptNo: ko.observable(),
                    Amount: ko.observable(),
                    DateTime: ko.observable()
                };
                vm.payment().PaymentDistributionCollection.push(payment);
            },
            save = function () {
                var tcs = new $.Deferred();
                var rentPayment = ko.mapping.toJS(vm.rent().PaymentDistributionCollection());
                var postdata = JSON.stringify({ id: vm.rent().RentId(), rents: rentPayment });
                context.post(postdata, "/Rent/Save").done(function (e) {
                    logger.log("Rent payment received", e, "rent", true);
                    tcs.resolve(true);
                });
                return tcs.promise();
            };

var vm = {
    isBusy: isBusy,
    activate: activate,
    contractNo: ko.observable(),
    idSsmNo: ko.observable(),
    searchPaymentCommand : searchPayment,
    paymentCollection: ko.observableArray([]),
    payment: ko.observable(new bespoke.sphcommercialspace.domain.Payment()),
    showDetailsCommand: showDetails,
    addPaymentCommand: addPayment,
    saveCommand: save

};

return vm;


});
