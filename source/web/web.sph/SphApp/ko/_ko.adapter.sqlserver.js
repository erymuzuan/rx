///<reference path="../../web/core.sph/Scripts/jstree.min.js"/>
///<reference path="../../web/core.sph/Scripts/require.js"/>
///<reference path="../../web/core.sph/Scripts/underscore.js"/>
///<reference path="../../web/core.sph/SphApp/objectbuilders.js"/>
///<reference path="../../web/core.sph/SphApp/schemas/form.designer.g.js"/>

define(["knockout"], function (ko) {

    ko.bindingHandlers.adapterTree = {
        init: function (element, valueAccessor) {
            var system = require(objectbuilders.system),
                value = valueAccessor(),
                adapter = ko.unwrap(value.adapter),
                searchbox = ko.unwrap(value.searchbox),
                addOperation = value.addOperation,
                addTable = value.addTable,
                member = value.selected,
                jsTreeData = [{
                    id: "table-node",
                    text: "Tables",
                    type: "table-node",
                    state: {
                        opened: true
                    },
                    children: []
                },
                    {
                        "id": "node-operations",
                        text: "Operations",
                        icon: "fa fa-cogs",
                        state: {
                            opened: true
                        }
                    }],
                calculateColumnName = function (col) {

                    var column = ko.toJS(col),
                        lookup = column.LookupColumnTable.IsEnabled ? " <i class='fa fa-binoculars column-icon' style='margin-left:5px;color:darkgreen'></i>" : "",
                        complex = column.IsComplex ? " <i class='fa fa-link column-icon' style='margin-left:5px;color:darkblue'></i>" : "",
                        ignore = column.Ignore ? " <i class='fa fa-eye-slash column-icon' style='margin-left:5px;color:grey'></i>" : "",
                        readonly = column.IsComputed ? " <i class='fa fa-eye column-icon' style='margin-left:5px;color:grey'></i>" : "",
                        primaryKey = column.IsPrimaryKey ? "<i class='icon-key column-icon' style='margin-right:3px;color:#ff8c00;font-weight: bold'></i> " : "",
                        displayName = column.DisplayName || "",
                        bracket = displayName ? " [" : "",
                        bracket2 = displayName ? "]" : "";

                    return primaryKey + column.Name +  readonly + bracket + displayName + bracket2 + complex + lookup + ignore;


                },
                mapTable = function (v) {
                    var table = ko.toJS(v),
                        columns = _(v.ColumnCollection()).map(function (col) {
                            return {
                                id: "column-" + ko.unwrap(col.WebId),
                                text: calculateColumnName(col),
                                type: ko.unwrap(col.TypeName),
                                data: col
                            }
                        }),
                        relations = _(v.ChildRelationCollection()).map(function (col) {
                            return {
                                text: ko.unwrap(col.Table),
                                type: ko.unwrap(col.IsSelected) ? "child-table-selected" : "child-table",
                                data: col
                            }
                        }),
                        actions = _(v.ControllerActionCollection()).map(function(act){
                            act.guid = "action-" + system.guid();
                            return {
                                id: act.guid,
                                text: ko.unwrap(act.Name),
                                type: "api-action-" + (ko.unwrap(act.IsEnabled) ? "enabled": "disabled"),
                                data: act
                            }
                        });

                    return {
                        id: "table-" + ko.unwrap(v.Schema) + "-" + ko.unwrap(v.Name),
                        text: ko.unwrap(v.Schema) + "." + ko.unwrap(v.Name),
                        state: "open",
                        type: "table",
                        data: v,
                        children: [{
                            text: "Columns",
                            state: "open",
                            type: "column",
                            data: v,
                            children: columns
                        },
                            {
                                text: "Related tables",
                                state: "open",
                                type: "related-table",
                                data: v,
                                children: relations
                            },
                            {
                                text: "API actions",
                                state: "open",
                                type: "api-actions",
                                data: v,
                                children: actions
                            }]
                    };
                },
                mapOperation = function (v) {

                    return {
                        id : "operation-" + ko.unwrap(v.Uuid),
                        text: ko.unwrap(v.Schema) + "." + ko.unwrap(v.Name),
                        state: "open",
                        type: ko.unwrap(v.ObjectType),
                        data: v
                    };
                },

                loadJsTree = function () {
                    jsTreeData[0].children = _(adapter.TableDefinitionCollection()).map(mapTable);
                    jsTreeData[1].children = _(adapter.OperationDefinitionCollection()).map(mapOperation);

                    $(element)
                        .on("select_node.jstree", function (node, selected) {
                            member(selected.node.data);
                        })
                        .on("move_node.jstree", function (e, data) {
                            var ref = $(element).jstree(true),
                                column = data.node.data,
                                parent = ref.get_node(data.parent),
                                collection = parent.data.ColumnCollection,
                                order = 1;

                            collection.remove(function (c) {
                                return ko.unwrap(column.WebId) === ko.unwrap(c.WebId);
                            });
                            collection.splice(data.position, 0, column);
                            _(ko.unwrap(collection)).each(function(c){
                                if(ko.isObservable(c.Order)){
                                    c.Order(order++);
                                }
                            });
                        })
                        .on("create_node.jstree", function (event, node) {
                            console.log(node, "node");
                        })
                        .jstree({
                            "core": {
                                "animation": 0,
                                "check_callback": function (operation, node, nodeParent) {
                                    if (operation === "move_node") {
                                        var column = node.data,
                                            ref = $(element).jstree(true),
                                            target = ref.get_node(nodeParent);
                                        if (!column) {
                                            return false;
                                        }
                                        if (!target) {
                                            return false;
                                        }
                                        if(target.text !== "Columns") return false;
                                        if(target.id !== node.parent)return false;

                                        //console.log("dragged into target %s , and id ",target.text, target.id);

                                        return true;
                                    }
                                    return true;
                                },
                                "themes": { "stripes": true },
                                'data': jsTreeData
                            },
                            "dnd": {
                                "is_draggable": function (node) {
                                    return node.id.startsWith("column-");
                                }
                            },
                            "contextmenu": {
                                "items": function ($node) {
                                    //console.log($node);
                                    var ref = $(element).jstree(true),
                                        removeMenu = {
                                            label: "Remove",
                                            action: function () {
                                                ref.delete_node($node);

                                                if ($node.type === "table") {
                                                    adapter.TableDefinitionCollection.remove(function (v) {
                                                        return ko.unwrap(v.Schema) == ko.unwrap($node.data.Schema)
                                                            && ko.unwrap(v.Name) == ko.unwrap($node.data.Name);
                                                    });
                                                } else {
                                                    adapter.OperationDefinitionCollection.remove(function (v) {
                                                        return ko.unwrap(v.Schema) == ko.unwrap($node.data.Schema)
                                                            && ko.unwrap(v.Name) == ko.unwrap($node.data.Name);
                                                    });
                                                }

                                                return true;

                                            }
                                        },
                                        editOperation = {
                                            "label": "Edit operation",
                                            "action": function () {
                                                var op = ko.toJS($node.data);
                                                window.location = '/sph#adapter.sqlserver.sproc/' + ko.unwrap(adapter.Id) + '/' + op.Schema + '.' + op.Name;
                                            }
                                        },
                                        selectRelatedTable = {
                                            label: "Make a relation",
                                            action: function () {
                                                $node.data.IsSelected(true);
                                                ref.set_type($node, "child-table-selected");
                                            }
                                        },
                                        unselectRelatedTable = {
                                            label: "Undo relation",
                                            action: function () {
                                                ref.set_type($node, "child-table");
                                                $node.data.IsSelected(false);
                                            }
                                        },
                                        setNodeText = function (col) {
                                            var text = calculateColumnName(col);

                                            $("#" + $node.id + ">a.jstree-anchor>i.column-icon").remove();

                                            $(element).jstree(true)
                                                .rename_node($node, text);
                                        },
                                        markComplex = {
                                            label: "Make a complex resource",
                                            action: function () {

                                                $node.data.IsComplex(true);
                                                setNodeText($node.data);
                                            }
                                        },
                                        makeInline = {
                                            label: "Make an inline member",
                                            action: function () {
                                                $node.data.IsComplex(false);
                                                setNodeText($node.data);
                                            }
                                        },
                                        makeLookup = {
                                            label: "Enable lookup value",
                                            action: function () {
                                                $node.data.LookupColumnTable().IsEnabled(true);
                                                setNodeText($node.data);
                                            }
                                        },
                                        undoLookup = {
                                            label: "Disable lookup value",
                                            action: function () {
                                                $node.data.LookupColumnTable().IsEnabled(false);
                                                setNodeText($node.data);
                                            }
                                        },
                                        ignoreMenu = {
                                            label: "Ignore",
                                            action: function () {
                                                $node.data.Ignore(true);
                                                setNodeText($node.data);
                                            }
                                        },
                                        includeMenu = {
                                            label: "Include",
                                            action: function () {
                                                $node.data.Ignore(false);
                                                setNodeText($node.data);
                                            }
                                        },
                                        enableActionMenu = {
                                            label: "Enable",
                                            action: function () {
                                                $node.data.IsEnabled(true);
                                                ref.set_type($node, "api-action-enabled");
                                            }
                                        },
                                        disableActionMenu = {
                                            label: "Disable",
                                            action: function () {
                                                $node.data.IsEnabled(false);
                                                ref.set_type($node, "api-action-disabled");
                                            }
                                        };
                                    var data = $node.data;

                                    if($node.id === "table-node" && addTable){
                                        return [{
                                            label:"Add new table/view",
                                            action: addTable
                                        }]
                                    }

                                    if($node.id === "node-operations" && addOperation){
                                        return [{
                                            label:"Add new sproc/function",
                                            action: addOperation,
                                            "icon" : "fa fa-cogs"
                                        }]
                                    }


                                    if ($node.id.startsWith("column-")) {
                                        var items = [];

                                        if (ko.unwrap(data.Ignore))
                                            items.push(includeMenu);
                                        else
                                            items.push(ignoreMenu);

                                        if (ko.unwrap(data.IsComplex))
                                            items.push(makeInline);
                                        else
                                            items.push(markComplex);

                                        if (ko.unwrap(data.LookupColumnTable().IsEnabled))
                                            items.push(undoLookup);
                                        else
                                            items.push(makeLookup);


                                        return items;
                                    }

                                    if ($node.id.startsWith("action-")) {
                                        if (ko.unwrap(data.IsEnabled))
                                            return [disableActionMenu];

                                        return [enableActionMenu];
                                    }
                                    if ($node.type === "child-table-selected")
                                        return [unselectRelatedTable];
                                    if ($node.type === "child-table")
                                        return [selectRelatedTable];
                                    if ($node.type === "table")
                                        return [removeMenu];
                                    if ($node.id.startsWith("operation-"))
                                        return [editOperation, removeMenu];
                                    return [];

                                }
                            },
                            "types": {

                                "api-action-enabled": {
                                    "icon": "fa fa-check-square-o"
                                },
                                "api-action-disabled": {
                                    "icon": "fa fa-square-o"
                                },
                                "api-actions": {
                                    "icon": "fa fa-cloud"
                                },
                                "table-node": {
                                    "icon": "fa fa-table"
                                },
                                "table": {
                                    "icon": "fa fa-list"
                                },
                                "related-table": {
                                    "icon": "fa fa-list-alt"
                                },
                                "child-table": {
                                    "icon": "fa fa-square-o"
                                },
                                "child-table-selected": {
                                    "icon": "fa fa-check"
                                },
                                "column": {
                                    "icon": "fa fa-columns"
                                },
                                "P ": {
                                    "icon": "fa fa-cog"
                                },
                                "P": {
                                    "icon": "fa fa-cog"
                                },
                                "FN": {
                                    "icon": "fa fa-calculator"
                                },
                                "TF": {
                                    "icon": "fa fa-th"
                                },
                                "IF": {
                                    "icon": "fa fa-th-list"
                                },
                                "System.String, mscorlib": {
                                    "icon": "glyphicon glyphicon-bold",
                                    "valid_children": []
                                },
                                "System.DateTime, mscorlib": {
                                    "icon": "glyphicon glyphicon-calendar",
                                    "valid_children": []
                                },
                                "System.DateTimeOffset, mscorlib": {
                                    "icon": "fa fa-hourglass-o",
                                    "valid_children": []
                                },
                                "System.TimeSpan, mscorlib": {
                                    "icon": "fa fa-clock-o",
                                    "valid_children": []
                                },
                                "System.Int16, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Int32, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Int64, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Byte, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Byte[], mscorlib": {
                                    "icon": "fa fa-picture-o",
                                    "valid_children": []
                                },
                                "System.Decimal, mscorlib": {
                                    "icon": "glyphicon glyphicon-usd",
                                    "valid_children": []
                                },
                                "System.Double, mscorlib": {
                                    "icon": "fa fa-gg",
                                    "valid_children": []
                                },
                                "System.Guid, mscorlib": {
                                    "icon": "fa fa-glide",
                                    "valid_children": []
                                },
                                "System.Single, mscorlib": {
                                    "icon": "fa fa-eur",
                                    "valid_children": []
                                },
                                "System.Boolean, mscorlib": {
                                    "icon": "glyphicon glyphicon-ok",
                                    "valid_children": []
                                },
                                "System.Object, mscorlib": {
                                    "icon": "fa fa-building-o"
                                },
                                "ComplexMember": {
                                    "icon": "glyphicon glyphicon-list"
                                },
                                "System.Xml.XmlDocument, System.Xml": {
                                    "icon": "fa fa-code"
                                },
                                "Bespoke.Sph.Domain.ComplexMember, domain.sph": {
                                    "icon": "glyphicon glyphicon-list"
                                }
                            },
                            "plugins": ["contextmenu", "types", "dnd"]
                        });
                };


            adapter.TableDefinitionCollection.subscribe(function (changes) {
                console.log(changes);
                var tables = _(changes).filter(function (c) {
                        return c.status === "added";
                    }),
                    children = _(tables).map(function (c) {
                        return mapTable(c.value);
                    });
                var ref = $(element).jstree(true),
                    sel = ref.get_node("table-node");

                sel = sel.length ? sel[0] : sel;
                _(children).each(function (tableNode) {
                    sel = ref.create_node(sel, tableNode);
                });

            }, null, "arrayChange");


            adapter.OperationDefinitionCollection.subscribe(function (changes) {
                console.log(changes);
                var operations = _(changes).filter(function (c) {
                        return c.status === "added";
                    }),
                    children = _(operations).map(function (c) {
                        return mapOperation(c.value);
                    });
                var ref = $(element).jstree(true),
                    sel = ref.get_node("operation-node");

                sel = sel.length ? sel[0] : sel;
                _(children).each(function (opNode) {
                    sel = ref.create_node(sel, opNode);
                });

            }, null, "arrayChange");
            loadJsTree();

            var to = false;
            $(searchbox).keyup(function () {
                if (to) {
                    clearTimeout(to);
                }
                to = setTimeout(function () {
                    var v = $(searchbox).val();
                    $(element).jstree(true).search(v);
                }, 250);
            });

        }
    };

    ko.bindingHandlers.sprocRequestSchemaTree = {
        init: function (element, valueAccessor) {
            var system = require(objectbuilders.system),
                value = valueAccessor(),
                entity = ko.unwrap(value.entity),
                searchbox = ko.unwrap(value.searchbox),
                member = value.selected,
                jsTreeData = {
                    text: entity.Name(),
                    state: {
                        opened: true,
                        selected: true
                    }
                },
                recurseChildMember = function (node) {
                    node.children = _(node.data.MemberCollection()).map(function (v) {
                        return {
                            text: v.Name(),
                            state: "open",
                            type: v.TypeName(),
                            data: v
                        };
                    });
                    _(node.children).each(recurseChildMember);
                },
                loadJsTree = function () {
                    jsTreeData.children = _(entity.MemberCollection()).map(function (v) {
                        return {
                            text: v.Name(),
                            state: "open",
                            type: v.TypeName(),
                            data: v
                        };
                    });
                    _(jsTreeData.children).each(recurseChildMember);
                    $(element)
                        .on("select_node.jstree", function (node, selected) {
                            if (selected.node.data && typeof selected.node.data.Name === "function") {
                                member(selected.node.data);

                                // subscribe to Name change
                                member().Name.subscribe(function (name) {
                                    $(element).jstree(true)
                                        .rename_node(selected.node, name);
                                });
                                // type
                                member().TypeName.subscribe(function (name) {
                                    $(element).jstree(true)
                                        .set_type(selected.node, name);
                                });
                            }
                        })
                        .on("create_node.jstree", function (event, node) {
                            console.log(node, "node");
                        })
                        .on("rename_node.jstree", function (ev, node) {
                            var mb = node.node.data;
                            mb.Name(node.text);
                        })
                        .jstree({
                            "core": {
                                "animation": 0,
                                "check_callback": true,
                                "themes": {"stripes": true},
                                'data': jsTreeData
                            },
                            "contextmenu": {
                                "items": [
                                    {
                                        label: "Remove",
                                        action: function () {
                                            var ref = $(element).jstree(true),
                                                sel = ref.get_selected();

                                            // now delete the member
                                            var n = ref.get_selected(true)[0],
                                                p = ref.get_node($("#" + n.parent)),
                                                parentMember = p.data;
                                            if (parentMember && typeof parentMember.MemberCollection === "function") {
                                                var child = _(parentMember.MemberCollection()).find(function (v) {
                                                    return v.WebId() === n.data.WebId();
                                                });
                                                parentMember.MemberCollection.remove(child);
                                            } else {
                                                var child2 = _(entity.MemberCollection()).find(function (v) {
                                                    return v.WebId() === n.data.WebId();
                                                });
                                                entity.MemberCollection.remove(child2);
                                            }

                                            if (!sel.length) {
                                                return false;
                                            }
                                            ref.delete_node(sel);

                                            return true;

                                        }
                                    }
                                ]
                            },
                            "types": {

                                "System.String, mscorlib": {
                                    "icon": "glyphicon glyphicon-bold",
                                    "valid_children": []
                                },
                                "System.DateTime, mscorlib": {
                                    "icon": "glyphicon glyphicon-calendar",
                                    "valid_children": []
                                },
                                "System.Int32, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Decimal, mscorlib": {
                                    "icon": "glyphicon glyphicon-usd",
                                    "valid_children": []
                                },
                                "System.Boolean, mscorlib": {
                                    "icon": "glyphicon glyphicon-ok",
                                    "valid_children": []
                                },
                                "System.Object, mscorlib": {
                                    "icon": "fa fa-building-o"
                                },
                                "System.Array, mscorlib": {
                                    "icon": "glyphicon glyphicon-list"
                                }
                            },
                            "plugins": ["contextmenu", "dnd", "types", "search"]
                        });
                };
            loadJsTree();

            var to = false;
            $(searchbox).keyup(function () {
                if (to) {
                    clearTimeout(to);
                }
                to = setTimeout(function () {
                    var v = $(searchbox).val();
                    $(element).jstree(true).search(v);
                }, 250);
            });

        }
    };
    ko.bindingHandlers.sprocResponseSchemaTree = {
        init: function (element, valueAccessor) {
            var system = require(objectbuilders.system),
                value = valueAccessor(),
                entity = ko.unwrap(value.entity),
                name = ko.unwrap(value.name),
                searchbox = ko.unwrap(value.searchbox),
                member = value.selected,
                jsTreeData = {
                    text: entity.Name(),
                    state: {
                        opened: true,
                        selected: true
                    }
                },
                recurseChildMember = function (node) {
                    node.children = _(node.data.MemberCollection()).map(function (v) {
                        return {
                            text: v.Name(),
                            state: "open",
                            type: v.TypeName(),
                            data: v
                        };
                    });
                    _(node.children).each(recurseChildMember);
                },
                loadJsTree = function () {
                    jsTreeData.children = _(entity.MemberCollection()).map(function (v) {

                        var icon = ko.unwrap(v.$type) === "Bespoke.Sph.Domain.ComplexMember, domain.sph" ? "Bespoke.Sph.Domain.ComplexMember, domain.sph" : ko.unwrap(v.TypeName);
                        return {
                            text: v.Name(),
                            state: "open",
                            type: icon,
                            data: v
                        };
                    });
                    _(jsTreeData.children).each(recurseChildMember);
                    $(element)
                        .on("select_node.jstree", function (node, selected) {
                            if (selected.node.data && typeof selected.node.data.Name === "function") {
                                member(selected.node.data);

                                // subscribe to Name change
                                member().Name.subscribe(function (name) {
                                    $(element).jstree(true)
                                        .rename_node(selected.node, name);
                                });
                                // type
                                member().TypeName.subscribe(function (name) {
                                    $(element).jstree(true)
                                        .set_type(selected.node, name);
                                });
                            }
                        })
                        .on("create_node.jstree", function (event, node) {
                            console.log(node, "node");
                        })
                        .on("rename_node.jstree", function (ev, node) {
                            var mb = node.node.data;
                            mb.Name(node.text);
                        })
                        .jstree({
                            "core": {
                                "animation": 0,
                                "check_callback": true,
                                "themes": {"stripes": true},
                                'data': jsTreeData
                            },
                            "contextmenu": {
                                "items": [
                                    {
                                        label: "Add result set",
                                        action: function () {
                                            var text = name + "Result1",
                                                child = new bespoke.sph.domain.ComplexMember({
                                                    WebId: system.guid(),
                                                    AllowMultiple: true,
                                                    TypeName: text,
                                                    Name: text
                                                }),
                                                parent = $(element).jstree("get_selected", true),
                                                mb = parent[0].data,
                                                newNode = {
                                                    state: "open",
                                                    type: ko.unwrap(child.$type),
                                                    text: text,
                                                    data: child
                                                };

                                            var ref = $(element).jstree(true),
                                                sel = ref.get_selected();
                                            if (!sel.length) {
                                                return false;
                                            }
                                            sel = sel[0];
                                            sel = ref.create_node(sel, newNode);
                                            if (sel) {
                                                ref.edit(sel);
                                                if (mb && mb.MemberCollection) {
                                                    mb.MemberCollection.push(child);
                                                } else {
                                                    entity.MemberCollection.push(child);
                                                }
                                                return true;
                                            }
                                            return false;


                                        }
                                    },
                                    {
                                        label: "Add record",
                                        action: function () {
                                            var child = {
                                                    $type: "Bespoke.Sph.Integrations.Adapters.SprocResultMember, sqlserver.adapter",
                                                    WebId: system.guid(),
                                                    TypeName: ko.observable("System.String, mscorlib"),
                                                    Name: ko.observable("Member_Name"),
                                                    SqlDbType: ko.observable(),
                                                    IsNullable: ko.observable(false)
                                                },
                                                parent = $(element).jstree("get_selected", true),
                                                mb = parent[0].data,
                                                newNode = {
                                                    state: "open",
                                                    type: ko.unwrap(child.TypeName),
                                                    text: ko.unwrap(child.Name),
                                                    data: child
                                                };


                                            var ref = $(element).jstree(true),
                                                sel = ref.get_selected();
                                            if (!sel.length) {
                                                return false;
                                            }
                                            sel = sel[0];
                                            sel = ref.create_node(sel, newNode);
                                            if (sel) {
                                                ref.edit(sel);
                                                if (mb && mb.MemberCollection) {
                                                    mb.MemberCollection.push(child);
                                                } else {
                                                    entity.MemberCollection.push(child);
                                                }
                                                return true;
                                            }
                                            return false;


                                        }
                                    },
                                    {
                                        label: "Remove",
                                        action: function () {
                                            var ref = $(element).jstree(true),
                                                sel = ref.get_selected();

                                            // now delete the member
                                            var n = ref.get_selected(true)[0],
                                                p = ref.get_node($("#" + n.parent)),
                                                parentMember = p.data;
                                            if (parentMember && typeof parentMember.MemberCollection === "function") {
                                                var child = _(parentMember.MemberCollection()).find(function (v) {
                                                    return ko.unwrap(v.WebId) === ko.unwrap(n.data.WebId);
                                                });
                                                parentMember.MemberCollection.remove(child);
                                            } else {
                                                var child2 = _(entity.MemberCollection()).find(function (v) {
                                                    return v.WebId() === n.data.WebId();
                                                });
                                                entity.MemberCollection.remove(child2);
                                            }

                                            if (!sel.length) {
                                                return false;
                                            }
                                            ref.delete_node(sel);

                                            return true;

                                        }
                                    }
                                ]
                            },
                            "types": {

                                "System.String, mscorlib": {
                                    "icon": "glyphicon glyphicon-bold",
                                    "valid_children": []
                                },
                                "System.DateTime, mscorlib": {
                                    "icon": "glyphicon glyphicon-calendar",
                                    "valid_children": []
                                },
                                "System.DateTimeOffset, mscorlib": {
                                    "icon": "fa fa-hourglass-o",
                                    "valid_children": []
                                },
                                "System.TimeSpan, mscorlib": {
                                    "icon": "fa fa-clock-o",
                                    "valid_children": []
                                },
                                "System.Int16, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Int32, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Int64, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Byte, mscorlib": {
                                    "icon": "fa fa-sort-numeric-asc",
                                    "valid_children": []
                                },
                                "System.Byte[], mscorlib": {
                                    "icon": "fa fa-picture-o",
                                    "valid_children": []
                                },
                                "System.Decimal, mscorlib": {
                                    "icon": "glyphicon glyphicon-usd",
                                    "valid_children": []
                                },
                                "System.Double, mscorlib": {
                                    "icon": "fa fa-gg",
                                    "valid_children": []
                                },
                                "System.Guid, mscorlib": {
                                    "icon": "fa fa-glide",
                                    "valid_children": []
                                },
                                "System.Single, mscorlib": {
                                    "icon": "fa fa-eur",
                                    "valid_children": []
                                },
                                "System.Boolean, mscorlib": {
                                    "icon": "glyphicon glyphicon-ok",
                                    "valid_children": []
                                },
                                "System.Object, mscorlib": {
                                    "icon": "fa fa-building-o"
                                },
                                "ComplexMember": {
                                    "icon": "glyphicon glyphicon-list"
                                },
                                "System.Xml.XmlDocument, System.Xml": {
                                    "icon": "fa fa-code"
                                },
                                "Bespoke.Sph.Domain.ComplexMember, domain.sph": {
                                    "icon": "glyphicon glyphicon-list"
                                }
                            },
                            "plugins": ["contextmenu", "types"]
                        });
                };
            loadJsTree();

            var to = false;
            $(searchbox).keyup(function () {
                if (to) {
                    clearTimeout(to);
                }
                to = setTimeout(function () {
                    var v = $(searchbox).val();
                    $(element).jstree(true).search(v);
                }, 250);
            });

        }
    };

});