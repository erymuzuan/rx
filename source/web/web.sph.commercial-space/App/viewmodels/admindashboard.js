/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext'], function (context) {

    var isBusy = ko.observable(false),
        activate = function () {
            isBusy(true);
            var apps = [{ status: 'New', count: -1, text: "BARU", color: "bred" },
                { status: 'Waiting', count: -1, text: "MENUNGGU", color: "bviolet" },
                { status: 'Approved', count: -1, text: "LULUS", color: "bgreen" },
                { status: 'Returned', count: -1, text: "DIKEMBALIKAN", color: "borange" },
                { status: 'Declined', count: -1, text: "BATAL", color: "bred" },
                { status: 'Completed', count: -1, text: "SELESAI", color: "bblue" }
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