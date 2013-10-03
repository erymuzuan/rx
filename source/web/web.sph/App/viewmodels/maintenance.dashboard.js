/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/cultures.my.js" />

define([objectbuilders.datacontext, objectbuilders.config,objectbuilders.cultures], function (context , config,culture) {

    var isBusy = ko.observable(false),
        department = ko.observable(),
        activate = function () {
            isBusy(true);
            department(config.profile.Department);
            var dept = String.format("Department eq '{0}'", department());
            var status = culture.maintenance;
            var m = [{ status: status.NEW_MAINTENANCE_STATUS_CAPTION, count: -1, text: status.NEW_MAINTENANCE_STATUS_CAPTION.toUpperCase(), color: "bviolet" },
                { status: status.INSPECTION_MAINTENANCE_STATUS_CAPTION, count: -1, text: status.INSPECTION_MAINTENANCE_STATUS_CAPTION.toUpperCase(), color: "blightblue" },
                { status: status.INPROGRESS_MAINTENANCE_STATUS_CAPTION, count: -1, text: status.INPROGRESS_MAINTENANCE_STATUS_CAPTION.toUpperCase(), color: "bblue" },
                { status: status.DONE_MAINTENANCE_STATUS_CAPTION, count: -1, text: status.DONE_MAINTENANCE_STATUS_CAPTION.toUpperCase(), color: "bgreen" }
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