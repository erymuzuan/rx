/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../../core.sph/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../../core.sph/Scripts/jstree.min.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            operationEndpoints = ko.observableArray(),
            queryEndpoints = ko.observableArray(),
            entities = ko.observableArray(),
            permissions = ko.observableArray(),
            selected = ko.observable(),
            activate = function () {
                return context.loadAsync("OperationEndpoint")
                    .then(function (lo) {
                        operationEndpoints(lo.itemCollection);
                        return context.loadAsync("QueryEndpoint");
                    })
                    .then(function (lo) {
                        queryEndpoints(lo.itemCollection);
                        return context.loadAsync("EntityDefinition");
                    })
                    .then(function (lo) {
                        entities(lo.itemCollection);
                    });
            },
            singleClick = function (e, data) {
                selected(data.node.data);
            },
            click = function (e) {
                e.stopPropagation();
                var data = selected().node.data;
                console.log(data);
            },
            attached = function (view) {
                var $panel = $(view).find("#endpoints-tree-panel"),
                    items = _(ko.unwrap(entities)).map(function (v) {
                        return {
                            data: {
                                Parent: ko.unwrap("#"),
                                Controller: ko.unwrap(v.Name),
                                Claims : ko.observableArray()
                            },
                            parent: "#",
                            text: ko.unwrap(v.Name),
                            icon: ko.unwrap(v.IconClass),
                            children: []
                        };
                    });
                _(queryEndpoints()).each(function (v) {
                    var parent = _(items).find(function (k) {
                        return k.text === ko.unwrap(v.Entity);
                    });
                    if (!parent) {
                        return;
                    }
                    var q = {
                        data: {
                            Parent: ko.unwrap(v.Entity),
                            Controller: ko.unwrap(),
                            Action : ko.unwrap(),
                            Claims: ko.observableArray()
                        },
                        text: ko.unwrap(v.Name),
                        icon: "fa fa-cubes"
                    };
                    parent.children.push(q);

                });
                _(operationEndpoints()).each(function (v) {
                    var parent = _(items).find(function (k) {
                        return k.text === ko.unwrap(v.Entity);
                    });
                    if (!parent) {
                        return;
                    }
                    var q = {
                        data: {
                            Parent: ko.unwrap(v.Entity),
                            Controller: ko.unwrap(),
                            Action : ko.unwrap(),
                            Claims: ko.observableArray()
                        },
                        text: ko.unwrap(v.Name),
                        icon: "fa fa-cogs"
                    };
                    parent.children.push(q);

                });

                $panel.jstree({
                    'core': {
                        'data': items
                    },
                    "search": {
                        "case_sensitive": false,
                        "show_only_matches": true,
                        "show_only_matches_children": true,
                        "search_callback": function (text, node) {
                            return (node.text.indexOf(text) > -1);
                        }
                    },
                    "plugins": ["search"]
                });

                $panel.on("select_node.jstree", singleClick);
                $panel.delegate("a", "dblclick", click);
            },
            addClaims = function() {
                selected().Claims.push({
                    Type : ko.observable(),
                    Value: ko.observable(),
                    Permission : ko.observable("Inherited allow")
                });
            },
            save = function() {
                return Task.fromResult(1);
            };

        return {
            operationEndpoints: operationEndpoints,
            queryEndpoints: queryEndpoints,
            selected: selected,
            addClaims: addClaims,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar : {
                saveCommand : save
            }
        };
    });
