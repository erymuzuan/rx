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
    return {
        Accrued: ko.observable()
    };
};