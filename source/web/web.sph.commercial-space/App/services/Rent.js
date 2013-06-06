/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />

bespoke.sphcommercialspace.domain.Rent.prototype.RentPaymentPaid = function () {
    var sum = _(this.PaymentDistributionCollection()).reduce(function (memo, val) {
        return memo + val.Amount;
    }, 0);

    return sum.toFixed(2);
};

bespoke.sphcommercialspace.domain.Rent.prototype.Accrued = function () {
    var sum = this.Amount() - this.RentPaymentPaid();
    return sum.toFixed(2);
};

