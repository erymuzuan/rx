/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />

bespoke.sphcommercialspace.domain.DepositPartial = function (model) {
    var depositPaid = ko.computed(function () {
        var sum = _(model.DepositPaymentCollection()).reduce(function (memo, val) {
                return memo + parseFloat(val.Amount());
            }, 0);

            return sum.toFixed(2);
        }),
        depositBalance = ko.computed(function () {
            var sum = model.Amount() - depositPaid();
            return sum.toFixed(2);
        });

    return {
        DepositPaid: depositPaid,
        DepositBalance: depositBalance
    };
};
