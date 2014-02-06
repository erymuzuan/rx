/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
	function (context) {

	    var
        isBusy = ko.observable(false),
        activate = function (mid) {
            var id = parseInt(mid);
            var query = String.format("MessageId eq {0}", id);
            var tcs = new $.Deferred();
            context.loadOneAsync("Message", query)
                .done(function (b) {
                    vm.message(b);
                    tcs.resolve(true);
                    vm.body(convertText(b.Body()));
                    markRead();
                });

            return tcs.promise();

        },
        attached = function (view) {

        },
	   convertText = function (text) {
	       var exp = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
	       return text.replace(exp, "<a href='$1'>$1</a>");
	   },
        markUnread = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.message);
            isBusy(true);

            context.post(data, "/Sph/Message/MarkUnread/" + vm.message().MessageId())
                .then(function (result) {
                    isBusy(false);
                    tcs.resolve(result);
                    vm.message().IsRead(true);
                });
            return tcs.promise();
        },
        markRead = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.message);
            isBusy(true);

            context.post(data, "/Sph/Message/MarkRead/" + vm.message().MessageId())
                .then(function (result) {
                    isBusy(false);
                    tcs.resolve(result);
                    vm.message().IsRead(true);
                });
            return tcs.promise();
        },
        remove = function () {

            var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(vm.message);
            isBusy(true);

            context.post(data, "/Sph/Message/Remove/" + vm.message().MessageId())
                .then(function (result) {
                    isBusy(false);
                    tcs.resolve(result);
                    window.location = '/sph#/message.inbox';
                });
            return tcs.promise();
        };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        attached: attached,
	        message: ko.observable(),
	        body: ko.observable(),
	        toolbar: {
	            removeCommand: remove,
	            commands: ko.observableArray([{
	                command: markUnread,
	                caption: 'Mark unread',
	                icon: 'fa fa-check-square'
	            },
	                {
	                    command: markRead,
	                    caption: 'Mark read',
	                    icon: 'fa fa-check'
	                }
	            ])
	        }
	    };

	    return vm;

	});
