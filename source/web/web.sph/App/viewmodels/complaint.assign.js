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
    function (context, router) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (routeData) {
                id(routeData.id);
                var query = String.format("ComplaintId eq {0}", id());
                var tcs = new $.Deferred();
                var complaintTask = context.loadOneAsync("Complaint", query);
                var departmentTask =context.loadOneAsync("Setting","Key eq 'Departments'");
               
                    $.when(complaintTask,departmentTask).done(function (b,s) {
                        vm.complaint(b);
                        context.loadOneAsync("Tenant", "TenantId eq " + b.TenantId())
                        .done(function (tenant) {
                            vm.tenant(tenant);
                        });
                        if (s) {
                            var departments = JSON.parse(s.Value());
                            vm.departmentOptions(departments);
                        }
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ comp: vm.complaint() });
                isBusy(true);

                context.post(data, "/Complaint/Assign")
                    .then(function (result) {
                        isBusy(false);
                        var url = '/#/complaint.dashboard';
                        router.navigateTo(url);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            departmentOptions: ko.observableArray(),
            complaint: ko.observable(new bespoke.sph.domain.Complaint()),
            tenant: ko.observable(new bespoke.sph.domain.Tenant()),
            toolbar : {
                commands : ko.observableArray([
                {
                    caption: 'Simpan',
                    icon: 'icon-file-text',
                    command: save,
                }
                ])
            }
        };

        return vm;

    });
