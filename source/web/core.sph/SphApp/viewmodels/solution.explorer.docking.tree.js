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
            solution = ko.observable(),
            wds = ko.observableArray(),
            transforms = ko.observableArray(),
            isBusy = ko.observable(true),
            addForm = function (EntityDefinitionName) {

                var edForm = new bespoke.sph.domain.EntityForm({ WebId: system.guid() });
                require(["viewmodels/add.entity-definition.form.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.entity(EntityDefinitionName);
                    dialog.entityId(EntityDefinitionName);
                    dialog.form(edForm);

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                context.post(ko.toJSON(edForm), "/entity-form")
                                        .done(function (result) {
                                            if (result.success) {
                                                router.navigate('/entity.form.designer/' + dialog.entity().Id() + '/' + dialog.form().Id());
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
                                                router.navigate('/entity.details/' + entity().Id());
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
            addView = function (EntityDefinitionName) {
                var edView = new bespoke.sph.domain.EntityForm({ WebId: system.guid() });
                require(["viewmodels/add.entity-definition.view.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.entity(EntityDefinitionName);
                    dialog.entityId(EntityDefinitionName);
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
                                        router.navigate('/entity.view.designer/' + dialog.entity().Id() + '/' + dialog.view().Id());
                                    });
                                return tcs.promise();
                            }
                        });

                });
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
            addBusinessRules = function (EntityDefinitionName) {
                var entity = ko.observable(new bespoke.sph.domain.EntityDefinition());
                var query = String.format("Id eq '{0}'", ko.unwrap(EntityDefinitionName)),
                tcs = new $.Deferred();

                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        entity(b);
                        window.typeaheadEntity = b.Name();
                    });

                console.log(entity);

                var br = new bespoke.sph.domain.BusinessRule({ WebId: system.guid() });
                var self = this;

                require(['viewmodels/business.rule.dialog', 'durandal/app'], function (dialog, app) {
                    dialog.rule(br);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result == "OK") {
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
                return $.getJSON("/Solution/open/asdasd").then(function (d) {
                    solution(context.toObservable(d));
                });
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
            click = function (e, data) {
                e.stopPropagation();
                if (data.node.parent === "EntityDefinition") {
                    router.navigate('entity.details/' + data.node.id);
                } else if (data.node.data.TypeName === "EntityForm") {
                    router.navigate('entity.form.designer/' + data.node.parentNode + "/" + data.node.id);
                } else if (data.node.data.TypeName === "EntityOperation") {
                    router.navigate('entity.operation.details/' + data.node.parentNode + "/" + data.node.id);
                } else if (data.node.data.TypeName === "EntityView") {
                    router.navigate('entity.view.designer/' + data.node.parentNode + "/" + data.node.id);
                } else if (data.node.data.TypeName === "BusinessRule") {
                    router.navigate('entity.details/' + data.node.parentNode);
                }
            };

        var vm = {
            attached: attached,
            click: click,
            solution: solution,
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
