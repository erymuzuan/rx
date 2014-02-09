﻿/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
	function (context, logger, router) {

		var isBusy = ko.observable(false),
			queues = ko.observableArray(),
			overview = ko.observable(),
			activate = function () {
				var tcs = new $.Deferred(),
					queuesTask = $.get("/sph/management/api/queues"),
				overviewTask = $.get("/sph/management/api/overview");

				$.when(queuesTask, overviewTask).done(function (queuesr, overviewr) {
					queues(queuesr[0]);
					overview(overviewr[0]);
					tcs.resolve(true);
				});
				return tcs.promise();

			},
			attached = function (view) {

			};

		var vm = {
			overview: overview,
			isBusy: isBusy,
			activate: activate,
			attached: attached,
			queues: queues
		};

		return vm;

	});
