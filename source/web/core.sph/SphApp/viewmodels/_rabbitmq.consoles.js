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
			bindings = ko.observableArray(),
			overview = ko.observable(),
			selectedQueue = ko.observable({ "memory": 22144, "messages": 0, "messages_details": { "rate": 0.0 }, "messages_ready": 0, "messages_ready_details": { "rate": 0.0 }, "messages_unacknowledged": 0, "messages_unacknowledged_details": { "rate": 0.0 }, "idle_since": "2015-07-04 6:21:04", "consumer_utilisation": "", "policy": "", "exclusive_consumer_tag": "", "consumers": 2, "backing_queue_status": { "q1": 0, "q2": 0, "delta": ["delta", 0, 0, 0], "q3": 0, "q4": 0, "len": 0, "pending_acks": 0, "target_ram_count": "infinity", "ram_msg_count": 0, "ram_ack_count": 0, "next_seq_id": 0, "persistent_count": 0, "avg_ingress_rate": 0.0, "avg_egress_rate": 0.0, "avg_ack_ingress_rate": 0.0, "avg_ack_egress_rate": 0.0 }, "state": "running", "incoming": [], "deliveries": [], "consumer_details": [{ "channel_details": { "name": "[::1]:2926 -> [::1]:5672 (1)", "number": 1, "connection_name": "[::1]:2926 -> [::1]:5672", "peer_port": 2926, "peer_host": "::1" }, "queue": { "name": "custom_entities_es_indexer", "vhost": "DevV1" }, "consumer_tag": "amq.ctag-mInIcty8szKTX0F1Sux59g", "exclusive": false, "ack_required": true, "prefetch_count": 1, "arguments": {} }, { "channel_details": { "name": "[::1]:3612 -> [::1]:5672 (1)", "number": 1, "connection_name": "[::1]:3612 -> [::1]:5672", "peer_port": 3612, "peer_host": "::1" }, "queue": { "name": "custom_entities_es_indexer", "vhost": "DevV1" }, "consumer_tag": "amq.ctag-zKiwxOBzWLgrzc6A0XwABQ", "exclusive": false, "ack_required": true, "prefetch_count": 1, "arguments": {} }], "name": "custom_entities_es_indexer", "vhost": "DevV1", "durable": true, "auto_delete": false, "arguments": { "x-dead-letter-exchange": "sph.ms-dead-letter" }, "node": "rabbit@ERYMUZUAN-PC" }),
            refresh = function () {
                var queuesTask = $.get("/management-api/rabbitmq?resource=queues"),
                 overviewTask = $.get("/management-api/rabbitmq?resource=overview");

                return $.when(queuesTask, overviewTask).done(function (queuesr, overviewr) {
                    if (typeof overview.success === "boolean" && !overview.success) {
                        return;
                    }
                    queues(_(queuesr[0]).filter(function (v) { return v.vhost === config.applicationName && !v.name.startsWith("sph.delay"); }));
                    overview(overviewr[0]);
                });
            },
            activate = function () {
                return refresh();
            },
            attached = function (view) {

            },
            viewQueueDetail = function (q) {
                return $.get("/management-api/rabbitmq?resource=queues/" + config.applicationName + "/" + q.name)
                    .then(function (qs) {
                        selectedQueue(qs);
                        return $.get("/management-api/rabbitmq?resource=queues/" + config.applicationName + "/" + q.name + "/bindings");
                    })
                    .then(bindings)
                    .done(function () {
                        $("#queue-detail-dialog").modal("show");
                    });
            };

        var vm = {
            viewQueueDetail: viewQueueDetail,
            bindings: bindings,
            selectedQueue: selectedQueue,
            refresh: refresh,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            overview: overview,
            queues: queues
        };

        return vm;

    });
