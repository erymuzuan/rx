/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function(context) {

        var isBusy = ko.observable(false),
            activate = function() {
                var tcs = new $.Deferred();
                context.loadAsync("RentalApplication", "RentalApplicationId gt 0").done(function (lo) {
                    vm.applicationCollection(lo.itemCollection);
                    tcs.resolve(true);
                });
               return  tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            applicationCollection: ko.observableArray([])
        };


        return vm;
    });