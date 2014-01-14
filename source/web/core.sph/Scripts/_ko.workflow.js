/// <reference path="jstree.min.js" />
/// <reference path="jstree.min.js" />
/// <reference path="typeahead.js" />
/// <reference path="knockout-3.0.0.debug.js" />
/// <reference path="knockout.mapping-latest.debug.js" />
/// <reference path="../App/services/datacontext.js" />
/// <reference path="../App/durandal/amd/text.js" />
/// <reference path="jquery-2.0.3.intellisense.js" />
/// <reference path="underscore.js" />

ko.bindingHandlers.activityClass = {
    init: function (element, valueAccessor) {

        var div = $(element),
            act = ko.unwrap(valueAccessor()),
        fullName = typeof act.$type === "function" ? act.$type() : act.$type,
        name = /Bespoke\.Sph\.Domain\.(.*?),/.exec(fullName)[1];

        div.addClass("activity32").addClass("activity32-" + name);
    }
};

ko.bindingHandlers.checkedItems = {
    init: function (element, valueAccessor) {
        var item = ko.dataFor(element),
            list = valueAccessor();

        $(element).change(function () {
            var checked = $(this).is(':checked');
            if (checked) {
                list.push(item);
            } else {
                list.remove(item);
            }
        });

    }
};

ko.bindingHandlers.typeahead = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var id = ko.unwrap(valueAccessor()),
        allBindings = allBindingsAccessor();
        $(element).typeahead({
            name: 'schema_paths' + id,
            limit: 10,
            prefetch: {
                url: '/WorkflowDefinition/GetVariablePath/' + id,
                ttl: 1000 * 60
            }
        })
            .on('typeahead:closed', function () {
                allBindings.value($(this).val());
            });
    }
};


ko.bindingHandlers.activityPopover = {
    init: function (element, valueAccessor) {

        var div = $(element),
            act = ko.unwrap(valueAccessor()),
            fullName = typeof act.$type === "function" ? act.$type() : act.$type,
            name = /Bespoke\.Sph\.Domain\.(.*?),/.exec(fullName)[1],
            wd = ko.dataFor(div.parent()[0]).wd(),
            pop = div.popover({
                title: name + ' : ' + act.Name(),
                html: true,
                content: function () {
                    $('a.edit-activity').popover('hide');
                    div.find("a.edit-activity").addClass(act.WebId());
                    div.find("a.delete-activity").addClass(act.WebId());
                    div.find("a.start-activity").addClass(act.WebId());
                    return div.find("div.context-menu").html();
                }
            });

        // just display it for 5 seconds
        pop.on('shown.bs.popover', function () {
            setTimeout(function () { pop.popover('hide'); }, 5000);
        });

        $(document).on('click', 'a.' + act.WebId(), function (e) {
            e.preventDefault();
            var app = require(objectbuilders.app),
                link = $(this);
            if (link.hasClass("delete-activity")) {
                app.showMessage("Are you sure you want to remove this Activity", "Remove activity", ["Yes", "No"])
                    .done(function (result) {
                        if (result === "Yes") {
                            wd.removeActivity(act)();
                        }
                    });
            }
            if (link.hasClass("edit-activity")) {
                wd.editActivity(act)();
            }
            if (link.hasClass("start-activity")) {
                wd.setStartActivity(act)();
            }
        });
    }
};

ko.bindingHandlers.comboBoxLookupOptions = {
    init: function (element, valueAccessor) {
        var lookup = ko.unwrap(valueAccessor()),
        value = lookup.value,
        context = require('services/datacontext');

        context.getTuplesAsync({
            entity: ko.unwrap(lookup.entity),
            query: ko.unwrap(lookup.query),
            field: ko.unwrap(lookup.valuePath),
            field2: ko.unwrap(lookup.displayPath)

        })
            .done(function (list) {
                element.options.length = 0;
                _(list).each(function (v) {
                    element.add(new Option(v.Item2, v.Item1));
                });

                $(element).val(ko.unwrap(value))
                    .on('change', function () {
                        value($(this).val());
                    });

            });
    },
    update: function (element, valueAccessor) {
        var lookup = ko.unwrap(valueAccessor()),
            value = lookup.value;
        $(element).val(ko.unwrap(value));
    }
};


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
                            "default": {
                                "valid_children": ["default", "file"]
                            },
                            "file": {
                                "icon": "glyphicon glyphicon-file",
                                "valid_children": []
                            }
                        },
                        "plugins": [
                          "contextmenu", "dnd", "search",
                          "state", "types", "wholerow"
                        ]
                    });
            };
        loadJsTree();

    }
};