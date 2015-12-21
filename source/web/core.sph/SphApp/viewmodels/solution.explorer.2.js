/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/jquery.signalR-2.1.2.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, "services/new-item"],
  function (context, logger, router, system, newItemService) {
      "use strict";
      var items = ko.observableArray(),
        triggers = ko.observableArray(),
        //solution = ko.observable(),
        selected = ko.observable(),
        wds = ko.observableArray(),
        transforms = ko.observableArray(),
        isBusy = ko.observable(true),
        addForm = function (ed) {

            var form = new bespoke.sph.domain.EntityForm({
                WebId: system.guid(),
                EntityDefinitionId: ed
            });
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
        addOperation = function (name) { },
        addView = function (ed) { },
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

            var br = new bespoke.sph.domain.BusinessRule({
                WebId: system.guid()
            });
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
            return true;
        },
        createTree = function (solution) {
            if (!solution.itemCollection) {
                return;
            }
            var items = [];
            _(solution.itemCollection).each(function (v) {
                v.data = {
                    id: v.id,
                    text: v.text,
                    parent: "#",
                    url: v.url,
                    createDialog: v.createDialog,
                    createdUrl: v.createdUrl,
                    dialog: v.dialog

                };
                v.parent = "#";

                items.push(v);
                _(v.itemCollection).each(function (k) {
                    k.parent = v.id;
                    k.data = {
                        id: k.id,
                        text: k.text,
                        url: k.url,
                        createDialog: k.createDialog,
                        createdUrl: k.createdUrl,
                        dialog: k.dialog,
                        codeEditor: k.codeEditor
                    };
                    items.push(k);
                });


            });

            //$.jstree.defaults.search.show_only_matches
            var element = document.getElementById("solution-explorer-panel");
            $(element).jstree({
                'core': {
                    'data': items
                },
                "search": {
                    "show_only_matches": true
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

        },
        attached = function () {

            $("#solution-explorer-panel a.jstree-clicked").css("color", "black");

            var to = false;
            $("#search-solution-tree").keyup(function () {
                if (to) {
                    clearTimeout(to);
                }
                to = setTimeout(function () {
                    var v = $("#search-solution-tree").val();
                    $("#solution-explorer-panel").jstree(true).search(v);
                }, 250);
            });
            var connection = $.connection("/signalr_solution");

            connection.received(function (data) {
                console.log(data);
                createTree(data);
            });

            connection.start().done(function (d) {
                console.log(d);
                createTree(d);
                console.log("started...connection to message connection");
            });




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

            var data = selected().node.data;
            if (data.url) {
                router.navigate(data.url);
            }

            if (data.codeEditor) {
                var params = [
                        "height=" + screen.height,
                        "width=" + screen.width,
                        "toolbar=0",
                        "location=0",
                        "fullscreen=yes"
                ].join(","),
                editor = window.open("/sph/editor/file?id=" + data.codeEditor, "_blank", params);
                editor.moveTo(0, 0);
            }

        };

      var vm = {
          attached: attached,
          click: click,
          singleClick: singleClick,
          isBusy: isBusy,
          transforms: transforms,
          wds: wds,
          triggers: triggers,
          items: items,
          activate: activate,
          addEntityDefinition: addEntityDefinition,
          addForm: addForm,
          addOperation: addOperation,
          addView: addView,
          addWorkflowDefinition: addWorkflowDefinition,
          addTransformDefinition: addTransformDefinition,
          addReportDefinition: addReportDefinition,
          addTrigger: addTrigger,
          addAdapter: addAdapter,
          addBusinessRules: addBusinessRules,
          newItemService: newItemService
      };


      return vm;

  });
