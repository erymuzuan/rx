/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/jsPlumb/jquery.jsPlumb.js" />
/// <reference path="../../Scripts/jsPlumb/jsPlumb.js" />
/// <reference path="../../Scripts/_task.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router', objectbuilders.system, objectbuilders.app, objectbuilders.eximp],
    function (context, logger, router, system, app, eximp) {

        var isBusy = ko.observable(false),
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition(system.guid())),
            populateToolbox = function () {
                var elements = [],
                    screen = new bespoke.sph.domain.ScreenActivity("@Guid.NewGuid()"),
                    expr = new bespoke.sph.domain.ExpressionActivity("@Guid.NewGuid()"),
                    decision = new bespoke.sph.domain.DecisionActivity("@Guid.NewGuid()"),
                    ce = new bespoke.sph.domain.CreateEntityActivity("@Guid.NewGuid()"),
                    ue = new bespoke.sph.domain.UpdateEntityActivity("@Guid.NewGuid()"),
                    de = new bespoke.sph.domain.DeleteEntityActivity("@Guid.NewGuid()"),
                    notification = new bespoke.sph.domain.NotificationActivity("@Guid.NewGuid()"),
                    receive = new bespoke.sph.domain.ReceiveActivity("@Guid.NewGuid()"),
                    send = new bespoke.sph.domain.SendActivity("@Guid.NewGuid()"),
                    listen = new bespoke.sph.domain.ListenActivity("@Guid.NewGuid()"),
                    parallel = new bespoke.sph.domain.ParallelActivity("@Guid.NewGuid()"),
                    delay = new bespoke.sph.domain.DelayActivity("@Guid.NewGuid()"),
                    end = new bespoke.sph.domain.EndActivity("@Guid.NewGuid()");

                screen.IsEnabled = ko.observable(true);
                screen.Name("Screen");
                screen.Note = "Creates a user interface activity";
                screen.CssClass = "pull-left activity32 activity32-ScreenActivity";
                elements.push(screen);


                expr.IsEnabled = ko.observable(true);
                expr.Name("Expression");
                expr.Note = "Custom expression";
                expr.CssClass = "pull-left activity32 activity32-ExpressionActivity";
                elements.push(expr);

                decision.IsEnabled = ko.observable(true);
                decision.Name("Decision");
                decision.Note = "Decision branches and expression";
                decision.CssClass = "pull-left activity32 activity32-DecisionActivity";
                elements.push(decision);

                ce.IsEnabled = ko.observable(true);
                ce.Name("Create Record");
                ce.Note = "Create a new record";
                ce.CssClass = "pull-left activity32 activity32-CreateEntityActivity";
                elements.push(ce);


                ue.IsEnabled = ko.observable(true);
                ue.Name("Update Record");
                ue.Note = "Update a record";
                ue.CssClass = "pull-left activity32 activity32-UpdateEntityActivity";
                elements.push(ue);


                de.IsEnabled = ko.observable(true);
                de.Name("Delete Record");
                de.Note = "Delete a record";
                de.CssClass = "pull-left activity32 activity32-DeleteEntityActivity";
                elements.push(de);

                notification.IsEnabled = ko.observable(true);
                notification.Name("Notify");
                notification.Note = "Notify via email and messages";
                notification.CssClass = "pull-left activity32 activity32-NotificationActivity";
                elements.push(notification);

                receive.IsEnabled = ko.observable(false);
                receive.Name("Receive");
                receive.Note = "Receive a message from another system";
                receive.CssClass = "pull-left activity32 activity32-ReceiveActivity";
                elements.push(receive);

                send.IsEnabled = ko.observable(false);
                send.Name("Send");
                send.Note = "Send a message to another system";
                send.CssClass = "pull-left activity32 activity32-SendActivity";
                elements.push(send);

                listen.IsEnabled = ko.observable(true);
                listen.Name("Listen");
                listen.Note = "Creates a race condition, first one wins";
                listen.CssClass = "pull-left activity32 activity32-ListenActivity";
                elements.push(listen);

                parallel.IsEnabled = ko.observable(false);
                parallel.Name("Parallel");
                parallel.Note = "Concurrent running activities";
                parallel.CssClass = "pull-left activity32 activity32-ParallelActivity";
                elements.push(parallel);


                delay.Name("Delay");
                delay.IsEnabled = ko.observable(true);
                delay.Note = "Wait for a certain time";
                delay.CssClass = "pull-left activity32 activity32-DelayActivity";
                elements.push(delay);

                end.Name("End");
                end.IsEnabled = ko.observable(true);
                end.Note = "Ends the workflow";
                end.CssClass = "pull-left activity32 activity32-EndActivity";
                elements.push(end);

                vm.toolboxElements(elements);
            },
            activate = function (routeData) {
                isBusy(true);
                var id = parseInt(routeData.id),
                    query = String.format("WorkflowDefinitionId eq {0}", id),
                    tcs = new $.Deferred();

                if (id) {
                    context.loadOneAsync("WorkflowDefinition", query)
                        .done(function (b) {
                            vm.wd(b);
                            tcs.resolve(true);
                            b.loadSchema();

                            var timer = setInterval(function () {
                                if (isJsPlumbReady) {
                                    clearInterval(timer);
                                    wdChanged(b);
                                }
                            }, 500);

                        });

                } else {
                    wd(new bespoke.sph.domain.WorkflowDefinition(system.guid()));

                    var timer2 = setInterval(function () {
                        if (isJsPlumbReady) {
                            clearInterval(timer2);
                            wdChanged(wd());
                        }
                    }, 500);
                    return Task.fromResult(true);

                }

                return tcs.promise();

            },
            isJsPlumbReady = false,
            connectorPaintStyle = {
                lineWidth: 2,
                strokeStyle: "#808080",
                joinstyle: "round",
                outlineColor: "#eaedef",
                outlineWidth: 1
            },
            connectorHoverStyle = {
                lineWidth: 2,
                strokeStyle: "#5C96BC",
                outlineWidth: 1,
                outlineColor: "white"
            },
            endpointHoverStyle = { fillStyle: "#5C96BC" },
            sourceEndpoint = {
                endpoint: "Dot",
                paintStyle: {
                    strokeStyle: "#1e8151",
                    fillStyle: "transparent",
                    radius: 7,
                    lineWidth: 2
                },
                isSource: true,
                connector: ["Flowchart", { stub: [10, 15], gap: 10, cornerRadius: 5, alwaysRespectStubs: true }],
                connectorStyle: connectorPaintStyle,
                hoverPaintStyle: endpointHoverStyle,
                connectorHoverStyle: connectorHoverStyle,
                dragOptions: {},
                overlays: [
                    ["Label", {
                        location: [0.5, 1.5],
                        cssClass: "endpointSourceLabel"
                    }]
                ]
            },
            targetEndpoint = {
                endpoint: "Dot",
                paintStyle: { fillStyle: "#1e8151", radius: 11 },
                hoverPaintStyle: endpointHoverStyle,
                maxConnections: -1,
                dropOptions: { hoverClass: "hover", activeClass: "active" },
                isTarget: true,
                overlays: [
                    ["Label", { location: [0.5, -0.5], cssClass: "endpointTargetLabel" }]
                ]
            },
            initializeActivity = function (act) {

                var sourceAnchors = ["BottomCenter"],
                    targetAnchors = ["TopCenter"],
                    fullName = typeof act.$type === "function" ? act.$type() : act.$type,
                    name = /Bespoke\.Sph\.Domain\.(.*?),/.exec(fullName)[1],
                    sourceAnchorOptions = ["BottomCenter", "BottomRight", "BottomLeft", "LeftMiddle", "RightMiddle"],
                    branchesCount = 0;


                if (act.multipleEndPoints) {
                    _(act.multipleEndPoints()).each(function (d) {

                        var idx = branchesCount % sourceAnchorOptions.length,
                            ep = jsPlumb.addEndpoint(act.WebId(), sourceEndpoint, { anchor: sourceAnchorOptions[idx], uuid: d.WebId() + "Source", id: d.WebId() });

                        d.endPointId = ep.id; // since multiple branches activity will have their own end point
                        branchesCount++;
                    });
                    sourceAnchors = [];
                }

                if (name == "EndActivity") {
                    sourceAnchors = [];
                }
                if (act.IsInitiator()) {
                    targetAnchors = [];
                }
                if (targetAnchors.length)
                    jsPlumb.addEndpoint(act.WebId(), targetEndpoint, { anchor: "TopCenter", uuid: act.WebId() + "Target" });
                if (sourceAnchors.length)
                    jsPlumb.addEndpoint(act.WebId(), sourceEndpoint, { anchor: "BottomCenter", uuid: act.WebId() + "Source" });



            },
            wdChanged = function (wd0) {
                var activities = _(wd0.ActivityCollection());
                activities.each(initializeActivity);

                activities.each(function (v) {
                    if (v.NextActivityWebId()) {
                        createConnection(v.WebId(), v.NextActivityWebId());
                    }
                    if (v.multipleEndPoints) {
                        _(v.multipleEndPoints()).each(function (d) {
                            createConnection(d.WebId(), d.NextActivityWebId(), d.Name());
                        });
                    }

                });

                jsPlumb.draggable($("div.activity"));
                isBusy(false);
            },
            createConnection = function (source, target, label) {

                var option = {
                    uuids: [source + "Source", target + "Target"],
                    editable: true
                };
                if (label)
                    option.overlays = [
                        [
                            "Label", {
                                cssClass: "l1 component conn-label",
                                label: label,
                                location: 0.2,
                                id: target + '-label'
                            }
                        ]
                    ];
                var connection = jsPlumb.connect(option);
                connection.setParameter({ label: label });



            },
            connectionClicked = function (conn) {
                var activities = _(vm.wd().ActivityCollection()),
                    source = activities.find(function (v) { return conn.sourceId === v.WebId(); }),
                    target = activities.find(function (v) { return conn.targetId === v.WebId(); }),
                    message = "Delete connection from " + source.Name() + " to " + target.Name() + "?";

                app.showMessage(message, "Delete Connection", ["Yes", "No"])
                    .done(function (result) {
                        if (result === "Yes") {
                            jsPlumb.detach(conn);
                            source.NextActivityWebId("");
                        }
                    });

            },
            connectionDragStop = function (connection) {
                var source = ko.dataFor(connection.source);
                if (source.multipleEndPoints) {
                    var ep = _(connection.endpoints).find(function (e) { return e.isSource; });
                    source = _(source.multipleEndPoints()).find(function (v) { return ep.id === v.endPointId; });
                }
                var target = ko.dataFor(connection.target);
                source.NextActivityWebId(target.WebId());
            },
            activitiesChanged = function (changes) {
                console.log(changes);
                if (_.isArray(changes)) {
                    var chg = changes[0];
                    if (chg.status === "deleted") {
                        // remove the associated endpoint
                        jsPlumb.selectEndpoints({ source: chg.value.WebId() }).setVisible(false).detachAll();
                    }
                }
            },
            jsPlumbReady = function () {
                isJsPlumbReady = true;
                jsPlumb.draggable($("div.activity"));
                jsPlumb.init();
                jsPlumb.Defaults.Container = "container-canvas";

                // setup some defaults for jsPlumb.
                jsPlumb.importDefaults({
                    Endpoint: ["Dot", { radius: 2 }],
                    HoverPaintStyle: { strokeStyle: "#000", lineWidth: 2 },
                    PaintStyle: { strokeStyle: "#575757", lineWidth: 2 },
                    ConnectionOverlays: [
                        ["Arrow", {
                            location: 1,
                            id: "arrow",
                            length: 14,
                            foldback: 0.8
                        }]
                    ]
                });


                jsPlumb.bind("click", connectionClicked);
                jsPlumb.bind("connectionDragStop", connectionDragStop);
                wd().ActivityCollection.subscribe(activitiesChanged, null, "arrayChange");
            },
            toolboxItemDraggedStop = function (arg) {
                var act = context.toObservable(ko.mapping.toJS(ko.dataFor(this))),
                    x = arg.clientX,
                    y = arg.clientY;

                act.Name(act.Name() + wd().ActivityCollection().length);
                act.WorkflowDesigner().X(x);
                act.WorkflowDesigner().Y(y - $('#container-canvas').offset().top);
                act.WebId(system.guid());
                wd().ActivityCollection.push(act);
                initializeActivity(act);
            },
            viewAttached = function (view) {
                var script = $('<script type="text/javascript" src="/Scripts/jsPlumb/bundle.js"></script>').appendTo('body'),
                    timer = setInterval(function () {
                        if (window.jsPlumb !== undefined) {
                            clearInterval(timer);
                            script.remove();

                            jsPlumb.ready(jsPlumbReady);
                        }
                    }, 2500);


                populateToolbox();

                $.getScript('/Scripts/jquery-ui-1.10.3.custom.min.js', function () {
                    $('div.toolbox-item').draggable({
                        helper: function () {
                            return $("<div></div>").addClass("dragHoverToolbox").append($(this).find('.activity32').clone());
                        },
                        stop: toolboxItemDraggedStop
                    });
                });

                var paintedConnectors = [];

                $(view).on('mouseenter', 'div.activity', function () {
                    var act = ko.dataFor(this),
                        cps = _.clone(connectorPaintStyle),
                        cps2 = _.clone(connectorPaintStyle),
                        targets = [act.NextActivityWebId()];
                    cps.strokeStyle = "#007aff";// blue
                    cps2.strokeStyle = "#ff6a00";// orange


                    var cs = jsPlumb.select({ source: act.WebId() })
                         .setPaintStyle(cps2);
                    paintedConnectors.push(cs);


                    var cs2 = jsPlumb.select({ target: act.WebId() })
                         .setPaintStyle(cps);
                    paintedConnectors.push(cs2);

                    if (act.multipleEndPoints) {
                        targets = _(act.multipleEndPoints()).map(function (v) { return v.NextActivityWebId(); });
                    }


                    $('div.activity').each(function () {
                        var div2 = $(this),
                            act2 = ko.dataFor(this);
                        if (act.WebId() === act2.NextActivityWebId()) {
                            div2.addClass("source-activity");
                        } else {
                            div2.removeClass("source-activity");
                        }
                        if (targets.indexOf(act2.WebId()) > -1) {
                            div2.addClass("target-activity");
                        } else {
                            div2.removeClass("target-activity");
                        }
                    });

                });

                $(view).on('mouseleave', 'div.activity', function () {
                    $('div.activity').each(function () {
                        var div2 = $(this);
                        div2.removeClass("source-activity")
                            .removeClass("target-activity");

                        // reset
                        _(paintedConnectors).each(function (c) { c.setPaintStyle(connectorPaintStyle); });
                        paintedConnectors = [];

                    });

                });


            },
            saveAsync = function () {
                $('div#container-canvas>div.activity').each(function () {
                    var p = $(this),
                        act = ko.dataFor(this),
                        x = parseInt(p.css("left")),
                        y = parseInt(p.css("top"));
                    if (typeof act.WorkflowDesigner !== "function") {
                        act.WorkflowDesigner = ko.observable(new bespoke.sph.domain.WorkflowDesigner(system.guid()));
                    }
                    act.WorkflowDesigner().X(x);
                    act.WorkflowDesigner().Y(y);

                });

                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(wd);
                isBusy(true);

                context.post(data, "/WorkflowDefinition/Save")
                    .then(function (result) {
                        isBusy(false);
                        logger.info("Data have been succesfully save");

                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            compileCompleted = function (result) {

                var clearItemHasError = function (v) {
                    v.hasError(false);
                },
                    setItemHasError = function (v) {
                        var hasError = typeof _(result.Errors).find(function (k) { return v.WebId() === k.ItemWebId; }) !== "undefined";
                        v.hasError(hasError);
                    };

                if (result.success) {
                    logger.info(result.message);

                    _(wd().ActivityCollection()).each(clearItemHasError);
                    _(wd().VariableDefinitionCollection()).each(clearItemHasError);
                    vm.errors.removeAll();
                } else {

                    vm.errors(result.Errors);
                    logger.error("There are errors in your Workflow, please fix them all");
                    //
                    _(wd().ActivityCollection()).each(setItemHasError);
                    _(wd().VariableDefinitionCollection()).each(setItemHasError);
                }
            },
            compileAsync = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(wd);

                context.post(data, "/WorkflowDefinition/Compile")
                    .then(function (result) {
                        compileCompleted(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            publishAsync = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(wd);

                context.post(data, "/WorkflowDefinition/Publish")
                    .then(function (result) {
                        compileCompleted(result);
                        if (result.success) {
                            wd().Version(result.version);
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();

            },
            importAsync = function () {
                return eximp.importJson()
                .done(function (json) {
                    try {

                        var obj = JSON.parse(json),
                            clone = context.toObservable(obj);

                        clone.WorkflowDefinitionId(0);
                        wd(clone);
                        wdChanged(clone);

                    } catch (error) {
                        logger.logError('Workflow definition is not valid', error, this, true);
                    }
                });
            },
            exportWd = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(wd);
                context.post(data, "/WorkflowDefinition/Export")
                    .then(function (result) {
                        tcs.resolve(result);
                        window.location = result.url;
                    });
                return tcs.promise();

            },
            itemAdded = function (element) {
                jsPlumb.draggable($(element));
            },
            showError = function (error) {
                console.log(error);
                wd().editActivity(_(wd().ActivityCollection()).find(function(v) { return v.WebId() == error.ItemWebId; }))();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            toolboxElements: ko.observableArray(),
            wd: wd,
            itemAdded: itemAdded,
            errors: ko.observableArray(),
            showError : showError,
            toolbar: {
                saveCommand: saveAsync,
                exportCommand: exportWd,
                importCommand: importAsync,
                commands: ko.observableArray([
                    {
                        command: compileAsync,
                        caption: 'Build',
                        icon: "fa fa-gear"
                    },
                    {
                        command: publishAsync,
                        caption: 'Publish',
                        icon: "fa fa-sign-out"
                    }])
            }
        };

        return vm;

    });
