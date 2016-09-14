///<reference path="~/Scripts/underscore.js"/>
///<reference path="~/Scripts/require.js"/>
///<reference path="~/Scripts/knockout-3.4.0.debug.js"/>
///<reference path="~/Scripts/jquery-2.2.0.intellisense.js"/>
///<reference path="../../web/core.sph/SphApp/schemas/form.designer.g.js"/>
/**
 * @param {{Id,TableDefinitionCollection:function, ControllerActionCollection:function, OperationDefinitionCollection:function,ColumnCollection:function,ChildRelationCollection,Table, Schema, Name}} adapter
 * @param{{create_node:function, get_node:function, jstree:function, rename_node, set_type: function, delete_node:function, get_type:function, get_selected:function, set_selected:function}} jstree
 * @param{{system:string, searchTextBox:string}} objectbuilders
 * @param{{RequestMemberCollection:function,ResponseMemberCollection:function, Uuid:string , IsSelected : function, Order: function}} OperationDefinition
 * @param {{ entity: object, MemberCollection:function}} entity
 * @param {{ debounce:function}} _
 */

/*globals define, console */
define(["knockout", "objectbuilders", "underscore"], function (ko, objectbuilders, _) {
    "use strict";
    ko.bindingHandlers.restApiAdapterTree = {
        init: function (element, valueAccessor) {

            var system = require(objectbuilders.system),
                app = require(objectbuilders.app),
                value = valueAccessor(),
                adapter = value.adapter,
                connected = value.connected || true,
                searchInput = $(ko.unwrap(value.searchTextBox)),
                addOperation = value.addOperation,
                member = value.selected,
                jsTreeData = [{
                    id: "table-node",
                    text: "Resources",
                    type: "table-node",
                    state: {
                        opened: true
                    },
                    children: []
                },
                    {
                        "id": "node-operations",
                        text: "Endpoints",
                        icon: "fa fa-cogs",
                        state: {
                            opened: true
                        }
                    }],
                computeNodeText = function (mbr) {

                    const field = ko.toJS(mbr),
                        displayName = field.DisplayName || "",
                        bracket = displayName ? " [" : "",
                        bracket2 = displayName ? "]" : "";

                    return field.Name + bracket + displayName + bracket2;


                },
                disposeSubscriptions = function (...subs) {
                    subs.forEach(v => {
                        if (v) {
                            v.dispose();
                            v = null;
                        }
                    });
                },
                nameSubscription = null,
                typeNameSubscription = null,
                mapTable = function (v) {
                    const table = ko.toJS(v),
                        columns = _(v.ColumnCollection()).map(function (col) {
                            return {
                                id: `column-${table.Name}-${ko.unwrap(col.WebId)}`,
                                text: computeNodeText(col),
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

                    const createNode = function (t) {
                        const complex = ko.unwrap(t.$type) === "Bespoke.Sph.Domain.ComplexMember, domain.sph",
                            allowMultiple = ko.unwrap(t.AllowMultiple),
                            icon = complex ? (allowMultiple ? "array" : "object") : ko.unwrap(t.TypeName),
                            members = _(t.MemberCollection()).map(createNode),
                            webid = ko.unwrap(t.WebId) || system.guid();

                        if (ko.isObservable(t.WebId))
                            t.WebId(webid);

                        return {
                            id: webid,
                            text: complex ? t.Name() : computeNodeText(t),
                            type: icon,
                            data: t,
                            children: members
                        };
                    },
                        requests = v.RequestMemberCollection().map(createNode),
                        responses = v.ResponseMemberCollection().map(createNode);
                    return {
                        id: `operation-${ko.unwrap(v.WebId)}`,
                        text: `${ko.unwrap(v.Name)}`,
                        state: "open",
                        type: ko.unwrap(v.HttpMethod),
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
                    jsTreeData[0].children = adapter().TableDefinitionCollection().map(mapTable);
                    jsTreeData[1].children = adapter().OperationDefinitionCollection().map(mapOperation);

                    $(element)
                        .on("select_node.jstree", function (node, selected) {

                            if (typeof member !== "function") {
                                return;
                            }
                            disposeSubscriptions(nameSubscription, typeNameSubscription);

                            const $node = selected.node,
                                 field = $node.data,
                                 ref = $(element).jstree(true),
                                 $index = $node.original.$index;
                            if (field) {
                                member(field);

                                const nodeTextChanged = function () {
                                    const text = computeNodeText(field, $index);
                                    $(`#${$node.id}`).find(`>a.jstree-anchor>i.column-icon`).remove();
                                    ref.rename_node($node, text);
                                };
                                nameSubscription = field.Name.subscribe(nodeTextChanged);
                                // type
                                if (ko.isObservable(field.TypeName)) {
                                    typeNameSubscription = field.TypeName.subscribe(t => ref.set_type($node, t));
                                }
                            }

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
                                    const ref = tree,
                                        removeMenu = {
                                            label: "Remove",
                                            action: function () {
                                                tree.delete_node($node);

                                                // TODO : remove

                                                return true;

                                            }
                                        },
                                        data = $node.data || {},
                                        dataJs = ko.toJS(data);
                                    if (dataJs.Name === "Headers") {
                                        return [
                                        {
                                            label: "Add header",
                                            action: function () {
                                                const child = new bespoke.sph.domain.Adapters.HttpHeaderMember({ WebId: system.guid(), TypeName: "System.String, mscorlib", Name: "Member_Name", FullName: "querystringKey" }),
                                                        parent = $(element).jstree("get_selected", true),
                                                        mb = parent[0].data,
                                                        newNode = { state: "open", type: "System.String, mscorlib", text: "Member_Name", data: child };


                                                const nn = ref.create_node($node, newNode);
                                                mb.MemberCollection.push(child);

                                                ref.deselect_node([parent]);
                                                ref.select_node(nn);

                                                return true;

                                            }
                                        }];
                                    }
                                    if (dataJs.Name === "QueryStrings") {
                                        return [
                                        {
                                            label: "Add query string",
                                            action: function () {
                                                const child = new bespoke.sph.domain.Adapters.QueryStringMember({ WebId: system.guid(), TypeName: "System.String, mscorlib", Name: "Member_Name", FullName: "querystringKey" }),
                                                        parent = $(element).jstree("get_selected", true),
                                                        mb = parent[0].data,
                                                        newNode = { state: "open", type: "System.String, mscorlib", text: "Member_Name", data: child };


                                                const nn = ref.create_node($node, newNode);
                                                mb.MemberCollection.push(child);

                                                ref.deselect_node([parent]);
                                                ref.select_node(nn);

                                                return true;

                                            }
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
                                    if ($node.id.startsWith("operation-")) {
                                        return [{
                                            label: "Remove",
                                            action: function() {
                                                app.showMessage("Are you sure you want to remove this endpoint", "RX Developers", ["Yes", "No"])
                                                    .done(function(dialogResult) {
                                                        if (dialogResult === "Yes") {
                                                            adapter().OperationDefinitionCollection.remove(data);
                                                            // remove the node
                                                            ref.delete_node($node);
                                                        }
                                                    });
                                            }
                                        }];
                                    }

                                    if (dataJs.$type === "Bespoke.Sph.Integrations.Adapters.QueryStringMember, restapi.adapter") {
                                        return [removeMenu];
                                    }
                                    if (dataJs.$type === "Bespoke.Sph.Integrations.Adapters.HttpHeaderMember, restapi.adapter") {
                                        return [removeMenu];
                                    }
                                    return [];

                                }
                            },
                            "types": {

                                "OPTIONS": {
                                    "icon": "fa fa-gear"
                                },
                                "PATCH": {
                                    "icon": "fa fa-pencil-square-o"
                                },
                                "HEAD": {
                                    "icon": "fa fa-use"
                                },
                                "DELETE": {
                                    "icon": "fa fa-minus-circle"
                                },
                                "PUT": {
                                    "icon": "fa fa-plus-circle"
                                },
                                "POST": {
                                    "icon": "fa fa-plus"
                                },
                                "GET": {
                                    "icon": "fa fa-get-pocket"
                                },
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
                                    "icon": "fa fa-check",
                                    "valid_children": []
                                },
                                "object": {
                                    "icon": "fa fa-object-group"
                                },
                                "array": {
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


    ko.bindingHandlers.restApiOperationSchemaTree = {
        init: function (element, valueAccessor) {
            var system = require(objectbuilders.system),
                value = valueAccessor(),
                operation = ko.unwrap(value.operation),
                name = ko.unwrap(value.name),
                member = value.selected,
                createNode = function (v) {

                    var icon = ko.unwrap(v.$type) === "Bespoke.Sph.Domain.ComplexMember, domain.sph" ? "Bespoke.Sph.Domain.ComplexMember, domain.sph" : ko.unwrap(v.TypeName);
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
                                    var $item = $node.data,
                                        addResultSet = {
                                            label: "Add result set",
                                            action: function () {
                                                var text = `${name}Result1`,
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
                                        addRecord =
                                        {
                                            label: "Add record",
                                            action: function () {
                                                var child = {
                                                    $type: "Bespoke.Sph.Integrations.Adapters.SprocResultMember, sqlserver.adapter",
                                                    WebId: system.guid(),
                                                    TypeName: ko.observable("System.String, mscorlib"),
                                                    FieldName: ko.observable("Member_Name"),
                                                    Name: ko.observable(""),
                                                    DisplayName: ko.observable(""),
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
                                        removeMember =
                                        {
                                            label: "Remove",
                                            action: function () {
                                                var ref = $(element).jstree(true),
                                                    sel = ref.get_selected();

                                                // now delete the member
                                                var n = ref.get_selected(true)[0],
                                                    p = ref.get_node($(`#${n.parent}`)),
                                                    parentMember = p.data;
                                                if (parentMember && typeof parentMember.MemberCollection === "function") {
                                                    var child = _(parentMember.MemberCollection()).find(v=>ko.unwrap(v.WebId) === ko.unwrap(n.data.WebId));
                                                    parentMember.MemberCollection.remove(child);
                                                } else {
                                                    var child2 = _(entity.MemberCollection()).find(v => v.WebId() === n.data.WebId());
                                                    entity.MemberCollection.remove(child2);
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
