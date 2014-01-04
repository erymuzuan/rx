/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.Rent.prototype.RentPaid = function () {
    var sumPaid = _(this.PaymentDistributionCollection()).reduce(function (memo, val) {
        return memo + val.Amount;
    }, 0);
    
    return sumPaid.toFixed(2);
};

bespoke.sph.domain.Rent.prototype.Accrued = function () {
    var sumPaid = this.Amount() - this.RentPaid;
    return sumPaid.toFixed(2);
};

bespoke.sph.domain.Rent.prototype.AccumulatedAccrued = ko.observable();

bespoke.sph.domain.Rent.prototype.TotalPayment = function() {
    var totalPayment = this.AccumulatedAccrued + this.Amount;

    return totalPayment.toFixed(2);
};

bespoke.sph.domain.RentPartial = function() {
    return {
        AccumulatedAccrued: ko.observable()
    };
};