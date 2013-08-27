/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" /> 
/// <reference path="../services/datacontext.js" />
define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            activate = function (tenant) {
                var query = "TenantIdSsmNo eq " + "'" + tenant.IdSsmNo() + "'";
                var tcs = new $.Deferred();
                context.loadAsync("Payment", query).done(function (lo) {
                    vm.payments(lo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            payments: ko.observableArray()
        };

        return vm;

    });
