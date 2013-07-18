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
            
            editedDeposit = ko.observable(),

            showDetails = function (deposit) {
                isBusy(true);
                var c1 = ko.mapping.fromJSON(ko.mapping.toJSON(deposit));
                var clone = _(c1).extend(new bespoke.sphcommercialspace.domain.DepositPartial(c1));
                editedDeposit(deposit);
                vm.deposit(clone);

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

                vm.depositCollection.replace(editedDeposit(), vm.deposit());

                var tcs = new $.Deferred();
                var postdata = ko.mapping.toJSON({
                    id: vm.deposit().DepositId,
                    deposits: vm.deposit().DepositPaymentCollection
                });
                context.post(postdata, "/Deposit/Save")
                    .done(function () {
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