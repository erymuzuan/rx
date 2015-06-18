/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define([objectbuilders.config],
    function (config) {

        var
            isBusy = ko.observable(false),
			queues = ko.observableArray(),
			overview = ko.observable(),
            refresh = function () {
                var queuesTask = $.get("/management-api/rabbitmq?resource=queues"),
                 overviewTask = $.get("/management-api/rabbitmq?resource=overview");

                return $.when(queuesTask, overviewTask).done(function (queuesr, overviewr) {
                    if (typeof overview.success === "boolean" && !overview.success) {

                        return;
                    }
                    queues(_(queuesr[0]).filter(function (v) { return v.vhost === config.applicationName; }));
                    overview(overviewr[0]);
                });
            },
            activate = function () {
                return refresh();
            },
            attached = function (view) {

            };

        var vm = {
            refresh: refresh,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            overview: overview,
            queues: queues
        };

        return vm;

    });
