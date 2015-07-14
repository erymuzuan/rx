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


define([objectbuilders.datacontext, objectbuilders.config, objectbuilders.logger],
    function (context, config, logger) {

        var isBusy = ko.observable(false),
            messages = ko.observableArray(),
            unread = ko.observable(),
            query = String.format("UserName eq '{0}' and IsRead eq 0", config.userName),
            activate = function () {
                return context.loadAsync("Message", query)
                    .then(function (lo) {
                        messages(lo.itemCollection);
                        unread(lo.rows);
                    });

            },
            attached = function (view) {
                $.getScript("/Scripts/jquery.signalR-2.1.2.min.js", function () {
                    var connection = $.connection("/signalr_message");

                    connection.received(function (data) {
                        if (typeof data.unread === "number") {
                            messages(data.messages);
                            unread(data.unread);
                        }

                        if (typeof data === "string") {
                            logger.info(data);
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
