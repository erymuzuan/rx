/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../partial/ExecutedActivity.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            locals = ko.observable(),
            id = ko.observable(),
            instance = ko.observable(),
            wd = ko.observable(),
            tracker = ko.observable(),
            loadWd = function (wf) {
                var query = String.format("WorkflowDefinitionId eq {0}", wf.WorkflowDefinitionId());
                var tcs = new $.Deferred();
                var wdTask = context.loadOneAsync("WorkflowDefinition", query),
                    trackerTask = context.loadOneAsync("Tracker", "WorkflowId eq " + wf.WorkflowId());
                $.when(wdTask, trackerTask)
                    .done(function (b, t) {

                        wd(b);
                        tracker(t);

                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            activate = function (routeData) {
                id(parseInt(routeData.id));
                var query = String.format("WorkflowId eq {0}", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("Workflow", query)
                    .done(function (b) {
                        var wf = context.toObservable(b, /Bespoke\.Sph\.Workflows.*\.(.*?),/);
                        instance(wf);
                        loadWd(wf).done(function () {
                            tcs.resolve(true);
                        });
                    });

                return tcs.promise();

            },
            ws = null,
            debugcontinue = function () {

                var model = {
                    Operation: "Continue"
                };
                ws.send(JSON.stringify(model));
            },
            send = function (bp) {
                var model = {
                    Operation: "AddBreakpoint",
                    Breakpoint: bp
                };
                ws.send(ko.mapping.toJSON(model));
            },
            remove = function (bp) {

                var model = {
                    Operation: "RemoveBreakpoint",
                    Breakpoint: bp
                };
                ws.send(ko.mapping.toJSON(model));
            },
            viewAttached = function (view) {
                $(view).on('dblclick', 'tr', function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var ea = ko.dataFor(this);
                    if (!ea) {
                        return;
                    }
                    if (ko.unwrap(ea.$type) === "Bespoke.Sph.Domain.ExecutedActivity, domain.sph") {
                        var bp = ea.breakpoint();
                        if (!bp) {
                            ea.breakpoint(new bespoke.sph.domain.Breakpoint({
                                IsEnabled: true,
                                ActivityWebId: ko.unwrap(ea.ActivityWebId),
                                WorkflowDefinitionId: instance().WorkflowDefinitionId()
                            }));
                            send(ea.breakpoint());
                        } else {
                            remove(bp);
                            ea.breakpoint(null);
                        }
                    }
                });
                var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);

                if (support == null) {
                    logger.error("No WebSocket support for debugging");
                }

                logger.info("* Connecting to server ..<br/>");
                // create a new websocket and connect
                ws = new window[support]('ws://localhost:50518/');

                ws.onmessage = function (evt) {
                    var model = JSON.parse(evt.data),
                        bp = model.Breakpoint,
                        wf = model.Data,
                        ea = _(tracker().ExecutedActivityCollection()).find(function (v) { return ko.unwrap(v.ActivityWebId) == bp.ActivityWebId; });
                    if (ea) {
                        _(tracker().ExecutedActivityCollection()).each(function(v) {
                            ea.hit(false);
                        });
                        ea.hit(true);
                    }
                    if (wf) {
                        locals(wf);
                    }
                    
                    console.log(evt.data);
                };

                // when the connection is established, this method is called
                ws.onopen = function () {
                    logger.info('* Connection open');
                };

                // when the connection is closed, this method is called
                ws.onclose = function () {
                    logger.error('* Connection closed');
                };
            };

        var vm = {
            instance: instance,
            tracker: tracker,
            isBusy: isBusy,
            wd: wd,
            activate: activate,
            debugcontinue: debugcontinue,
            viewAttached: viewAttached
        };

        return vm;

    });
