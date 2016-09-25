﻿/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/jsPlumb/jsPlumb.js" />
/// <reference path="~/Scripts/_task.js" />

/**
 *
 * @param no
 * @param name
 * @returns {{no, name, functoids, mappings, source, target, active}}
 * @constructor
 */
bespoke.sph.domain.TransformDefinitionPage = function (no, name) {
    return {
        no: ko.observable(no),
        name: ko.observable(name),
        functoids: ko.observableArray(),
        mappings: ko.observableArray(),
        source: ko.observable(),
        target: ko.observable(),
        active: ko.observable(true)
    };
};

/**
 * @param{{ItemWebId:function, FileName:string}} item
 */
define(["services/datacontext", "services/logger", objectbuilders.system, "ko/_ko.mapping", objectbuilders.app, objectbuilders.router, "services/app",
        "knockout", "bespoke", "underscore", "jquery", "jsPlumb"],
    function (context, logger, system, koMapping, app, router, app2, ko, bespoke, _, $) {

        var td = ko.observable(new bespoke.sph.domain.TransformDefinition({Id: "0"})),
            pages = ko.observableArray(),
            currentPage = ko.observable(),
            hideUnconnectedNodes = ko.observable(false),
            functoidToolboxItems = ko.observableArray(),
            functoids = ko.observableArray(),
            originalEntity = "",
            errors = ko.observableArray(),
            isBusy = ko.observable(false),
            sourceMember = ko.observable(),
            sourceSchema = ko.observable(),
            destinationSchema = ko.observable(),
            activate = function (id) {
            id = id || "0";
            if (id === "0") {
                td(new bespoke.sph.domain.TransformDefinition({
                    Name: "New Mapping Definition",
                    WebId: system.guid()
                }));
                return true;
            }


            var query = String.format("Id eq '{0}'", id);
            return $.getJSON("/api/transform-definitions/" + id + "/designer")
                .then(function (settingPage) {
                    if (settingPage) {
                        const items = settingPage.map(v => ko.mapping.fromJS(v));
                        pages(items);
                    }
                    return $.get("/api/transform-definitions/functoids");
                })
                .then(function (list) {
                    functoidToolboxItems(list.$values);
                    return context.loadOneAsync("TransformDefinition", query);
                })
                .then(function (b) {

                    _(b.FunctoidCollection()).each(function (v) {
                        v.designer = ko.observable({
                            FontAwesomeIcon: "",
                            "BootstrapIcon": "",
                            "PngIcon": "",
                            Category: ""
                        });
                    });
                    td(b);
                    if (pages().length === 0) {

                        const pg = new bespoke.sph.domain.TransformDefinitionPage(1, "Page 1");
                        pg.functoids(_(b.FunctoidCollection()).map(function (v) {
                            return ko.unwrap(v.WebId);
                        }));
                        pg.mappings(_(b.MapCollection()).map(v => ko.unwrap(v.WebId)));
                        pages.push(pg);
                    }

                    originalEntity = ko.toJSON(td);
                    return context.get(`/api/assemblies/${b.OutputTypeName()}/object-schema`);

                })
                .then(function (s) {
                    destinationSchema(s);
                    if (td().InputTypeName()) {
                        return context.get(`/api/assemblies/${td().InputTypeName()}/object-schema`);
                    }
                    return context.post(ko.toJSON(td), "/api/transform-definitions/object-schema");

                })
                .then(sourceSchema)
                .fail(function (a, b, text) {
                    console.error(a);
                    // console.error(b);
                    logger.error(text);
                });

        };
        var isJsPlumbReady,
            jsPlumbInstance = null,
            changingPage = false,
            connectorStyle = {strokeStyle: "#5c96bc", lineWidth: 2, outlineColor: "transparent", outlineWidth: 4},
            initializeFunctoid = function (fnc) {
                const element = $(`#${fnc.WebId()}`);
                jsPlumbInstance.makeSource(element, {
                    filter: ".fep",
                    endPoint: ["Rectangle", {width: 10, height: 10}],
                    anchor: "RightMiddle",
                    connector: ["Straight"],
                    connectorStyle: {strokeStyle: "#5c96bc", lineWidth: 2, outlineColor: "transparent", outlineWidth: 4}
                });

                var anchorOptions = ["LeftMiddle", "LeftTop", "LeftBottom"];
                if (fnc.ArgumentCollection().length) {
                    jsPlumbInstance.makeTarget(element, {
                        dropOptions: {hoverClass: "dragHover"},
                        anchors: anchorOptions,
                        maxConnections: fnc.ArgumentCollection().length,
                        onMaxConnections: function (info, e) {
                            alert("Maximum connections (" + info.maxConnections + ") reached" + e);
                        }
                    });
                }
                jsPlumbInstance.draggable(element);
            },
            toolboxItemDraggedStop = function (arg) {
                if (!td().Id() || td().Id() === "0") {
                    logger.error("Please save your mapping definition before using functoid!!");
                    return;
                }

                var functoid = context.toObservable(ko.mapping.toJS(ko.dataFor(this).functoid)),
                    x = arg.clientX,
                    y = arg.clientY,
                    canvas = $("#container-canvas"),
                    offset = canvas.offset(),
                    canvasWidth = parseFloat(canvas.css("width").replace("px", ""));

                if (x > (offset.left + canvasWidth)) {
                    logger.error("Please drop the functoid inside the mapping designer box");
                    return;
                }

                functoid.designer = ko.dataFor(this).designer;
                functoid.X(x - offset.left + $(window).scrollLeft());
                functoid.Y(y - offset.top + $(window).scrollTop());
                functoid.WebId(system.guid());
                td().FunctoidCollection.push(functoid);
                currentPage().functoids.push(ko.unwrap(functoid.WebId));
                functoids.push(functoid);

                setTimeout(function () {
                    initializeFunctoid(functoid);
                }, 500);
            },
            jsPlumbReady = function () {
                isJsPlumbReady = true;

                var instance = jsPlumb.getInstance({
                    Endpoint: ["Rectangle", {width: 10, height: 10}],
                    HoverPaintStyle: {strokeStyle: "#1e8151", lineWidth: 2},
                    ConnectionOverlays: [
                        ["Arrow", {
                            location: 1,
                            id: "arrow",
                            length: 14,
                            foldback: 0.8
                        }]
                    ],
                    Container: "container-canvas"
                });
                jsPlumbInstance = instance;

                //instance.draggable(windows);

                instance.bind("click", function (c) {
                    instance.detach(c);
                    if (c.map) {
                        td().MapCollection.remove(c.map);
                    }

                    if (typeof c.functoidArg === "function") {
                        c.functoidArg(null);
                    }
                    td().FunctoidCollection.remove(c.sf);

                });

                var connectionInitialized = false;
                instance.bind("connection", function (info) {
                    if (!connectionInitialized) {
                        return;
                    }
                    if (changingPage) return;

                    // direct map
                    if (info.sourceId.indexOf("source-field-") > -1 && info.targetId.indexOf("destination-field-") > -1) {
                        var sourceField2 = info.sourceId.replace("source-field-", "").replace("-", "."),
                            destinationField2 = info.targetId.replace("destination-field-", "").replace("-", ".");

                        var exists = _(td().MapCollection()).find(function (v) {
                            return ko.unwrap(v.Source) === sourceField2 && ko.unwrap(v.Destination) === destinationField2;
                        });
                        if (exists) return;
                        if (info.connection.map) return;


                        var dm = new bespoke.sph.domain.DirectMap({
                            Source: sourceField2,
                            Destination: destinationField2,
                            WebId: system.guid()
                        });
                        td().MapCollection.push(dm);
                        info.connection.map = dm;
                        currentPage().mappings.push(ko.unwrap(dm.WebId));
                    }
                    //  functoid map
                    if (info.targetId.indexOf("destination-field-") > -1 && info.sourceId.indexOf("source-field-") < 0) {
                        var destinationField = info.targetId.replace("destination-field-", "").replace("-", ".");

                        var fm = new bespoke.sph.domain.FunctoidMap({
                            Destination: destinationField,
                            WebId: system.guid()
                        });
                        fm.Functoid(info.sourceId);
                        td().MapCollection.push(fm);
                        info.connection.map = fm;
                        currentPage().mappings.push(ko.unwrap(fm.WebId));
                    }


                    var selectArg = function (sourceFunctoid, targetFunctoid) {

                        var tcs = new $.Deferred();
                        // for those with more than 1 arg, if array or 1 arg, just auto add or select
                        require(["viewmodels/functoid-args", "durandal/app"], function (dialog, app2) {

                            dialog.functoid(targetFunctoid);
                            app2.showDialog(dialog)
                                .done(function (result) {
                                    if (!result) return;
                                    if (result === "OK") {
                                        var arg = _(targetFunctoid.ArgumentCollection()).find(function (v) {
                                            return ko.unwrap(v.Name) === dialog.arg();
                                        });
                                        arg.Functoid(ko.unwrap(sourceFunctoid.WebId));
                                        info.connection.sf = sourceFunctoid;

                                        info.connection.setLabel(dialog.arg());
                                    }
                                    tcs.resolve(result);
                                });

                        });

                        return tcs.promise();
                    };
                    // source field functoid
                    if (info.sourceId.indexOf("source-field-") > -1 && info.targetId.indexOf("destination-field-") < 0) {
                        var sourceField = info.sourceId.replace("source-field-", "").replace(/-/g, "."),
                            targetFnc2 = ko.dataFor(document.getElementById(info.targetId));

                        var sourceFnc2 = new bespoke.sph.domain.SourceFunctoid({
                            Field: sourceField,
                            WebId: system.guid()
                        });

                        selectArg(sourceFnc2, targetFnc2).done(function (result) {
                            if (result === "OK") {
                                td().FunctoidCollection.push(sourceFnc2);
                                currentPage().functoids.push(ko.unwrap(sourceFnc2.WebId));
                                functoids.push(sourceFnc2);
                            } else {
                                instance.detach(info.connection);
                            }
                        });

                    }


                    // functoid- functoid
                    if (info.sourceId.indexOf("source-field-") < 0 && info.targetId.indexOf("destination-field-") < 0) {
                        var sourceFnc = ko.dataFor(document.getElementById(info.sourceId)),
                            targetFnc = ko.dataFor(document.getElementById(info.targetId));

                        selectArg(sourceFnc, targetFnc);

                    }
                });


                connectionInitialized = true;
                jsPlumb.fire("jsPlumbDemoLoaded", instance);
                currentPage(pages()[0]);

            },
            drawSchemaTree = function () {
                var icon = function (item) {
                        var type = item.type,
                            format = item.format;
                        if (typeof type === "object") {
                            type = type[0];
                        }
                        if (format === "date-time") {
                            return "glyphicon glyphicon-calendar";
                        }
                        if (type === "string") {
                            return "glyphicon glyphicon-bold";
                        }
                        if (type === "integer") {
                            return "fa fa-sort-numeric-asc";
                        }
                        if (type === "object") {
                            return "fa fa-building-o";
                        }
                        if (type === "number") {
                            return "glyphicon glyphicon-usd";
                        }
                        if (type === "boolean") {
                            return "glyphicon glyphicon-ok";
                        }
                        if (type === "array") {
                            return "fa fa-list";
                        }
                        return "";
                    },
                    root = sourceSchema();

                var sources = [],
                    buildTree = function (side, branch, parent, items, parentNode) {
                        var findLeaf = function (k) {
                            "use strict";
                            return v => ko.unwrap(v.SourceField) === side + parent + k;
                        };
                        for (var key in branch.properties) {
                            if (branch.properties.hasOwnProperty(key)) {

                                var leaf = {
                                    id: side + parent + key,
                                    icon: icon(branch.properties[key]),
                                    text: key + (branch.properties[key].required === false ? " ?" : "")
                                };
                                if (parentNode)
                                    leaf.parent = parentNode.id;
                                else
                                    leaf.parent = "#";
                                // see if there's any connection
                                if (hideUnconnectedNodes()) {
                                    var fl = findLeaf(key),
                                        connected = _(td().MapCollection()).find(fl);
                                    if (!connected) {
                                        continue;
                                    }

                                }


                                items.push(leaf);

                                var type = branch.properties[key].type;
                                if (typeof type === "object") {
                                    type = type[0];
                                }
                                if (type === "object") {
                                    buildTree(side, branch.properties[key], parent + key + "-", items, leaf);
                                }
                                if (type === "array") {
                                    buildTree(side, branch.properties[key].items, parent + key + "-", items, leaf);
                                }
                            }
                        }
                    };

                buildTree("source-field-", root, "", sources);

                $("#source-panel").jstree({
                        'core': {
                            'data': sources
                        },
                        "contextmenu": {
                            items: [
                                {
                                    label: "Hide",
                                    action: function () {
                                        var parent = $(element).jstree("get_selected", true),
                                            mb = parent[0].data;
                                        console.log(mb);
                                    }
                                }
                            ]
                        },
                        "search": {
                            "case_insensitive": true,
                            "show_only_matches": true,
                            "show_only_matches_children": true
                        },
                        "plugins": ["search", "contextmenu"]

                    })
                    .jstree("open_all")
                    .bind("after_open.jstree after_close.jstree", function () {
                        currentPage(currentPage());
                    });


                var destinations = [];
                buildTree("destination-field-", destinationSchema(), "", destinations);
                $("#destination-panel").jstree({
                        'core': {
                            'data': destinations
                        },
                        "contextmenu": {
                            items: [
                                {
                                    label: "Hide",
                                    action: function () {
                                        var parent = $(element).jstree("get_selected", true),
                                            mb = parent[0].data;

                                        console.log(mb);

                                    }
                                }
                            ]
                        },
                        "search": {
                            "case_insensitive": true,
                            "show_only_matches": true
                        },
                        "plugins": ["search", "contextmenu"]


                    })
                    .jstree("open_all")
                    .bind("after_open.jstree after_close.jstree", function () {
                        currentPage(currentPage());
                    });

                $("#destination-panel li.jstree-leaf").addClass("target-item");
            },
            attached = function () {

                $("ul#function-toolbox>li.list-group-item").draggable({
                    helper: function () {
                        return $("<div></div>").addClass("dragHoverToolbox").append($(this).find("i").clone());
                    },
                    stop: toolboxItemDraggedStop
                });


                var script = $("<script type=\"text/javascript\" src=\"/Scripts/jsPlumb/bundle.js\"></script>").appendTo("body"),
                    timer = setInterval(function () {
                        if (window.jsPlumb !== undefined) {
                            clearInterval(timer);
                            script.remove();

                            jsPlumb.ready(jsPlumbReady);
                        }
                    }, 1500);


                if (!td().OutputTypeName()) {
                    return;
                }

                drawSchemaTree();


                $("#search-box-source-tree").on("keyup", function (e) {
                    var code = e.which;
                    if (code === 13) e.preventDefault();
                    if (code === 32 || code === 13 || code === 188 || code === 186) {
                        var text = "\"\"" + $(this).val() + "\"\"";
                        console.log(text);
                        $("#source-panel").jstree("search", text);
                        var pg = currentPage();
                        currentPage(null);
                        currentPage(pg);
                    }

                });
                $("#clear-search-box-source-tree-button").on("click", function (e) {
                    e.preventDefault();
                    $("#source-panel").jstree("clear_search");
                    $("#search-box-source-tree").val("");
                });
                $("#search-box-destination-tree").on("keyup", function (e) {
                    var code = e.which;
                    if (code === 13) e.preventDefault();
                    if (code === 32 || code === 13 || code === 188 || code === 186) {
                        var text = "\"\"" + $(this).val() + "\"\"";
                        console.log(text);
                        $("#destination-panel").jstree("search", text);

                        var pg = currentPage();
                        currentPage(null);
                        currentPage(pg);
                    }
                });
                $("#clear-search-box-destination-tree-button").on("click", function (e) {
                    e.preventDefault();
                    $("#destination-panel").jstree("clear_search");
                    $("#search-box-destination-tree").val("");
                });


                $.getScript("/scripts/jquery.contextMenu.js", function () {

                    $.contextMenu({
                        selector: "ul.nav-pills>li",
                        callback: function (key, e) {
                            console.log(ko.dataFor(this));
                            console.log(key);
                            var pg = ko.dataFor(e.$trigger[0]);
                            if (key === "rename") {
                                app2.prompt("Rename your page", ko.unwrap(pg.name))
                                    .done(function (result) {
                                        if (result) {
                                            pg.name(result);
                                        }
                                    });
                            }

                            if (key === "delete") {
                                app.showMessage("Are you sure you want to delete '" + ko.unwrap(pg.name) + "', this will also delete all the connections within this page", "Rx Developer", ["Yes", "No"])
                                    .done(function (dialogResult) {
                                        if (dialogResult === "Yes") {
                                            pages.remove(pg);
                                            // remove the connection as well
                                            _(pg.mappings()).each(function (v) {
                                                var mp = (td().MapCollection()).find(function (k) {
                                                    return ko.unwrap(v) === ko.unwrap(k.WebId);
                                                });

                                                if (mp) {
                                                    td().MapCollection.remove(mp);
                                                }
                                            });
                                            _(pg.functoids()).each(function (v) {
                                                var mp = (td().FunctoidCollection()).find(function (k) {
                                                    return ko.unwrap(v) === ko.unwrap(k.WebId);
                                                });

                                                if (mp) {
                                                    td().FunctoidCollection.remove(mp);
                                                }
                                            });

                                            currentPage(pages()[0]);

                                        }
                                    });
                            }
                        },
                        items: {
                            "rename": {name: "Rename", icon: "bug"},
                            "delete": {name: "Delete", icon: "circle-o"}
                        }
                    });
                });

                // hideUnconnectedNodes.subscribe(drawSchemaTree);


            },
            save = function () {

                $("div.functoid").each(function () {
                    var fnt = ko.dataFor(this),
                        p = $(this),
                        x = parseInt(p.css("left")),
                        y = parseInt(p.css("top"));
                    if (!fnt) {
                        p.remove();
                    } else {
                        fnt.X(x);
                        fnt.Y(y);
                    }
                });
                var data = ko.mapping.toJSON(td),
                    pageJson = ko.mapping.toJSON(pages);
                isBusy(true);


                return context.post(pageJson, "/api/transform-definitions/" + ko.unwrap(td().Id) + "/designer")
                    .then(function () {
                        return context.post(data, "/api/transform-definitions");
                    })
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            originalEntity = ko.toJSON(td);
                        } else {
                            logger.error(result.message);
                        }
                    });
            },
            canDeactivate = function () {
                var tcs = new $.Deferred();
                if (originalEntity !== ko.toJSON(td)) {
                    app.showMessage("Save change to the item", "Rx Developer", ["Yes", "No", "Cancel"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {
                                save().done(function () {
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
            },
            editProp = function () {

                var tcs = new $.Deferred(),
                    clone = context.toObservable(ko.mapping.toJS(td));
                require(["viewmodels/transform.definition.prop.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.td(clone);

                    app2.showDialog(dialog)
                        .done(function (result) {
                            tcs.resolve(true);
                            $("div.modalBlockout,div.modalHost").remove();
                            if (!result) return;
                            if (result === "OK") {
                                var td1 = td();
                                td().Name(ko.unwrap(clone.Name));
                                for (var g in td1) {
                                    if (td1.hasOwnProperty(g)) {
                                        if (typeof td1[g] === "function" && (td1[g].name === "c" || td1[g].name === "observable")) {
                                            td1[g](ko.unwrap(clone[g]));
                                        } else {
                                            td1[g] = clone[g];
                                        }
                                    }
                                }

                                // try build the tree for new item
                                if (!td1.Id() || td1.Id() === "0") {
                                    var inTask = context.post(ko.toJSON(td), "/api/transform-definitions/object-schema"),
                                        outTask = context.get("/api/assemblies/" + td1.OutputTypeName() + "/object-schema");
                                    $.when(inTask, outTask).done(function (input, output) {
                                        sourceSchema(input[0]);
                                        destinationSchema(output[0]);
                                        attached();
                                    });
                                }
                            }
                        });

                });

                return tcs.promise();
            },
            viewFile = function (e) {
                let file = e.FileName || e,
                    line = e.Line || 1;
                const params = [
                        `height=${screen.height}`,
                        `width=${screen.width}`,
                        "toolbar=0",
                        "location=0",
                        "fullscreen=yes"
                    ].join(","),
                    editor = window.open("/sph/editor/file?id=" + file.replace(/\\/g, "/") + "&line=" + line, "_blank", params);
                editor.moveTo(0, 0);
            },
            validateAsync = function () {
                var tcs = new $.Deferred();
                context.post(ko.mapping.toJSON(td), "/api/transform-definitions/validate-fix")
                    .done(function (result) {
                        $("i.fa.fa-exclamation-circle.error").remove();
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {
                            logger.error("There are errors in your map, !!!");
                            const uniqueList = _.uniq(result.Errors, function (item) {
                                return item.ItemWebId;
                            });
                            errors(uniqueList);
                            _(uniqueList).each(function (v) {
                                $(`#${v.ItemWebId} div.toolbox-item`).append("<i class=\"fa fa-exclamation-circle error\"></i>");
                            });
                        }
                        tcs.resolve(true);

                    });

                return tcs.promise();
            },
            publishAsync = function () {
                $("i.error").remove();
                return context.post(ko.mapping.toJSON(td), "/api/transform-definitions/" + td().Id() + "/publish")
                    .done(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                            originalEntity = ko.toJSON(td);
                        } else {
                            errors(result.Errors);
                            logger.error("There are errors in your map, !!!");

                            _(result.Errors).each(function (v) {
                                $(`#${v.ItemWebId} div.toolbox-item`).append("<i class=\"fa fa-exclamation-circle error\"></i>");
                            });
                        }
                    });

            },
            generatePartialAsync = function () {
                return context.post(ko.mapping.toJSON(td), `/api/transform-definitions/${td().Id()}/generate-partial`)
                    .done(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                        } else {
                            logger.error(`You already have the partial code define, in ${result.message}`);
                        }
                        viewFile(result.file);
                    });
            },
            testTransform = function () {
                const uri = `/api/transform-definitions/${td().Id()}/execute-test`;
                return context.post(ko.toJSON(td), uri)
                    .done(function (result) {
                        var w = window.open("/sph/editor/ace?mode=javascript", "_blank", `height=${screen.height},width=${screen.width},toolbar=0,location=0,fullscreen=yes`);
                        if (typeof input === "string") {
                            w.window.code = result;
                        } else {
                            w.window.code = JSON.stringify(result, null, "\t");
                        }
                    }).fail(function (e) {
                        console.log(e);
                        logger.error(e.responseText);
                    });
            },
            editTestInput = function () {

                var tcs = new $.Deferred(),
                    uri = `/api/transform-definitions/${td().Id()}/test-input`;
                $.get(uri)
                    .done(function (input) {
                        var w = window.open("/sph/editor/ace?mode=javascript", "_blank", `height=${screen.height},width=${screen.width},toolbar=0,location=0,fullscreen=yes`);
                        if (typeof input === "string") {
                            w.window.code = input;
                        } else {
                            w.window.code = JSON.stringify(input, null, "\t");
                        }
                        w.window.saved = function (code, close) {
                            context.post(code, uri).done(tcs.resolve);
                            if (close) {
                                w.close();
                            }
                            tcs.resolve(true);
                        };
                    });
                return tcs.promise();
            },
            addPage = function () {
                return app2.prompt("Give your page a name", "Page " + (pages().length + 1))
                    .done(function (name) {

                        if (!name) {
                            return;
                        }
                        var pg = new bespoke.sph.domain.TransformDefinitionPage(pages().length + 1, name);
                        pages.push(pg);
                        currentPage(pg);
                    });
            },
            changePage = function (page) {
                console.log(ko.toJS(page));
                currentPage(page);
            };
        currentPage.subscribe(function (page) {
            if (!page) {
                return;
            }
            var instance = jsPlumbInstance;
            changingPage = true;

            _(pages()).each(function (p) {
                p.active(false);
            });
            page.active(true);

            instance.deleteEveryEndpoint();
            // redraw everything here
            const sourceWindows = jsPlumb.getSelector("#source-panel li.jstree-leaf");
            const targetWindows = jsPlumb.getSelector("#destination-panel li.jstree-leaf");


            instance.makeSource(sourceWindows, {
                anchor: ["RightMiddle"],
                connector: ["Straight"],
                connectorStyle: connectorStyle
            });


            instance.makeTarget(targetWindows, {
                dropOptions: {hoverClass: "dragHover"},
                anchor: ["LeftMiddle"],
                maxConnections: 1,
                onMaxConnections: function (info, e) {
                    alert("Maximum connections (" + info.maxConnections + ") reached" + e);
                }
            });

            const currentPageFunctoids = _(td().FunctoidCollection()).filter(function (v) {
                return page.functoids().indexOf(ko.unwrap(v.WebId)) > -1;
            });
            functoids(currentPageFunctoids);

            // -- STARTS
            var makeFunctoidElement = function (item) {
                if (!item) {
                    return;
                }
                const tool = _(functoidToolboxItems()).find(function (v) {
                    return ko.unwrap(item.$type) === ko.unwrap(ko.unwrap(v.functoid).$type);
                });
                if (typeof item.designer === "function") {
                    item.designer(tool.designer);
                }
                else {
                    item.designer = ko.observable(tool.designer);
                }

                const element = document.getElementById(ko.unwrap(item.WebId));
                instance.makeSource(element, {
                    filter: ".fep",
                    endPoint: ["Rectangle", {width: 10, height: 10}],
                    anchor: "RightMiddle",
                    connector: ["Straight"],
                    connectorStyle: {strokeStyle: "#5c96bc", lineWidth: 2, outlineColor: "transparent", outlineWidth: 4}
                });
                if (item.ArgumentCollection().length) {
                    instance.makeTarget(element, {
                        dropOptions: {hoverClass: "dragHover"},
                        anchor: ["LeftMiddle"],
                        maxConnections: item.ArgumentCollection().length,
                        onMaxConnections: function (info, e) {
                            alert("Maximum connections (" + info.maxConnections + ") reached" + e);
                        }
                    });
                }
                instance.draggable(element);
            };

            // functoids maps
            var fncContains = function (webid) {
                var found = null;
                _(td().FunctoidCollection()).each(function (f) {
                    if (page.functoids().indexOf(ko.unwrap(f.WebId)) < 0) return;
                    _(f.ArgumentCollection()).each(function (m) {
                        if (ko.unwrap(m.Functoid) === ko.unwrap(webid)) {
                            found = ko.unwrap(f.WebId);
                        }
                    });
                });
                return found;
            };

            // create the source and target for each functoid
            _(td().FunctoidCollection()).each(function (f) {
                if (page.functoids().indexOf(ko.unwrap(f.WebId)) < 0) return;
                if (ko.unwrap(f.$type) !== "Bespoke.Sph.Domain.SourceFunctoid, domain.sph") {
                    makeFunctoidElement(f);
                }
            });

            // creates the connection for each argument list
            _(td().FunctoidCollection()).each(function (f) {
                if (page.functoids().indexOf(ko.unwrap(f.WebId)) < 0) return;
                if (ko.unwrap(f.$type) !== "Bespoke.Sph.Domain.SourceFunctoid, domain.sph") {
                    _(f.ArgumentCollection()).each(function (a) {
                        var source = document.getElementById(ko.unwrap(a.Functoid));
                        if (typeof a.Functoid !== "function" || !source) {
                            return;
                        }
                        var conn = instance.connect({
                            source: source,
                            target: ko.unwrap(f.WebId),
                            paintStyle: connectorStyle
                        });
                        conn.functoidArg = a.Functoid;
                    });
                }
            });

            // for source to functoid
            _(td().FunctoidCollection()).each(function (f) {
                if (page.functoids().indexOf(ko.unwrap(f.WebId)) < 0) return;
                if (ko.unwrap(f.$type) === "Bespoke.Sph.Domain.SourceFunctoid, domain.sph") {
                    var target = document.getElementById(fncContains(f.WebId)),
                        src = "source-field-" + ko.unwrap(f.Field).replace(".", "-"),
                        src2 = "source-field-" + ko.unwrap(f.Field).replace(".", "-");
                    if (!target) {
                        td().FunctoidCollection.remove(f);
                        return;
                    }
                    while (src.lastIndexOf("-") > -1) {
                        if (document.getElementById(src)) break;
                        src = src.substring(0, src.lastIndexOf("-"));
                    }

                    var original = src2 === src;
                    if (!original) {

                        var conn2 = jsPlumbInstance.connect({
                            source: src,
                            target: target,
                            paintStyle: {lineWidth: 1, strokeStyle: "rgba(190, 190, 190, 0.4)"},
                            anchors: ["Right", "Left"],
                            endpoint: ["Rectangle", {width: 10, height: 8}]
                        });
                        conn2.sf = f;
                        return;
                    }


                    try {

                        var conn = instance.connect({source: src, target: target, paintStyle: connectorStyle});
                        conn.sf = f;
                    }
                    catch (e) {
                        console.log("Cannot connect", ko.mapping.toJS(f));
                        console.log(e);
                    }

                }
            });

            // END
            // direct maps
            _(td().MapCollection()).each(function (m) {
                var id = ko.unwrap(m.WebId);
                if (page.mappings().indexOf(id) < 0) return;
                if (!ko.unwrap(m.Source)) return;

                try {

                    var src1 = "source-field-" + ko.unwrap(m.Source).replace(".", "-"),
                        src = src1,
                        target1 = "destination-field-" + ko.unwrap(m.Destination).replace(".", "-"),
                        target = target1;

                    while (src.lastIndexOf("-") > -1) {
                        if (document.getElementById(src)) break;
                        src = src.substring(0, src.lastIndexOf("-"));
                    }

                    while (target.lastIndexOf("-") > -1) {
                        if (document.getElementById(target)) break;
                        target = target.substring(0, target.lastIndexOf("-"));
                    }


                    if (!document.getElementById(src)) return;
                    if (!document.getElementById(target)) return;

                    var original = src1 === src && target === target1;
                    if (!original) {

                        var conn2 = jsPlumbInstance.connect({
                            source: src,
                            target: target,
                            paintStyle: {lineWidth: 1, strokeStyle: "rgba(190, 190, 190, 0.4)"},
                            anchors: ["Right", "Left"],
                            endpoint: ["Rectangle", {width: 10, height: 8}]
                        });
                        conn2.map = m;
                        return;
                    }
                    var label = "From : " + ko.unwrap(m.Source) + "<br>To : " + ko.unwrap(m.Destination),
                        conn = jsPlumbInstance.connect({source: src, target: target, paintStyle: connectorStyle});
                    conn.bind("mouseenter", function (conn1) {
                        if (conn1.getOverlay("connLabel")) {
                            return;
                        }
                        conn1.addOverlay(["Label", {
                            label: label,
                            location: 0.5,
                            id: "connLabel",
                            cssClass: "connector-label"
                        }]);
                        setTimeout(function () {
                            try {
                                conn1.removeOverlay("connLabel");
                            } catch (err) {
                                console.log(err, "Connection might have been removed from the page");
                            }
                        }, 5000);
                    });

                    conn.bind("mouseout", function (conn1) {
                        conn1.removeOverlay("connLabel");
                    });
                    conn.map = m;
                } catch (e) {
                    console.log("Cannot connect", ko.mapping.toJS(m));
                    console.log(e);
                }

            });
            // functoid maps
            _(td().MapCollection()).each(function (m) {
                var id = ko.unwrap(m.WebId);
                if (page.mappings().indexOf(id) < 0) {
                    return;
                }

                if (typeof m.Source === "undefined") {
                    var conn = jsPlumbInstance.connect({
                        source: ko.unwrap(m.Functoid),
                        target: "destination-field-" + ko.unwrap(m.Destination).replace(".", "-"),
                        paintStyle: connectorStyle
                    });
                    conn.map = m;
                }
            });

            changingPage = false;
        });

        var vm = {
            isBusy: isBusy,
            errors: errors,
            viewFile: viewFile,
            functoids: functoids,
            functoidToolboxItems: functoidToolboxItems,
            activate: activate,
            canDeactivate: canDeactivate,
            attached: attached,
            sourceMember: sourceMember,
            sourceSchema: sourceSchema,
            destinationSchema: destinationSchema,
            td: td,
            editProp: editProp,
            changePage: changePage,
            hideUnconnectedNodes: hideUnconnectedNodes,
            pages: pages,
            toolbar: {
                saveCommand: save,
                groupCommands: [
                    {
                        caption: "Test",
                        commands: [

                            {
                                command: testTransform,
                                caption: "Test Mapping",
                                icon: "fa fa-play",
                                tooltip: "Test the generated mapping",
                                tooltipPlacement: "bottom"
                            },
                            {
                                command: editTestInput,
                                caption: "Edit test input",
                                icon: "fa fa-plus",
                                tooltip: "Edit json representation of the test input",
                                tooltipPlacement: "bottom"
                            }
                        ]
                    }
                ],
                commands: ko.observableArray([
                    {
                        command: editProp,
                        caption: "Edit Properties",
                        icon: "fa fa-table"
                    },
                    {
                        command: validateAsync,
                        caption: "Validate",
                        icon: "fa fa-check"
                    },
                    {
                        command: publishAsync,
                        enable: ko.computed(function () {
                            if (!td().Name())
                                return false;
                            if (td().Name() === "New Mapping Definition")
                                return false;
                            if (td().Name() === "0")
                                return false;
                            return true;
                        }),
                        caption: "Publish",
                        tooltip: "Generate and compile your TrasnformDefinition",
                        tooltipPlacement: "bottom",
                        icon: "fa fa-sign-out"
                    },
                    {
                        command: generatePartialAsync,
                        caption: "Edit Partial",
                        icon: "fa fa-code",
                        tooltip: "Generate C# partial code for before and after transform custom code",
                        tooltipPlacement: "bottom",
                        enable: ko.computed(function () {
                            if (ko.unwrap(td().Id) === "0") {
                                return false;
                            }
                            if (!ko.unwrap(td().Id)) {
                                return false;
                            }
                            return true;
                        })
                    },
                    {
                        command: addPage,
                        caption: "Add Page",
                        icon: "fa fa-file-o"
                    }
                ]),
                htmlCommands: ko.observableArray([
                    {
                        html: "<input type=\"search\" id=\"search-box-source-tree\" style=\"width:200px; height:28px;padding:6px 12px\" placeholder=\"search source\">" +
                        "<button title=\"Clear the source search box\" id=\"clear-search-box-source-tree-button\" class=\"btn btn-default\"><i class=\"fa fa-times\"></i></button>",
                        icon: "fa fa-users"
                    },
                    {
                        html: "<input type=\"search\" id=\"search-box-destination-tree\" style=\"width:200px; height:28px;padding:6px 12px\" placeholder=\"search destination\">" +
                        "<button title=\"Clear the destination search box\" id=\"clear-search-box-destination-tree-button\" class=\"btn btn-default\"><i class=\"fa fa-times\"></i></button>",
                        icon: "fa fa-users"
                    }])
            }
        };

        return vm;

    });
