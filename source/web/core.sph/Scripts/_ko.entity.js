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
            getNodeMemberType = function (v) {
                var type = ko.unwrap(v.TypeName) || ko.unwrap(v.$type);
                if (type.indexOf(",") < 0) {
                    type = ko.unwrap(v.$type);
                }
                return type;
            },
            memberNameSubscription = null,
            memberTypeNameSubscription = null,
            recurseChildMember = function (node) {
                node.children = _(node.data.MemberCollection()).map(function (v) {

                    var type = getNodeMemberType(v);
                    return {
                        text: v.Name(),
                        state: "open",
                        type: type,
                        data: v
                    };
                });
                _(node.children).each(recurseChildMember);
            },
            loadJsTree = function () {
                jsTreeData.children = _(entity.MemberCollection()).map(function (v) {

                    return {
                        text: ko.unwrap(v.Name),
                        state: "open",
                        type: getNodeMemberType(v),
                        data: v
                    };
                });
                _(jsTreeData.children).each(recurseChildMember);
                $(element)
                    .on("select_node.jstree", function (node, selected) {
                        if (typeof member !== "function") {
                            return;
                        }
                        if (memberNameSubscription) {
                            memberNameSubscription.dispose();
                            memberNameSubscription = null;
                        }

                        if (memberTypeNameSubscription) {
                            memberTypeNameSubscription.dispose();
                            memberTypeNameSubscription = null;
                        }

                        var tag = selected.node.data;
                        if (tag) {
                            if (selected.node.type === "default") {
                                return;
                            }

                            if (typeof tag.childOfValueMember !== "function") {
                                tag.childOfValueMember = ko.observable(true);
                            }

                            var ref = $(element).jstree(true),
                                   parents = _(selected.node.parents).map(function (n) { return ref.get_node(n); }),
                                   valueMember = _(parents).find(function (n) { return n.type === "Bespoke.Sph.Domain.ValueObjectMember, domain.sph"; });
                            tag.childOfValueMember(valueMember || false);
                            if (!tag.childOfValueMember()) {
                                ref.edit(selected.node);
                            }

                            member(tag);


                            // subscribe to Name change
                            memberNameSubscription = member().Name.subscribe(function (name) {
                                $(element).jstree(true)
                                    .rename_node(selected.node, name);
                                console.log("rename " + name);
                            });
                            // type
                            if (typeof member().TypeName === "function") {
                                memberTypeNameSubscription = member().TypeName.subscribe(function (name) {
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
                            "items": function ($node) {

                                var ref = $(element).jstree(true),
                                    parents = _($node.parents).map(function (n) { return ref.get_node(n); }),
                                    valueMember = _(parents).find(function (n) { return n.type === "Bespoke.Sph.Domain.ValueObjectMember, domain.sph"; }),
                                        sel = ref.get_selected();
                                if (valueMember) {
                                    return [];
                                }

                                var simpleMenu = {
                                    label: "Add Simple Child",
                                    action: function () {
                                        var child = new bespoke.sph.domain.SimpleMember({ WebId: system.guid(), TypeName: "System.String, mscorlib", Name: "Member_Name" }),
                                            parent = $(element).jstree("get_selected", true),
                                            mb = parent[0].data,
                                            newNode = { state: "open", type: "System.String, mscorlib", text: "Member_Name", data: child };

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
                                    valueObjectMenu = {
                                        label: "Add Value Object Child",
                                        action: function () {
                                            var typeName = "Bespoke.Sph.Domain.ValueObjectDefinition, domain.sph",
                                                child = new bespoke.sph.domain.ValueObjectMember({ WebId: system.guid(), TypeName: typeName, Name: "Member_Name" }),
                                                parent = $(element).jstree("get_selected", true),
                                                mb = parent[0].data,
                                                newNode = { state: "open", type: "Bespoke.Sph.Domain.ValueObjectMember, domain.sph", text: "Member_Name", data: child };

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
                                    complexChildMenu = {
                                        label: "Add Complex Child",
                                        action: function () {
                                            var child = new bespoke.sph.domain.ComplexMember({ WebId: system.guid(), Name: "Member_Name" }),
                                                parent = $(element).jstree("get_selected", true),
                                                mb = parent[0].data,
                                                newNode = { state: "open", type: "Bespoke.Sph.Domain.ComplexMember, domain.sph", text: "Member_Name", data: child };

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
                                    removeMenu = {
                                        label: "Remove",
                                        action: function () {
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
                                    };

                                var items = [];

                                if ($node.type === "default" || $node.type === "Bespoke.Sph.Domain.ComplexMember, domain.sph") {
                                    items.push(simpleMenu);
                                    items.push(valueObjectMenu);
                                    items.push(complexChildMenu);
                                }

                                console.log($node);
                                if ($node.type !== "default") {
                                    items.push(removeMenu);
                                }
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
                                "icon": "fa fa-building-o"
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
var substringMatcher = function (strs) {
    return function findMatches(q, cb) {
        var matches, substringRegex;

        // an array that will be populated with substring matches
        matches = [];
        // regex used to determine if a string contains the substring `q`
        substringRegex = new RegExp(q, "i");

        // iterate through the pool of strings and for any string that
        // contains the substring `q`, add it to the `matches` array
        $.each(strs, function (i, str) {
            if (substringRegex.test(str)) {
                // the typeahead jQuery plugin expects suggestions to a
                // JavaScript object, refer to typeahead docs for more info
                matches.push({ value: str });
            }
        });

        cb(matches);
    };
};


ko.bindingHandlers.typeaheadUrl = {
    init: function (element, valueAccessor) {
        var types = ko.unwrap(valueAccessor()),
            ttl = 300000,
            url = String.format("/list?table={0}&column={1}", types[0], "Route"),
            suggestions = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace("name"),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                limit: 10,
                remote: {
                    url: url,
                    tt1: ttl,
                    filter: function (list) {
                        return _(list).map(function (v) {
                            return { name: v };
                        });
                    }
                }

            });

        suggestions.initialize();
        $(element).typeahead(null,
            {
                name: "EntityView_" + $(element).prop("id"),
                displayKey: "name",
                source: suggestions.ttAdapter()
            });
    }
};
ko.bindingHandlers.entityTypeaheadPath = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            context = require(objectbuilders.datacontext),
            config = require(objectbuilders.config),
            allBindings = allBindingsAccessor(),
            idOrName = ko.unwrap(valueAccessor()) || window.typeaheadEntity,
            setup = function (options) {

                var name = options.name || options,
                    eid = options.id || options;

                var ed = ko.toJS(bespoke[config.applicationName + "_" + eid.toLowerCase()].domain[name]());
                var input = $(element),
                         div = $("<div></div>").css({
                             'height': "28px"
                         });
                input.hide().before(div);

                var c = completely(div[0], {
                    fontSize: "12px",
                    color: "#555;",
                    fontFamily: "\"Open Sans\", Arial, Helvetica, sans-serif"
                });

                c.setText(ko.unwrap(allBindings.value));
                for (var ix in ed) {
                    if (ed.hasOwnProperty(ix)) {
                        if (ix === "$type") continue;
                        if (ix === "addChildItem") continue;
                        if (ix === "removeChildItem") continue;
                        if (ix === "Empty") continue;
                        if (ix === "WebId") continue;
                        c.options.push("" + ix);
                    }
                }
                c.options.sort();

                var currentObject = ed;
                c.onChange = function (text) {
                    if (text.lastIndexOf(".") === text.length - 1) {
                        c.options = [];
                        var props = text.split(".");

                        currentObject = ed;
                        _(props).each(function (v) {
                            if (v === "") { return; }
                            currentObject = currentObject[v];
                        });
                        console.log("currentObject", currentObject);
                        for (var i in currentObject) {
                            if (currentObject.hasOwnProperty(i)) {
                                if (i === "$type") continue;
                                if (i === "addChildItem") continue;
                                if (i === "removeChildItem") continue;
                                if (i === "Empty") continue;
                                if (i === "WebId") continue;
                                c.options.push("" + i);
                            }
                        }
                        c.options.sort();
                        c.startFrom = text.lastIndexOf(".") + 1;
                    }
                    c.repaint();
                };

                c.repaint();
                $(c.input)
                    .attr("autocomplete", "off")
                    .blur(function () {
                        allBindings.value($(this).val());
                    }).parent().find("input")
                    .css({ "padding": "6px 12px", "height": "28px" });

                if ($(element).prop("required")) {
                    $(c.input).prop("required", true);
                }


            };


        if (idOrName) {
            context.loadOneAsync("EntityDefinition", "Name eq '" + idOrName + "' OR id eq '" + idOrName + "'", "Id")
                .done(function (edf) {
                    setup({ name: edf.Name(), id: edf.Id() });
                });

        }


        if (typeof value === "function" && typeof value.subscribe === "function") {
            value.subscribe(function (entity) {
                $(element).typeahead("destroy");
                setup(entity);
            });
        }
    }
};

ko.bindingHandlers.cssTypeahead = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var allBindings = allBindingsAccessor(),
            results = ["fa", "fa-user", "fa-user-o"],
            extractor = function (query) {
                var result = /([^,]+)$/.exec(query);
                if (result && result[1])
                    return result[1].trim();
                return "";
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
                return this.$element.val().replace(/[^,]*$/, "") + item + ",";
            },
            matcher: function (item) {
                var tquery = extractor(this.query);
                if (!tquery) return false;
                return ~item.toLowerCase().indexOf(tquery.toLowerCase());
            }
        },
            {
                name: "css-class",
                displayKey: "path",
                source: members.ttAdapter()
            })
            .on("typeahead:closed", function () {
                allBindings.value($(this).val());
            });

    }
};


ko.bindingHandlers.chart = {
    init: function (element, valueAccessor) {
        var chart = ko.unwrap(valueAccessor()),
            context = require("services/datacontext"),
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

                context.getScalarAsync("EntityView", "Id eq '" + chart.EntityViewId() + "'", "Name")
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
            $picker = $("<a href=\"#\" class=\"btn btn-link\">Pick Icon</a>");

        $input.parent().append($picker);
        $picker.click(function (e) {
            e.preventDefault();
            require(["viewmodels/icon.picker", "durandal/app"], function (dialog, app2) {
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
    init: function (element, valueAccessor) {
        var $link = $(element),
            options = valueAccessor(),
            member = ko.unwrap(options.member);

        $link.click(function (e) {
            e.preventDefault();
            e.stopPropagation();

            require(["viewmodels/entity.lookup.dialog", "durandal/app"], function (dialog, app2) {
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
ko.virtualElements.allowedBindings.lookupText = true;
bespoke.lookupText = function (element, valueAccessor) {
    var options = valueAccessor(),
        entity = ko.unwrap(options.entity),
        displayPath = ko.unwrap(options.displayPath),
        valuePath = ko.unwrap(options.valuePath),
        val = ko.unwrap(options.value),
        context = require("services/datacontext"),
        setTextContent = function (ele, textContent) {
            var value = ko.utils.unwrapObservable(textContent);
            if ((value === null) || (value === undefined))
                value = "";
            var innerTextNode = ko.virtualElements.firstChild(ele);
            if (!innerTextNode || innerTextNode.nodeType !== 3 || ko.virtualElements.nextSibling(innerTextNode)) {
                ko.virtualElements.setDomNodeChildren(ele, [ele.ownerDocument.createTextNode(value)]);
            } else {
                innerTextNode.data = value;
            }

        };

    context.getScalarAsync(entity, valuePath + " eq '" + val + "'", displayPath)
    .done(function (text) {
        setTextContent(element, text);
        console.log(text);
    });
};
ko.bindingHandlers.lookupText = {
    init: bespoke.lookupText,
    update: bespoke.lookupText
};


ko.bindingHandlers.readonly = {
    init: function (element, valueAccessor) {
        var input = $(element),
            ro = ko.unwrap(valueAccessor());

        if (ro) {
            input.prop("readonly", true);
        } else {

            input.prop("readonly", false);
        }
    },
    update: function (element, valueAccessor) {
        var input = $(element),
             ro = ko.unwrap(valueAccessor());

        if (ro) {
            input.prop("readonly", true);
        } else {

            input.prop("readonly", false);
        }
    }
};