﻿///<reference path="~/Scripts/underscore.js"/>
///<reference path="~/Scripts/require.js"/>
///<reference path="~/Scripts/knockout-3.4.0.debug.js"/>
///<reference path="~/Scripts/jquery-2.2.0.intellisense.js"/>
///<reference path="../../web/core.sph/SphApp/schemas/form.designer.g.js"/>
/// <reference path="../schemas/adapter.restapi.operation.js" />
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

            let tree = null,
                nullableSubscription = null,
                allowMultipleSubscription = null,
                nameSubscription = null,
                typeNameSubscription = null;
            const system = require(objectbuilders.system),
                app = require(objectbuilders.app),
                value = valueAccessor(),
                adapter = value.adapter,
                connected = value.connected || true,
                searchInput = $(ko.unwrap(value.searchTextBox)),
                addOperation = value.addOperation,
                member = value.selected,
                crumbs = value.crumbs || ko.observalbe(),
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
                        multiple = field.AllowMultiple ? "<i class='fa fa-ellipsis-h column-icon' style='margin-right:5px;color:darkorange' title='Allow multiple'></i> " : "",
                        nullable = field.IsNullable ? "<i class='fa fa-question column-icon' style='margin-right:5px;color:green' title='Nullalbe'></i> " : "",
                        extraQueryString = field.$type === "Bespoke.Sph.Integrations.Adapters.QueryStringMember, restapi.adapter" ? " <i class='fa fa-info-circle column-icon' style='margin-left:5px;color:orange' title='Extra query string parameter added by user'></i>" : "",
                        extraHeader = field.$type === "Bespoke.Sph.Integrations.Adapters.HttpHeaderMember, restapi.adapter" ? " <i class='fa fa-plus-circle column-icon' style='margin-left:5px;color:orange' title='Extra header added by user'></i>" : "",
                        displayName = field.FullName || "",
                        bracket = displayName ? " [" : "",
                        bracket2 = displayName ? "]" : "";

                    return multiple + nullable + field.Name + bracket + displayName + bracket2 + extraHeader + extraQueryString;


                },
                disposeSubscriptions = function (...subs) {
                    subs.forEach(v => {
                        if (v) {
                            v.dispose();
                            v = null;
                        }
                    });
                },
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
                            icon = complex ? "object" : ko.unwrap(t.TypeName),
                            members = _(t.MemberCollection()).map(createNode),
                            webid = ko.unwrap(t.WebId) || system.guid();

                        if (ko.isObservable(t.WebId))
                            t.WebId(webid);

                        return {
                            id: webid,
                            text: computeNodeText(t),
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
                loadJsTree = function () {
                    jsTreeData[0].children = adapter().TableDefinitionCollection().map(mapTable);
                    jsTreeData[1].children = adapter().OperationDefinitionCollection().map(mapOperation);

                    $(element)
                        .on("select_node.jstree", function (node, selected) {

                            if (typeof member !== "function") {
                                return;
                            }
                            disposeSubscriptions(nameSubscription, typeNameSubscription, nullableSubscription, allowMultipleSubscription);

                            const $node = selected.node,
                                 field = $node.data,
                                 ref = $(element).jstree(true),
                                 $index = $node.original.$index;
                            const crumbs1 = selected.node.parents
                                .reverse()
                                .map(x =>ref.get_node(x))
                                .filter(x => typeof x.text === "string")
                                .map(x => x.text)
                                .reduce((trails, cur) => `${trails} &gt; ${cur}`) + ` &gt; ${$node.text}`;
                            crumbs(crumbs1);
                            if ($node.type === "default") {
                                return;
                            }
                            if (field && ko.isObservable(field.Name)) {
                                member(field);

                                const nodeTextChanged = function () {
                                    const text = computeNodeText(field, $index);
                                    $(`#${$node.id}`).find(`>a.jstree-anchor>i.column-icon`).remove();
                                    ref.rename_node($node, text);
                                };
                                nameSubscription = field.Name.subscribe(nodeTextChanged);
                                // nullable
                                if (ko.isObservable(field.IsNullable)) {
                                    nullableSubscription = field.IsNullable.subscribe(nodeTextChanged);
                                }
                                // type
                                if (ko.isObservable(field.TypeName)) {
                                    typeNameSubscription = field.TypeName.subscribe(t => ref.set_type($node, t));
                                }
                                // allow multiple
                                if (ko.isObservable(field.AllowMultiple)) {
                                    allowMultipleSubscription = field.AllowMultiple.subscribe(nodeTextChanged);
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
                                        console.log(`Dragged into target '${target.text}', id:'${target.id}', type : '${target.type}'`);
                                        const bodyParent = target.text === "Body" || target.type === "array";
                                        if (!bodyParent) return false;
                                        if (target.id !== node.parent) return false;


                                        return true;
                                    }
                                    return true;
                                },
                                "themes": { "stripes": true },
                                'data': jsTreeData
                            },
                            "dnd": {
                                "is_draggable": function (node) {
                                    if (!node.data.$type) {
                                        return false;
                                    }
                                    if (node.data.$type === "Bespoke.Sph.Domain.SimpleMember, domain.sph") {
                                        return true;
                                    }
                                    return node.id.startsWith("column-");
                                }
                            },
                            "contextmenu": {
                                "items": function ($node) {
                                    console.log($node);
                                    const ref = tree,
                                        simpleMenu = {
                                            label: "Add Simple Child",
                                            action: function () {
                                                const child = new bespoke.sph.domain.SimpleMember({ WebId: system.guid(), TypeName: "System.String, mscorlib", Name: "Member_Name", FullName: "" }),
                                                    parent = $(element).jstree("get_selected", true),
                                                    mb = parent[0].data,
                                                    newNode = { state: "open", type: "System.String, mscorlib", text: "Member_Name", data: child };

                                                child.FullName = ko.observable("");
                                                const nn = ref.create_node($node, newNode);
                                                mb.MemberCollection.push(child);

                                                ref.deselect_node([parent]);
                                                ref.select_node(nn);

                                                return true;


                                            }
                                        },
                                    complexChildMenu = {
                                        label: "Add Complex Child",
                                        action: function () {
                                            const child = new bespoke.sph.domain.ComplexMember({ WebId: system.guid(), Name: "Member_Name" }),
                                                parent = $(element).jstree("get_selected", true),
                                                mb = parent[0].data,
                                                newNode = { state: "open", type: "object", text: "Member_Name", data: child };

                                            const nn = ref.create_node($node, newNode);
                                            mb.MemberCollection.push(child);

                                            ref.deselect_node([parent]);
                                            ref.select_node(nn);

                                            return true;


                                        }
                                    },
                                        removeMenu = {
                                            label: "Remove",
                                            action: function () {

                                                const n = ref.get_selected(true)[0],
                                                    p = ref.get_node($(`#${n.parent}`)),
                                                    parentMember = p.data;
                                                if (parentMember && typeof parentMember.MemberCollection === "function") {
                                                    const child = _(parentMember.MemberCollection()).find(function (v) {
                                                        return v.WebId() === n.data.WebId();
                                                    });
                                                    parentMember.MemberCollection.remove(child);
                                                }
                                                ref.delete_node($node);
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
                                                const parent = $(element).jstree("get_selected", true),
                                                      mb = parent[0].data, count = mb.MemberCollection().length,
                                                      child = new bespoke.sph.domain.Adapters.HttpHeaderMember({ WebId: system.guid(), TypeName: "System.String, mscorlib", Name: `Header${count + 1}`, FullName: "" }),
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
                                    if (dataJs.Name === "RouteParameters") {
                                        return [
                                        {
                                            label: "Add route parameter",
                                            action: function () {
                                                const child = new bespoke.sph.domain.Adapters.RouteParameterMember({ WebId: system.guid(), TypeName: "System.String, mscorlib", Name: "Member_Name", FullName: "querystringKey" }),
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
                                            label: "Add new REST endpoint",
                                            action: addOperation,
                                            "icon": "fa fa-cogs",
                                            _disabled: !ko.unwrap(connected)
                                        }];
                                    }
                                    if ($node.id.startsWith("operation-")) {
                                        return [{
                                            label: "Remove",
                                            action: function () {
                                                app.showMessage("Are you sure you want to remove this endpoint", "RX Developers", ["Yes", "No"])
                                                    .done(function (dialogResult) {
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
                                    if (dataJs.$type === "Bespoke.Sph.Domain.SimpleMember, domain.sph") {
                                        return [removeMenu];
                                    }

                                    if (dataJs.Name === "Body") {
                                        return [simpleMenu, complexChildMenu];
                                    }
                                    if (dataJs.$type === "Bespoke.Sph.Domain.ComplexMember, domain.sph") {
                                        return [simpleMenu, complexChildMenu, removeMenu];
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
                const  tables = _(changes).filter(function (c) {
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
                const operations = _(changes).filter(function (c) {
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

            const clearSearch = $(`<a class="pull-right" id="clear-search" href="javascript:;" style="margin-top: -27px;margin-right: 8px" title="Clear search text">
                <i class="fa fa-times" style="color: grey"></i>
                </a>`),
                search = _.debounce(function () {
                    const text = searchInput.val();
                    if (!text) {
                        $(element).jstree("clear_search");
                        return;
                    }
                    const f = `""${text}""`;
                    console.log(f);
                    $(element).jstree("search", f);
                }, 400);
            clearSearch.click(function () {
                $(element).jstree("clear_search");
                searchInput.val("");
            });
            searchInput.after(clearSearch);
            searchInput.keyup(search);

        }
    };

});
