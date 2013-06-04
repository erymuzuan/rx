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
            applicationId = ko.observable(),
            activate = function () {
                var tcs = new $.Deferred();
                context.loadAsync("RentalApplication", "Status eq 'Confirmed'").done(function (lo) {
                    vm.applicationCollection(lo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            showDetails = function () {
                isBusy(true);
                var data = this;
                var query = "RentalApplicationId eq " + data.RentalApplicationId();
                applicationId(data.RentalApplicationId());
                context.loadOneAsync("RentalApplication", query)
                     .then(function (ra) {
                         ko.mapping.fromJSON(ko.mapping.toJSON(ra.Offer), {}, vm.offer);
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
                var offer = ko.mapping.toJS(vm.offer);
                var postdata = JSON.stringify({ id: applicationId(), offer: offer });
                context.post(postdata, "/RentalApplication/SaveDepositPayment").done(function (e) {
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
            applicationCollection: ko.observableArray([]),
            offer: {
                Deposit: ko.observable(),
                DepositPaid: ko.observable(),
                DepositBalance: ko.observable(),
                DepositPaymentCollection: ko.observableArray()
            },
            addPaymentCommand: addPayment,
            saveCommand: save
        };

        return vm;
    });