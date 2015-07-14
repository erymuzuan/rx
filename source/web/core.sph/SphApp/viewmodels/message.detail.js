/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", objectbuilders.router],
	function (context, router) {

	    var message = ko.observable(),
        body = ko.observable(),
        isBusy = ko.observable(false),
	    convertText = function (text) {
	       var exp = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
	       return text.replace(exp, "<a href='$1'>$1</a>");
	   },
        markUnread = function () {
            isBusy(true);

            var data = JSON.stringify({ id: message().Id() });
            return context.post(data, "/sph-message/mark-unread")
                .then(function () {
                    isBusy(false);
                    message().IsRead(true);
                });
        },
        markRead = function () {
            var data = JSON.stringify({id: message().Id()});
            isBusy(true);

            return context.post(data, "/sph-message/mark-read")
                .then(function () {
                    isBusy(false);
                    message().IsRead(true);
                });
        },
        activate = function (id) {
            var query = String.format("Id eq '{0}'", id);
            return context.loadOneAsync("Message", query)
                .done(function (b) {
                    message(b);
                    body(convertText(b.Body()));
                    markRead();
                });


        },
        attached = function () {

        },
        remove = function () {
            return context.sendDelete("/sph-message/" + message().Id())
                .then(function () {
                    router.navigateBack();
                });
        };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        attached: attached,
	        message: message,
	        body: body,
	        toolbar: {
	            removeCommand: remove,
	            commands: ko.observableArray([{
	                command: markUnread,
	                caption: "Mark unread",
	                icon: "fa fa-check-square"
	            },
	                {
	                    command: markRead,
	                    caption: "Mark read",
	                    icon: "fa fa-check"
	                }
	            ])
	        }
	    };

	    return vm;

	});
