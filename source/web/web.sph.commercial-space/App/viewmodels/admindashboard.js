/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger'], function (context, logger) {

    var isBusy = ko.observable(false),
        activate = function () {
            isBusy(true);
            var apps = [{ status: 'New', count: -1, text: "Baru", color: "bred" },
                { status: 'Approved', count: -1, text: "Lulus", color: "bgreen" },
                { status: 'Waiting', count: -1, text: "Menunggu", color: "bviolet" },
                { status: 'Declined', count: -1, text: "Ditolak", color: "bred" },
                { status: 'Confirmed', count: -1, text: "Tawaran diterima", color: "bblue" },
                { status: 'OfferRejected', count: -1, text: "Tawaran ditolak", color: "bred" },
                { status: 'Offered', count: -1, text: "Ditwarkan", color: "bgreen" },
                { status: 'Returned', count: -1, text: "Dikembalikan", color: "borange" }
            ];
            var tcs = new $.Deferred();
            _(apps).each(function (s) {
                context.getCountAsync("RentalApplication", "Status eq '" + s.status + "'", "Status")
                    .then(function (c) {
                        s.count = c;
                        var done = _(apps).every(function (st) { return st.count >= 0; });
                        if (done) {
                            vm.applications(apps);
                            isBusy(false);
                            tcs.resolve(true);
                        }
                    });
            });
            return tcs.promise();
        };


    var vm = {
        activate: activate,
        isBusy: isBusy,
        title: 'Papan Tugas',
        applications: ko.observableArray(),
        contracts: ko.observableArray()

    };

    return vm;


});