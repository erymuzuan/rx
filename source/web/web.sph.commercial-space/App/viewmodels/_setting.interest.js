/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../durandal/app.js" />


define(['services/datacontext', 'durandal/app'],
    function (context,app) {

        var isBusy = ko.observable(false),
            activate = function () {
                
            },
            init = function () {
                var query = String.format("BuildingId gt 0");
                var tcs = new $.Deferred();

                context.loadAsync("Building", query)
                    .done(function (lo) {
                        isBusy(false);
                        _.each(lo.itemCollection, function (item) {
                            var interest = new bespoke.sphcommercialspace.domain.Interest();
                            interest.Building(item.Name());
                            interest.Period(1);
                            interest.CommercialSpaceCategory('Cafeteria');
                            interest.PeriodType('Minggu');
                            interest.Percentage(10);
                            vm.interestCollection.push(interest);
                        });

                        tcs.resolve(true);
                    });
                return tcs.promise();
            },
            save = function() {
                var tcs = new $.Deferred();
                var setting = new bespoke.sphcommercialspace.domain.Setting();
                setting.Key('Interest.Collection');
                setting.InterestCollection.push(vm.interestCollection());
                var data = JSON.stringify({settings: [setting]});
                isBusy(true);

                context.post(data, "/Setting/Save")
                    .then(function(result) {
                        isBusy(false);

                        app.showMessage('sav')
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            init: init,
            activate: activate,
            interestCollection: ko.observableArray(),
            saveCommand:save
        };

        return vm;

    });
