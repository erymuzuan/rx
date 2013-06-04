/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            deposit = ko.observable(new bespoke.sphcommercialspace.domain.Deposit()),
            activate = function () {
                var tcs = new $.Deferred();
                context.loadAsync("Deposit", "IsPaid eq 0").done(function (lo) {
                    vm.depositCollection(lo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            showDetails = function () {
                isBusy(true);
                var data = this;
                var query = "DepositId eq " + data.DepositId();
                context.loadOneAsync("Deposit", query)
                     .then(function (d) {
                         ko.mapping.fromJSON(ko.mapping.toJSON(d), {}, vm.deposit);
                         isBusy(false);
                     });
                $('#deposit-modal').modal({});
            },
            addPayment = function () {
                var payment = {
                    ReceiptNo: ko.observable(),
                    Amount: ko.observable(),
                    Date: ko.observable()
                };
                vm.offer.DepositPaymentCollection.push(payment);
            },
            configureDate = function () {
            },
            save = function () {
                var tcs = new $.Deferred();
                var depositPayment = ko.mapping.toJS(vm.deposit);
                var postdata = JSON.stringify({ id: vm.DepositId(), deposit: depositPayment });
                context.post(postdata, "/Deposit/Save").done(function (e) {
                    logger.log("Deposit payment received", e, "deposit", true);
                    tcs.resolve(true);
                });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            configureDate: configureDate,
            showDetailsCommand: showDetails,
            depositCollection: ko.observableArray([]),
            deposit: deposit,
            addPaymentCommand: addPayment,
            saveCommand: save
        };

        return vm;
    });