/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />

bespoke.sphcommercialspace.domain.Offer.prototype.DepositPaid = function () {
    var sum =  _(this.DepositPaymentCollection()).reduce(function (memo, val) {
        return memo + val.Amount;
    }, 0);

    return sum.toFixed(2);
};

bespoke.sphcommercialspace.domain.Contract.prototype.Accrued = ko.observable();

bespoke.sphcommercialspace.domain.ContractPartial = function () {


    var calculateAccumulatedAccrued = function (context) {
        var r = this;
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
    };
    return {
        Accrued: ko.observable(),
        getAccruedAmount : calculateAccumulatedAccrued
    };
};