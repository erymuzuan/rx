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
            setting = new bespoke.sphcommercialspace.domain.Setting(),
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
                            interest.PeriodType('Week');
                            interest.Percentage(10);
                            vm.interestCollection.push(interest);
                        });

                        tcs.resolve(true);
                    });
                return tcs.promise();
            },
            save = function() {
                var tcs = new $.Deferred();
                vm.setting().Key('Interest.Collection');
                vm.setting().Value(ko.mapping.toJSON(vm.interestCollection()));
                var data = JSON.stringify({settings: [ko.mapping.toJS(vm.setting())]});
                isBusy(true);

                context.post(data, "/Setting/Save")
                    .then(function(result) {
                        isBusy(false);
                        app.showMessage('setting saved');
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            init: init,
            activate: activate,
            setting: ko.observable(setting),
            interestCollection: ko.observableArray(),
            saveCommand:save
        };

        return vm;

    });
