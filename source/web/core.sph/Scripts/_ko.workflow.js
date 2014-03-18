/// <reference path="jstree.min.js" />
/// <reference path="jstree.min.js" />
/// <reference path="typeahead.bundle.js" />
/// <reference path="knockout-3.1.0.debug.js" />
/// <reference path="knockout.mapping-latest.debug.js" />
/// <reference path="../App/services/datacontext.js" />
/// <reference path="../SphApp/objectbuilders.js" />
/// <reference path="../App/durandal/amd/text.js" />
/// <reference path="jquery-2.0.3.intellisense.js" />
/// <reference path="underscore.js" />
/// <reference path="require.js" />

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
            allBindings = allBindingsAccessor(),
            members = new Bloodhound({
                datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.Path); },
                queryTokenizer: Bloodhound.tokenizers.nonword,
                prefetch: '/WorkflowDefinition/GetVariablePath/' + id

            });
        members.initialize();
        $(element).typeahead({ highlight: true }, {
            name: 'schema_paths' + id,
            displayKey: "Path",
            source: members.ttAdapter()
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
        var $select = $(element),
            lookup = ko.unwrap(valueAccessor()),
            value = lookup.value,
            caption = lookup.caption,
            context = require('services/datacontext');

        context.getTuplesAsync({
            entity: ko.unwrap(lookup.entity),
            query: ko.unwrap(lookup.query),
            field: ko.unwrap(lookup.valuePath),
            field2: ko.unwrap(lookup.displayPath)

        })
            .done(function (list) {
                element.options.length = 0;
                if (caption) {
                    element.add(new Option(caption, ""));
                }
                _(list).each(function (v) {
                    element.add(new Option(v.Item2, v.Item1));
                });

                $select.val(ko.unwrap(value))
                    .on('change', function () {
                        value($select.val());
                    });

            });
    },
    update: function (element, valueAccessor) {
        var lookup = ko.unwrap(valueAccessor()),
            value = lookup.value;
        $(element).val(ko.unwrap(value));
    }
};

