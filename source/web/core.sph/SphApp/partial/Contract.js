/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ContractPartial = function () {
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