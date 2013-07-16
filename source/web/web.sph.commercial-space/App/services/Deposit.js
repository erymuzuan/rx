/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />

bespoke.sphcommercialspace.domain.DepositPartial = function (model) {
    var calcPayment = function () {
        var sum = _(model.DepositPaymentCollection()).reduce(function (memo, val) {
            return memo + parseFloat(val.Amount());
        }, 0);

        return sum.toFixed(2);
    },
        calcBalance = function () {
            var paid = _(model.DepositPaymentCollection()).reduce(function (memo, val) {
                return memo + parseFloat(val.Amount());
            }, 0);
             var sum = model.Amount() - paid;
             return sum.toFixed(2);
        };
    
    return {
        DepositPaid: ko.computed(calcPayment),
        DepositBalance: ko.computed(calcBalance)
    };
};