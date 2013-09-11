/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.DepositPartial = function (model) {
    var calcPayment = function () {
        var sum = _(model.DepositPaymentCollection).reduce(function (memo, val) {
            return memo + parseFloat(val.Amount);
        }, 0);

        return sum.toFixed(2);
    },
        calcBalance = function () {
            var paid = _(model.DepositPaymentCollection).reduce(function (memo, val) {
                return memo + parseFloat(val.Amount);
            }, 0);
             var sum = model.Amount - paid;
             return sum.toFixed(2);
        };
    
    return {
        DepositPaid: ko.computed(calcPayment),
        DepositBalance: ko.computed(calcBalance)
    };
};