/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/jsPlumb/jquery.jsPlumb-1.5.4.js" />
/// <reference path="../../Scripts/jsPlumb/jsPlumb.js" />
/// <reference path="../../Scripts/_task.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app],
    function (context, logger, router, system, app) {

        var isBusy = ko.observable(false),
            isPublishing = ko.observable(false),
            selectedActivity = ko.observable(false),
            originalEntity = "",
            publishingMessage = ko.observable(),
            toolboxElements = ko.observableArray(),
            errors = ko.observableArray(),
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition(system.guid())),
            isJsPlumbReady = false,
            activate = function (id) {
                isBusy(true);
                var query = String.format("Id eq '{0}'", id);


                return $.get("/api-rx/wf-designer/toolbox-items")
                    .then(function (result) {
                        toolboxElements(result);
                        return context.loadOneAsync("WorkflowDefinition", query);
                    })
                    .then(function (b) {
                        wd(b);
                        b.loadSchema();

                        var timer = setInterval(function () {
                            if (isJsPlumbReady) {
                                clearInterval(timer);
                                wdChanged(b);
                            }
                        }, 500);

                        isBusy(false);

                    });

            },
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

                if (name === "EndActivity") {
                    sourceAnchors = [];
                }
                if (act.IsInitiator()) {
                    targetAnchors = [];
                }
                if (targetAnchors.length)
                    jsPlumb.addEndpoint(act.WebId(), targetEndpoint, { anchor: "TopCenter", uuid: act.WebId() + "Target" });
                if (sourceAnchors.length)
                    jsPlumb.addEndpoint(act.WebId(), sourceEndpoint, { anchor: "BottomCenter", uuid: act.WebId() + "Source" });

                console.log("Done initializing activity ", ko.unwrap(act.Name));

            },
            autoSave = ko.observable(false),
            autoSaveInterval = ko.observable(5000),
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
                var as = null;
                autoSave.subscribe(function (enabled) {
                    if (!enabled && as) {
                        clearInterval(as);
                    }
                    if (enabled) {
                        as = setInterval(saveAsync, autoSaveInterval());
                    }
                });
                autoSaveInterval.subscribe(function (interval) {
                    if (autoSave() && as) {
                        clearInterval(as);
                        as = setInterval(saveAsync, interval);
                    }
                });
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
                                id: target + "-label"
                            }
                        ]
                    ];
                try {
                    var connection = jsPlumb.connect(option);
                    connection.setParameter({ label: label });
                } catch (e) {
                    console.log(e);
                }



            },
            connectionClicked = function (conn) {
                var activities = _(wd().ActivityCollection()),
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
                var act = context.toObservable(ko.mapping.toJS(ko.dataFor(this).activity)),
                    x = arg.clientX,
                    y = arg.clientY;

                act.Name(act.TypeName() + "_" + wd().ActivityCollection().length);
                act.WorkflowDesigner().X(x - 60);
                act.WorkflowDesigner().Y(y - $("#container-canvas").offset().top + $(window).scrollTop() - 30);
                act.WebId(system.guid());
                wd().ActivityCollection.push(act);
                initializeActivity(act);
            },
            attached = function (view) {
                $(view).on("click", "div.activity", function() {
                    var act = ko.dataFor(this);
                    _(wd().ActivityCollection()).each(function(v) {
                        if (typeof v.selected !== "function") {
                            v.selected = ko.observable(false);
                        }
                        v.selected(false);
                    });
                    act.selected(true);
                    selectedActivity(act);
                });

                // delete selected element when [delete] key is pressed
                $(view).on("keyup", "div.selected-activity", function (e) {
                    if (e.keyCode === 46 && typeof selectedActivity() != "undefined") {
                        wd().removeActivity(selectedActivity())();
                    }
                });

                var script = $("<script type=\"text/javascript\" src=\"/Scripts/jsPlumb/bundle.js\"></script>").appendTo("body"),
                    timer = setInterval(function () {
                        if (window.jsPlumb !== undefined) {
                            clearInterval(timer);
                            script.remove();

                            jsPlumb.ready(jsPlumbReady);
                        }
                    }, 2500);


                $("div.toolbox-item").draggable({
                    helper: function () {
                        return $("<div></div>").addClass("dragHoverToolbox").append($(this).find(".activity32").clone());
                    },
                    stop: toolboxItemDraggedStop
                });




                var paintedConnectors = [],
                    hoveredActivity = null;

                $(view).on("mouseenter", "div.activity", function () {
                    var act = ko.dataFor(this),
                        cps = _.clone(connectorPaintStyle),
                        cps2 = _.clone(connectorPaintStyle),
                        targets = [act.NextActivityWebId()];
                    cps.strokeStyle = "#007aff";// blue
                    cps2.strokeStyle = "#ff6a00";// orange

                    hoveredActivity = act;


                    var cs = jsPlumb.select({ source: act.WebId() })
                         .setPaintStyle(cps2);
                    paintedConnectors.push(cs);


                    var cs2 = jsPlumb.select({ target: act.WebId() })
                         .setPaintStyle(cps);
                    paintedConnectors.push(cs2);

                    if (act.multipleEndPoints) {
                        targets = _(act.multipleEndPoints()).map(function (v) { return v.NextActivityWebId(); });
                    }


                    $("div.activity").each(function () {
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

                $(view).on("mouseleave", "div.activity", function () {

                    hoveredActivity = null;
                    $("div.activity").each(function () {
                        var div2 = $(this);
                        div2.removeClass("source-activity")
                            .removeClass("target-activity");

                        // reset
                        _(paintedConnectors).each(function (c) { c.setPaintStyle(connectorPaintStyle); });
                        paintedConnectors = [];

                    });

                });
                $("#open-close-toolbox-button").on("click", function (e) {
                    e.preventDefault();
                    $("#toolbox-panel").hide();
                    return false;
                });
                $(document).on("keyup", function (e) {
                    if (e.ctrlKey && e.altKey && e.keyCode === 88) {
                        $("#toolbox-panel").show();
                    }
                });
                var clipboardItem = null;
                $(view).on("copy", "div.activity", function (e) {
                    e.preventDefault();
                    clipboardItem = hoveredActivity;
                    console.log("Copied " + clipboardItem.Name());
                });
                $(view).on("paste", "div#container-canvas", function (e) {
                    e.preventDefault();
                    if (clipboardItem) {

                        console.log("paste " + clipboardItem.Name());
                        var act = context.toObservable(ko.mapping.toJS(clipboardItem)),
                        x = act.WorkflowDesigner().X() + 20,
                        y = act.WorkflowDesigner().Y() + 20;

                        act.Name(act.Name() + wd().ActivityCollection().length);
                        act.WorkflowDesigner().X(x);
                        act.WorkflowDesigner().Y(y - $("#container-canvas").offset().top);
                        act.WebId(system.guid());
                        wd().ActivityCollection.push(act);
                        initializeActivity(act);
                    }
                });

                // TODO - got towait untill all initializeActivity has finished
                setTimeout(function () {
                    originalEntity = ko.toJSON(wd);
                }, 5000);
                console.log("Doe attached");
            },
            saveAsync = function () {
                $("div#container-canvas>div.activity").each(function () {
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

                var data = ko.mapping.toJSON(wd());
                isBusy(true);

                return context.post(data, "/api/workflow-definitions")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info("Data have been succesfully save");
                            wd().Id(result.id);
                            originalEntity = ko.toJSON(wd);
                        } else {
                            logger.error(result.message);
                        }
                    });
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
                    errors.removeAll();
                } else {

                    errors(result.Errors);
                    logger.error("There are errors in your Workflow, please fix them all");
                    //
                    _(wd().ActivityCollection()).each(setItemHasError);
                    _(wd().VariableDefinitionCollection()).each(setItemHasError);
                }
            },
            compileAsync = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(wd);

                context.post(data, "/api/workflow-definitions/compile")
                    .then(function (result) {
                        compileCompleted(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            publishAsync = function () {
                var data = ko.mapping.toJSON(wd);

                isPublishing(true);
                publishingMessage("Compiling....");
                return context.post(data, "/api/workflow-definitions/publish")
                    .then(function (result) {
                        compileCompleted(result);
                        if (result.success) {
                            wd().Version(result.version);
                            publishingMessage("Stopping all subscribers...");
                            setTimeout(function () {
                                publishingMessage("Deployment in progress");
                                setTimeout(function () {
                                    publishingMessage("Starting the subscribers");
                                    setTimeout(function () {
                                        isPublishing(false);
                                    }, 5 * 1000);
                                }, 2 * 1000);
                            }, 5 * 1000);

                            originalEntity = ko.toJSON(wd);
                        } else {
                            publishingMessage("Errors");
                            isPublishing(false);
                        }
                    });

            },
            exportWd = function () {
                var data = ko.mapping.toJSON(wd);
                return context.post(data, "/api/workflow-definitions/export")
                    .then(function (result) {
                        window.location = result.url;
                    });


            },
            itemAdded = function (element) {
                jsPlumb.draggable($(element));
            },
            showError = function (error) {
                console.log(error);
                wd().editActivity(_(wd().ActivityCollection()).find(function (v) { return v.WebId() === error.ItemWebId; }))();
            },
            remove = function () {
                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want delete this workflow definition ", "SPH - Workflow", ["Yes", "No"])
                   .done(function (dr) {
                       if (dr === "Yes") {
                           context.sendDelete("/api/workflow-definitions/" + ko.unwrap(wd().Id)).then(tcs.resolve);
                       } else {
                           tcs.resolve(false);
                       }
                   });

                return tcs.promise();
            },
            reload = function () {

                if (!wd().Id()) {
                    var tcs = new $.Deferred();
                    app.showMessage("You have yet to save your work ", "SPH - Workflow", ["OK"])
                       .done(tcs.resolve);

                    return tcs.promise();
                }
                return activate(wd().Id())
                .done(function () {
                    $("div.modalHost, div.modalBlockout").remove();
                });
            },
            viewPages = function () {
                window.location = "#page.list/" + wd().Id();
                return Task.fromResult(true, 1500);
            },
            canDeactivate = function () {
                var tcs = new $.Deferred();
                if (originalEntity !== ko.toJSON(wd)) {
                    app.showMessage("Save change to the item", "Rx Developer", ["Yes", "No", "Cancel"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {
                                saveAsync().done(function () {
                                    tcs.resolve(true);
                                });
                            }
                            if (dialogResult === "No") {
                                tcs.resolve(true);
                            }
                            if (dialogResult === "Cancel") {
                                tcs.resolve(false);
                            }

                        });
                } else {
                    return true;
                }
                return tcs.promise();
            };

        var vm = {
            publishingMessage: publishingMessage,
            selectedActivity: selectedActivity,
            isPublishing: isPublishing,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            canDeactivate: canDeactivate,
            toolboxElements: toolboxElements,
            wd: wd,
            itemAdded: itemAdded,
            errors: errors,
            showError: showError,
            autoSave: autoSave,
            autoSaveInterval: autoSaveInterval,
            toolbar: {
                saveCommand: saveAsync,
                canExecuteSaveCommand: function () {
                    return wd().Name();
                },
                exportCommand: exportWd,
                removeCommand: remove,
                reloadCommand: reload,
                commands: ko.observableArray([
                    {
                        command: viewPages,
                        caption: "Pages",
                        icon: "fa fa-code"
                    },
                    {
                        command: compileAsync,
                        caption: "Build",
                        icon: "fa fa-gear"
                    },
                    {
                        command: publishAsync,
                        caption: "Publish",
                        icon: "fa fa-sign-out"
                    }])
            }
        };

        return vm;

    });
