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
                         vm.deposit(d);
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
                vm.deposit().DepositPaymentCollection.push(payment);
            },
            save = function () {
                var tcs = new $.Deferred();
                var depositPayment = ko.mapping.toJS(vm.deposit().DepositPaymentCollection());
                var postdata = JSON.stringify({ id: vm.deposit().DepositId(), deposits: depositPayment });
                context.post(postdata, "/Deposit/Save").done(function (e) {
                    tcs.resolve(true);
                });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            showDetailsCommand: showDetails,
            depositCollection: ko.observableArray(),
            deposit: ko.observable(new bespoke.sphcommercialspace.domain.Deposit()),
            addPaymentCommand: addPayment,
            saveCommand: save
        };

        return vm;
    });