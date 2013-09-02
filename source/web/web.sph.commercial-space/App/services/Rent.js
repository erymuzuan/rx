/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


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

bespoke.sphcommercialspace.domain.Rent.prototype.AccumulatedAccrued = ko.observable();

bespoke.sphcommercialspace.domain.Rent.prototype.TotalPayment = function() {
    var totalPayment = this.AccumulatedAccrued + this.Amount;

    return totalPayment.toFixed(2);
};

bespoke.sphcommercialspace.domain.RentPartial = function() {
    return {
        AccumulatedAccrued: ko.observable()
    };
};