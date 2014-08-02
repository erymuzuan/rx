/// <reference path="../../Scripts/jquery-2.1.0.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/.js" />
/// <reference path="../services/datacontext.js" />


define(['services/datacontext', 'services/logger', objectbuilders.system, 'ko/_ko.mapping'],
    function (context, logger, system) {

        var td = ko.observable(),
            functoidToolboxItems = ko.observableArray(),
            isBusy = ko.observable(false),
            sourceMember = ko.observable(),
            sourceSchema = ko.observable(),
            destinationSchema = ko.observable(),
            activate = function (id) {
                var query = String.format("TransformDefinitionId eq {0}", id);
                var tcs = new $.Deferred();
                context.loadOneAsync("TransformDefinition", query)
                    .done(function (b) {
                        if (b) {
                            td(b);
                            $.get("/sph/transformdefinition/schema?type=" + b.InputTypeName(), function (s) {
                                sourceSchema(s);
                            });
                            $.get("/sph/transformdefinition/schema?type=" + b.OutputTypeName(), function (s) {
                                destinationSchema(s);
                            });
                        } else {
                            td(new bespoke.sph.domain.TransformDefinition({ Name: 'New Mapping Definition', WebId: system.guid() }));
                        }
                        tcs.resolve(true);
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
                connector: ["Straight", { stub: [10, 15], gap: 10 }],
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
            jsPlumbReady = function () {
                isJsPlumbReady = true;
                jsPlumb.draggable($("li.source-field, span.destination-field"));
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
                $.get("/sph/transformdefinition/GetFunctoids", function (list) {
                    functoidToolboxItems(list);
                    jsPlumb.draggable($("ul#function-toolbox>li"));
                });

                //jsPlumb.bind("click", connectionClicked);
                //jsPlumb.bind("connectionDragStop", connectionDragStop);
                //wd().ActivityCollection.subscribe(activitiesChanged, null, "arrayChange");
            },
            attached = function (view) {

                var script = $('<script type="text/javascript" src="/Scripts/jsPlumb/bundle.js"></script>').appendTo('body'),
                   timer = setInterval(function () {
                       if (window.jsPlumb !== undefined) {
                           clearInterval(timer);
                           script.remove();

                           jsPlumb.ready(jsPlumbReady);
                           $('#source-panel li>span.source-field').each(function () {
                               var id = $(this).prop('id');
                               jsPlumb.addEndpoint(id, sourceEndpoint, { anchor: "Right", uuid: id + "Source" });
                           });
                           $('#destination-panel li>span.destination-field').each(function () {
                               var id = $(this).prop('id');
                               jsPlumb.addEndpoint(id, targetEndpoint, { anchor: "Left", uuid: id + "Source" });
                           });
                       }
                   }, 2500);

                if (td().TransformDefinitionId() === 0) {
                    return;
                }
                var icon = function (html, item) {
                    var type = item.type;
                    if (typeof type === "object") {
                        type = type[0];
                    }
                    if (type === "string") {
                        return '<i class="glyphicon glyphicon-bold" style="font-size:14px;color:brown;margin-right:5px"></i>';
                    }
                    if (type === "integer") {
                        return '<i class="fa fa-sort-numeric-asc" style="font-size:14px;color:blue;margin-right:5px"></i>';
                    }
                    if (type === "object") {
                        return '<i class="fa fa-building-o" style="font-size:14px;color:grey;margin-right:5px"></i>';
                    }
                    if (type === "number") {
                        return '<i class="glyphicon glyphicon-usd" style="font-size:14px;color:green;margin-right:5px"></i>';
                    }
                    if (type === "boolean") {
                        return '<i class="glyphicon glyphicon-ok" style="font-size:14px;color:red;margin-right:5px"></i>';
                    }
                    if (type === "array") {
                        return '<i class="fa fa-list" style="font-size:14px;color:gray;margin-right:5px"></i>';
                    }
                    return "";
                },
                shtml = "",
                root = sourceSchema();

                var buildSourceTree = function (branch) {
                    for (var key in branch.properties) {
                        var iconHtml = icon(shtml, branch.properties[key]);
                        shtml += '<li>' + iconHtml + '<span class="source-field" id="source-field-' + key + '">' + key + '</span>';

                        var type = branch.properties[key].type;
                        if (typeof type === "object") {
                            type = type[0];
                        }
                        if (type === "object") {
                            shtml += '<ul style="list-style: none">';
                            buildSourceTree(branch.properties[key]);
                            shtml += '</ul>';
                        }
                        if (type === "array") {
                            shtml += '<ul style="list-style: none">';
                            buildSourceTree(branch.properties[key].items);
                            shtml += '</ul>';
                        }
                        shtml += '</li>';
                    }
                };

                buildSourceTree(root);
                $('#source-panel').html(shtml);

                var dhtml = "",
                    buildDestinationTree = function (branch) {
                        for (var key in branch.properties) {
                            var iconHtml = icon(dhtml, branch.properties[key]);
                            dhtml += '<li>' + iconHtml + '<span class="destination-field" id="destination-field-' + key + '">' + key + '</span>';

                            var type = branch.properties[key].type;
                            if (typeof type === "object") {
                                type = type[0];
                            }
                            if (type === "object") {
                                dhtml += '<ul style="list-style: none">';
                                buildDestinationTree(branch.properties[key]);
                                dhtml += '</ul>';
                            }
                            if (type === "array") {
                                dhtml += '<ul style="list-style: none">';
                                buildDestinationTree(branch.properties[key].items);
                                dhtml += '</ul>';
                            }
                            dhtml += '</li>';
                        }
                    };

                buildDestinationTree(destinationSchema());
                $('#destination-panel').html(dhtml);

                $('#search-box-tree').on('keyup', function () {
                    var text = $(this).val().toLowerCase();
                    $('#source-panel li>span.source-field').each(function () {
                        var span = $(this),
                            li = span.parent(),
                            content = span.text().toLowerCase();
                        if (content.indexOf(text) < 0) {
                            li.hide();
                        } else {
                            li.show();
                        }

                    });
                });

            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(td);
                isBusy(true);

                context.post(data, "/sph/transformdefinition")
                    .then(function (result) {
                        isBusy(false);


                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            editProp = function () {

                var tcs = new $.Deferred(),
                    clone = context.toObservable(ko.mapping.toJS(td));
                require(['viewmodels/transform.definition.prop.dialog', 'durandal/app'], function (dialog, app2) {
                    dialog.td(clone);

                    app2.showDialog(dialog)
                        .done(function (result) {
                            tcs.resolve(true);
                            $('div.modalBlockout,div.modalHost').remove();
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in td()) {
                                    if (typeof td()[g] === "function" && (td()[g].name === "c" || td()[g].name === "observable")) {
                                        td()[g](ko.unwrap(clone[g]));
                                    } else {
                                        td()[g] = clone[g];
                                    }
                                }
                            }
                        });

                });

                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            functoidToolboxItems: functoidToolboxItems,
            activate: activate,
            attached: attached,
            sourceMember: sourceMember,
            sourceSchema: sourceSchema,
            destinationSchema: destinationSchema,
            td: td,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                {
                    command: editProp,
                    caption: 'Edit Properties',
                    icon: 'fa fa-table'
                }])
            }
        };

        return vm;

    });
