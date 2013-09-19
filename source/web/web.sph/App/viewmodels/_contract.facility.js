/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />


define(['services/datacontext'],
    function (context) {
        var init = function (id) {

            var query = String.format("RentalApplicationId eq {0}", id());
            var tcs = new $.Deferred();
            context.loadOneAsync("RentalApplication", query)
                .done(function(b) {
                    vm.facilities(b.FeatureCollection());
                    tcs.resolve(true);
                });

            return tcs.promise();
        };

        var vm = {
            init: init,
            facilities : ko.observableArray()
        };

        return vm;

    });
