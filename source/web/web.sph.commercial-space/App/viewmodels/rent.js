/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context) {

        var isBusy = ko.observable(false),
            tenantId = ko.observable(),
            activate = function (routeData) {
                tenantId(parseInt(routeData.tenantId));
                title('Maklumat Sewa dan Invois');

                var query = String.format("RentId eq {0}", routeData.tenantId);
                var tcs = new $.Deferred();
                context.loadOneAsync("Rent", query)
                    .done(function (r) {
                        vm.rent(r);
                        tcs.resolve(true);

                    });

                return tcs.promise();

            },
            viewAttached = function (view) {

            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            rent: ko.observable(new bespoke.CommercialSpace.domain.Rent()),
            viewAttached: viewAttached
        };

        return vm;

    });
