/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'config'],
    function (context, config) {

        var isBusy = ko.observable(false),
            activate = function () {
                var query = "LandId gt 0";
                var tcs = new $.Deferred();

                context.loadAsync("Land", query)
                    .then(function (lo) {
                        isBusy(false);

                        vm.lands(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();


            },
            viewAttached = function (view) {

            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            lands: ko.observableArray(),
            toolbar: {
                addNew: {
                    location: '/#/land.detail/0',
                    caption: 'Tanah Baru'
                }
            }
        };

        return vm;

    });
