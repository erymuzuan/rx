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


define(['services/datacontext', 'services/logger', 'durandal/plugins/router', objectbuilders.system],
    function (context, logger, router, system) {

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
                var id = routeData.id,
                    query = String.format("WorkflowDefinitionId eq {0}", id),
                    tcs = new $.Deferred();
                context.loadOneAsync("WorkflowDefinition", query)
                    .done(function (b) {
                        vm.wd(b);
                        tcs.resolve(true);
                    });

                return tcs.promise();

            },
            jsPlumbReady = function () {
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
            },
            toolboxItemDraggedStop = function (arg) {
                var act = context.toObservable(ko.mapping.toJS(ko.dataFor(this))),
                    x = arg.clientX,
                    y = arg.clientY;

                act.WorkflowDesigner().X(x);
                act.WorkflowDesigner().Y(y);
                act.WebId(system.guid());
                wd().ActivityCollection.push(act);
            },
            viewAttached = function () {
                var script = $('<script type="text/javascript" src="/Scripts/jsPlumb/bundle.js"></script>').appendTo('body'),
                    timer = setInterval(function () {
                          if (window.jsPlumb !== undefined) {
                              clearInterval(timer);
                              script.remove();

                              //jsPlumb.ready(jsPlumbReady);
                              setTimeout(
                              jsPlumbReady,2500);
                          }
                      }, 2500),
                    count = 1,
                    activities = wd().ActivityCollection();


                populateToolbox();

                $.getScript('/Scripts/jquery-ui-1.10.3.custom.min.js', function () {
                    $('div.toolbox-item').draggable({
                        helper: 'clone',
                        stop: toolboxItemDraggedStop
                    });
                });


                // set the default position and add class
                $('div#container-canvas>div.activity').each(function () {
                    var p = $(this),
                        act = ko.dataFor(this),
                        x = act.WorkflowDesigner().X() || 250,
                        y = act.WorkflowDesigner().Y() || 150 * count,
                        fullName = typeof act.$type === "function" ? act.$type() : act.$type,
                        name = /Bespoke\.Sph\.Domain\.(.*?),/.exec(fullName)[1];


                    p.css("left", x + "px")
                        .css("top", y + "px")
                        .find("div.activity32")
                        .addClass("activity32-" + name);

                    count++;

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
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            toolboxElements: ko.observableArray(),
            wd: wd,
            saveCommand: saveAsync
        };

        return vm;

    });
