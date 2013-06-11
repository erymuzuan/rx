/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" /> 
/// <reference path="../services/datacontext.js" />
/// <reference path="~/App/services/Rent.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            activate = function (contractId) {
                var query = "RentId gt 0";
                var tcs = new $.Deferred();
                context.loadAsync("Rent", query).done(function (lo) {
                    var rents = _(lo.itemCollection).map(function (r) {
                        var r2 = _(r).extend(new bespoke.sphcommercialspace.domain.RentPartial());
                        calculateAccumulatedAccrued(r2);
                        return r2;
                    });
                    vm.rentCollection(rents);
                    tcs.resolve(true);
                });
                return tcs.promise();

            },



            calculateAccumulatedAccrued = function (r) {
                var query = String.format("RentId eq {0} and ContractNo eq '{1}'", r.RentId(), r.ContractNo());
                var prvMonthQuery = String.format("RentId eq {0} and ContractNo eq '{1}' and Month eq {2} and Year eq {3}", r.RentId(), r.ContractNo(), r.Month(), r.Year());

                var accruedPrvMonthTask = context.loadOneAsync("Rent", prvMonthQuery);
                var currentMonthTask = context.loadOneAsync("Rent", query);

                var tcs = new $.Deferred();
                $.when(currentMonthTask, accruedPrvMonthTask)
                     .then(function (current, prv) {
                         var accumulatedAccrued = prv.AccumulatedAccrued - prv.TotalPayment;
                         r.AccumulatedAccrued(accumulatedAccrued);
                         tcs.resolve(accumulatedAccrued);

                     });
                return tcs.promise();
            },

            showDetails = function () {
                isBusy(true);

                $('#tenant-rent-payment-modal').modal({});
            },


            addPayment = function () {
                var payment = {
                    ReceiptNo: ko.observable(),
                    Amount: ko.observable(),
                    DateTime: ko.observable()
                };
                vm.rent().PaymentDistributionCollection.push(payment);
            },
            save = function () {
                var tcs = new $.Deferred();
                var rentPayment = ko.mapping.toJS(vm.rent().PaymentDistributionCollection());
                var postdata = JSON.stringify({ id: vm.rent().RentId(), rents: rentPayment });
                context.post(postdata, "/Rent/Save").done(function (e) {
                    logger.log("Rent payment received", e, "rent", true);
                    tcs.resolve(true);
                });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            rentCollection: ko.observableArray([]),
            rent: ko.observable(_.extend(new bespoke.sphcommercialspace.domain.Rent(), new bespoke.sphcommercialspace.domain.RentPartial())),
            showDetailsCommand: showDetails,
            addPaymentCommand: addPayment,
            saveCommand: save

        };

        return vm;

    });
