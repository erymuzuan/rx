/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />

bespoke.sphcommercialspace.domain.Rent.prototype.Accrued = function () {
    var sumPaid = _(this.PaymentDistributionCollection()).reduce(function (memo, val) {
        return memo + val.Amount;
    }, 0);
    var accrued = this.Amount() - sumPaid;
    return accrued.toFixed(2);
};


