﻿///<reference path="../../Scripts/jstree.min.js"/>
///<reference path="../../Scripts/require.js"/>
///<reference path="../../Scripts/underscore.js"/>
///<reference path="../../SphApp/objectbuilders.js"/>
///<reference path="../../SphApp/schemas/form.designer.g.js"/>
///<reference path="../../Scripts/jquery-2.2.0.intellisense.js"/>
/**
 * @param {{Id,TableDefinitionCollection:function, ControllerActionCollection:function, OperationDefinitionCollection:function,ColumnCollection:function,ChildRelationCollection,Table, Schema, Name}} adapter
 * @param{{create_node:function, get_node:function, jstree:function, rename_node, set_type: function, delete_node:function, get_type:function, get_selected:function, set_selected:function}} jstree
 * @param{{system:string, searchTextBox:string}} objectbuilders
 * @param{{RequestMemberCollection:function,ResponseMemberCollection:function, Uuid:string , IsSelected : function, Order: function}} OperationDefinition
 * @param{{LookupColumnTable:function, IsEnabled, IsComplex, Ignore, IsComputed, IsPrimaryKey:boolean, Unsupported:boolean}} column
 * @param {{ port: object, MemberCollection:function}} port
 * @param {{ debounce:function}} _
 */

/*globals define, console */
define(["knockout", "objectbuilders", "underscore"], function (ko, objectbuilders, _) {
    "use strict";

    ko.bindingHandlers.portTree = {
        init: function (element, valueAccessor) {
            var system = require(objectbuilders.system),
                value = valueAccessor(),
                port = ko.unwrap(value.port),
                searchbox = ko.unwrap(value.searchbox),
                selectedField = value.selected,
                jsTreeData = {
                    text: port.Entity(),
                    state: {
                        opened: true,
                        selected: true
                    }
                },
                getNodeMemberType = function (v) {
                    var type = ko.unwrap(v.TypeName) || ko.unwrap(v.$type);
                    if (type.indexOf(",") < 0) {
                        type = ko.unwrap(v.$type);
                    }

                    // complex
                    if (v.FieldMappingCollection().length > 0) {
                        return "complex";
                    }

                    return type;
                },
                nullableSubscription = null,
                ignoreSubscription = null,
                nameSubscription = null,
                fieldTypeNameSubscription = null,
                recurseChildMember = function (node) {
                    node.children = _(node.data.FieldMappingCollection()).map(function (v) {

                        const type = getNodeMemberType(v);
                        return {
                            text: `${ko.unwrap(v.Name)} (${ko.unwrap(v.SampleValue)})`,
                            state: "open",
                            type: type,
                            data: v
                        };
                    });
                    _(node.children).each(recurseChildMember);
                },

                computeNodeText = function (fieldMapping) {
                    const field = ko.toJS(fieldMapping),
                        nullable = field.IsNullable
                            ? " <i class='fa fa-question column-icon' style='margin-left:5px;color:darkgreen' title='Nullable'></i>"
                            : "",
                        ignore = field.Ignore
                            ? " <i class='fa fa-eye-slash column-icon' style='margin-left:5px;color:grey' title='Ignored : will not serialized into Json object for submitting to API'></i>"
                            : "",
                        sample = field.SampleValue;

                    return `${field.Name}${nullable}${ignore} (${sample})`;
                },
                disposeSubscriptions = function(...subs) {
                    subs.forEach(v => {
                        if (v) {
                            v.dispose();
                            v = null;
                        }
                    });
                },
                loadJsTree = function () {
                    jsTreeData.children = _(port.FieldMappingCollection()).map(function (v) {

                        return {
                            text: `${ko.unwrap(v.Name)} (${ko.unwrap(v.SampleValue)})`,
                            state: "open",
                            type: getNodeMemberType(v),
                            data: v
                        };
                    });
                    _(jsTreeData.children).each(recurseChildMember);
                    $(element)
                        .on("select_node.jstree", function (node, selected) {
                            if (typeof selectedField !== "function") {
                                return;
                            }
                            disposeSubscriptions(nameSubscription, nullableSubscription, ignoreSubscription, fieldTypeNameSubscription);
                          
                            const field = selected.node.data;
                            if (field) {

                                selectedField(field);
                                // subscribe to Name change
                                const nodeTextChanged = function () {
                                    $(element).jstree(true)
                                        .rename_node(selected.node, computeNodeText(selected.node.data));
                                };
                                nameSubscription = selectedField().Name.subscribe(nodeTextChanged);
                                ignoreSubscription = selectedField().Ignore.subscribe(nodeTextChanged);
                                nullableSubscription = selectedField().IsNullable.subscribe(nodeTextChanged);
                                // type
                                if (typeof selectedField().TypeName === "function") {
                                    fieldTypeNameSubscription = selectedField().TypeName.subscribe(function (name) {
                                        $(element).jstree(true)
                                            .set_type(selected.node, name);
                                    });

                                }
                            }
                        })
                        .on("create_node.jstree", function (event, node) {
                            console.log(node, "node");
                        })
                        .jstree({
                            "core": {
                                "animation": 0,
                                "check_callback": function (operation, node, nodeParent, nodePosition, more) {
                                    if (operation === "move_node") {
                                        var mbr = node.data,
                                            ref = $(element).jstree(true),
                                            target = ref.get_node(nodeParent);
                                        if (!mbr) {
                                            return false;
                                        }
                                        if (!target) {
                                            return false;
                                        }

                                        return true;
                                    }
                                    return true;
                                },
                                "themes": { "stripes": true },
                                'data': jsTreeData
                            },
                            "contextmenu": {
                                "items": function ($node) {
                                    if ($node.type === "default") {
                                        return [];
                                    }
                                    var field = $node.data,
                                        ref = $(element).jstree(true),
                                        parents = _($node.parents).map(function (n) { return ref.get_node(n); }),
                                        sel = ref.get_selected(),
                                        setNodeText = function (col) {
                                            const text = computeNodeText(col);
                                            $(`#${$node.id}`).find(`>a.jstree-anchor>i.column-icon`).remove();
                                            ref.rename_node($node, text);
                                        };

                                    const ignore = {
                                        label: "Ignore",
                                        action: function () {
                                            field.Ignore(true);
                                            setNodeText($node.data);

                                        }
                                    }, include = {
                                        label: "Include",
                                        action: function () {
                                            field.Ignore(false);
                                            setNodeText($node.data);
                                        }
                                    },
                                    makeNullable = {
                                        label: "Nullable",
                                        action: function () {
                                            field.IsNullable(true);
                                            setNodeText($node.data);

                                        }
                                    },
                                    makeNonNullable = {
                                        label: "Make non nullable",
                                        action: function () {
                                            field.IsNullable(false);
                                            setNodeText($node.data);
                                        }
                                    };


                                    const items = [];
                                    if ($node.type !== "complex") {
                                        if (ko.unwrap(field.Ignore)) {
                                            items.push(include);
                                        }
                                        else {
                                            items.push(ignore);
                                        }
                                        if (ko.unwrap(field.IsNullable)) {
                                            items.push(makeNonNullable);
                                        }
                                        else {
                                            items.push(makeNullable);
                                        }
                                    }

                                    return items;
                                }
                            },
                            "types": {
                                "default": {
                                    "icon": "fa fa-clipboard"
                                },
                                "complex": {
                                    "icon": "fa fa-object-group"
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
                                    "icon": "glyphicon glyphicon-ok",
                                    "valid_children": []
                                }
                            },
                            "plugins": ["contextmenu", "types", "search"]
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