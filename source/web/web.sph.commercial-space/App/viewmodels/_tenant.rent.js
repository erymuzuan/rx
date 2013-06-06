/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" /> 
/// <reference path="../services/datacontext.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            activate = function () {
            },
            
            showDetails = function () {
                isBusy(true);
                var data = this;
                var query = "RentId eq " + data.RentId();
                context.loadOneAsync("Rent", query)
                     .then(function (d) {
                         vm.rent(d);
                         isBusy(false);
                     });
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
            init: function (b) {
                var query = "RentId gt 0";
                var tcs = new $.Deferred();
                context.loadAsync("Rent", query).done(function (lo) {
                    vm.rentCollection(lo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            activate: activate,
            rentCollection: ko.observableArray([]),
            rent: ko.observable(new bespoke.sphcommercialspace.domain.Rent()),
            showDetailsCommand: showDetails,
            addPaymentCommand: addPayment,
            saveCommand: save
            
        };

        return vm;

    });
