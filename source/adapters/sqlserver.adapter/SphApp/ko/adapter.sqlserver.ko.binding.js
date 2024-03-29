﻿///<reference path="../../web/core.sph/Scripts/jstree.min.js"/>
///<reference path="../../web/core.sph/Scripts/require.js"/>
///<reference path="../../web/core.sph/Scripts/underscore.js"/>
///<reference path="../../web/core.sph/SphApp/objectbuilders.js"/>
///<reference path="../../web/core.sph/SphApp/schemas/form.designer.g.js"/>
/**
 * @param {{Id,TableDefinitionCollection:function, ControllerActionCollection:function, OperationDefinitionCollection:function,ColumnCollection:function,ChildRelationCollection,Table, Schema, Name}} adapter
 * @param{{create_node:function, get_node:function, jstree:function, rename_node, set_type: function, delete_node:function, get_type:function, get_selected:function, set_selected:function}} jstree
 * @param{{system:string, searchTextBox:string}} objectbuilders
 * @param{{RequestMemberCollection:function,ResponseMemberCollection:function, Uuid:string , IsSelected : function, Order: function}} OperationDefinition
 * @param{{LookupColumnTable:function, IsEnabled, IsComplex, Ignore, IsComputed, IsPrimaryKey:boolean, Unsupported:boolean}} column
 * @param {{ entity: object, MemberCollection:function}} entity
 * @param {{ debounce:function}} _
 */

/*globals define, console */
define(["knockout", "objectbuilders", "underscore"], function (ko, objectbuilders, _) {
    "use strict";
    ko.bindingHandlers.adapterTree = {
        init: function (element, valueAccessor) {


            var system = require(objectbuilders.system),
                value = valueAccessor(),
                adapter = value.adapter,
                connected = value.connected,
                searchInput = $(ko.unwrap(value.searchTextBox)),
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
                        lookup = column.LookupColumnTable.IsEnabled ? " <i class='fa fa-binoculars column-icon' style='margin-left:5px;color:darkgreen' title='Lookup: The value is stored in another table'></i>" : "",
                        complex = column.IsComplex ? " <i class='fa fa-link column-icon' style='margin-left:5px;color:darkblue' title='Complex: creates a link to the column value'></i>" : "",
                        ignore = column.Ignore ? " <i class='fa fa-eye-slash column-icon' style='margin-left:5px;color:grey' title='Ignored : will not be read in select'></i>" : "",
                        readonly = column.IsComputed ? " <i class='fa fa-info-circle column-icon' style='margin-left:5px;color:#0000ee' title='Computed readonly column'></i>" : "",
                        primaryKey = column.IsPrimaryKey ? "<i class='icon-key column-icon' style='margin-right:3px;color:#ff8c00;font-weight: bold' title='Primary Key'></i> " : "",
                        unsupported = column.Unsupported ? "<i class='fa fa-exclamation-triangle column-icon' style='margin-right:3px;color:#ff0028;font-weight: bold' title='The column type is not supported'></i> " : "",
                        displayName = column.DisplayName || "",
                        bracket = displayName ? " [" : "",
                        bracket2 = displayName ? "]" : "";

                    return primaryKey + unsupported + column.Name + readonly + bracket + displayName + bracket2 + complex + lookup + ignore;


                },
                mapTable = function (v) {
                    var table = ko.toJS(v),
                        columns = _(v.ColumnCollection()).map(function (col) {
                            return {
                                id: `column-${table.Name}-${ko.unwrap(col.WebId)}`,
                                text: calculateColumnName(col),
                                type: ko.unwrap(col.TypeName),
                                data: col
                            };
                        }),
                        relations = _(v.ChildRelationCollection()).map(function (col) {
                            return {
                                text: ko.unwrap(col.Table),
                                type: ko.unwrap(col.IsSelected) ? "child-table-selected" : "child-table",
                                data: col
                            };
                        }),
                        actions = _(v.ControllerActionCollection()).map(function (act) {
                            act.guid = `action-${system.guid()}`;
                            return {
                                id: act.guid,
                                text: ko.unwrap(act.Name),
                                type: `api-action-${(ko.unwrap(act.IsEnabled) ? "enabled" : "disabled")}`,
                                data: act
                            };
                        });

                    return {
                        id: `table-${table.Schema}-${table.Name}`,
                        text: `${table.Schema}.${table.Name}`,
                        state: "open",
                        type: table.Type,
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

                    var createNode = function (t, isResponse) {
                        var isResultSet = ko.unwrap(t.$type) === "Bespoke.Sph.Domain.ComplexMember, domain.sph",
                            icon = isResultSet ? "Bespoke.Sph.Domain.ComplexMember, domain.sph" : ko.unwrap(t.TypeName),
                            members = _(t.MemberCollection()).map(function (x) {
                                return {
                                    id: `parameter-${ko.unwrap(x.WebId)}`,
                                    text: calculateColumnName(x),
                                    type: x.TypeName(),
                                    data: x
                                };
                            }),
                            webid = ko.unwrap(t.WebId);


                        if (ko.isObservable(t.WebId))
                            t.WebId(webid);

                        return {
                            id: (isResponse ? "parameter-" : "") + webid,
                            text: isResultSet ? t.Name() : calculateColumnName(t),
                            type: icon,
                            data: t,
                            children: members
                        };
                    },
                        requests = _(v.RequestMemberCollection()).map(createNode),
                        responses = _(v.ResponseMemberCollection()).map(function (vt) {
                            return createNode(vt, true);
                        });
                    return {
                        id: `operation-${ko.unwrap(v.Uuid)}`,
                        text: `${ko.unwrap(v.Schema)}.${ko.unwrap(v.Name)}`,
                        state: "open",
                        type: ko.unwrap(v.ObjectType),
                        data: v,
                        children: [{
                            text: "Request",
                            type: "request",
                            state: {
                                opened: true
                            },
                            children: requests
                        }, {
                            text: "Response",
                            type: "response",
                            state: {
                                opened: true
                            },
                            children: responses
                        }]
                    };
                },
                tree = null,
                loadJsTree = function () {
                    jsTreeData[0].children = _(adapter().TableDefinitionCollection()).map(mapTable);
                    jsTreeData[1].children = _(adapter().OperationDefinitionCollection()).map(mapOperation);

                    $(element)
                        .on("select_node.jstree", function (node, selected) {
                            member(selected.node.data);
                        })

                        .on("move_node.jstree", function (e, data) {
                            var ref = $(element).jstree(true),
                                column = data.node.data,
                                parent = ref.get_node(data.parent),
                                collection = parent.data.ColumnCollection || parent.data.MemberCollection,
                                order = 1;

                            collection.remove(function (c) {
                                return ko.unwrap(column.WebId) === ko.unwrap(c.WebId);
                            });
                            collection.splice(data.position, 0, column);
                            _(ko.unwrap(collection)).each(function (c) {
                                if (ko.isObservable(c.Order)) {
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
                                            target = tree.get_node(nodeParent);
                                        if (!column) {
                                            return false;
                                        }
                                        if (!target) {
                                            return false;
                                        }
                                        var columnsParent = target.text === "Columns" || target.type === "Bespoke.Sph.Domain.ComplexMember, domain.sph";
                                        if (!columnsParent) return false;
                                        if (target.id !== node.parent) return false;

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
                                    console.log($node);
                                    var ref = tree,
                                        removeMenu = {
                                            label: "Remove",
                                            action: function () {
                                                tree.delete_node($node);

                                                if ($node.type === "U") {
                                                    adapter().TableDefinitionCollection.remove(function (v) {
                                                        return ko.unwrap(v.Schema) === ko.unwrap($node.data.Schema) && ko.unwrap(v.Name) === ko.unwrap($node.data.Name);
                                                    });
                                                } else {
                                                    adapter().OperationDefinitionCollection.remove(function (v) {
                                                        return ko.unwrap(v.Schema) === ko.unwrap($node.data.Schema) && ko.unwrap(v.Name) === ko.unwrap($node.data.Name);
                                                    });
                                                }

                                                return true;

                                            }
                                        },
                                        editOperation = {
                                            "label": "Edit operation",
                                            _disabled: !ko.unwrap(connected),
                                            "action": function () {
                                                var op = ko.toJS($node.data);
                                                window.location = `/sph#adapter.sqlserver.sproc/${ko.unwrap(adapter().Id)}/${op.Schema}.${op.Name}`;
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

                                            $(`#${$node.id}`).find(`>a.jstree-anchor>i.column-icon`).remove();

                                            tree.rename_node($node, text);
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
                                            },
                                            _disabled: !ko.unwrap(connected)
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
                                                tree.set_type($node, "api-action-enabled");
                                            }
                                        },
                                        disableActionMenu = {
                                            label: "Disable",
                                            action: function () {
                                                $node.data.IsEnabled(false);
                                                tree.set_type($node, "api-action-disabled");
                                            }
                                        };
                                    var data = $node.data;

                                    if ($node.id === "table-node" && addTable) {
                                        return [{
                                            label: "Add new table/view",
                                            action: addTable,
                                            _disabled: !ko.unwrap(connected)
                                        }];
                                    }

                                    if ($node.id === "node-operations" && addOperation) {
                                        return [{
                                            label: "Add new sproc/function",
                                            action: addOperation,
                                            "icon": "fa fa-cogs",
                                            _disabled: !ko.unwrap(connected)
                                        }];
                                    }


                                    if ($node.id.startsWith("column-")) {
                                        let items = [];

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
                                    if ($node.id.startsWith("parameter-")) {
                                        let items = [];

                                        if (ko.unwrap(data.Ignore))
                                            items.push(includeMenu);
                                        else
                                            items.push(ignoreMenu);

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
                                    if ($node.type === "U")
                                        return [removeMenu];
                                    if ($node.type === "V")
                                        return [removeMenu];
                                    if ($node.id.startsWith("operation-"))
                                        return [editOperation, removeMenu];
                                    if ($node.type === "api-actions")
                                        return [{
                                            label: "Enable all endpoints", action: function () {

                                                _($node.children).each(function (v) {
                                                    var cn = ref.get_node(v),
                                                        ep = cn.data;
                                                    ep.IsEnabled(true);
                                                    tree.set_type(cn, "api-action-enabled");
                                                });
                                            }
                                        },
                                            {
                                                label: "Disable all endpoints",
                                                action: function () {

                                                    _($node.children).each(function (v) {
                                                        var cn = ref.get_node(v),
                                                            ep = cn.data;
                                                        ep.IsEnabled(false);
                                                        tree.set_type(cn, "api-action-disabled");
                                                    });
                                                }
                                            }];
                                    return [];

                                }
                            },
                            "types": {

                                "api-action-enabled": {
                                    "icon": "fa fa-check-square-o"
                                },
                                "request": {
                                    "icon": "fa fa-envelope-o"
                                },
                                "response": {
                                    "icon": "fa fa-envelope"
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
                                "U": {
                                    "icon": "fa fa-list"
                                },
                                "V": {
                                    "icon": "fa fa-bars"
                                },
                                "related-table": {
                                    "icon": "fa fa-list-alt"
                                },
                                "child-table": {
                                    "icon": "fa fa-square-o"
                                },
                                "child-table-selected": {
                                    "icon": "fa fa-check-square-o"
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
                                "SqlScript": {
                                    "icon": "bowtie-icon bowtie-file-type-sql"
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
                            "search": {
                                "case_sensitive": false,
                                "show_only_matches": true,
                                "show_only_matches_children": true,
                                "search_callback": function (text, node) {
                                    return (node.text.indexOf(text) > -1);
                                }
                            },
                            "plugins": ["contextmenu", "types", "dnd", "search"]
                        });
                };


            loadJsTree();
            tree = $(element).jstree(true);


            adapter.subscribe(function () {

                tree.destroy();
                loadJsTree();
                tree = $(element).jstree(true);
            });


            adapter().TableDefinitionCollection.subscribe(function (changes) {
                console.log(changes);
                var tables = _(changes).filter(function (c) {
                    return c.status === "added";
                }),
                    children = _(tables).map(function (c) {
                        return mapTable(c.value);
                    });
                var sel = tree.get_node("table-node");

                sel = sel.length ? sel[0] : sel;
                _(children).each(function (tableNode) {
                    sel = tree.create_node(sel, tableNode);
                });

            }, null, "arrayChange");

            adapter().OperationDefinitionCollection.subscribe(function (changes) {
                console.log(changes);
                var operations = _(changes).filter(function (c) {
                    return c.status === "added";
                }),
                    children = _(operations).map(function (c) {
                        return mapOperation(c.value);
                    });
                var sel = tree.get_node("node-operations");

                sel = sel.length ? sel[0] : sel;
                _(children).each(function (opNode) {
                    sel = tree.create_node(sel, opNode);
                });

            }, null, "arrayChange");
            $(element).on("click", "i.fa-square-o, i.fa-check-square-o", function () {
                var id = $(this).parents("li").attr("id"),
                    node = tree.get_node(id);
                if (!node) return;

                var action = node.data;

                if (node.id.startsWith("action-")) {
                    if (ko.unwrap(action.IsEnabled)) {
                        action.IsEnabled(false);
                        tree.set_type(node, "api-action-disabled");

                    } else {

                        action.IsEnabled(true);
                        tree.set_type(node, "api-action-enabled");
                    }
                }

                // child-table
                if (node.type.startsWith("child-table")) {
                    if (ko.unwrap(action.IsSelected)) {
                        action.IsSelected(false);
                        tree.set_type(node, "child-table");

                    } else {

                        action.IsSelected(true);
                        tree.set_type(node, "child-table-selected");
                    }
                }
            });

            var clearSearch = $(`<a class="pull-right" id="clear-search" href="javascript:;" style="margin-top: -27px;margin-right: 8px" title="Clear search text">
                <i class="fa fa-times" style="color: grey"></i>
                </a>`),
                search = _.debounce(function () {
                    var text = $(this).val();

                    if (!text) {
                        $(element).jstree("clear_search");
                    } else {
                        var f = `""${text}""`;
                        console.log(f);
                        $(element).jstree("search", f);
                    }
                }, 400);
            clearSearch.click(function () {
                $(element).jstree("clear_search");
                searchInput.val("");
            });
            searchInput.after(clearSearch);
            searchInput.keyup(search);

        }
    };


    ko.bindingHandlers.operationSchemaTree = {
        init: function (element, valueAccessor) {
            var system = require(objectbuilders.system),
                value = valueAccessor(),
                operation = ko.unwrap(value.operation),
                name = ko.unwrap(value.name),
                member = value.selected,
                createNode = function (v) {

                    const icon = ko.unwrap(v.$type) === "Bespoke.Sph.Domain.ComplexMember, domain.sph" ? "Bespoke.Sph.Domain.ComplexMember, domain.sph" : ko.unwrap(v.TypeName);
                    return {
                        text: v.Name(),
                        state: "open",
                        type: icon,
                        data: v
                    };
                },
                jsTreeData = [{
                    text: "Request",
                    type: "request",
                    state: {
                        opened: true
                    }
                }, {
                    text: "Response",
                    type: "response",
                    state: {
                        opened: true
                    }
                }],
                recurseChildMember = function (node) {
                    node.children = _(node.data.MemberCollection()).map(function (v) {
                        return {
                            text: typeof v.FieldName === "function" ? v.FieldName() : v.Name(),
                            state: "open",
                            type: v.TypeName(),
                            data: v
                        };
                    });
                    _(node.children).each(recurseChildMember);
                },
                loadJsTree = function () {
                    jsTreeData[0].children = _(operation.RequestMemberCollection()).map(createNode);
                    jsTreeData[1].children = _(operation.ResponseMemberCollection()).map(createNode);
                    _(jsTreeData[1].children).each(recurseChildMember);
                    $(element)
                        .on("select_node.jstree", function (node, selected) {
                            var tree = $(element).jstree(true);
                            if (selected.node.data && typeof selected.node.data.Name === "function") {
                                member(selected.node.data);

                                if (typeof member().FieldName === "function") {
                                    member().FieldName.subscribe(function (name) {
                                        tree.rename_node(selected.node, name);
                                    });

                                } else {
                                    // subscribe to Name change
                                    member().Name.subscribe(function (name) {
                                        tree.rename_node(selected.node, name);
                                    });
                                    // type
                                    member().TypeName.subscribe(function (name) {
                                        tree.set_type(selected.node, name);
                                    });

                                }
                            }
                        })
                        .on("create_node.jstree", function (event, node) {
                            console.log(node, "node");
                        })
                        .on("rename_node.jstree", function (ev, node) {
                            var mb = node.node.data;
                            if (typeof mb.FieldName === "function" && ko.isObservable(mb.FieldName)) {
                                mb.FieldName(node.text);
                                if (!ko.unwrap(mb.Name)) {
                                    mb.Name(node.text.replace(/ /g, ""));
                                }
                                if (!ko.unwrap(mb.DisplayName)) {
                                    mb.DisplayName(node.text.replace(/ /g, ""));
                                }

                            } else {
                                mb.Name(node.text);
                            }
                        })
                        .jstree({
                            "core": {
                                "animation": 0,
                                "check_callback": true,
                                "themes": { "stripes": true },
                                'data': jsTreeData
                            },
                            "contextmenu": {
                                "items": function ($node) {
                                    const $item = $node.data,
                                        addResultSet = {
                                            label: "Add result set",
                                            action: function () {
                                                const text = `${name}Result1`,
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
                                                    if (mb && ko.isObservable(mb.MemberCollection)) {
                                                        mb.MemberCollection.push(child);
                                                        return true;
                                                    }
                                                    if (parent.type === "request")
                                                        operation.RequestMemberCollection.push(child);
                                                    else
                                                        operation.ResponseMemberCollection.push(child);

                                                    return true;
                                                }
                                                return false;


                                            }
                                        },
                                        addRecord =
                                        {
                                            label: "Add record",
                                            action: function () {
                                                const parent = $(element).jstree("get_selected", true),
                                                    mb = parent[0].data,
                                                    addChildRecord = function (child) {
                                                        const newNode = {
                                                            state: "open",
                                                            type: ko.unwrap(child.TypeName),
                                                            text: ko.unwrap(child.Name),
                                                            data: child
                                                        },
                                                            ref = $(element).jstree(true);

                                                        let sel = ref.get_selected();
                                                        if (!sel.length) {
                                                            return false;
                                                        }
                                                        sel = sel[0];
                                                        sel = ref.create_node(sel, newNode);
                                                        if (sel) {
                                                            ref.edit(sel);
                                                            // complex type
                                                            if (mb && ko.isObservable(mb.MemberCollection)) {
                                                                mb.MemberCollection.push(child);
                                                                return true;
                                                            }
                                                            const tag =( _.isArray(parent) && parent.length === 1) ? parent[0] : parent;
                                                            if (tag.type === "request")
                                                                operation.RequestMemberCollection.push(child);
                                                            else
                                                                operation.ResponseMemberCollection.push(child);

                                                            return true;
                                                        }
                                                        return false;
                                                    };


                                                require(["viewmodels/adapter.sqlserver.add.operation.member.dialog", "durandal/app"],
                                                    function (dialog, app2) {
                                                        dialog.adapter({});
                                                        app2.showDialog(dialog)
                                                            .done(function (result) {
                                                                if (!result) return;
                                                                if (result === "OK") {

                                                                    addChildRecord(dialog.selectedMember());
                                                                }
                                                            });
                                                    });


                                            }
                                        },
                                        removeMember =
                                        {
                                            label: "Remove",
                                            action: function () {
                                                var ref = $(element).jstree(true),
                                                    sel = ref.get_selected();

                                                // now delete the member
                                                const n = ref.get_selected(true)[0],
                                                    parent = ref.get_node($(`#${n.parent}`));
                                                if (parent.type === "request") {
                                                    const child = operation.RequestMemberCollection().find(v=>ko.unwrap(v.WebId) === ko.unwrap(n.data.WebId));
                                                    operation.RequestMemberCollection.remove(child);
                                                }
                                                if (parent.type === "response") {
                                                    const child3 = operation.ResponseMemberCollection().find(v=>ko.unwrap(v.WebId) === ko.unwrap(n.data.WebId));
                                                    operation.ResponseMemberCollection.remove(child3);
                                                }

                                                if (parent.type !== "response" && parent.type !== "request") {
                                                    const complex = parent.data;
                                                    const child2 = _(complex.MemberCollection()).find(v => v.WebId() === n.data.WebId());
                                                    complex.MemberCollection.remove(child2);
                                                }


                                                if (!sel.length) {
                                                    return false;
                                                }
                                                ref.delete_node(sel);

                                                return true;

                                            }
                                        };

                                    console.log($item);
                                    if ($node.type === "Bespoke.Sph.Domain.ComplexMember, domain.sph")
                                        return [addRecord, removeMember];
                                    if ($node.type === "response")
                                        return [addRecord, addResultSet];
                                    if ($node.type === "request")
                                        return [addRecord];
                                    if ($node.text === "@return_value")
                                        return [];

                                    return [removeMember];

                                }
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
                                },
                                "request": {
                                    "icon": "fa fa-envelope-o"
                                },
                                "response": {
                                    "icon": "fa fa-envelope"
                                }
                            },
                            "plugins": ["contextmenu", "types"]
                        });
                };
            loadJsTree();


        }
    };

});
