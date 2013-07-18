/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'durandal/plugins/router'],
    function (context,router) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (routeData) {
                id(routeData.id);
                var maintenanceQuery = String.format("MaintenanceId eq {0}", id());
                var officerQuery = String.format("Designation eq 'Petugas'");
                var tcs = new $.Deferred();

                var maintenanceTask = context.loadOneAsync("Maintenance", maintenanceQuery);
                var officerTask = context.getTuplesAsync("UserProfile", officerQuery, 'Username', 'FullName');

                $.when(maintenanceTask, officerTask).then(function (m, o) {
                    isBusy(false);
                    vm.maintenance(m);
                    vm.officerOptions(o);
                    tcs.resolve(true);
                });

                return tcs.promise();
            },

            saveAssignment = function () {
                var tcs = new $.Deferred();
                var data = ko.toJSON({ officer: vm.maintenance().Officer, id: id() });
                isBusy(true);

                context.post(data, "/Maintenance/Assign")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                        $('#success-panel').modal({})
                         .on('hidden', function () {
                             var url = '/#/maintenance.list';
                             router.navigateTo(url);
                         });
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            maintenance: ko.observable(new bespoke.sphcommercialspace.domain.Maintenance()),
            officerOptions: ko.observableArray(),
            saveAssignmentCommand: saveAssignment
        };

        return vm;

    });
