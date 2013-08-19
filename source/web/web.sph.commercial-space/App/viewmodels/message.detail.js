﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
	function (context, logger, router) {

	    var
        isBusy = ko.observable(false),
        activate = function (routeData) {
            var id = parseInt(routeData.id);
            var query = String.format("MessageId eq {0}", id);
            var tcs = new $.Deferred();
            context.loadOneAsync("Message", query)
                .done(function (b) {
                    vm.message(b);
                    tcs.resolve(true);
                });

            return tcs.promise();

        },
        viewAttached = function (view) {

        },
        markUnread = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.message);
            isBusy(true);

            context.post(data, "/Message/MarkUnread/" + vm.message().MessageId())
                .then(function (result) {
                    isBusy(false);
                    tcs.resolve(result);
                });
            return tcs.promise();
        },
        markRead = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.message);
            isBusy(true);

            context.post(data, "/Message/MarkRead/" + vm.message().MessageId())
                .then(function (result) {
                    isBusy(false);
                    tcs.resolve(result);
                });
            return tcs.promise();
        };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        viewAttached: viewAttached,
	        message: ko.observable(),
	        toolbar: {
	            commands: ko.observableArray([{
	                command: markUnread,
	                caption: 'mark unread',
	                icon: 'icon-envelope'
	            },
	                {
	                    command: markRead,
	                    caption: 'mark read',
	                    icon: 'icon-check'
	                }
	            ])
	        }
	    };

	    return vm;

	});
