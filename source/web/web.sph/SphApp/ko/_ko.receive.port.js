///<reference path="../../web/core.sph/Scripts/jstree.min.js"/>
///<reference path="../../web/core.sph/Scripts/require.js"/>
///<reference path="../../web/core.sph/Scripts/underscore.js"/>
///<reference path="../../web/core.sph/SphApp/objectbuilders.js"/>
///<reference path="../../web/core.sph/SphApp/schemas/form.designer.g.js"/>
///<reference path="../../web/core.sph/Scripts/jquery-2.2.0.intellisense.js"/>
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
                    text: port.Name(),
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
                    return type;
                },
                fieldPathSubscription = null,
                fieldTypeNameSubscription = null,
                recurseChildMember = function (node) {
                    node.children = _(node.data.FieldMappingCollection()).map(function (v) {

                        const type = getNodeMemberType(v);
                        return {
                            text: `${ko.unwrap(v.Path)} (${ko.unwrap(v.SampleValue)})`,
                            state: "open",
                            type: type,
                            data: v
                        };
                    });
                    _(node.children).each(recurseChildMember);
                },
                loadJsTree = function () {
                    jsTreeData.children = _(port.FieldMappingCollection()).map(function (v) {

                        return {
                            text: `${ko.unwrap(v.Path)} (${ko.unwrap(v.SampleValue)})`,
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
                            if (fieldPathSubscription) {
                                fieldPathSubscription.dispose();
                                fieldPathSubscription = null;
                            }

                            if (fieldTypeNameSubscription) {
                                fieldTypeNameSubscription.dispose();
                                fieldTypeNameSubscription = null;
                            }

                            const field = selected.node.data;
                            if (field) {

                                selectedField(field);
                                // subscribe to Name change
                                fieldPathSubscription = selectedField().Path.subscribe(function (path) {
                                    $(element).jstree(true)
                                        .rename_node(selected.node, path);
                                    console.log("rename " + path);
                                });
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
                        .on("rename_node.jstree", function (ev, node) {
                            const mb = node.node.data;
                            mb.Name(node.text);
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
                                        if (!ko.isObservable(mbr.childOfValueMember)) {
                                            setNodePropertyForChildOfValueObject(node, mbr);
                                        }
                                        if (ko.unwrap(mbr.childOfValueMember)) {
                                            return false;
                                        }
                                        if (target.type === "Bespoke.Sph.Domain.ValueObjectMember, domain.sph") {
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

                                    var ref = $(element).jstree(true),
                                        parents = _($node.parents).map(function (n) { return ref.get_node(n); }),
                                        sel = ref.get_selected();
                                   


                                    let items = [];

                                    return items;
                                }
                            },
                            "types": {

                                "default": {
                                    "icon": "fa fa-clipboard"
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
                                },
                                "Bespoke.Sph.Domain.ValueObjectMember, domain.sph": {
                                    "icon": "fa fa-object-ungroup"
                                },
                                "Bespoke.Sph.Domain.ComplexMember, domain.sph": {
                                    "icon": "fa fa-object-group"
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
