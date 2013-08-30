/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'config'], function (context , config) {

    var isBusy = ko.observable(false),
        department = ko.observable(),
        activate = function () {
            isBusy(true);
            department(config.profile.Department);
            var dept = String.format("Department eq '{0}'", department());
            
            var m = [{ status: 'Baru', count: -1, text: "BARU", color: "bviolet" },
                { status: 'Pemeriksaan', count: -1, text: "PEMERIKSAAN", color: "blightblue" },
                { status: 'Penyenggaraan', count: -1, text: "PENYENGGARAAN", color: "bblue" },
                { status: 'Selesai', count: -1, text: "SELESAI", color: "bgreen" }
            ];
            var tcs = new $.Deferred();
            _(m).each(function (s) {
                context.getCountAsync("Maintenance", "Status eq '" + s.status + "' and " + dept, "Status")
                    .then(function (c) {
                        s.count = c;
                        var done = _(m).every(function (st) { return st.count >= 0; });
                        if (done) {
                            vm.maintenances(m);
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
        title: 'Papan Senggara',
        maintenances: ko.observableArray(),

    };

    return vm;


});