/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext'], function (context) {

    var isBusy = ko.observable(false),
        activate = function () {
            isBusy(true);
            var apps = [{ status: 'Baru', count: -1, text: "BARU"},
                { status: 'Menunggu', count: -1, text: "MENUNGGU"},
                { status: 'Diluluskan', count: -1, text: "LULUS"},
                { status: 'Dikembalikan', count: -1, text: "DIKEMBALIKAN" },
                { status: 'Ditolak', count: -1, text: "BATAL" },
                { status: 'Selesai', count: -1, text: "SELESAI" }
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