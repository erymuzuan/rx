﻿
/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../../core.sph/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../../core.sph/Scripts/jstree.min.js" />
/// <reference path="../../../core.sph/Scripts/_task.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />

/*globals define, console*/

/**
 * @param {{ itemCollection:function, Entity:object, Parent:function, Tables:function, Actions:function, IsHttpPost:function,IsHttpPut:function
 * IsHttpPatch:function,IsHttpDelete:function, IconClass:function, Children:function, Operations:function, Controller:function, Claims:function,Action:function,
 * ControllerName : function}} lo
 *
 * @param {{fromResult: function}} Task
 */
define([objectbuilders.app, "services/datacontext", "services/logger", "plugins/router", "knockout", "jquery", "Task", "underscore"],
    function (app, context, logger, router, ko, $) {
        "use strict";
        const isBusy = ko.observable(false),
            operationEndpoints = ko.observableArray(),
            queryEndpoints = ko.observableArray(),
            entities = ko.observableArray(),
            wds = ko.observableArray(),
            adapters = ko.observableArray(),
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
                        return $.getJSON("/management-api/workflow-endpoints");
                    })
                    .then(function (lo) {
                        wds(lo);
                        return $.getJSON("/management-api/adapter-endpoints");
                    })
                    .then(function (results) {
                        adapters(results);
                        return $.getJSON("/management-api/endpoint-permissions");
                    })
                    .then(permissions);
            },
            singleClick = function (e, data) {
                var tag = data.node.data;
                if (!tag) {
                    return Task.fromResult(0);
                }
                isBusy(true);

                var url = "/management-api/endpoint-permissions/";
                if (tag.parent) {
                    url += `?parent=${tag.parent}`;
                }
                if (tag.controller) {
                    url += `&controller=${tag.controller}`;
                }

                if (tag.action) {
                    url += `&action=${tag.action}`;
                }
                return $.getJSON(url)
                    .done(function (result) {
                        isBusy(false);
                        var item = ko.mapping.fromJS(result);
                        // if no parent, controller and action is set, the api will return all settings
                        if (_(result).isArray()) {
                            const defaultItem = _(result).find(v => !v.Parent && !v.Controller && !v.Action);
                            item = ko.mapping.fromJS(defaultItem);
                        }
                        if (ko.isObservable(item.Parent)) {
                            item.Parent(tag.parent);
                        }
                        if (ko.isObservable(item.Controller)) {
                            item.Controller(tag.controller);
                        }
                        if (ko.isObservable(item.Action)) {
                            item.Action(tag.action);
                        }

                        var claimChanged = function () {
                            item.IsInherited(false);
                        };

                        item.Claims.subscribe(function (changes) {
                            console.log("changes", changes);
                            if (changes[0].status === "added") {
                                changes[0].value.Value.subscribe(claimChanged);
                                changes[0].value.Type.subscribe(claimChanged);
                                changes[0].value.Permission.subscribe(claimChanged);
                            }
                        }, null, "arrayChange");


                        selected(item);
                    });

            },
            click = function (e) {
                e.stopPropagation();
                var data = selected().node.data;
                console.log(data);
            },
            createTag = function (parent, controller, action) {
                return {
                    parent: ko.unwrap(parent),
                    controller: ko.unwrap(controller),
                    action: ko.unwrap(action)
                };
            },
            hasImplementation = function (parent, controller, action) {
                const perm = _(ko.unwrap(permissions)).find(function (v) {
                    if (action) {
                        return v.Parent === ko.unwrap(parent) && v.Controller === ko.unwrap(controller) && v.Action === ko.unwrap(action);
                    }
                    if (controller) {
                        return v.Parent === ko.unwrap(parent) && v.Controller === ko.unwrap(controller);
                    }
                    if (parent) {
                        return v.Parent === ko.unwrap(parent);
                    }
                    return !v.Parent && !v.Controller && !v.Action;
                });
                if (perm) {
                    return " has-implementation";
                }
                return "";
            },
            attached = function (view) {
                const defaultItem = _(ko.unwrap(permissions)).find(v => !v.Parent && !v.Controller && !v.Action);
                selected(ko.mapping.fromJS(defaultItem));

                const $panel = $(view).find("#endpoints-tree-panel"),
                    root = {
                        parent: "#",
                        data: createTag(null, null, null),
                        text: "Default Setting",
                        icon: "fa fa-share-alt",
                        state: {
                            opened: true,
                            selected: true
                        },
                        children: []
                    },
                    items = [root];
                _(ko.unwrap(entities)).each(function (v) {

                    const serviceContractController = ko.unwrap(v.Name) + "ServiceContract",
                        entityNode = {
                            data: createTag(v.Name),
                            parent: "root",
                            text: ko.unwrap(v.Name),
                            icon: ko.unwrap(v.IconClass),
                            state: {
                                opened: false
                            },
                            a_attr: {
                                "class": hasImplementation(v.Name)
                            },
                            children: [{
                                data: createTag(v.Name, serviceContractController, "Search"),
                                text: "Search",
                                a_attr: {
                                    "class": hasImplementation(v.Name, serviceContractController, "Search")
                                },
                                icon: "fa fa-search"
                            }, {
                                data: createTag(ko.unwrap(v.Name), serviceContractController, "GetOneByIdAsync"),
                                text: "GetOneByIdAsync",
                                a_attr: {
                                    "class": hasImplementation(ko.unwrap(v.Name), serviceContractController, "GetOneByIdAsync")
                                },
                                icon: "fa fa-file-o"
                            }, {
                                data: createTag(ko.unwrap(v.Name), serviceContractController, "OdataApi"),
                                text: "OdataApi",
                                a_attr: {
                                    "class": hasImplementation(ko.unwrap(v.Name), serviceContractController, "OdataApi")
                                },
                                icon: "fa fa-database"
                            }]
                        };

                    root.children.push(entityNode);
                });
                _(ko.unwrap(wds)).each(function (v) {
                    var wdNode = {
                        data: createTag(v.Name),
                        parent: "root",
                        text: ko.unwrap(v.Name),
                        icon: "fa fa-code-fork",
                        state: {opened: false},
                        a_attr: {
                            "class": hasImplementation(v.Name)
                        },
                        children: [{
                            data: createTag(v.Name, v.Name, "GetOneAsync"),
                            text: "Get by id",
                            icon: "fa fa-file-o"
                        }, {
                            data: createTag(v.Name, v.Name, "Search"),
                            text: "Search",
                            icon: "fa fa-search"
                        },
                            {
                                data: createTag(v.Name, v.Name, "GetPendingTasksAsync"),
                                text: "Pending Tasks",
                                icon: "fa fa-users"
                            },
                            {
                                data: createTag(v.Name, v.Name, "Schemas"),
                                text: "Javascript schema",
                                icon: "fa fa-object-ungroup"
                            }]
                    };
                    _(v.Children).each(function (c) {
                        const action = {
                            data: createTag(v.Name, v.Name, c.Action),
                            text: c.Action,
                            icon: "fa fa-envelope"
                        };
                        wdNode.children.push(action);
                    });
                    root.children.push(wdNode);
                });


                _(ko.unwrap(adapters)).each(function (adp) {
                    var wdNode = {
                        data: createTag(adp.Name),
                        parent: "root",
                        text: ko.unwrap(adp.Name),
                        icon: "fa fa-database",
                        state: {opened: false},
                        a_attr: {
                            "class": hasImplementation(adp.Name)
                        },
                        children: []
                    };
                    _(adp.Operations).each(function (op) {
                        const action = {
                            data: createTag(adp.Name, adp.Name, op),
                            text: op,
                            a_attr: {
                                "class": hasImplementation(adp.Name, adp.Name, op)
                            },
                            icon: "fa fa-bolt"
                        };
                        wdNode.children.push(action);
                    });
                    _(adp.Tables).each(function (table) {

                        const actions = _(table.Actions).map(function(ctrlAction) {
                                return {
                                    data: createTag(adp.Name, table.Name, ctrlAction),
                                    text: ctrlAction,
                                    icon: "fa fa-bolt"
                                };
                            }),
                            tableController = {
                            data: createTag(adp.Name, table.Name, null),
                            text: table.Name,
                            icon: "fa fa-list",
                            children: actions
                        };
                        wdNode.children.push(tableController);
                    });
                    root.children.push(wdNode);
                });

                //custom
                const customs = permissions().filter(x => x.Parent === "Custom");
                root.children.push({
                    data: createTag("Custom"),
                    id : "custom-tag",
                    parent: "root",
                    text: "Custom",
                    icon: "fa fa-code",
                    state: { opened: false },
                    a_attr: {
                        "class": hasImplementation("Custom")
                    },
                    children: customs.map(x =>({
                        data: createTag("Custom", x.Controller),
                        parent: "custom-tag",
                        id: `custom-tag-${x.Controller}`,
                        text: x.Controller,
                        icon: "fa fa-code",
                        state: { opened: false },
                        a_attr: {
                            "class": hasImplementation("Custom", x.Controller)
                        }
                    }))
                });

                _(queryEndpoints()).each(function (v) {
                    const parent = _(root.children).find(k => k.text === ko.unwrap(v.Entity));
                    if (!parent) {
                        return;
                    }


                    const qeController = ko.unwrap(v.ControllerName),
                        q = {
                            data: createTag(v.Entity, qeController),
                            text: ko.unwrap(v.Name),
                            icon: "fa fa-cubes",
                            a_attr: {
                                "class": hasImplementation(v.Entity, qeController)
                            },
                            children: []
                        };
                    parent.children.push(q);

                    q.children.push({
                        data: createTag(v.Entity, qeController, "GetAction"),
                        text: "GetAction",
                        a_attr: {
                            "class": hasImplementation(v.Entity, qeController, "GetAction")
                        },
                        icon: "fa fa-list"
                    });
                    q.children.push({
                            data: createTag(v.Entity, qeController, "GetCount"),
                            text: "GetCount",
                            a_attr: {
                                "class": hasImplementation(v.Entity, qeController, "GetCount")
                            },
                            icon: "fa fa-tachometer"
                        }
                    );

                });

                _(operationEndpoints()).each(function (v) {
                    var parent = _(root.children).find(k => k.text === ko.unwrap(v.Entity));
                    if (!parent) {
                        return;
                    }


                    var
                        oeController = ko.unwrap(v.Entity) + ko.unwrap(v.Name) + "OperationEndpoint",
                        q = {
                            data: createTag(v.Entity, oeController),
                            text: ko.unwrap(v.Name),
                            a_attr: {
                                "class": hasImplementation(v.Entity, oeController)
                            },
                            icon: "fa fa-cogs",
                            children: []
                        };
                    parent.children.push(q);

                    if (ko.unwrap(v.IsHttpPost)) {
                        var postAction = "Post" + ko.unwrap(v.Name),
                            postNode = {
                                data: createTag(v.Entity, oeController, postAction),
                                text: "HTTP POST",
                                a_attr: {
                                    "class": hasImplementation(v.Entity, oeController, postAction)
                                },
                                icon: "fa fa-plus"
                            };
                        q.children.push(postNode);
                    }


                    if (ko.unwrap(v.IsHttpPut)) {
                        var putAction = "Put" + ko.unwrap(v.Name),
                            putNode = {
                                data: createTag(v.Entity, oeController, putAction),
                                text: "HTTP PUT",
                                a_attr: {
                                    "class": hasImplementation(v.Entity, oeController, putAction)
                                },
                                icon: "fa fa-file-text-o"
                            };
                        q.children.push(putNode);
                    }

                    if (ko.unwrap(v.IsHttpPatch)) {
                        var patchAction = "Patch" + ko.unwrap(v.Name),
                            patchNode = {
                                data: createTag(v.Entity, oeController, patchAction),
                                text: "HTTP PATCH",
                                a_attr: {
                                    "class": hasImplementation(v.Entity, oeController, patchAction)
                                },
                                icon: "fa fa-pencil-square"
                            };
                        q.children.push(patchNode);
                    }

                    if (ko.unwrap(v.IsHttpDelete)) {
                        var deleteAction = "Delete" + ko.unwrap(v.Name),
                            deleteNode = {
                                data: createTag(v.Entity, oeController, deleteAction),
                                text: "HTTP DELETE",
                                a_attr: {
                                    "class": hasImplementation(v.Entity, oeController, deleteAction)
                                },
                                icon: "fa fa-minus-circle"
                            };
                        q.children.push(deleteNode);
                    }


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

                var setDesignerHeight = function () {
                    if ($panel.length === 0) {
                        return;
                    }

                    var dev = $("#developers-log-panel").height(),
                        top = $panel.offset().top,
                        height = dev + top + 50;
                    $panel.css("max-height", $(window).height() - height);

                };
                $("#developers-log-panel-collapse,#developers-log-panel-expand").on("click", setDesignerHeight);
                setDesignerHeight();
            },
            addClaims = function () {
                selected().Claims.push({
                    Type: ko.observable(),
                    Value: ko.observable(),
                    Permission: ko.observable("Inherited allow"),
                    IsInherited: ko.observable(false)
                });
            },
            save = function () {
                var json = ko.toJSON(selected),
                    tcs = new $.Deferred();
                context.post(json, "/management-api/endpoint-permissions")
                    .then(function (e) {
                        tcs.resolve(e);
                        logger.info("The permission has been successfully saved");
                    });


                return tcs.promise();
            },
            removeClaim = function (claim) {
                const parent = this;
                return function () {
                    app.showMessage("Are you sure?", "Annoying", ["Yes", "No"])
                        .done(function(dialogResult) {
                            if (dialogResult === "Yes") {
                                parent.Claims.remove(claim);
                            }
                        });
                };
            };

        return {
            operationEndpoints: operationEndpoints,
            queryEndpoints: queryEndpoints,
            selected: selected,
            addClaims: addClaims,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            removeClaim: removeClaim,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                    {
                        command: function () {
                        },
                        caption: "Discard changes",
                        icon: "fa fa-undo"
                    }
                ])
            }
        };
    });
