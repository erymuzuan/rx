define([], function () {


    var intiFilter = function (element, options, search) {
        var path = options.path,
            tooltip = options.tooltip || "Type to filter current page or type and [ENTER] to apply _all in remote query",
            $element = $(element),
            $filterInput = $("<input data-toggle=\"tooltip\" title=\"" + tooltip + "\" type=\"search\" class=\"search-query input-medium form-control\" placeholder=\"Filter.. \">"),
            $serverLoadButton = $("<a href='/#' title='Carian server'><i class='add-on icon-search'></i><a>"),
            $form = $("<form class='form-search row'>" +
                " <div class='input-group pull-right' style='width:300px'>" +
                "<span class='input-group-addon'>" +
                " <span class='glyphicon glyphicon-remove'></span>" +
                "</span> " +
                "</div>" +
                " </form>");


        $form.find("span.input-group-addon").before($filterInput);
        $form.find("span.glyphicon-remove").after($serverLoadButton);
        $element.before($form);

        $form.submit(function (e) {
            e.preventDefault();
            var filter = $filterInput.val().toLowerCase(),
                tcs = new $.Deferred();
            if (!filter) {
                return tcs.promise();
            }
            if (typeof search === "function") {
                return search("_all:" + filter);
            }
            return tcs.promise();
        });



        var dofilter = function () {
            var $rows = $element.find(path),
                filter = $filterInput.val().toLowerCase();
            $rows.each(function () {
                var $tr = $(this),
                    text = $tr.text().toLowerCase().trim();
                if (!text) {
                    $("input", $tr).each(function (i, v) { text += " " + $(v).val() });
                    text = text.toLowerCase().trim();
                }
                if (text.indexOf(filter) > -1) {
                    $tr.show();
                } else {
                    $tr.hide();
                }
            });


        },
        throttled = _.throttle(dofilter, 800);

        $filterInput.on("keyup", throttled).siblings("span.input-group-addon")
            .click(function () {
                $filterInput.val("");
                dofilter();
            });

        if ($filterInput.val()) {
            dofilter();
        }
        $filterInput.tooltip();

    };


    ko.bindingHandlers.queryPaging = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = valueAccessor(),
                query = value.query,
                executedQuery = null,
                list = value.list,
                map = value.map,
                pagerHidden = value.pagerHidden || false,
                searchButton = value.searchButton,
                $element = $(element),
                logger = require("services/logger"),
                cultures = require(objectbuilders.cultures),
                $pagerPanel = $("<div></div>"),
                $spinner = $("<img src=\"/Images/spinner-md.gif\" alt=\"loading\" class=\"absolute-center\" />"),
                startLoad = function () {
                    $spinner.show();
                    $element.fadeTo("fast", 0.33);
                },
                endLoad = function () {
                    $spinner.hide();
                    $element.fadeTo("fast", 1);
                },
                pager = null,
                setItemsSource = function (items) {

                    if (!pagerHidden) {
                        _(items).each(function (v) {
                            v.pager = {
                                page: pager.page(),
                                size: pager.pageSize()
                            };
                        });
                    }

                    if (map) {
                        items = _(items).map(map);
                    }
                    if (typeof list === "string") {
                        viewModel[list](items);
                    }
                    if (typeof list === "function") {
                        list(items);
                    }
                },
                pageChanged = function (page, size) {
                    startLoad();
                    var url = query + "?page=" + (page || 1) + "&size=" + (size || 20);
                    if (executedQuery) url += "&q=" + executedQuery;
                    $.getJSON(url)
                         .then(function (lo) {
                             setItemsSource(lo._results);
                             endLoad();
                         });
                },
                search = function (page, size) {
                    var tcs1 = new $.Deferred();
                    startLoad();
                    var url = query + "?page=" + (page || 1) + "&size=" + (size || 20);
                    if (query.indexOf("?") > -1) url = query + "&page=" + (page || 1) + "&size=" + (size || 20);
                    if (executedQuery) url += "&q=" + executedQuery;
                    $.getJSON(url)
                        .then(function (lo) {
                            if (pager) {
                                pager.update(lo._count);
                            } else {
                                var pagerOptions = {
                                    element: $pagerPanel,
                                    count: lo._count,
                                    changed: pageChanged,
                                    hidden: pagerHidden
                                };
                                pager = new bespoke.utils.ServerPager(pagerOptions);

                            }

                            setTimeout(function () {
                                setItemsSource(lo._results);
                                tcs1.resolve(lo);
                                endLoad();
                            }, 500);

                        });
                    return tcs1.promise();
                },
                filterAndSearch = function (text) {
                    pager.destroy();
                    pager = null;
                    executedQuery = text;
                    return search();
                },
                filter = ko.unwrap(value.filter) || {};

            //exposed the search function
            intiFilter(element, { path: filter.path || 'tbody>tr' }, filterAndSearch);

            $element.after($pagerPanel).after($spinner)
                .fadeTo("slow", 0.33);

            if (searchButton) {
                $(document).on("click", searchButton, function (e) {
                    e.preventDefault();
                    if (!$(this).parents("form")[0].checkValidity()) {
                        logger.error(cultures.messages.FORM_IS_NOT_VALID);
                        return;
                    }
                    search(ko.toJS(query), 1, pager.pageSize());
                });

            }


            search(ko.toJS(executedQuery));
            return {
                search: search,
                filterAndSearch: filterAndSearch
            };

        }
    };

    ko.bindingHandlers.treeCheckbox = {
        init: function (element, valueAccessor) {
            var value = valueAccessor(),
                entity = ko.unwrap(value.entity),
                selectedItems = value.selectedItems,
                searchbox = ko.unwrap(value.searchbox),
                jsTreeData = {
                    text: entity.Name(),
                    state: {
                        opened: true,
                        selected: true
                    }
                },
                recurseChildMember = function (node, parentPath) {
                    var parent = "";
                    if (node.data && node.data.Name && parentPath)
                        parent = parentPath + "-" + ko.unwrap(node.data.Name()) + "-";
                    if (node.data && node.data.Name && !parentPath)
                        parent = ko.unwrap(node.data.Name()) + "-";

                    parent = parent.replace(/--/g, "-");

                    node.children = _(node.data.MemberCollection()).map(function (v) {

                        var type = ko.unwrap(v.TypeName) || ko.unwrap(v.$type);
                        return {
                            text: v.Name(),
                            state: "open",
                            type: type,
                            data: v,
                            id: parent + v.Name()
                        };
                    });
                    _(node.children).each(function(n) {
                        recurseChildMember(n, parent);
                    });
                },
                loadJsTree = function () {
                    entity.MemberCollection.unshift(new bespoke.sph.domain.SimpleMember({Name :"Id", TypeName : "System.String, mscorlib"}));
                    jsTreeData.children = _(entity.MemberCollection()).map(function (v) {
                        return {
                            text: ko.unwrap(v.Name),
                            state: "open",
                            type: ko.unwrap(v.TypeName) || ko.unwrap(v.$type),
                            data: v,
                            id: ko.unwrap(v.Name)
                        };
                    });
                    _(jsTreeData.children).each(function(n) {
                        recurseChildMember(n, "");
                    });
                    $(element)
                        .jstree({
                            "core": {
                                "animation": 0,
                                "check_callback": true,
                                "themes": { "stripes": true },
                                'data': jsTreeData
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
                            "plugins": ["checkbox", "types", "search"]
                        });

                    var $tree = $(element).jstree(true);
                    $tree.deselect_all();
                    _(ko.unwrap(selectedItems)).each(function(n) {
                        var id = n.replace(/\./g, "-"),
                            ref = $tree.get_node(id);
                        if (ref) {
                            $tree.select_node(ref);
                        }
                    });

                    $(element)
                        .on("select_node.jstree", function(node, selected) {
                            var id = selected.node.id;
                            selectedItems.push(id.replace(/-/g, "."));
                        })
                        .on("deselect_node.jstree", function(node, selected) {
                            var id = selected.node.id;
                            selectedItems.remove(id.replace(/-/g, "."));
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
