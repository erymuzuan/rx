ko.bindingHandlers.tree = {
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
                        state: 'open',
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
                        state: 'open',
                        type: v.TypeName(),
                        data: v
                    };
                });
                _(jsTreeData.children).each(recurseChildMember);
                $(element)
                    .on('select_node.jstree', function (node, selected) {
                        if (selected.node.data) {
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
                    .on('create_node.jstree', function (event, node) {
                        console.log(node, "node");
                    })
                    .on('rename_node.jstree', function (ev, node) {
                        var mb = node.node.data;
                        mb.Name(node.text);
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
                                    label: "Add Child",
                                    action: function () {
                                        var child = new bespoke.sph.domain.Member({ WebId: system.guid(), TypeName: 'System.String, mscorlib', Name: 'Member_Name' }),
                                            parent = $(element).jstree('get_selected', true),
                                            mb = parent[0].data,
                                            newNode = { state: "open", type: "System.String, mscorlib", text: 'Member_Name', data: child };

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
                                            p = ref.get_node($('#' + n.parent)),
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

ko.bindingHandlers.help = {
    init: function (element, valueAccessor) {
        var link = $(element),
            href = ko.unwrap(valueAccessor());
        link.click(function (e) {
            e.preventDefault();
            window.open("/docs/#" + href);
        });
    }
};
ko.bindingHandlers.entityTypeaheadPath = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            id = ko.unwrap(valueAccessor()),
            allBindings = allBindingsAccessor(),
            setup = function (entity) {

                if (!entity) {
                    console.log("Cannot determine entity for the typeahead intellisense");
                    return;
                }
                $.get('/Sph/EntityDefinition/GetVariablePath/' + entity).done(function (results) {
                    var paths = _(results).map(function (v) {
                            return { path: v };
                        }),
                        members = new Bloodhound({
                            datumTokenizer: function (d) {
                                return d.path.split(/s+/);
                            },
                            queryTokenizer: function (s) {
                                return s.split(/\./);
                            },
                            local: paths
                        });
                    members.initialize();

                    $(element).typeahead({
                            minLength: 0,
                            highlight: true,
                        },
                        {
                            name: 'ed_paths' + id,
                            displayKey: 'path',
                            source: members.ttAdapter()
                        })
                        .on('typeahead:closed', function () {
                            allBindings.value($(this).val());
                        });
                });
            };

        setup(id);
        if (typeof value === "function" && typeof value.subscribe === "function") {
            value.subscribe(function (entity) {
                $(element).typeahead('destroy');
                setup(entity);
            })
        }
    }
};

ko.bindingHandlers.cssTypeahead = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var id = ko.unwrap(valueAccessor()),
            allBindings = allBindingsAccessor(),
            results = ["fa", "fa-user", "fa-user-o"],
            extractor = function (query) {
                var result = /([^,]+)$/.exec(query);
                if (result && result[1])
                    return result[1].trim();
                return '';
            },
            paths = _(results).map(function (v) {
                return { path: v };
            }),
            members = new Bloodhound({
                datumTokenizer: function (d) {
                    return d.path.split(/s+/);
                },
                queryTokenizer: function (s) {
                    return s.split(/\./);
                },
                local: paths
            });
        members.initialize();

        $(element).typeahead({
                minLength: 0,
                highlight: true,
                updater: function () {
                    return this.$element.val().replace(/[^,]*$/, '') + item + ',';
                },
                matcher: function (item) {
                    var tquery = extractor(this.query);
                    if (!tquery) return false;
                    return ~item.toLowerCase().indexOf(tquery.toLowerCase())
                }
            },
            {
                name: 'css-class',
                displayKey: 'path',
                source: members.ttAdapter()
            })
            .on('typeahead:closed', function () {
                allBindings.value($(this).val());
            });

    }
};


ko.bindingHandlers.chart = {
    init: function (element, valueAccessor) {
        var chart = ko.unwrap(valueAccessor()),
            context = require('services/datacontext'),
            entity = chart.Entity(),
            query = JSON.parse(chart.Query()),
            type = chart.Type(),
            tcs = new $.Deferred(),
            name = chart.Name();


        context.searchAsync(entity, query)
            .done(function (result) {

                var buckets = result.aggregations.category.buckets || result.aggregations.category,
                    data = _(buckets).map(function (v) {
                        return {
                            category: v.key_as_string || v.key.toString(),
                            value: v.doc_count
                        };
                    }),
                    categories = _(buckets).map(function (v) {
                        return v.key_as_string || v.key.toString();
                    }),
                    kendoChart = $(element).kendoChart({
                        theme: "metro",
                        chartArea: {
                            background: ""
                        },
                        title: {
                            text: name
                        },
                        legend: {
                            position: "bottom"
                        },
                        seriesDefaults: {
                            labels: {
                                visible: true,
                                format: "{0}",
                                background: "transparent",
                                template: "#= category #: #= value#"
                            }
                        },
                        series: [
                            {
                                type: type,
                                data: data
                            }
                        ],
                        categoryAxis: {
                            categories: categories,
                            majorGridLines: {
                                visible: false
                            }
                        },
                        tooltip: {
                            visible: true,
                            format: "{0}",
                            template: "#= category #: #= value #"
                        }
                    }).data("kendoChart");
                tcs.resolve(true);

                context.getScalarAsync("EntityView", "EntityViewId eq " + chart.EntityViewId(), "Name")
                    .done(function (viewName) {
                        kendoChart.options.title.text = name + " (" + viewName + ")";
                        kendoChart.refresh();
                    });

            });


        return tcs.promise();
    }
};

ko.bindingHandlers.iconPicker = {
    init: function (element, valueAccessor) {
        var $input = $(element),
            value = valueAccessor(),
            $picker = $('<a href="#" class="btn btn-link">Pick Icon</a>');

        $input.parent().append($picker);
        $picker.click(function (e) {
            e.preventDefault();
            require(['viewmodels/icon.picker', 'durandal/app'], function (dialog, app2) {
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (result === "OK") {
                            value(dialog.icon());
                            $input.val(dialog.icon());
                        }
                    });

            });
        });

    }
};

var bespoke = bespoke || {};
bespoke.observableArray = bespoke.observableArray || {};

bespoke.getSingletonObservableArray = function (key) {
    if (bespoke.observableArray[key]) {
        return bespoke.observableArray[key];
    }

    bespoke.observableArray[key] = ko.observableArray();
    return bespoke.observableArray[key];
};


ko.bindingHandlers.lookup = {
    init: function (element, valueAccessor, allBindings) {
        var $link = $(element),
            options = valueAccessor(),
            entity = ko.unwrap(options.entity),
            member = ko.unwrap(options.member);

        $link.click(function (e) {
            e.preventDefault();
            e.stopPropagation();

            require(['viewmodels/entity.lookup.dialog', 'durandal/app'], function (dialog, app2) {
                dialog.options(options);
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            var item = dialog.selected();
                            options.value(item[member]);
                        }
                    });

            });

        });
    }
};