/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.ContractPartial = function () {
    var calculateAccumulatedAccrued = function (context) {
        var r = this;
        var queryInvoice = String.format("ContractNo eq '{0}'", r.ReferenceNo());
        var queryPayment = String.format("ContractNo eq '{0}'", r.ReferenceNo());

        var totalInvoiceTask = context.getSumAsync("Invoice", queryInvoice, "Amount");
        var totalPaymentTask = context.getSumAsync("Payment", queryPayment, "Amount");

        var tcs = new $.Deferred();
        $.when(totalInvoiceTask, totalPaymentTask)
            .then(function (totalinvoice, totalpayment) {
                var accrued = parseFloat(totalinvoice) - parseFloat(totalpayment);
                r.Accrued(accrued);
                r.CanPaid(true);
                if (!accrued) {
                    r.CanPaid(false);
                }
                tcs.resolve(accrued);
            });
        return tcs.promise();
    };
    return {
        Accrued: ko.observable(),
        CanPaid: ko.observable(),
        getAccruedAmount: calculateAccumulatedAccrued
    };
};