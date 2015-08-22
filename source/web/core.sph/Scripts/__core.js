// String.js - liberated from MicrosoftAjax.js on 03/28/10 by Sky Sanders 

/*
    Copyright (c) 2009, CodePlex Foundation
    All rights reserved.

    Redistribution and use in source and binary forms, with or without modification, are permitted 
    provided that the following conditions are met:

    *   Redistributions of source code must retain the above copyright notice, this list of conditions 
        and the following disclaimer.

    *   Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
        and the following disclaimer in the documentation and/or other materials provided with the distribution.

    *   Neither the name of CodePlex Foundation nor the names of its contributors may be used to endorse or 
        promote products derived from this software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS AS IS AND ANY EXPRESS OR IMPLIED 
    WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR 
    A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE 
    FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
    LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
    INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
    OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
    IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.</textarea>
*/

(function (window) {

    $type = String;
    $type.__typeName = 'String';
    $type.__class = true;

    $prototype = $type.prototype;
    $prototype.endsWith = function String$endsWith(suffix) {
        /// <summary>Determines whether the end of this instance matches the specified string.</summary>
        /// <param name="suffix" type="String">A string to compare to.</param>
        /// <returns type="Boolean">true if suffix matches the end of this instance; otherwise, false.</returns>
        return (this.substr(this.length - suffix.length) === suffix);
    };

    $prototype.startsWith = function String$startsWith(prefix) {
        /// <summary >Determines whether the beginning of this instance matches the specified string.</summary>
        /// <param name="prefix" type="String">The String to compare.</param>
        /// <returns type="Boolean">true if prefix matches the beginning of this string; otherwise, false.</returns>
        return (this.substr(0, prefix.length) === prefix);
    };

    $prototype.trim = function String$trim() {
        /// <summary >Removes all leading and trailing white-space characters from the current String object.</summary>
        /// <returns type="String">The string that remains after all white-space characters are removed from the start and end of the current String object.</returns>
        return this.replace(/^\s+|\s+$/g, '');
    };

    $prototype.trimEnd = function String$trimEnd() {
        /// <summary >Removes all trailing white spaces from the current String object.</summary>
        /// <returns type="String">The string that remains after all white-space characters are removed from the end of the current String object.</returns>
        return this.replace(/\s+$/, '');
    };

    $prototype.trimStart = function String$trimStart() {
        /// <summary >Removes all leading white spaces from the current String object.</summary>
        /// <returns type="String">The string that remains after all white-space characters are removed from the start of the current String object.</returns>
        return this.replace(/^\s+/, '');
    }

    $type.format = function String$format(format, args) {
        /// <summary>Replaces the format items in a specified String with the text equivalents of the values of   corresponding object instances. The invariant culture will be used to format dates and numbers.</summary>
        /// <param name="format" type="String">A format string.</param>
        /// <param name="args" parameterArray="true" mayBeNull="true">The objects to format.</param>
        /// <returns type="String">A copy of format in which the format items have been replaced by the   string equivalent of the corresponding instances of object arguments.</returns>
        return String._toFormattedString(false, arguments);
    };

    $type._toFormattedString = function String$_toFormattedString(useLocale, args) {
        var result = '';
        var format = args[0];

        for (var i = 0;;) {
            // Find the next opening or closing brace
            var open = format.indexOf('{', i);
            var close = format.indexOf('}', i);
            if ((open < 0) && (close < 0)) {
                // Not found: copy the end of the string and break
                result += format.slice(i);
                break;
            }
            if ((close > 0) && ((close < open) || (open < 0))) {

                if (format.charAt(close + 1) !== '}') {
                    throw new Error('format stringFormatBraceMismatch');
                }

                result += format.slice(i, close + 1);
                i = close + 2;
                continue;
            }

            // Copy the string before the brace
            result += format.slice(i, open);
            i = open + 1;

            // Check for double braces (which display as one and are not arguments)
            if (format.charAt(i) === '{') {
                result += '{';
                i++;
                continue;
            }

            if (close < 0) throw new Error('format stringFormatBraceMismatch');


            // Find the closing brace

            // Get the string between the braces, and split it around the ':' (if any)
            var brace = format.substring(i, close);
            var colonIndex = brace.indexOf(':');
            var argNumber = parseInt((colonIndex < 0) ? brace : brace.substring(0, colonIndex), 10) + 1;

            if (isNaN(argNumber)) throw new Error('format stringFormatInvalid');

            var argFormat = (colonIndex < 0) ? '' : brace.substring(colonIndex + 1);

            var arg = args[argNumber];
            if (typeof(arg) === "undefined" || arg === null) {
                arg = '';
            }

            // If it has a toFormattedString method, call it.  Otherwise, call toString()
            if (arg.toFormattedString) {
                result += arg.toFormattedString(argFormat);
            } else if (useLocale && arg.localeFormat) {
                result += arg.localeFormat(argFormat);
            } else if (arg.format) {
                result += arg.format(argFormat);
            } else
                result += arg.toString();

            i = close + 1;
        }

        return result;
    };

})(window);
/// <reference path="toastr.js" />
/// <reference path="_ko.kendo.js" />
/// <reference path="../kendo/js/kendo.pager.js" />
/// <reference path="../kendo/js/kendo.all.js" />




var bespoke = bespoke || {};
bespoke.utils = {};

bespoke.utils.ServerPager = function (options) {
    options = options || {};
    var element = options.element,
        count = options.count || 0,
        sizes = options.sizes || [10, 20, 50],
        defaultSize = options.defaultSize || 20,
        changed = options.changed || function () {
            console.log("no change event");
        },
        self2 = this,
        rows = _.range(count),
        pagerDataSource = new kendo.data.DataSource({
            data: rows,
            pageSize: defaultSize
        });
    if (options.hidden) {
        return self2;
    }

    var pager = element.kendoPager({
        dataSource: pagerDataSource,
        pageSizes: sizes
    }).data("kendoPager");
    pager.page(1);
    pager.bind("change", function () {
        if (changed) {
            changed(pager.page(), pager.pageSize());
        }
    });

    self2.update = function (count2) {
        rows = [];
        for (var j = 0; j < count2 ; j++) {
            rows[j] = j;
        }
        setTimeout(function () {
            pagerDataSource.data(rows);
        }, 500);
    };
    self2.destroy = function () {
        pager.destroy();
        element.empty();
    };

    self2.pageSize = function (size) {
        if (size) {
            pager.pageSize(size);
        }
        return pager.pageSize();
    };
    self2.page = function (pg) {
        if (pg) {
            pager.page(pg);
        }
        return pager.page();
    };

    var dropdownlist = $(element).find("select").data("kendoDropDownList");
    dropdownlist.bind("change", function () {
        try {
            changed(1, parseInt(this.value()));
        } catch (e) {

        }
    });



    return self2;

};
/// <reference path="jstree.min.js" />
/// <reference path="jstree.min.js" />
/// <reference path="typeahead.bundle.js" />
/// <reference path="knockout-3.2.0.debug.js" />
/// <reference path="knockout.mapping-latest.debug.js" />
/// <reference path="../App/services/datacontext.js" />
/// <reference path="../SphApp/objectbuilders.js" />
/// <reference path="../App/durandal/amd/text.js" />
/// <reference path="jquery-2.1.3.intellisense.js" />
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
            allBindings = allBindingsAccessor();

        if (typeof id === "undefined") {
            return;
        }
          var  members = new Bloodhound({
                datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.Path); },
                queryTokenizer: Bloodhound.tokenizers.nonword,
                prefetch: "/WorkflowDefinition/GetVariablePath/" + id

            });
        members.initialize();
        $(element).typeahead({ highlight: true }, {
            name: "schema_paths" + id,
            displayKey: "Path",
            source: members.ttAdapter()
        })
            .on("typeahead:closed", function () {
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
                    $("a.edit-activity").popover("hide");
                    div.find("a.edit-activity").addClass(act.WebId());
                    div.find("a.delete-activity").addClass(act.WebId());
                    div.find("a.start-activity").addClass(act.WebId());
                    return div.find("div.context-menu").html();
                }
            });

        // just display it for 5 seconds
        pop.on("shown.bs.popover", function () {
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
            context = require("services/datacontext"),
            displayPath = ko.unwrap(lookup.displayPath),
            valuePath = ko.unwrap(lookup.valuePath);

        var setup = function (query) {

            //getListAsync
            var promise = ko.unwrap(lookup.valuePath) === ko.unwrap(lookup.displayPath) ?
                context.getListAsync( ko.unwrap(lookup.entity), query, ko.unwrap(lookup.valuePath)) :
                context.getTuplesAsync({
                    entity: ko.unwrap(lookup.entity),
                    query: query,
                    field: valuePath,
                    field2: displayPath
                });

            
                promise.done(function(list) {
                    element.options.length = 0;
                    if (caption) {
                        element.add(new Option(caption, ""));
                    }
                    _(list).each(function (v) {
                        if (typeof v === "string") {
                            element.add(new Option(v, v));
                            return;
                        }
                        if (typeof v.Item1 === "string" && typeof v.Item2 === "string") {
                            element.add(new Option(v.Item2, v.Item1));
                            return;
                        }
                        if (typeof v[valuePath] === "string" && typeof v[displayPath] === "string") {
                            element.add(new Option(v[valuePath], v[displayPath]));
                            return;
                        }
                    });

                    $select.val(ko.unwrap(value))
                        .on("change", function() {
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


ko.bindingHandlers.solutiontree = {
    init: function (element, valueAccessor) {
        var value = valueAccessor(),
            //solution = ko.unwrap(value.solution),            
            items = ko.unwrap(value.items),
            transforms = ko.unwrap(value.transforms),
            wds = ko.unwrap(value.wds),
            click = value.click,
            singleClick = value.singleClick,
            addEntityDefinition = value.addEntityDefinition,
            addForm = value.addForm,
            addOperation = value.addOperation,
            addView = value.addView,
            addBusinessRules = value.addBusinessRules;

        var eds = [];
        var treeRoots = [
            { "id": "EntityDefinition", "parent": "#", "text": "Entity Definitions", icon: "fa fa-file", data: { TypeName: "#" } },
            { "id": "WorkflowDefinition", "parent": "#", "text": "Workflow Definitions", icon: "fa fa-code-fork", data: { TypeName: "#" } },
            { "id": "TransformDefinition", "parent": "#", "text": "Transform Definitions", icon: "fa fa-random", data: { TypeName: "#" } },
            { "id": "Adapter", "parent": "#", "text": "Adapters", icon: "fa fa-puzzle-piece", data: { TypeName: "#" } },
            { "id": "CustomForm", "parent": "#", "text": "Custom Forms", icon: "fa fa-file-o", data: { TypeName: "#" } },
            { "id": "CustomDialog", "parent": "#", "text": "Custom Dialog", icon: "fa fa-file-o", data: { TypeName: "#" } },
            { "id": "CustomScript", "parent": "#", "text": "Custom Scripts", icon: "fa fa-file-o", data: { TypeName: "#" } },
            { "id": "PartialView", "parent": "#", "text": "Partial View", icon: "fa fa-file-o", data: { TypeName: "#" } }
        ];

        _.each(items, function (pmd) {
            eds.push({
                id: ko.unwrap(pmd.Id),
                text: ko.unwrap(pmd.Name),
                parent: "EntityDefinition",
                icon: "fa fa-clipboard",
                data: { TypeName: "EntityDefinition" }
            });

            _.each(pmd.Forms, function (forms) {
                eds.push({
                    id: ko.unwrap(forms.Id),
                    text: ko.unwrap(forms.Name),
                    parent: ko.unwrap(pmd.Id),
                    icon: "fa fa-edit",
                    data: { TypeName: "EntityForm" }
                });
            });

            _.each(pmd.Operations, function (op) {
                eds.push({
                    id: ko.unwrap(op.WebId),
                    text: ko.unwrap(op.Name),
                    parent: ko.unwrap(pmd.Id),
                    icon: "fa fa-gavel",
                    data: { TypeName: "EntityOperation", Name: ko.unwrap(op.Name) }

                });
            });

            _.each(pmd.Rdl, function (rdl) {
                eds.push({
                    id: ko.unwrap(rdl.Id),
                    text: ko.unwrap(rdl.Title),
                    parent: ko.unwrap(pmd.Id),
                    icon: "fa fa-clipboard",
                    type: "Rdl"
                });
            });

            _.each(pmd.Triggers, function (trigger) {
                eds.push({
                    id: ko.unwrap(trigger.Id),
                    text: ko.unwrap(trigger.Name),
                    parent: ko.unwrap(pmd.Id),
                    icon: "fa fa-clipboard",
                    data: { TypeName: "Trigger" }
                });
            });

            _.each(pmd.Views, function (view) {
                eds.push({
                    id: ko.unwrap(view.Id),
                    text: ko.unwrap(view.Name),
                    parent: ko.unwrap(pmd.Id),
                    icon: "fa fa-table",
                    data: { TypeName: "EntityView" }
                });
            });

        });

        _.each(transforms, function (pmd) {
            eds.push({
                id: ko.unwrap(pmd.Id),
                text: ko.unwrap(pmd.Name),
                parent: "TransformDefinition",
                icon: "fa fa-clipboard",
                data: { TypeName: "TransformDefinition" }
            });
        });
        _.each(wds, function (pmd) {
            eds.push({
                id: ko.unwrap(pmd.Id),
                text: ko.unwrap(pmd.Name),
                parent: "WorkflowDefinition",
                icon: "fa fa-code-fork",
                data: { TypeName: "WorkflowDefinition" }
            });
        });

        $(element).jstree({
            'core': {
                'data': treeRoots.concat(eds)
            },
            "plugins": ["contextmenu", "search"],
            "contextmenu": {
                "items": function (node) {

                    if (node.id === "EntityDefinition") {
                        return {
                            "Create": {
                                "label": "Add New Entity",
                                "action": function (obj) {
                                    //this.create(obj);
                                    addEntityDefinition();
                                }
                            }
                        };
                    } else if (node.id === "WorkflowDefinition") {
                        return {
                            "Create": {
                                "label": "Add New Workflow Definition",
                                "action": function (obj) {
                                    addWorkflowDefinition();
                                }
                            }
                        };
                    } else if (node.id === "TransformDefinition") {
                        return {
                            "Create": {
                                "label": "Add New Transform Definition",
                                "action": function (obj) {
                                    addTransformDefinition();
                                }
                            }
                        };
                    } else if (node.id === "Adapter") {
                        return {
                            "Create": {
                                "label": "Add New Adapter",
                                "action": function (obj) {
                                    addAdapter();
                                }
                            }
                        };
                    } else if (node.id === "Trigger") {
                        return {
                            "Create": {
                                "label": "Add New Trigger",
                                "action": function (obj) {
                                    addTrigger();
                                }
                            }
                        };
                    } else if (node.parent === "EntityDefinition") {
                        return {
                            "Create Business Rules": {
                                "label": "Add New Business Rules",
                                "action": function (obj) {
                                    addBusinessRules(node.id);
                                    console.log(ob);
                                }
                            },
                            "Create Form": {
                                "label": "Add New Form",
                                "action": function (obj) {
                                    addForm(node.id);
                                }
                            },
                            "Create Views": {
                                "label": "Add New View",
                                "action": function (obj) {
                                    addView(node.id);
                                    console.log(obj);
                                }
                            },
                            "Create Operation": {
                                "label": "Add New Operation",
                                "action": function (obj) {
                                    addOperation(node.id);
                                    console.log(obj);
                                }
                            }
                        };
                    }

                }
            }
        });

        $(element).on("select_node.jstree", singleClick);
        $(element).delegate("a", "dblclick", click);
    }
};

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
                            "items": [
                                {
                                    label: "Add Child",
                                    action: function () {
                                        var child = new bespoke.sph.domain.Member({ WebId: system.guid(), TypeName: "System.String, mscorlib", Name: "Member_Name" }),
                                            parent = $(element).jstree("get_selected", true),
                                            mb = parent[0].data,
                                            newNode = { state: "open", type: "System.String, mscorlib", text: "Member_Name", data: child };

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
                    if (ix === "$type") continue;
                    if (ix === "addChildItem") continue;
                    if (ix === "removeChildItem") continue;
                    c.options.push("" + ix);
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
/// <reference path="knockout-3.2.0.debug.js" />
/// <reference path="underscore.js" />
/// <reference path="moment.js" />
/// <reference path="~/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="~/Scripts/require.js" />
/// <reference path="~/kendo/js/kendo.all.js" />
/// <reference path="_pager.js" />
/// <reference path="/SphApp/services/datacontext.js" />
/// <reference path="/SphApp/objectbuilders.js" />



ko.bindingHandlers.kendoEditor = {
    init: function (element, valueAccessor) {
        var $editor = $(element),
            value = valueAccessor(),
            updating = false;

        setTimeout(function () {
            var editor = $editor.kendoEditor({
                change: function () {
                    if (updating) return;
                    updating = true;
                    value(this.value());
                    setTimeout(function () { updating = false; }, 500);
                }
            }).data("kendoEditor");

            editor.value(value());

            value.subscribe(function (html) {
                if (updating) return;
                updating = true;
                editor.value(html);
                setTimeout(function () { updating = false; }, 500);
            });

        }, 500);
    },
    update2: function (element, valueAccessor) {
        var $editor = $(element),
            value = valueAccessor(),
            ke = $editor.data("kendoEditor");
        if (!$editor.data("updating")) {
            ke.value(value());
        }
    }
};

ko.bindingHandlers.kendoDropDownListValue = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        var currentModelValue = ko.utils.unwrapObservable(value);
        var dd = $(element).data("kendoDropDownList");
        dd.value(currentModelValue);

        dd.bind("change", function () {
            var nv = dd.value();
            value(nv);
        });
    },

    update: function (element, valueAccessor) {
        //update value based on a model change
        var value = valueAccessor();
        var modelValue = ko.utils.unwrapObservable(value);

        if (modelValue) {
            $(element).data("kendoDropDownList").value(modelValue);
        }
    }
};

ko.bindingHandlers.source = {
};
ko.bindingHandlers.kendoComboBox = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            currentModelValue = ko.utils.unwrapObservable(value),
            dd = $(element).data("kendoComboBox") ||
                $(element).kendoComboBox({
                    dataSource: allBindings.source()
                }).data("kendoComboBox");

        dd.value(currentModelValue);
        allBindings.source.subscribe(function (options) {
            // console.log(options, dd);
            dd.dataSource.data(options);
        });
        dd.bind("change", function () {
            var nv = dd.value();
            value(nv);
        });
    },

    update: function (element, valueAccessor) {
        //update value based on a model change
        var value = valueAccessor();
        var modelValue = ko.utils.unwrapObservable(value);

        if (modelValue) {
            $(element).data("kendoComboBox").value(modelValue);
        }
    }
};


ko.bindingHandlers.decimal = {};
ko.bindingHandlers.money = {
    init: function (element, valueAccessor, allBindingsAccessor) {

        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            decimal = function () {
                if (typeof allBindings.decimal === "undefined") {
                    return 2;
                }
                return parseInt(allBindings.decimal);
            },
            textbox = $(element),
            val = parseFloat(ko.unwrap(value) || "0"),
            fm = val.toFixed(decimal()).replace(/./g, function (c, i, a) {
                return i && c !== "." && !((a.length - i) % 3) ? "," + c : c;
            });


        if (element.tagName.toLowerCase() === "span") {
            textbox.text(fm);
            return;
        }

        textbox.val(fm);

        textbox.on("blur", function () {
            var tv = $(this).val().replace(/,/g, "");
            console.log(tv);
            value(parseFloat(tv));
        });

    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            decimal = function () {
                if (typeof allBindings.decimal === "undefined") {
                    return 2;
                }
                return parseInt(allBindings.decimal);
            },
             textbox = $(element),
             val = parseFloat(ko.unwrap(value) || "0"),
             fm = val.toFixed(decimal()).replace(/./g, function (c, i, a) {
                 return i && c !== "." && !((a.length - i) % 3) ? "," + c : c;
             });

        textbox.val(fm);

    }
};

///user moment format
ko.bindingHandlers.date = {
    init: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());

        if (!value) {
            $(element).text("");
            $(element).val("");
            return;
        }

        var dv = ko.unwrap(value.value),
        inputFormat = ko.unwrap(value.inputFormat) || "YYYY-MM-DD",
        date = moment(dv, inputFormat),
        invalid = ko.unwrap(value.invalid) || "invalid date",
        format = ko.unwrap(value.format) || "DD/MM/YYYY";

        if (!value.format && typeof ko.unwrap(value) === "string") {
            dv = ko.unwrap(value);
            date = moment(dv);
        }

        $(element).on("change", function () {
            var nv = $(this).val();
            value.value(nv);
        });
        if (!dv) {
            $(element).text(invalid);
            $(element).val(invalid);
            return;
        }
        if (!date) {
            $(element).text("");
            $(element).val("");
            return;
        }
        if (date.year() === 1) { // DateTime.Min
            $(element).text("");
            $(element).val("");
            return;
        }


        var dateString = date.format(format).toString();
        if (dateString.indexOf("NaN") < 0) {
            $(element).text(dateString);
            $(element).val(dateString);
        }



    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());

        if (!value) {
            $(element).text("");
            $(element).val("");
            return;
        }

        var dv = ko.unwrap(value.value),
        inputFormat = ko.unwrap(value.inputFormat) || "YYYY-MM-DD",
        date = moment(dv, inputFormat),
        invalid = ko.unwrap(value.invalid) || "invalid date",
        format = ko.unwrap(value.format) || "DD/MM/YYYY";

        if (!value.format && typeof ko.unwrap(value) === "string") {
            dv = ko.unwrap(value);
            date = moment(dv);
        }

        $(element).on("change", function () {
            var nv = $(this).val();
            value.value(nv);
        });
        if (!dv) {
            $(element).text(invalid);
            $(element).val(invalid);
            return;
        }
        if (!date) {
            $(element).text("");
            $(element).val("");
            return;
        }
        if (date.year() === 1) { // DateTime.Min
            $(element).text("");
            $(element).val("");
            return;
        }


        var dateString = date.format(format).toString();
        if (dateString.indexOf("NaN") < 0) {
            $(element).text(dateString);
            $(element).val(dateString);
        }



    }
};


ko.bindingHandlers.kendoUpload = {
    init: function (element, valueAccessor) {
        var context = require(objectbuilders.datacontext),
             logger = require(objectbuilders.logger),
             value = valueAccessor(),
             options = valueAccessor(),
                 extensions = [];
        if (options && typeof options === "object") {
            value = options.value;
            extensions = options.extensions;
        }

        $(element).attr("name", "files").kendoUpload({
            async: {
                saveUrl: "/BinaryStore/Upload",
                removeUrl: "/BinaryStore/Remove",
                autoUpload: true
            },
            multiple: false,
            error: function (e) {
                logger.logError(e, e, this, true);
            },
            select: function (e) {
                if (extensions.length === 0) {
                    return;
                }
                _(e.files).each(function (v) {
                    if (extensions.indexOf(v.extension) < 0) {
                        logger.error("Only " + extensions.join(",") + " files can be uploaded");
                        e.preventDefault();
                    }
                });
            },
            success: function (e) {
                logger.info("Your file has been " + e.operation);

                var storeId = e.response.storeId,
                    uploaded = e.operation === "upload",
                    removed = e.operation !== "upload",
                    oldFile = value();
                if (uploaded) {
                    value(storeId);
                    if (oldFile) {
                        context.post(JSON.stringify({ id: oldFile }), "/BinaryStore/Remove/");
                    }
                }
                if (removed) {
                    value("");
                }
            },
            remove: function () {
                var tcs = new $.Deferred(),
                    data = JSON.stringify({ id: value() });
                context.post(data, "/BinaryStore/Remove/")
                    .then(function (result) {
                        tcs.resolve(result);
                    });
                return tcs.promise();
            }
        });
    }
};

///user moment format
ko.bindingHandlers.kendoDate = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            $input = $(element),
            allBindings = allBindingsAccessor(),
            currentValue = ko.unwrap(value),
            date = moment(currentValue, "DD/MM/YYYY"),
            changed = function (e) {
                console.log(e);
                var nv = this.value();
                if (typeof nv == "string") {
                    date = moment(nv, "DD/MM/YYYY");
                } else {
                    date = moment(nv);
                }
                // DO NOT fire update
                $input.data("stop", "true");
                value(date.format("YYYY-MM-DD"));
                $input.data("stop", "false");

            },
            picker = $input.kendoDatePicker({ format: "dd/MM/yyyy", change: changed }).data("kendoDatePicker");

        if (typeof allBindings.enable === "boolean") {
            if (!allBindings.enable) {
                picker.enable(false);
            }
        }
        if (typeof allBindings.enable === "function" && typeof allBindings.enable.subscribe === "function") {
            allBindings.enable.subscribe(function (enable) {
                picker.enable(enable);
            });
        }

        if (!date) {
            picker.value(null);
            return;
        }

        if (date.year() === 1) { // DateTime.Min
            picker.value(null);
            return;
        }

        picker.value(date.toDate());
        if (typeof currentValue === "undefined" && typeof value === "function") {
            value(date.format("YYYY-MM-DD"));
        }
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var $input = $(element),
            allBindings = allBindingsAccessor();
        if ($input.data("stop") === "true") return;

        var value = valueAccessor(),
            modelValue = ko.utils.unwrapObservable(value),
            date = moment(modelValue),
            picker = $input.data("kendoDatePicker");

        if (!date) {
            picker.value(null);
            return;
        }
        if (date.year() === 1) { // DateTime.Min
            picker.value(null);
            return;
        }

        if (typeof allBindings.enable === "boolean") {
            if (!allBindings.enable) {
                picker.enable(false);
            }
        }
        if (typeof allBindings.enable === "function" && typeof allBindings.enable.subscribe === "function") {
            allBindings.enable.subscribe(function (enable) {
                picker.enable(enable);
            });
        }
        picker.value(date.toDate());

    }
};


///user moment format
ko.bindingHandlers.kendoDateTime = {
    init: function (element, valueAccessor) {
        var value = valueAccessor(),
            $input = $(element),
            currentValue = ko.utils.unwrapObservable(value),
            date = moment(currentValue, "DD/MM/YYYY "),
            changed = function (e) {
                console.log(e);
                var nv = this.value();
                if (typeof nv == "string") {
                    date = moment(nv, "DD/MM/YYYY hh:mm");
                } else {
                    date = moment(nv);
                }
                // DO NOT fire update
                $input.data("stop", "true");
                value(date.format());
                $input.data("stop", "false");

            },
            picker = $input.kendoDateTimePicker({ format: "dd/MM/yyyy HH:mm", change: changed }).data("kendoDateTimePicker");

        if (!date) {
            picker.value(null);
            return;
        }

        if (date.year() === 1) { // DateTime.Min
            picker.value(null);
            return;
        }

        picker.value(date.toDate());
    },
    update: function (element, valueAccessor) {
        var $input = $(element);
        if ($input.data("stop") === "true") return;

        var value = valueAccessor(),
            modelValue = ko.utils.unwrapObservable(value),
            date = moment(modelValue),
            picker = $input.data("kendoDateTimePicker");

        if (!date) {
            picker.value(null);
            return;
        }
        if (date.year() === 1) { // DateTime.Min
            picker.value(null);
            return;
        }

        picker.value(date.toDate());

    }
};

ko.bindingHandlers.slideVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(value));
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
    }
};

ko.bindingHandlers.kendoEnable = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        var enable = ko.utils.unwrapObservable(value);
        if (enable) {
            $(element).removeClass("k-state-disabled");
        } else {
            $(element).addClass("k-state-disabled");
        }
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        var enable = ko.utils.unwrapObservable(value);
        if (enable) {
            $(element).removeClass("k-state-disabled");
            $(element).removeAttr("disabled");
        } else {
            $(element).addClass("k-state-disabled");
            $(element).attr("disabled", "disabled");
        }
    }
};

ko.bindingHandlers.command = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var action = valueAccessor(),
            $button = $(element),
            allBindings = allBindingsAccessor(),
            inputValue = $button.val(),
            logger = require("services/logger");

        if (allBindings.isvisible) {
            var visible = typeof allBindings.isvisible === "function" ? allBindings.isvisible() : allBindings.isvisible;
            if (!visible) {
                $button.hide();
            } else {
                $button.show();
            }

            if (typeof allBindings.isvisible === "function") {
                allBindings.isvisible.subscribe(function (v) {
                    if (v)
                        $button.show();
                    else
                        $button.hide();
                });
            }
        }

        var $spinner = $("<i class='fa fa-spin fa-spinner'></i>").hide(),
            $warning = $("<i class='fa fa-warning' style='color:red'></i>").hide();
        $button.append($spinner);
        $button.append($warning);

        $button.click(function (e) {
            e.preventDefault();
            if (this.form) {
                if (!this.form.checkValidity()) return;
            }
            $spinner.show();
            $warning.hide();

            action()
                .fail(function (err, o, message) {
                    $button
                       .button("complete")
                       .prop("disabled", false)
                       .val(inputValue)
                       .removeClass("btn-disabled");
                    $spinner.hide();
                    $warning.show();
                    if (err.status === 404) {
                        logger.error(message);
                        logger.error(err.statusText);
                        return;
                    }
                    if (err.responseText) {
                        logger.error(err.responseText);
                        console.error(err.responseText);
                    }
                    if (err.responseJSON) {
                        logger.error(JSON.stringify(err.responseJSON));
                        console.error(err.responseJson);
                    }
                })
                .done(function () {
                    $button
                        .button("complete")
                        .prop("disabled", false)
                        .val(inputValue)
                        .removeClass("btn-disabled");
                    $spinner.hide();
                });
            if ($button.data("loading-text")) {
                $button.button("loading");
            }
            $button.addClass("btn-disabled").prop("disabled", true);

        });
    }
};

ko.bindingHandlers.field = {
    init: function () {
    }
};
ko.bindingHandlers.unwrapClick = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var action = valueAccessor(),
            button = $(element),
            allBindings = allBindingsAccessor();


        button.click(function (e) {
            e.preventDefault();
            var prop = allBindings.property,
                accessor = allBindings.accessor,
                type = allBindings.field,
                entity = allBindings.entity;
            /* if we can't get to the function , i.e. it's still object not ko.observable
            
            */
            if (prop) {
                if (typeof accessor[prop] === "undefined") {
                    console.log("Can't figure out the accessor.prop");
                }
                if (typeof accessor[prop] === "object") {
                    accessor[prop] = ko.observable(accessor[prop]);
                }
                if (typeof accessor[prop] === "function") {
                    action(accessor[prop], type, entity);
                }
            } else {
                action(accessor, type, entity);
            }

        });
    }
};

ko.bindingHandlers.fieldImage = {
    init: function (element, valueAccessor) {
        var type = valueAccessor(),
            img = $(element);

        var ft = typeof type === "function" ? type() : type;
        img.attr("src", "/image/index/" + ft);


    }
};


ko.bindingHandlers.stringArrayAutoComplete = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            options = allBindings.data();

        $(element).data("kendoAutoComplete") ||
           $(element).kendoAutoComplete({
               dataSource: options,
               change: function () {
                   var data = _(this.value().split(",")).filter(function (s) {
                       return s;
                   });
                   value(data);
               },
               filter: "startswith",
               placeholder: "....",
               separator: ","
           }).val(value());
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        $(element).data("kendoAutoComplete").value(value());
    }

};



ko.bindingHandlers.pathAutoComplete = {
    init: function (element, valueAccessor) {
        var command = valueAccessor();
        var value = command.value;
        var type = command.type;

        if (!type()) return;

        $.get("/App/TriggerPathPickerJson/" + type())
            .done(function (json) {
                var tree = JSON.parse(json);
                var data = _.chain(tree)
                    .map(function (t) {
                        if (t.parent === "")
                            return t.name;
                        return undefined;
                    })
                    .filter(function (t) {
                        return typeof t !== "undefined";
                    })
                    .value();

                console.log(data);

                var dataSource = new kendo.data.DataSource({
                    data: data
                });
                var input = $(element).data("kendoAutoComplete") ||
                   $(element).kendoAutoComplete({
                       dataSource: tree,
                       dataTextField: "path",
                       change: function () {
                           var path = this.value();
                           console.log("selected path ", path);

                       },
                       filter: "startswith",
                       placeholder: "Select path...",
                       separator: ""
                   }).data("kendoAutoComplete");

                $(element)
                        .change(function () {
                            value($(this).val());
                            console.log("new value", value());
                        })
                       .val(value())
                    .on("keydown3", function (e) {
                        if (e.which === 110 || e.which === 190) {
                            var path = $(this).val() + ".";
                            console.log("show the auto complete", path);
                            var filtered = _.chain(tree)
                                .filter(function (t) {
                                    return t.parent === path;
                                })
                                .map(function (t) {
                                    return t.name;
                                })
                                .value();
                            console.log(filtered);
                            //input.setDataSource(dataSource);
                            dataSource.data(filtered);
                            input.refresh();
                        }
                    });
            });

    }

};

ko.bindingHandlers.commandWithParameter = {
    init: function (element, valueAccessor) {
        var command = valueAccessor();
        var callback = command.command;
        var parameter = command.commandParameter;

        var button = $(element);
        var completeText = button.data("complete-text") || button.html();
        button.click(function (e) {
            e.preventDefault();
            callback(parameter)
                .then(function () {
                    button.button("complete");
                    if (button.get(0).tagName === "BUTTON" || button.get(0).tagName === "A") {
                        button.html(completeText);
                    } else {
                        button.val(completeText);
                    }

                });
            if (button.data("loading-text")) {
                button.button("loading");
            }
        });
    }

};


ko.bindingHandlers.filter = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            bindingAccessor = allBindingsAccessor(),
            path = value.path,
            tooltip = value.tooltip || "Type to filter current page or type and [ENTER] to search the whole view",
            $element = $(element),
            $filterInput = $("<input data-toggle=\"tooltip\" title=\"" + tooltip + "\" type=\"search\" class=\"search-query input-medium form-control\" placeholder=\"Filter.. \">"),
            $serverLoadButton = $("<a href='/#' title='Carian server'><i class='add-on icon-search'></i><a>"),
            $form = $("<form class='form-search row'>" +
                " <div class='input-group pull-right' style='width:300px'>" +
                "<span class='input-group-addon'>" +
                " <span class='glyphicon glyphicon-remove'></span>" +
                "</span> " +
                "</div>" +
                " </form>"),
            pagedSearch = bindingAccessor.searchPaging;


        $form.find("span.input-group-addon").before($filterInput);
        if (pagedSearch) {
            $form.find("span.glyphicon-remove").after($serverLoadButton);
        }
        $element.before($form);

        $form.submit(function (e) {
            e.preventDefault();
            var filter = $filterInput.val().toLowerCase(),
                tcs = new $.Deferred();
            if (!filter) {
                return tcs.promise();
            }
            if (pagedSearch && typeof pagedSearch.query !== "undefined" && typeof pagedSearch.query.filterAndSearch === "function") {
                return pagedSearch.query.filterAndSearch(filter);
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

    }
};

ko.bindingHandlers.serverPaging = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            entity = value.entity,
            query = value.query,
            list = value.list,
            map = value.map,
            pagerHidden = value.pagerHidden || false,
            $element = $(element),
            context = require("services/datacontext"),
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
            setItemsSource = function (items) {
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
            changed = function (page, size) {
                startLoad();
                context.loadAsync({
                    entity: entity,
                    page: page,
                    size: size,
                    includeTotal: true
                }, query)
                     .then(function (lo) {
                         setItemsSource(lo.itemCollection);
                         endLoad();
                     });
            };

        $element.after($pagerPanel).after($spinner)
            .fadeTo("slow", 0.33);

        var tcs = new $.Deferred();
        context.loadAsync({
            entity: entity,
            page: 1,
            size: 20,
            includeTotal: true
        }, query)
        .then(function (lo) {

            var options = {
                element: $pagerPanel,
                count: lo.rows,
                changed: changed,
                hidden: pagerHidden
            },
                pager = new bespoke.utils.ServerPager(options);
            console.log(pager);
            setTimeout(function () {
                setItemsSource(lo.itemCollection);
                tcs.resolve(true);
                endLoad();
            }, 500);

        });
        return tcs.promise();



    }
};


ko.bindingHandlers.searchPaging = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            entity = value.entity,
            query = value.query,
            executedQuery = value.query || value.initialQuery || {},
            list = value.list,
            map = value.map,
            pagerHidden = value.pagerHidden || false,
            searchButton = value.searchButton,
            $element = $(element),
            context = require("services/datacontext"),
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
                context.searchAsync({
                    entity: entity,
                    page: page,
                    size: size
                }, executedQuery)
                     .then(function (lo) {
                         setItemsSource(lo.itemCollection);
                         endLoad();
                     });
            },
            search = function (q, page, size) {
                executedQuery = q;
                var tcs1 = new $.Deferred();
                startLoad();
                context.searchAsync({
                    entity: entity,
                    page: page || 1,
                    size: size || 20
                }, q)
                    .then(function (lo) {
                        if (pager) {
                            pager.update(lo.rows);
                        } else {
                            var pagerOptions = {
                                element: $pagerPanel,
                                count: lo.rows,
                                changed: pageChanged,
                                hidden: pagerHidden
                            };
                            pager = new bespoke.utils.ServerPager(pagerOptions);

                        }

                        setTimeout(function () {
                            setItemsSource(lo.itemCollection);
                            tcs1.resolve(lo);
                            endLoad();
                        }, 500);

                    });
                return tcs1.promise();
            },
            filterAndSearch = function (text) {
                var q = JSON.parse(ko.toJSON(value.query || value.initialQuery)),
                    q2 = {
                        "from": 0,
                        "size": 20,
                        "query": {}
                    };
                q.query = q.query || {};
                q.query.filtered = q.query.filtered || {};

                q2.query.filtered = q.query.filtered;
                q2.query.filtered.query = {
                    "query_string": {
                        "default_field": "_all",
                        "query": text
                    }
                };
                q2.sort = q.sort;
                pager.destroy();
                pager = null;
                search(q2);
            };

        //exposed the search function
        query.search = search;
        query.filterAndSearch = filterAndSearch;

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
/// <reference path="typeahead.bundle.js" />
/// <reference path="knockout-3.2.0.debug.js" />
/// <reference path="underscore.js" />
/// <reference path="jquery-2.1.1.intellisense.js" />

ko.bindingHandlers.bootstrapDropDown = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor()) || '[Select your value]',
            anchor = $(element),
            opened = false,
            dropDown = function (e) {
                e.preventDefault();
                e.stopPropagation();
                var button = $(this);

                if (opened) {
                    button.parent().removeClass("open");
                    opened = false;

                } else {
                    button.parent().addClass("open");
                    opened = true;
                    $(document).one('click', function () {
                        button.parent().removeClass("open");
                        opened = false;
                    });
                }


            };
        anchor.click(dropDown);

        if (!anchor.html()) {
            if (text.toString() === "[object Object]") {
                text = "[Select you value]";
            }
            anchor.html(text + ' <i class="fa fa-caret-down"></i>');
        }
    }
};

ko.bindingHandlers.tooltip = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor());
        $(element).tooltip({ title: text });
    }
};

ko.bindingHandlers.bootstrapPopover = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor());
        $(element).popover({ content: '<pre>' + text + '</pre>', html: true });
    }
};
ko.bindingHandlers.popover = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor());
        $(element).popover({ content: '<pre>' + text + '</pre>', html: true });
    }
};
ko.bindingHandlers.bootstrapTooltip = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor());
        $(element).tooltip({ title: text });
    }
};




ko.bindingHandlers.cssAutoComplete = {
    init: function (element, valueAccessor) {
        var value = valueAccessor(),
            bootstrap = _(document.styleSheets).find(function (s) {
                // TODO : what happend if were to combine the css with Bundle
                if (!s) return false;
                if (!s.href) return false;

                return s.href.indexOf("bootstrap") > -1;
            });
        var data = ['btn', 'btn-warning', 'btn-success', 'btn-link'];
        if (bootstrap) {
            data = _.chain(bootstrap.rules).filter(function (r) {
                return /^\./g.test(r.selectorText)
                    && !/:/g.test(r.selectorText)
                    && !/\s/g.test(r.selectorText)
                    && !/\+/g.test(r.selectorText)
                    && !/>/g.test(r.selectorText)
                    && !/\[/g.test(r.selectorText);
            }).map(function (s) {
                return s.selectorText.replace(/\./g, "");
            })
                .value();
        }

        $(element).typeahead({
            name: 'css_class',
            limit: 10,
            local: data
        })
        .on('typeahead:closed', function () {
            value($(this).val());
        });


    }

};



ko.bindingHandlers.autocomplete = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var va = ko.unwrap(valueAccessor()),
         entity = ko.unwrap(va.entity),
         field = ko.unwrap(va.field),
         query = ko.unwrap(va.query),
         ttl = va.ttl || 300000,
         allBindings = allBindingsAccessor(),
         url = String.format("/list?table={0}&column={1}&filter={2}", entity, field, query),
         suggestions = new Bloodhound({
             datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
             queryTokenizer: Bloodhound.tokenizers.whitespace,
             prefetch: {
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
        $(element).typeahead(null, {
            name: 'autocomplete_' + $(element).prop("id"),
            displayKey: "name",
            source: suggestions.ttAdapter()
        })
           .on('typeahead:closed', function () {
               allBindings.value($(this).val());
           });

    }
};


ko.bindingHandlers.scroll = {
    init: function (element, valueAccessor) {
        var rows = ko.unwrap(valueAccessor()),
            done = false;
        // wait for attached
        //setTimeout(function () {
        //    // TODO : just do it this once rows is more than certail values
        //    $(element).tableScroll({ height: height });
        //    done = true;
        //}, 500);
    }
};

(function () {
    Function.prototype.partial = function () {
        var fn = this, args = Array.prototype.slice.call(arguments);
        return function () {
            var arg = 0;
            for (var i = 0; i < args.length && arg < arguments.length; i++) {
                if (args[i] === undefined) {
                    args[i] = arguments[arg++];
                }
            }
            return fn.apply(this, args);
        };
    };

})();
(function (window) {
    window.bespoke = window.bespoke || {};
    window.bespoke.ServerOperationStatus = {        
        OK: "OK",
        ERROR : "ERROR"
    };

    window.bespoke.messagesText = {
        SAVE_SUCCESS : "Your item has been saved"
    };

})(window);

/// <reference path="bootstrap.js" />
/// <reference path="google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="bootstrap-datepicker.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery-1.9.1.intellisense.js" />
/// <reference path="moment.js" />
/// <reference path="underscore.js" />
/// <reference path="modernizr-2.6.2.js" />


var _uiready = function () {
    var init = function (view) {
    };
    return {
        init: init
    };

}();

(function (window, $) {
    window.bespoke = window.bespoke || {};
    bespoke.utils = bespoke.utils || {};
    bespoke.utils.form = bespoke.utils.form || {};
    bespoke.utils.form.checkValidity = function (button) {
        if (button.form)
            return button.form.checkValidity();
        var form = $(button).attr('form');
        if (form) {
           return document.getElementById(form).checkValidity();
        }
        throw "cannot find the form for the button";
    };


    window.console = window.console || {};
    window.console.dir = window.console.dir || function (){};


})(window, jQuery);
(function (window, $) {
    window.Task = window.Task || {};
    window.Task.fromResult = function (returnValue, delay) {
        var tcs = new $.Deferred(),
            ret = returnValue || true,
            d = delay || 100;

        setTimeout(function () {
            tcs.resolve(ret);
        }, d);
        return tcs.promise();

    };

})(window, jQuery);


ko.bindingHandlers.theme = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        // First get the latest data that we're bound to
        var value = valueAccessor(), allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.unwrap(value);
        if(bespoke.sph.Theme) {
            var theme = new bespoke.sph.Theme();
            if (theme[valueUnwrapped]) {
                theme[valueUnwrapped](element, viewModel, allBindings, bindingContext);
            }
            
        }

    }
};
/// <reference path="jquery-2.1.3.js" />
/// <reference path="jquery-ui-1.11.1.js" />
/// <reference path="modernizr-2.8.3.js" />
/// <reference path="bootstrap.js" />
/// <reference path="breeze.debug.js" />
/// <reference path="knockout-2.2.1.debug.js" />
/// <reference path="moment.js" />
/// <reference path="q.js" />
/// <reference path="toastr-1.1.5.js" />
