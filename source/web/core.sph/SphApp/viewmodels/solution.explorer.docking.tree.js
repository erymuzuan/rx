/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog', 'plugins/router', objectbuilders.system],
    function (context, logger, dialog, router, system) {
        "use strict";
        var items = ko.observableArray(),
            triggers = ko.observableArray(),
            wds = ko.observableArray(),
            transforms = ko.observableArray(),
            isBusy = ko.observable(true),
            addBlogForm = function (name) {
                return router.navigate('/entity.form.designer/' + name + '/0');
            },
            addBlogOperation = function (name) {
                return router.navigate('/entity.operation.details/' + name + '/<New Operation>');
            },
            addBlogView = function (name) {
                return router.navigate('/entity.view.designer/' + name + '/0');
            },
            addWorkflowDefinition = function (name) {
                return router.navigate('/workflow.definition.visual/0');
            },
            addTransformDefinition = function (name) {
                return router.navigate('/transform.definition.edit/0');
            },
            addReportDefinition = function (name) {
                return router.navigate('/reportdefinition.edit/0');
            },
            addTrigger = function (name) {
                return router.navigate('/trigger.setup/0');
            },
            addAdapter = function (name) {
                return router.navigate('/adapter.definition.list');
            },
            addEntityDefinition = function () {
                var ed = new bespoke.sph.domain.EntityDefinition(system.guid());
                require(["viewmodels/add.entity-definition.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.ed(ed);
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                context.post(ko.toJSON(ed), "/entity-definition")
                                        .done(function (edr) {
                                            if (edr.success) {
                                                router.navigate("entity.details/" + edr.id);
                                            }
                                        });
                            }
                        });

                });
            },
            activate = function () {
                console.log("solution.explorer.docking.tree.js activate 1");
                console.log("solution.explorer.docking.tree.js activate 2");

            },
            attached = function () {

                console.log("solution.explorer.docking.tree.js attached 1");
                var eds = [];
                var treeRoots = [
                    { "id": "EntityDefinition", "parent": "#", "text": "Entity Definitions", icon: "fa fa-file", data: { TypeName: "#" } },
                    { "id": "WorkflowDefinition", "parent": "#", "text": "Workflow Definitions", icon: "fa fa-file", data: { TypeName: "#" } },
                    { "id": "TransformDefinition", "parent": "#", "text": "Transform Definitions", icon: "fa fa-file", data: { TypeName: "#" } },
                    { "id": "Adapter", "parent": "#", "text": "Adapters", icon: "fa fa-file", data: { TypeName: "#" } },
                    { "id": "Trigger", "parent": "#", "text": "Triggers", icon: "fa fa-file", data: { TypeName: "#" } }
                ];

                return $.getJSON("/Solution/open/asdasd").then(function (data) {

                    _.each(data.ProjectMetadataCollection, function (pmd) {
                        eds.push({
                            id: pmd.Id,
                            text: pmd.Name,
                            parent: "EntityDefinition",
                            icon: "fa fa-clipboard"
                        });

                        _.each(pmd.ChildItemCollection, function (child) {
                            var icon = "";

                            switch (child.TypeName) {
                                case "EntityForm":
                                    icon = "fa fa-edit";
                                    break;
                                case "EntityOperation":
                                    icon = "fa fa-gavel";
                                    break;
                                case "EntityView":
                                    icon = "fa fa-table";
                                    break;
                                case "BusinessRule":
                                    icon = "fa fa-bold";
                                    break;
                            }


                            eds.push({
                                id: child.Id,
                                parentId: child.ParentId,
                                text: child.Name,
                                parent: pmd.Id,
                                icon: icon,
                                data: child

                            });
                        });

                    });



                    $("#jstree_demo_div").jstree({
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
                                        "Create Form": {
                                            "label": "Add New Form",
                                            "action": function (obj) {
                                                // this.create(obj);
                                                addBlogForm(node.id);
                                            }
                                        },
                                        "Create Views": {
                                            "label": "Add New View",
                                            "action": function (obj) {
                                                addBlogView(node.id);
                                            }
                                        },
                                        "Create Operation": {
                                            "label": "Add New Operation",
                                            "action": function (obj) {
                                                addBlogOperation(node.id);
                                            }
                                        }
                                    };
                                }

                            }
                        }
                    });

                    $('#jstree_demo_div').on("select_node.jstree", function (e, data) {
                        e.stopPropagation();
                        if (data.node.parent === "EntityDefinition") {
                            return router.navigate('entity.details/' + data.node.id);
                        } else if (data.node.data.TypeName === "EntityForm") {
                            return router.navigate('entity.form.designer/' + data.node.parentNode + "/" + data.node.id);
                        } else if (data.node.data.TypeName === "EntityOperation") {
                            return router.navigate('entity.operation.details/' + data.node.parentNode + "/" + data.node.id);
                        } else if (data.node.data.TypeName === "EntityView") {
                            return router.navigate('entity.view.designer/' + data.node.parentNode + "/" + data.node.id);
                        } else if (data.node.data.TypeName === "BusinessRule") {
                            return router.navigate('entity.details/' + data.node.parentNode);
                        }


                    });
                });




            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            hassanRefresh = function () {
                console.log("clicking hassan refresh function");
                activate();
            };

        var vm = {
            attached: attached,
            isBusy: isBusy,
            transforms: transforms,
            wds: wds,
            triggers: triggers,
            items: items,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick,
            hassanRefresh: hassanRefresh,
            addEntityDefinition: addEntityDefinition,
            addBlogForm: addBlogForm,
            addBlogOperation: addBlogOperation,
            addBlogView: addBlogView,
            addWorkflowDefinition: addWorkflowDefinition,
            addTransformDefinition: addTransformDefinition,
            addReportDefinition: addReportDefinition,
            addTrigger: addTrigger,
            addAdapter: addAdapter
        };


        return vm;

    });
