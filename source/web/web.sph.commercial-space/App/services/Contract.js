/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />

bespoke.sphcommercialspace.domain.Offer.prototype.DepositPaid = function () {
    return _(this.DepositPaymentCollection).reduce(function (memo, val) {
        return memo + val.Amount;
    }, 0);
};