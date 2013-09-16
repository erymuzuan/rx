/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (routeData) {
                id(routeData.id);
                var query = String.format("ComplaintId eq {0}", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("Complaint", query)
                    .done(function (c) {
                    vm.complaint(c);
                    context.loadOneAsync("Tenant", "TenantId eq " + c.TenantId())
                    .done(function (tenant) {
                        vm.tenant(tenant);
                    });
                    tcs.resolve(true);
                });

                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            complaint: ko.observable(new bespoke.sph.domain.Complaint()),
            tenant: ko.observable(new bespoke.sph.domain.Tenant())
        };

        return vm;

    });
