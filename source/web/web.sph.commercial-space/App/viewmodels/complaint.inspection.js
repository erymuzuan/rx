/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/mockComplaintContext', 'services/datacontext'],
    function (mockcontext,context) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (routeData) {
                id(routeData.id);
                var query = String.format("ComplaintId eq {0}", id());
                var tcs = new $.Deferred();
                mockcontext.loadOneAsync("Complaint", query)
                    .done(function (b) {
                        vm.complaint(b);
                        var tenantTask = context.loadOneAsync("Tenant", "TenantId eq " + b.TenantId());
                        var csTask = context.loadOneAsync("CommercialSpace", "CommercialSpaceId eq " + b.CommercialSpaceId());
                        $.when(tenantTask, csTask).then(function (tenant, cs) {
                            vm.tenant(tenant);
                            vm.commercialSpace(cs);
                        });

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

                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            officerCollection: ko.observableArray(),
            complaint: ko.observable(new bespoke.sphcommercialspace.domain.Complaint()),
            tenant: ko.observable(new bespoke.sphcommercialspace.domain.Tenant()),
            commercialSpace: ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpace()),
            saveCommand: save
        };

        return vm;

    });
