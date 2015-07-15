/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system],
    function (context, logger, router, system) {
        "use strict";
        var items = ko.observableArray(),
            triggers = ko.observableArray(),
            //solution = ko.observable(),
            selected = ko.observable(),
            wds = ko.observableArray(),
            transforms = ko.observableArray(),
            isBusy = ko.observable(true),
            loadAsync = function () {
                var entitiesTask = context.loadAsync("EntityDefinition"),
                    formsTask = context.loadAsync({ entity: "EntityForm", size: 200 }),
                    viewsTask = context.loadAsync({ entity: "EntityView", size: 200 }),
                    triggersTask = context.loadAsync({ entity: "Trigger", size: 200 }),
                    rdlTask = context.loadAsync({ entity: "ReportDefinition", size: 200 }),
                    transformTask = context.loadAsync({ entity: "TransformDefinition", size: 200 }),
                    wdTask = context.loadAsync({ entity: "WorkflowDefinition", size: 200 });


                return $.when(entitiesTask, formsTask, viewsTask, triggersTask, wdTask, rdlTask, transformTask)
                    .then(function (entitiesLo, formsLo, viewsLo, triggersLo, wdsLo, rdlLo, transformLo) {
                        isBusy(false);
                        var entities = _(entitiesLo.itemCollection).map(function (v) {
                            return {
                                Name: v.Name,
                                Id: v.Id,
                                Route: "#entity.details/" + v.Id(),
                                Operations: v.EntityOperationCollection(),
                                Rdl: _(rdlLo.itemCollection).filter(function (f) { return f.DataSource().EntityName() === v.Name(); }),
                                Triggers: _(triggersLo.itemCollection).filter(function (f) { return f.Entity() === v.Name(); }),
                                Forms: _(formsLo.itemCollection).filter(function (f) { return f.EntityDefinitionId() === v.Id(); }),
                                Views: _(viewsLo.itemCollection).filter(function (f) { return f.EntityDefinitionId() === v.Id(); })
                            };
                        });
                        items(entities);
                        wds(wdsLo.itemCollection);
                        transforms(transformLo.itemCollection);
                        triggers(triggersLo.itemCollection);

                    });

            },
            addForm = function (ed) {

                var form = new bespoke.sph.domain.EntityForm({ WebId: system.guid(), EntityDefinitionId : ed });
                require(["viewmodels/add.entity-definition.form.dialog", "durandal/app"], function (dlg, app2) {
                    dlg.form(form);
                    app2.showDialog(dlg)
                        .done(function (dlgr) {
                            if (!dlgr) return;
                            if (dlgr === "OK") {
                                context.post(ko.toJSON(form), "/entity-form")
                                        .done(function (result) {
                                            if (result.success) {
                                                router.navigate(result.location);
                                                logger.info("Your form has been successfully saved.");
                                            }
                                        });
                            }
                        });

                });
            },
            addOperation = function (name) {
                //return router.navigate('/entity.operation.details/' + name + '/<New Operation>');
                var entity = ko.observable(new bespoke.sph.domain.EntityDefinition());
                require(["viewmodels/add.entity-definition.operation.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.entity(entity);
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                var tcs = new $.Deferred();

                                context.post(ko.toJSON(dialog.entity), "/entity-definition")
                                    .then(function (result) {
                                        tcs.resolve(true);
                                        //isBusy(false);
                                        if (result.success) {
                                            logger.info(result.message);
                                            entity().Id(result.id);
                                            //errors.removeAll();
                                            setTimeout(function () {
                                                router.navigate("/entity.details/" + entity().Id());
                                            }, 1000);
                                        } else {

                                            //errors(result.Errors);
                                            console.log(result.Errors);
                                            logger.error("There are errors in your entity, !!!");
                                        }
                                    });

                            }
                        });

                });
            },
            addView = function (ed) {
                var edView = new bespoke.sph.domain.EntityForm({ WebId: system.guid() });
                require(["viewmodels/add.entity-definition.view.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.entity(ed);
                    dialog.entityId(ed);
                    dialog.view(edView);
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                var tcs = new $.Deferred(),
                                data = ko.mapping.toJSON(dialog.view);

                                context.post(data, "/Sph/EntityView/Save")
                                    .then(function (result) {
                                        dialog.view().Id(result.id);
                                        tcs.resolve(result);
                                        router.navigate("/entity.view.designer/" + dialog.entity().Id() + "/" + dialog.view().Id());
                                    });
                                return tcs.promise();
                            }
                        });

                });
            },
            addWorkflowDefinition = function (name) {
                return router.navigate("/workflow.definition.visual/0");
            },
            addTransformDefinition = function (name) {
                return router.navigate("/transform.definition.edit/0");
            },
            addReportDefinition = function (name) {
                return router.navigate("/reportdefinition.edit/0");
            },
            addTrigger = function (name) {
                return router.navigate("/trigger.setup/0");
            },
            addAdapter = function (name) {
                return router.navigate("/adapter.definition.list");
            },
            addEntityDefinition = function () {
                var ed = new bespoke.sph.domain.EntityDefinition(system.guid());
                require(["viewmodels/add.entity-definition.dialog", "durandal/app"], function (dialog, app2) {
                    ed.Name.subscribe(function (name) {
                        if (!ed.Plural()) {
                            $.get("/entity-definition/plural/" + name, function (v) {
                                ed.Plural(v);
                            });
                        }
                        window.typeaheadEntity = name;
                    });

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
            addBusinessRules = function (ed) {
                var entity = ko.observable(new bespoke.sph.domain.EntityDefinition());
                var query = String.format("Id eq '{0}'", ko.unwrap(ed)),
                tcs = new $.Deferred();

                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        entity(b);
                        window.typeaheadEntity = b.Name();
                    });

                console.log(entity);

                var br = new bespoke.sph.domain.BusinessRule({ WebId: system.guid() });
                var self = this;

                require(["viewmodels/business.rule.dialog", "durandal/app"], function (dialog, app) {
                    dialog.rule(br);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                entity().BusinessRuleCollection().push(br);

                                var tcs = new $.Deferred(),
                                data = ko.mapping.toJSON(entity);
                                isBusy(true);

                                context.post(data, "/entity-definition")
                                    .then(function (result) {
                                        tcs.resolve(true);
                                        isBusy(false);
                                        if (result.success) {
                                            logger.info(result.message);
                                            if (!entity().Id()) {
                                                //reload forms and views 
                                                context.loadAsync("EntityForm", "EntityDefinitionId eq '" + result.id + "'")
                                                    .done(function (lo) {
                                                        forms(lo.itemCollection);
                                                    });
                                                context.loadAsync("EntityView", "EntityDefinitionId eq '" + result.id + "'")
                                                    .done(function (lo) {
                                                        views(lo.itemCollection);
                                                    });

                                            }
                                            entity().Id(result.id);
                                        } else {
                                            logger.error("There are errors in your entity, !!!");
                                        }
                                    });
                                return tcs.promise();
                            }

                        });
                });
            },
            activate = function () {


                if (items().length === 0) {
                    return loadAsync();
                }
                return true;
            },
            attached = function () {




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
            },
            singleClick = function (e, data) {
                selected(data);
            },
            click = function (e, data) {
                e.stopPropagation();
                // parent id e.currentTarget.parentNode.parentNode.parentNode.id
                // current id e.currentTarget.parentNode.id

                var parent = e.currentTarget.parentNode.parentNode.parentNode.id;
                var current = e.currentTarget.parentNode.id;

                if (parent === "EntityDefinition") {
                    router.navigate("entity.details/" + current);
                } else if (selected().node.data.TypeName === "EntityForm") {
                    router.navigate("entity.form.designer/" + selected().node.parent + "/" + selected().node.id);
                } else if (selected().node.data.TypeName === "EntityOperation") {
                    router.navigate("entity.operation.details/" + selected().node.parent + "/" + selected().node.data.Name);
                } else if (selected().node.data.TypeName === "EntityView") {
                    router.navigate("entity.view.designer/" + selected().node.parent + "/" + selected().node.id);
                } else if (selected().node.data.TypeName === "BusinessRule") {
                    router.navigate("entity.details/" + selected().node.parent);
                } else if (selected().node.data.TypeName === "TransformDefinition") {
                    router.navigate("transform.definition.edit/" + current);
                } else if (selected().node.data.TypeName === "Trigger") {
                    router.navigate("trigger.setup/" + selected().node.id);
                }

            };

        var vm = {
            attached: attached,
            click: click,
            singleClick: singleClick,
            //solution: solution,
            loadAsync: loadAsync,
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
            addForm: addForm,
            addOperation: addOperation,
            addView: addView,
            addWorkflowDefinition: addWorkflowDefinition,
            addTransformDefinition: addTransformDefinition,
            addReportDefinition: addReportDefinition,
            addTrigger: addTrigger,
            addAdapter: addAdapter,
            addBusinessRules: addBusinessRules
        };


        return vm;

    });
