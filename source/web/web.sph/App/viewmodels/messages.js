/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
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
                var query = String.format("UserName eq '{0}' and IsRead eq 0", config.userName);
                var tcs = new $.Deferred();

                context.loadAsync("Message", query)
                    .then(function(lo) {
                        vm.messages(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();


            },
            viewAttached = function (view) {
                return activate();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            messages : ko.observableArray()
        };

        return vm;

    });
