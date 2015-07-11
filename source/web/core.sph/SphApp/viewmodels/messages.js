/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define([objectbuilders.datacontext, objectbuilders.config],
    function (context, config) {

        var isBusy = ko.observable(false),
            messages = ko.observableArray(),
            unread = ko.observable(),
            query = String.format("UserName eq '{0}' and IsRead eq 0", config.userName),
        activate = function () {
            var tcs = new $.Deferred();

            context.loadAsync("Message", query)
                .then(function (lo) {
                    messages(lo.itemCollection);
                    tcs.resolve(true);
                });

            context.getCountAsync("Message", query, "Id")
                .done(function (c) {
                    unread(c);
                });
            return tcs.promise();

        },
        attached = function (view) {
            $.getScript("/Scripts/jquery.signalR-2.1.2.min.js", function () {
                var connection = $.connection('/signalr_message');

                connection.received(function (data) {
                    var message = context.toObservable(JSON.parse(data));
                    if (typeof message.UserName !== "function") {
                        return;
                    }
                    if (message.UserName() === config.userName) {
                        context.getCountAsync("Message", query, "Id")
                                .done(function (c) {
                                    unread(c);
                                });
                    }

                    var item = _(messages()).find(function (v) {
                        return v.Id() === message.Id();
                    });
                    if (item) {
                        messages.remove(item);
                    } else {
                        // new item - put it on top
                        var query1 = String.format("Id eq '{0}'", message.Id());
                        context.getCountAsync("Message", query1, "Id")
                               .done(function (c) {
                                   if (c === 1) {
                                       messages.splice(0, 0, message);
                                   }
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
            unread: unread,
            activate: activate,
            attached: attached,
            messages: messages
        };

        return vm;

    });
