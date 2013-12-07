/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../partial/WorkflowDefinition.js" />
/// <reference path="../partial/Activity.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'viewmodels/workflow.jsplumb'],
    function (context, logger, router, jp) {
        var isBusy = ko.observable(false),
            locals = ko.observableArray(),
            id = ko.observable(),
            instance = ko.observable(),
            port = ko.observable(50518),
            wd = ko.observable(),
            activate = function (routeData) {
                id(parseInt(routeData.id));
                var query = String.format("WorkflowDefinitionId eq {0}", id()),
                    tcs = new $.Deferred();
                context.loadOneAsync("WorkflowDefinition", query)
                    .done(wd)
                    .done(tcs.resolve);

                jp.activate(id());

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
                jp.viewAttached(view);
                $(view).on('click', 'div.activity', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();

                    var act = ko.dataFor(this);
                    if (!act) {
                        return;
                    }
                    var bp = act.breakpoint();
                    if (!bp) {
                        act.breakpoint(new bespoke.sph.domain.Breakpoint({
                            IsEnabled: true,
                            ActivityWebId: ko.unwrap(act.WebId),
                            WorkflowDefinitionId: wd().WorkflowDefinitionId()
                        }));
                        send(act.breakpoint());
                    } else {
                        remove(bp);
                        act.breakpoint(null);
                    }

                });
                var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);

                if (support == null) {
                    logger.error("No WebSocket support for debugging");
                }

                logger.info("* Connecting to server ..<br/>");
                // create a new websocket and connect
                ws = new window[support]('ws://localhost:' + port() + '/');

                ws.onmessage = function (evt) {
                    var model = JSON.parse(evt.data),
                        bp = model.Breakpoint,
                        wf = model.Data,
                        act = _(wd().ActivityCollection()).find(function (v) { return ko.unwrap(v.WebId) == bp.ActivityWebId; });
                    if (act) {
                        _(wd().ActivityCollection()).each(function (v) {
                            act.hit(false);
                        });
                        act.hit(true);
                    }
                    if (wf) {
                        locals.removeAll();
                        var vals = [];
                        for (var name in wf) {
                            vals.push({
                                name: name,
                                type: 'object',
                                value: ko.unwrap(wf[name])
                            });
                        }
                        locals(vals);
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
            locals: locals,
            isBusy: isBusy,
            wd: wd,
            port: port,
            activate: activate,
            debugcontinue: debugcontinue,
            viewAttached: viewAttached
        };

        return vm;

    });
