/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" /> 
/// <reference path="../services/datacontext.js" />
/// <reference path="~/App/services/Rent.js" />


define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            activate = function (tenant) {
                var query = "TenantIdSsmNo eq " + "'" + tenant.IdSsmNo() + "'" + " and Type eq 'Rental'";
                var tcs = new $.Deferred();
                context.loadAsync("Invoice", query).done(function (lo) {
                    vm.rentCollection(lo.itemCollection);
                    tcs.resolve(true);
                });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            rentCollection: ko.observableArray([])
        };

        return vm;

    });
