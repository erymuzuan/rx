﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
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
                var elements = [
                    new bespoke.sph.domain.ScreenActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.DecisionActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.CreateEntityActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.NotificationActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.ReceiveActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.SendActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.ListenActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.ParallelActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.DelayActivity("@Guid.NewGuid()"),
                    // new bespoke.sph.domain.ThrowActivity("@Guid.NewGuid()"),
                    new bespoke.sph.domain.EndActivity("@Guid.NewGuid()")
                ];
                elements[0].Name("Screen");
                elements[1].Name("Decision");
                elements[2].Name("Create Record");
                elements[3].Name("Notifify");
                elements[4].Name("Receive");
                elements[5].Name("Send");
                elements[6].Name("Listen");
                elements[7].Name("Parallel");
                elements[8].Name("Delay");
                elements[9].Name("End");

                elements[0].Note = "Creates a user interface activity";
                elements[1].Note = "Decision braches and expression";
                elements[2].Note = "Create a new record";
                elements[3].Note = "Notify via email and messages";
                elements[4].Note = "Receieve a message from another system";
                elements[5].Note = "Send a message to another system";
                elements[6].Note = "Creates a race condition, firts one wins";
                elements[7].Note = "Concurrent running activities";
                elements[8].Note = "Wait for a certain time";
                elements[9].Note = "Ends the workflow";

                elements[0].CssClass = "pull-left activity64 activity64-ScreenActivity";
                elements[1].CssClass = "pull-left activity64 activity64-DecisionActivity";
                elements[2].CssClass = "pull-left activity64 activity64-CreateEntityActivity";
                elements[3].CssClass = "pull-left activity64 activity64-NotificationActivity";
                elements[4].CssClass = "pull-left activity64 activity64-ReceiveActivity";
                elements[5].CssClass = "pull-left activity64 activity64-SendActivity";
                elements[6].CssClass = "pull-left activity64 activity64-ListenActivity";
                elements[7].CssClass = "pull-left activity64 activity64-ParallelActivity";
                elements[8].CssClass = "pull-left activity64 activity64-DelayActivity";
                elements[9].CssClass = "pull-left activity64 activity64-EndActivity";

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
                lineWidth: 4,
                strokeStyle: "#808080",
                joinstyle: "round",
                outlineColor: "#eaedef",
                outlineWidth: 2
            },
            connectorHoverStyle = {
                lineWidth: 4,
                strokeStyle: "#5C96BC",
                outlineWidth: 2,
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
                connector: ["Flowchart", { stub: [40, 60], gap: 10, cornerRadius: 5, alwaysRespectStubs: true }],
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
                    sourceAnchorOptions = ["BottomCenter", "BottomLeft", "BottomRight", "LeftMiddle", "RightMiddle",
                                            "BottomCenter", "BottomLeft", "BottomRight", "LeftMiddle", "RightMiddle",
                                            "BottomCenter", "BottomLeft", "BottomRight", "LeftMiddle", "RightMiddle"],
                    count1 = 0;


                if (act.multipleEndPoints) {
                    _(act.multipleEndPoints()).each(function (d) {

                        var ep = jsPlumb.addEndpoint(act.WebId(), sourceEndpoint, { anchor: sourceAnchorOptions[count1], uuid: d.WebId() + "Source", id: d.WebId() });
                        d.endPointId = ep.id; // since multiple branches activity will have their own end point
                        count1++;
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

                var connection = jsPlumb.connect({
                    uuids: [source + "Source", target + "Target"],
                    editable: true,
                    overlays: [
                        [
                            "Label", {
                                cssClass: "l1 component conn-label",
                                label: label || '',
                                location: 0.2,
                                id: target + '-label'
                            }
                        ]
                    ]
                });
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
            },
            toolboxItemDraggedStop = function (arg) {
                var act = context.toObservable(ko.mapping.toJS(ko.dataFor(this))),
                    x = arg.clientX,
                    y = arg.clientY;

                act.WorkflowDesigner().X(x);
                act.WorkflowDesigner().Y(y);
                act.WebId(system.guid());
                wd().ActivityCollection.push(act);
                initializeActivity(act);
            },
            viewAttached = function () {
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
                        helper: 'clone',
                        stop: toolboxItemDraggedStop
                    });
                });


            },
            saveAsync = function () {
                $('div#container-canvas>div.activity').each(function () {
                    var p = $(this),
                        act = ko.dataFor(this),
                        x = parseInt(p.css("left")),
                        y = parseInt(p.css("top"));

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
            compileAsync = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(wd);

                context.post(data, "/WorkflowDefinition/Compile")
                    .then(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                        } else {
                            logger.error(result);
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            publishAsync = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(wd);

                context.post(data, "/WorkflowDefinition/Publish")
                    .then(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                            wd().Version(result.version);
                        } else {
                            logger.error(result);
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
                return eximp.exportJson("workflow.definition." + wd().WorkflowDefinitionId() + ".json", ko.mapping.toJSON(wd));

            },
            itemAdded = function (element) {
                jsPlumb.draggable($(element));
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            toolboxElements: ko.observableArray(),
            wd: wd,
            itemAdded: itemAdded,
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
