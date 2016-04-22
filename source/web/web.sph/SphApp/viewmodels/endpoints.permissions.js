/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../../core.sph/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../../core.sph/Scripts/jstree.min.js" />
/// <reference path="../../../core.sph/Scripts/_task.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            operationEndpoints = ko.observableArray(),
            queryEndpoints = ko.observableArray(),
            entities = ko.observableArray(),
            wds = ko.observableArray(),
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
                        return $.getJSON("/management-api/endpoint-permissions");
                    }).then(permissions);
            },
            singleClick = function (e, data) {
                var tag = data.node.data;
                if (!tag) {
                    return Task.fromResult(0);
                }
                isBusy(true);

                var url = "/management-api/endpoint-permissions/";
                if (tag.parent) {
                    url += "?parent=" + tag.parent;
                }
                if (tag.controller) {
                    url += "&controller=" + tag.controller;
                }

                if (tag.action) {
                    url += "&action=" + tag.action;
                }
                return $.getJSON(url)
                .done(function (result) {
                    isBusy(false);
                    var item = ko.mapping.fromJS(result);
                    // if no parent, controller and action is set, the api will return all settings
                    if (_(result).isArray()) {
                        var defaultItem = _(result).find(function(v) {
                            return !v.Parent && !v.Controller && !v.Action;
                        });
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

                    var claimChanged = function (val) {
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
                var perm = _(ko.unwrap(permissions)).find(function (v) {
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
                var defaultItem = _(ko.unwrap(permissions)).find(function (v) {
                    return !v.Parent && !v.Controller && !v.Action;
                });
                selected(ko.mapping.fromJS(defaultItem));

                var $panel = $(view).find("#endpoints-tree-panel"),
                    root = {
                        parent: "#",
                        data: createTag(null, null, null),
                        text: "Default Setting",
                        icon: "fa fa-share-alt",
                        state: {
                            opened: true,
                            selected : true
                        },
                        children: []
                    },
                    items = [root];
                _(ko.unwrap(entities)).each(function (v) {

                    var serviceContractController = ko.unwrap(v.Name) + "ServiceContract",
                        entityNode = {
                            data: createTag(v.Name),
                            parent: "root",
                            text: ko.unwrap(v.Name),
                            icon: ko.unwrap(v.IconClass),
                            state: {
                                opened: false
                            },
                            a_attr : {
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
                _(ko.unwrap(wds)).each(function(v) {
                    var wdNode = {
                        data: createTag(v.Name),
                        parent: "root",
                        text: ko.unwrap(v.Name),
                        icon: "fa fa-code-fork",
                        state: { opened: false },
                        a_attr : {
                            "class": hasImplementation(v.Name)
                        },
                        children: [{
                            data: createTag(v.Name, v.Name, "GetOneAsync"),
                            text: "Get by id",
                            icon: "fa fa-file-o"
                        },{
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
                    _(v.Children).each(function(c) {
                        var action = {
                            data: createTag(v.Name, v.Name, c.Action),
                            text: c.Action,
                            icon : "fa fa-envelope"
                        }
                        wdNode.children.push(action);
                    });
                    root.children.push(wdNode);
                });

                _(queryEndpoints()).each(function (v) {
                    var parent = _(root.children).find(function (k) {
                        return k.text === ko.unwrap(v.Entity);
                    });
                    if (!parent) {
                        return;
                    }


                    var qeController = ko.unwrap(v.ControllerName),
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
                    var parent = _(root.children).find(function (k) {
                        return k.text === ko.unwrap(v.Entity);
                    });
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
            };

        return {
            operationEndpoints: operationEndpoints,
            queryEndpoints: queryEndpoints,
            selected: selected,
            addClaims: addClaims,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                    {
                        command: function () { },
                        caption: "Discard changes",
                        icon: "fa fa-undo"
                    }
                ])
            }
        };
    });
