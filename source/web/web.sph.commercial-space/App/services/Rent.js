/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />

bespoke.sphcommercialspace.domain.Rent.prototype.RentPaid = function () {
    var sumPaid = _(this.PaymentDistributionCollection()).reduce(function (memo, val) {
        return memo + val.Amount;
    }, 0);
    
    return sumPaid.toFixed(2);
};

bespoke.sphcommercialspace.domain.Rent.prototype.Accrued = function () {
    var sumPaid = this.Amount() - this.RentPaid;
    return sumPaid.toFixed(2);
};

bespoke.sphcommercialspace.domain.Rent.prototype.AccumulatedAccrued = function () {
    
};

bespoke.sphcommercialspace.domain.Rent.prototype.TotalPayment = function() {
    var totalPayment = this.AccumulatedAccrued + this.Amount;

    return totalPayment.toFixed(2);
};

