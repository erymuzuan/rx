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
            errors = ko.observableArray(),
            functoids = ko.observableArray(),
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
            m_instance = null,
            initializeFunctoid = function (fnc) {
                var element = $('#' + fnc.WebId());
                m_instance.makeSource(element, {
                    endPoint: ["Rectangle", {width: 10, height: 10}],
                    anchor: "RightMiddle",
                    connector: [ "Straight"],
                    connectorStyle: { strokeStyle: "#5c96bc", lineWidth: 2, outlineColor: "transparent", outlineWidth: 4 }
                });

                if (fnc.ArgumentCollection().length) {
                    m_instance.makeTarget(element, {
                        dropOptions: { hoverClass: "dragHover" },
                        anchor: ["LeftMiddle"],
                        maxConnections: fnc.ArgumentCollection().length,
                        onMaxConnections: function (info, e) {
                            alert("Maximum connections (" + info.maxConnections + ") reached" + e);
                        }
                    });
                }
                m_instance.draggable(element);
            },
            toolboxItemDraggedStop = function (arg) {
                var functoid = context.toObservable(ko.mapping.toJS(ko.dataFor(this).functoid)),
                    x = arg.clientX,
                    y = arg.clientY;

                functoid.designer = ko.dataFor(this).designer;
                functoid.X(x - $('#map-canvas').offset().left + $(window).scrollLeft());
                functoid.Y(y - $('#map-canvas').offset().top + $(window).scrollTop());
                functoid.WebId(system.guid());
                functoids.push(functoid);

                setTimeout(function () {
                    initializeFunctoid(functoid);
                }, 500);
            },
            jsPlumbReady = function () {
                isJsPlumbReady = true;

                var instance = jsPlumb.getInstance({
                    Endpoint: ["Rectangle", {width: 10, height: 10}],
                    HoverPaintStyle: {strokeStyle: "#1e8151", lineWidth: 2 },
                    ConnectionOverlays: [
                        [ "Arrow", {
                            location: 1,
                            id: "arrow",
                            length: 14,
                            foldback: 0.8
                        } ]
                    ],
                    Container: "container-canvas"
                });
                m_instance = instance;

                var sourceWindows = jsPlumb.getSelector("div.source-field");
                var targetWindows = jsPlumb.getSelector("div.destination-field");

                //instance.draggable(windows);

                instance.bind("click", function (c) {
                    instance.detach(c);
                    if (c.map) {
                        td().MapCollection.remove(c.map);
                    }
                });

                var connectionInitialized = false;
                instance.bind("connection", function (info) {
                    if (!connectionInitialized) {
                        return;
                    }

                    // direct map
                    if (info.sourceId.indexOf("source-field-") > -1 && info.targetId.indexOf("destination-field-") > -1) {
                        var sourceField = info.sourceId.replace("source-field-", ""),
                            destinationField = info.targetId.replace("destination-field-", "");

                        var dm = new bespoke.sph.domain.DirectMap({Source: sourceField, Destination: destinationField, WebId: system.guid()});
                        td().MapCollection.push(dm);
                        info.connection.map = dm;
                    }
                    //  functoid map
                    if (info.targetId.indexOf("destination-field-") > -1 && info.sourceId.indexOf("source-field-") < 0 ) {
                        var destinationField = info.targetId.replace("destination-field-", ""),
                            fnc = ko.dataFor($('#' + info.sourceId)[0]);

                        var fm = new bespoke.sph.domain.FunctoidMap({Destination: destinationField, WebId: system.guid()});
                        fm.Functoid = ko.observable(fnc);
                        td().MapCollection.push(fm);
                        info.connection.map = fm;
                    }


                    var selectArg = function(sourceFunctoid,targetFunctoid){

                        // for those with more than 1 arg, if array or 1 arg, just auto add or select
                        require(['viewmodels/functoid.args', 'durandal/app'], function (dialog, app2) {

                            dialog.functoid(targetFunctoid);
                            app2.showDialog(dialog)
                                .done(function (result) {
                                    if (!result) return;
                                    if (result === "OK") {
                                        var arg = _(targetFunctoid.ArgumentCollection()).find(function(v){
                                            return ko.unwrap(v.Name) === dialog.arg();
                                        });
                                        arg.Functoid(sourceFunctoid);
                                        info.connection.sf = sourceFunctoid;

                                        info.connection.getOverlay("label").setLabel(dialog.arg());
                                    }
                                });

                        });
                    };
                    // source functoid
                    if (info.sourceId.indexOf("source-field-") > -1 && info.targetId.indexOf("destination-field-") < 0) {
                        var sourceField = info.sourceId.replace("source-field-", ""),
                            targetFnc = ko.dataFor(document.getElementById(info.targetId));

                        var sourceFnc = new bespoke.sph.domain.SourceFunctoid({Field: sourceField, WebId: system.guid()});
                        selectArg(sourceFnc, targetFnc);
                    }


                    // functoid- functoid
                    if (info.sourceId.indexOf("source-field-") < 0 && info.targetId.indexOf("destination-field-") < 0) {
                        var sourceFnc = ko.dataFor(document.getElementById(info.sourceId)),
                            targetFnc = ko.dataFor(document.getElementById(info.targetId));

                        selectArg(sourceFnc, targetFnc);

                    }
                });

                // suspend drawing and initialise.
                instance.doWhileSuspended(function () {

                    instance.makeSource(sourceWindows, {
                        filter: ".ep01",
                        anchor: ["RightMiddle"],
                        connector: [ "Straight"],
                        connectorStyle: { strokeStyle: "#5c96bc", lineWidth: 2, outlineColor: "transparent", outlineWidth: 4 }
                    });


                });

                instance.makeTarget(targetWindows, {
                    filter: "ep02",
                    dropOptions: { hoverClass: "dragHover" },
                    anchor: ["LeftMiddle"],
                    maxConnections: 1,
                    onMaxConnections: function (info, e) {
                        alert("Maximum connections (" + info.maxConnections + ") reached" + e);
                    }
                });


                var loopFunctoid = function(item, parent){
                    if(!item){
                        return;
                    }
                    if(ko.unwrap(item.$type) === "Bespoke.Sph.Domain.SourceFunctoid, domain.sph"){
                        var conn = instance.connect({source: "source-field-" + ko.unwrap(item.Field), target: ko.unwrap(parent.WebId) });
                        conn.sf = item;
                        return;
                    }

                    var tool = _(functoidToolboxItems()).find(function(v){
                        return ko.unwrap(item.$type) === ko.unwrap(ko.unwrap(v.functoid).$type);
                    });
                    item.designer = tool.designer;
                    functoids.push(item);

                    var element = document.getElementById( ko.unwrap(item.WebId));
                    instance.makeSource(element, {
                        endPoint: ["Rectangle", {width: 10, height: 10}],
                        anchor: "RightMiddle",
                        connector: [ "Straight"],
                        connectorStyle: { strokeStyle: "#5c96bc", lineWidth: 2, outlineColor: "transparent", outlineWidth: 4 }
                    });
                    if (item.ArgumentCollection().length) {
                        instance.makeTarget(element, {
                            dropOptions: { hoverClass: "dragHover" },
                            anchor: ["LeftMiddle"],
                            maxConnections: item.ArgumentCollection().length,
                            onMaxConnections: function (info, e) {
                                alert("Maximum connections (" + info.maxConnections + ") reached" + e);
                            }
                        });
                    }

                    if(parent){
                        var conn = instance.connect({source:  ko.unwrap(item.WebId), target: ko.unwrap(parent.WebId) });
                        conn.sf = item;
                    }
                    _(item.ArgumentCollection()).each(function(v){
                        loopFunctoid(v.Functoid(), item);
                    });
                };

                // functoid maps
                _(td().MapCollection()).each(function (m) {
                    if(typeof  m.Source === "undefined"){
                        loopFunctoid(m.Functoid());
                        var conn = instance.connect({source:  ko.unwrap(m.Functoid().WebId), target:'destination-field-'+ ko.unwrap(m.Destination) });
                        conn.map = m;
                    }
                });

                // direct maps
                _(td().MapCollection()).each(function (m) {
                    if(ko.unwrap(m.Source)){
                        var conn = instance.connect({source: "source-field-" + ko.unwrap(m.Source), target: "destination-field-" + ko.unwrap(m.Destination) });
                        conn.map = m;
                    }
                });
                connectionInitialized = true;

                jsPlumb.fire("jsPlumbDemoLoaded", instance);

            },
            attached = function (view) {

                $(view).on('dblclick','div.functoid', function(e){
                    e.preventDefault(true);
                    e.stopPropagation(true);
                    e.stopImmediatePropagation(true);
                    console.log(ko.dataFor(this));
                    
                });
                $.get("/sph/transformdefinition/GetFunctoids", function (list) {
                    functoidToolboxItems(list.$values);
                    $('ul#function-toolbox>li.list-group-item').draggable({
                        helper: function () {
                            return $("<div></div>").addClass("dragHoverToolbox").append($(this).find('i').clone());
                        },
                        stop: toolboxItemDraggedStop
                    });
                });

                var script = $('<script type="text/javascript" src="/Scripts/jsPlumb/bundle.js"></script>').appendTo('body'),
                    timer = setInterval(function () {
                        if (window.jsPlumb !== undefined) {
                            clearInterval(timer);
                            script.remove();

                            jsPlumb.ready(jsPlumbReady);
                        }
                    }, 1500);


                if (td().TransformDefinitionId() === 0) {
                    return;
                }
                var icon = function (html, item) {
                        var type = item.type;
                        if (typeof type === "object") {
                            type = type[0];
                        }
                        if (type === "string") {
                            return '<i class="glyphicon glyphicon-bold" style="font-size:12px;color:brown;margin-right:5px"></i>';
                        }
                        if (type === "integer") {
                            return '<i class="fa fa-sort-numeric-asc" style="font-size:12px;color:blue;margin-right:5px"></i>';
                        }
                        if (type === "object") {
                            return '<i class="fa fa-building-o" style="font-size:12px;color:grey;margin-right:5px"></i>';
                        }
                        if (type === "number") {
                            return '<i class="glyphicon glyphicon-usd" style="font-size:12px;color:green;margin-right:5px"></i>';
                        }
                        if (type === "boolean") {
                            return '<i class="glyphicon glyphicon-ok" style="font-size:12px;color:red;margin-right:5px"></i>';
                        }
                        if (type === "array") {
                            return '<i class="fa fa-list" style="font-size:12px;color:gray;margin-right:5px"></i>';
                        }
                        return "";
                    },
                    shtml = "",
                    root = sourceSchema();

                var buildSourceTree = function (branch) {
                    for (var key in branch.properties) {
                        var iconHtml = icon(shtml, branch.properties[key]);
                        shtml += '<li><div style="display: inline-block" class="source-field" id="source-field-' + key + '">' + iconHtml
                            + '<span class="ep01">' + key + '</span>';

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
                        shtml += '</div></li>';
                    }
                };

                buildSourceTree(root);
                $('#source-panel').html(shtml);

                var dhtml = "",
                    buildDestinationTree = function (branch) {
                        for (var key in branch.properties) {
                            var iconHtml = icon(dhtml, branch.properties[key]);
                            dhtml += '<li><div><div class="destination-field" id="destination-field-' + key + '">' + iconHtml +
                                '<span class="ep02">' + key + '</span></div>';

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
                            dhtml += '</div></li>';
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
                            if (td().TransformDefinitionId() === 0) {
                                td().TransformDefinitionId(result.id);
                            }
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
            },
            validateAsync = function(){
                var tcs = new $.Deferred();
                context.post(ko.mapping.toJSON(td), '/sph/transformdefinition/validatefix')
                    .done(function(result){
                        $('i.fa.fa-exclamation-circle.error').remove();
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {
                            logger.error("There are errors in your map, !!!");
                            var uniqueList = _.uniq(result.Errors, function(item, key, a) {
                                return item.ItemWebId;
                            });
                            errors(uniqueList);
                            _(uniqueList).each(function(v){
                                $('#' + v.ItemWebId + ' div.toolbox-item').append('<i class="fa fa-exclamation-circle error"></i>');
                            });
                        }
                        tcs.resolve(true);

                    });

                return tcs.promise();
            },
            publishAsync = function(){
                var tcs = new $.Deferred();
                context.post(ko.mapping.toJSON(td), '/sph/transformdefinition/publish')
                    .done(function(result){
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {
                            errors(result.Errors);
                            logger.error("There are errors in your map, !!!");
                        }
                        tcs.resolve(true);

                    });

                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            errors: errors,
            functoidToolboxItems: functoidToolboxItems,
            functoids: functoids,
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
                    },
                    {
                        command : validateAsync,
                        caption : 'Validate',
                        icon : 'fa fa-check'
                    },
                    {
                        command : publishAsync,
                        caption : 'Publish',
                        icon : 'fa fa-sign-out'
                    }
                ])
            }
        };

        return vm;

    });
