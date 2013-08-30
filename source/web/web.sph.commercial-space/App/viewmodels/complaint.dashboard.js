/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext'], function (context) {

    var isBusy = ko.observable(false),
        activate = function () {
            isBusy(true);
            var compl = [{ status: 'Baru', count: -1, text: "BARU", color: "bblue" },
                { status: 'Dalam Proses', count: -1, text: "MENUNGGU", color: "bgreen" },
            ];
            var tcs = new $.Deferred();
            _(compl).each(function (s) {
                context.getCountAsync("Complaint", "Status eq '" + s.status + "'", "Status")
                    .then(function (c) {
                        s.count = c;
                        var done = _(compl).every(function (st) { return st.count >= 0; });
                        if (done) {
                            vm.complaints(compl);
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
        complaints: ko.observableArray(),
        contracts: ko.observableArray()

    };

    return vm;


});