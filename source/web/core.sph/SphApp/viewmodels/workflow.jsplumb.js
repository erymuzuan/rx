/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
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


define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.system],
    function (context, logger, router, system) {

        var isBusy = ko.observable(false),
            wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition(system.guid())),
            activate = function (id) {
                isBusy(true);
                var query = String.format("Id eq '{0}'", id),
                    tcs = new $.Deferred();

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
                try {
                    var connection = jsPlumb.connect(option);
                    connection.setParameter({ label: label });
                } catch (e) {
                    console.log(e);
                }



            },
            jsPlumbReady = function () {
                isJsPlumbReady = true;
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


            },
            attached = function (view) {
                var script = $('<script type="text/javascript" src="/Scripts/jsPlumb/bundle.js"></script>').appendTo('body'),
                    timer = setInterval(function () {
                        if (window.jsPlumb !== undefined) {
                            clearInterval(timer);
                            script.remove();

                            jsPlumb.ready(jsPlumbReady);
                        }
                    }, 2500);


                var paintedConnectors = [];


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
                $('#open-close-toolbox-button').on('click', function (e) {
                    e.preventDefault();
                    $('#toolbox-panel').hide();
                    return false;
                });
                $(document).on('keyup', function (e) {
                    if (e.ctrlKey && e.altKey && e.keyCode === 88) {
                        $('#toolbox-panel').show();
                    }
                });


            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            wd: wd
        };

        return vm;

    });
