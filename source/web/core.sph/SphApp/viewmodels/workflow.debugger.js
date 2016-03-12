/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../partial/WorkflowDefinition.js" />
/// <reference path="../partial/Activity.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="~/Scripts/_task.js" />

define(["services/datacontext", "services/logger", "plugins/router", "viewmodels/workflow.jsplumb", "jquery.contextmenu", "jquery.ui.position"],
    function (context, logger, router, jp) {
        var isBusy = ko.observable(false),
            activity = ko.observableArray(),
            running = ko.observable(false),
            locals = ko.observableArray(),
            breakpoints = ko.observableArray(),
            id = ko.observable(),
            host = ko.observable("localhost"),
            consoleOutput = ko.observable(),
            instance = ko.observable(),
            executingActivity = ko.observable(),
            port = ko.observable(50518),
            wd = ko.observable(),
            activate = function (id2) {
                id(id2);
                var query = String.format("Id eq '{0}'", id()),
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

                return Task.fromResult(true, 800);
            },
            send = function (bp) {
                var model = {
                    Operation: "AddBreakpoint",
                    Breakpoint: bp
                };
                if (ws) {
                    ws.send(ko.mapping.toJSON(model));
                } else {
                    breakpoints.push(bp);
                }
            },
            remove = function (bp) {

                var model = {
                    Operation: "RemoveBreakpoint",
                    Breakpoint: bp
                };
                if (ws) {
                    ws.send(ko.mapping.toJSON(model));
                } else {
                    breakpoints.remove(bp);
                }
            },
            generateLocals = function (loc) {
                var list = [];
                for (var n in loc) {
                    if (!loc.hasOwnProperty(n)) {
                        continue;
                    }
                    var val = ko.unwrap(loc[n]),
                        type = typeof val;
                    if (_(val).isArray()) {
                        type = "Array";
                    }

                    var item = {
                        name: n,
                        type: type,
                        value: val
                    };
                    if (typeof val === "undefined") { // for null, undefined and other falsey
                        return list;
                    }
                    if (null === val) { // for null, undefined and other falsey
                        return list;
                    }
                    if (_(val).isObject()) {
                        item.items = generateLocals(val);
                    }
                    list.push(item);

                }
                return list;

            },
            attached = function (view) {
                jp.attached(view);
                $(view).on("click", "div.activity", function () {
                    $("div.activity").removeClass("selected-activity");
                    $(this).addClass("selected-activity");
                    var act = ko.dataFor(this),
                        list = generateLocals(act);
                    activity(list);
                });

                $.contextMenu({
                    selector: "div.activity",
                    callback: function (key) {
                        var act = ko.dataFor(this[0]);
                        if (!act) {
                            return;
                        }
                        var bp = act.breakpoint();
                        if (key === "add") {

                            act.breakpoint(new bespoke.sph.domain.Breakpoint({
                                IsEnabled: true,
                                ActivityWebId: ko.unwrap(act.WebId),
                                WorkflowDefinitionId: wd().Id()
                            }));
                            send(act.breakpoint());

                        }
                        if (key === "delete") {

                            remove(bp);
                            act.breakpoint(null);
                        }

                    },
                    items: {
                        "add": { name: "Add Breakpoint", icon: "bug" },
                        "delete": { name: "Remove Breakpoint", icon: "circle-o" }
                    }
                });
                $(view).on("click", "div.modal-footer>a", function () {
                    $("div.modal-backdrop").remove();
                });

            },
            consoleScript = ko.observable(),
            runConsole = function () {

                var model = {
                    Operation: "Console",
                    Console: consoleScript()
                };
                ws.send(JSON.stringify(model));
            },
            configure = function () {
                var tcs = new $.Deferred();
                require(["viewmodels/wf.debugger.config.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.config({
                            host : ko.observable(ko.unwrap(host)),
                            port : ko.observable(ko.unwrap(port))
                        });
                        app2.showDialog(dialog)
                            .done(function (result) {
                                if (!result) return;
                                if (result === "OK") {
                                    host(dialog.config().host());
                                    port(dialog.config().port());

                                }
                                tcs.resolve(result);
                            });
                    });
                return tcs.promise();
            },
            start = function () {
                var tcs = new $.Deferred(),
                    support = "MozWebSocket" in window ? "MozWebSocket" : ("WebSocket" in window ? "WebSocket" : null);

                if (support == null) {
                    logger.error("No WebSocket support for debugging");
                }

                logger.info("* Connecting to server ..<br/>");
                // create a new websocket and connect
                ws = new window[support]("ws://" + host() + ":" + port() + "/");

                ws.onmessage = function (evt) {
                    var model = JSON.parse(evt.data),
                        bp = model.Breakpoint || {},
                        wf = model.Data,
                        act = _(wd().ActivityCollection()).find(function (v) { return ko.unwrap(v.WebId) === bp.ActivityWebId; });
                    if (act) {
                        _(wd().ActivityCollection()).each(function (v) {
                            v.hit(false);
                        });
                        act.hit(true);
                    }
                    if (wf) {
                        locals.removeAll();
                        var vals = [];
                        for (var name in wf) {
                            if (wf.hasOwnProperty(name)) {
                                vals.push({
                                    name: name,
                                    type: "object",
                                    value: ko.unwrap(wf[name])
                                });
                            }
                        }
                        locals(vals);
                    }

                    if (!wf && !act) {
                        consoleOutput(evt.data);
                    }

                    console.log(evt.data);
                };

                // when the connection is established, this method is called
                ws.onopen = function () {
                    logger.info("* Connection open");

                    var model = {
                        Operation: "ClearBreakpoint"
                    };
                    ws.send(JSON.stringify(model));

                    tcs.resolve(true);
                    running(true);
                    // Send the breakpoint which has been added before starting the connection
                    _(breakpoints()).each(function (v) {
                        var sm = {
                            Operation: "AddBreakpoint",
                            Breakpoint: v
                        };
                        ws.send(ko.mapping.toJSON(sm));
                    });
                };

                // when the connection is closed, this method is called
                ws.onclose = function () {
                    logger.error("* Connection closed");
                };

                return tcs.promise();
            },
            f10 = function () {

                var model = {
                    Operation: "StepThrough"
                };
                ws.send(JSON.stringify(model));

                return Task.fromResult(true, 800);
            },
            watches = ko.observableArray(),
            addToWatch = function (local) {
                console.log(local);
                watches.push(local);
            },
            removeFromWatch = function (local) {
                watches.remove(local);
            },
            refreshWatch = function (local) {
                console.log(local);
            },
            expandObjects = function (loc) {
                if (!loc.items) return;
                console.log(loc.items);
            },
            stop = function () {

                var model = {
                    Operation: "Stop"
                };
                ws.send(JSON.stringify(model));

                setTimeout(function () {
                    running(false);
                }, 1000);
                return Task.fromResult(true, 800)
                    .done(function () {
                        running(false);
                    });
            };

        var vm = {
            expandObjects: expandObjects,
            activity: activity,
            watches: watches,
            refreshWatch: refreshWatch,
            addToWatch: addToWatch,
            removeFromWatch: removeFromWatch,
            host: host,
            executingActivity: executingActivity,
            consoleOutput: consoleOutput,
            instance: instance,
            locals: locals,
            isBusy: isBusy,
            running: running,
            wd: wd,
            port: port,
            activate: activate,
            debugcontinue: debugcontinue,
            attached: attached,
            consoleScript: consoleScript,
            runConsole: runConsole,
            f10: f10,
            toolbar: {

                commands: ko.observableArray([{
                    command: configure,
                    caption: "Configuration",
                    icon: "fa fa-gear"
                },
                    {

                        command: start,
                        caption: "Start",
                        icon: "fa fa-play",
                        enable: ko.computed(function () {
                            return !running();
                        })
                    },
                    {

                        command: f10,
                        caption: "Step Through",
                        icon: "fa fa-arrow-down",
                        enable: running
                    },
                    {

                        command: debugcontinue,
                        caption: "Continue",
                        icon: "fa fa-chevron-right",
                        enable: running
                    },
                    {

                        command: stop,
                        caption: "Stop",
                        icon: "fa fa-stop",
                        enable: running
                    }])
            }
        };

        return vm;

    });
