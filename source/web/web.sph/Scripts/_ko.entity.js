
ko.bindingHandlers.tree = {
    init: function (element, valueAccessor) {
        var value = valueAccessor(),
            entity = ko.unwrap(value.entity),
            member = value.selected,
            jsTreeData = {
                text: entity.Name(),
                state: {
                    opened: true,
                    selected: true
                }
            },
            loadJsTree = function () {
                jsTreeData.children = _(entity.MemberCollection()).map(function (v) {
                    return {
                        text: v.Name(),
                        data: v
                    };
                });
                $(element)
                    .on('select_node.jstree', function (node, selected) {
                        if (selected.node.data) {
                            member(selected.node.data);
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
                            "items": [
                            {
                                label: "Add Text",
                                action: function(node,ev) {
                                    console.log(node);
                                    console.log(ev);
                                    entity.MemberCollection.push(new bespoke.sph.domain.Member({ TypeName: 'System.String, mscrolib', Name: '<StringMember>' }));

                                    $(element).jstree('create_node','[obj, node]');
                                }
                            },
                            {
                                label: "Add DateTime",
                                action: function () { }
                            },
                            {
                                label: "Add integer",
                                action: function () { }
                            },
                            {
                                label: "Add decimal",
                                action: function () { }
                            },
                            {
                                label: "Add boolean",
                                action: function () { }
                            },
                            {
                                label: "Add Complex",
                                action: function () { }
                            }]
                        },
                        "types": {
                            "#": {
                                "max_children": 1,
                                "max_depth": 4,
                                "valid_children": ["root"]
                            },
                            "root": {
                                "icon": "/static/3.0.0-beta3/assets/images/tree_icon.png",
                                "valid_children": ["default"]
                            },
                            "System.String, mscorlib": {
                                "icon": "glyphicon glyphicon-text-width",
                                "valid_children": ["default", "file"]
                            },
                            "Complex": {
                                "icon": "glyphicon glyphicon-file",
                                "valid_children": []
                            }
                        },
                        "plugins": ["contextmenu", "dnd", "types"]
                    });
            };
        loadJsTree();

    }
};