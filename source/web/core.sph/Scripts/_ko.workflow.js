/// <reference path="jstree.min.js" />
/// <reference path="jstree.min.js" />
/// <reference path="typeahead.bundle.js" />
/// <reference path="knockout-3.2.0.debug.js" />
/// <reference path="knockout.mapping-latest.debug.js" />
/// <reference path="../App/services/datacontext.js" />
/// <reference path="../SphApp/objectbuilders.js" />
/// <reference path="../App/durandal/amd/text.js" />
/// <reference path="jquery-2.1.1.intellisense.js" />
/// <reference path="underscore.js" />
/// <reference path="require.js" />
/// <reference path="C:\project\work\sph\source\web\core.sph\SphApp/services/datacontext.js" />

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


ko.bindingHandlers.wdTypeaheadPath = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            instance = ko.unwrap(valueAccessor()),
            setup = function () {

                var wd = ko.mapping.toJS(instance);
                var input = $(element),
                         div = $('<div></div>').css({
                             'height': '28px'
                         });
                input.hide().before(div);

                var c = completely(div[0], {
                    fontSize: '12px',
                    color: '#555;',
                    fontFamily: '"Open Sans", Arial, Helvetica, sans-serif'
                });

                c.setText(ko.unwrap(allBindings.value));
                for (var ix in wd) {
                    if (ix === "$type") continue;
                    if (ix === "addChildItem") continue;
                    if (ix === "removeChildItem") continue;
                    c.options.push("" + ix);
                }
                c.options.sort();

                var currentObject = wd;
                c.onChange = function (text) {
                    if (text.lastIndexOf(".") === text.length - 1) {
                        c.options = [];
                        var props = text.split(".");

                        currentObject = wd;
                        _(props).each(function (v) {
                            if (v === "") { return; }
                            currentObject = currentObject[v];
                        });
                        console.log("currentObject", currentObject);
                        for (var i in currentObject) {
                            if (i === "$type") continue;
                            if (i === "addChildItem") continue;
                            if (i === "removeChildItem") continue;
                            c.options.push("" + i);
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
                    }).parent().find('input')
                    .css({ "padding": "6px 12px", "height": "28px" });

                if ($(element).prop("required")) {
                    $(c.input).prop("required", true);
                }


            };


        if (typeof value === "function" && typeof value.subscribe === "function") {
            value.subscribe(function () {
                $(element).typeahead("destroy");
                setup();
            });
        }
        setup();
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
        var $select = $(element),
            lookup = ko.unwrap(valueAccessor()),
            value = lookup.value,
            caption = lookup.caption,
            context = require('services/datacontext');

        var setup = function(query) {
            context.getTuplesAsync({
                    entity: ko.unwrap(lookup.entity),
                    query: query,
                    field: ko.unwrap(lookup.valuePath),
                    field2: ko.unwrap(lookup.displayPath)

                })
                .done(function(list) {
                    element.options.length = 0;
                    if (caption) {
                        element.add(new Option(caption, ""));
                    }
                    _(list).each(function(v) {
                        element.add(new Option(v.Item2, v.Item1));
                    });

                    $select.val(ko.unwrap(value))
                        .on('change', function() {
                            value($select.val());
                        });

                });
        };
        setup(ko.unwrap(lookup.query));

        if (typeof lookup.query === "function") {
            if (typeof lookup.query.subscribe === "function") {
                lookup.query.subscribe(function(v) {
                    setup(ko.unwrap(v));
                });
            }
        }
    },
    update: function (element, valueAccessor) {
        var lookup = ko.unwrap(valueAccessor()),
            value = lookup.value;
        $(element).val(ko.unwrap(value));
    }
};

