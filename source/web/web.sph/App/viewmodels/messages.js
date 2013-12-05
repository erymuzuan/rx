/// <reference path="../../Scripts/jquery.signalR-2.0.0.js" />
/// <reference path="../../Scripts/jquery.signalR-2.0.0.js" />
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
            query = String.format("UserName eq '{0}' and IsRead eq 0", config.userName),
            activate = function () {
                var tcs = new $.Deferred();

                context.loadAsync("Message", query)
                    .then(function (lo) {
                        vm.messages(lo.itemCollection);
                        tcs.resolve(true);
                    });

                context.getCountAsync("Message", query, "MessageId")
                    .done(function (c) {
                        vm.unread(c);
                    });
                return tcs.promise();

            },
            viewAttached = function (view) {
                $.getScript("/Scripts/jquery.signalR-2.0.0.min.js", function () {
                    var connection = $.connection('/signalr_message');

                    connection.received(function (data) {
                        var message = context.toObservable(JSON.parse(data));
                        if (typeof message.UserName !== "function") {
                            return;
                        }
                        if (message.UserName() === config.userName) {
                            context.getCountAsync("Message", query, "MessageId")
                                    .done(function (c) {
                                        vm.unread(c);
                                    });
                        }
                    });

                    connection.start().done(function () {
                        console.log("started...connection to message connection");
                    });

                });
                return activate();
            };

        var vm = {
            isBusy: isBusy,
            unread: ko.observable(),
            activate: activate,
            viewAttached: viewAttached,
            messages: ko.observableArray()
        };

        return vm;

    });
