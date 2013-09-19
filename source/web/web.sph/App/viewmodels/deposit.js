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
            deposit = deposit,
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
                var clone = c1;
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
            removeDepositPayment = function (payment) {
               vm.deposit().DepositPaymentCollection.remove(payment);
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
            },
            exportList = function () { },
        search = function () {
            var tcs = new $.Deferred();
            var depositQuery = String.format("DepositId gt 0");

            if (vm.searchTerm.registrationNo()) {
                depositQuery = String.format("RegistrationNo eq '{0}'", vm.searchTerm.registrationNo());
            }
            if (vm.searchTerm.keyword()) {
                depositQuery += String.format(" or Name like '%{0}%'", vm.searchTerm.keyword());
            }
            console.log(depositQuery);
            var depositTask = context.loadAsync("Deposit", depositQuery);
            $.when(depositTask)
                .done(function (lo) {
                    vm.depositCollection(lo.itemCollection);
                    tcs.resolve(true);
                });
            return tcs.promise();
        };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            showDetailsCommand: showDetails,
            depositCollection: ko.observableArray(),
            deposit: ko.observable(new bespoke.sph.domain.Deposit()),
            addPaymentCommand: addPayment,
            removeDepositPaymentCommand: removeDepositPayment,
            saveCommand: save,
            toolbar: ko.observable({
                reloadCommand: function () {
                    return activate();
                },
                printCommand: ko.observable({
                    entity: ko.observable("Tenant"),
                    id: ko.observable(0),
                    item: deposit,
                }),
                exportCommand: exportList
            }),
            searchTerm: {
                registrationNo: ko.observable(),
                keyword: ko.observable()
            },
            searchCommand: search
        };

        return vm;
    });