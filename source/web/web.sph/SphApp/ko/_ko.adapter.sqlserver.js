///<reference path="../../web/core.sph/Scripts/jstree.min.js"/>
///<reference path="../../web/core.sph/Scripts/require.js"/>
///<reference path="../../web/core.sph/Scripts/underscore.js"/>
///<reference path="../../web/core.sph/SphApp/objectbuilders.js"/>
///<reference path="../../web/core.sph/SphApp/schemas/form.designer.g.js"/>

define(["knockout"], function (ko) {

    ko.bindingHandlers.adapterTree = {
        init: function (element, valueAccessor) {
            var funcType = "Bespoke.Sph.Integrations.Adapters.FuncOperationDefinition, sqlserver.adapter",
                sprocType = "Bespoke.Sph.Integrations.Adapters.SprocOperationDefinition, sqlserver.adapter",
                value = valueAccessor(),
                adapter = ko.unwrap(value.adapter),
                searchbox = ko.unwrap(value.searchbox),
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
                        "id": "operation-node",
                        text: "Operations",
                        icon: "fa fa-cogs",
                        state: {
                            opened: true
                        }
                    }],
                mapTable = function (v) {
                    var columns = _(v.ColumnCollection()).map(function (col) {
                            return {
                                text: ko.unwrap(col.Name),
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
                            }]
                    };
                },
                mapOperation = function (v) {

                    return {
                        text: ko.unwrap(v.Schema) + "." + ko.unwrap(v.Name),
                        state: "open",
                        type: ko.unwrap(v.$type),
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
                        .on("create_node.jstree", function (event, node) {
                            console.log(node, "node");
                        })
                        .jstree({
                            "core": {
                                "animation": 0,
                                "check_callback": true,
                                "themes": {"stripes": true},
                                'data': jsTreeData
                            },
                            "contextmenu": {
                                "items": function ($node) {
                                    console.log($node);
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
                                        }
                                        ;

                                    if ($node.type === "child-table-selected")
                                        return [unselectRelatedTable];
                                    if ($node.type === "child-table")
                                        return [selectRelatedTable];
                                    if ($node.type === "table")
                                        return [removeMenu];
                                    if ($node.type === funcType || $node.type == sprocType)
                                        return [editOperation, removeMenu];
                                    return [];

                                }
                            },
                            "types": {

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
                                "Bespoke.Sph.Integrations.Adapters.SprocOperationDefinition, sqlserver.adapter": {
                                    "icon": "fa fa-cog"
                                },
                                "Bespoke.Sph.Integrations.Adapters.FuncOperationDefinition, sqlserver.adapter": {
                                    "icon": "fa fa-calculator"
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
                            "plugins": ["contextmenu", "types"]
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