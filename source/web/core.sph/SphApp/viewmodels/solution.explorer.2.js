/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/jquery.signalR-2.2.0.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/jstree.min.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="~/Scripts/__vendor.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, "services/new-item", "services/app"],
  function (context, logger, router, system, newItemService, app) {
      "use strict";
      let solutionExplorerToggleButton = null;
      const triggers = ko.observableArray(),
        selected = ko.observable(),
        wds = ko.observableArray(),
        transforms = ko.observableArray(),
        isBusy = ko.observable(true),
        activate = function () {
            return true;
        },
        singleClick = function (e, data) {
            selected(data);
        },
        click = function (e) {
            e.stopPropagation();
            const hide = function () { solutionExplorerToggleButton.trigger("click"); };

            const data = selected().node.data;
            if (data.url) {
                router.navigate(data.url);
                hide();
            }

            if (data.codeEditor) {
                const params = [
                    `height=${screen.height}`,
                    `width=${screen.width}`,
                    "toolbar=0",
                    "location=0",
                    "fullscreen=yes"
                ].join(",");
                const editor = window.open(`/sph/editor/file?id=${data.codeEditor}`, "_blank", params);
                editor.moveTo(0, 0);
            }

        },
        createTree = function (solution) {
            if (!solution) {
                return;
            }
            if (solution.Exception) {
                logger.error(solution.Message);
                app.showMessage(solution.Details, "Error loading your solution",["OK"], { pre: true, alert : { "class" : "danger", "icon" :"warning", "text": solution.Message} });

                return;
            }

            const tree =  $("#solution-explorer-panel").jstree(true);
            if(solution.changedType === "Deleted"){
                const deletedNode = tree.get_node($(`#${solution.id}`)),
                    deleted = tree.delete_node(deletedNode);
                console.log(`Node ${solution.id} deleted : ${deleted}`, deletedNode);
            }
            if(solution.changedType === "Created"){
                const parent = tree.get_node($(`#${solution.type}`)),
                    data =  {
                        id: solution.id,
                        text: solution.text,
                        url: solution.url,
                        createDialog: solution.createDialog,
                        createdUrl: solution.createdUrl,
                        dialog: solution.dialog,
                        codeEditor: solution.codeEditor
                    },
                    nn = { id: solution.id, parent: solution.type, icon: solution.icon, state: "open", text: solution.text, data: data },
                    itemNode = tree.create_node(parent, nn);
                console.log(`Created new node ${solution.id} created : `, itemNode);
            }


            if(solution.changedType === "Changed"){
                const changedNode = tree.get_node($(`#${solution.id}`));

                tree.rename_node(changedNode, solution.text);
                console.log(`Rename node ${solution.id} : `, changedNode);
            }

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

            const element = document.getElementById("solution-explorer-panel");
            $(element).jstree({
                'core': {
                    'check_callback': true,
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

            $(element).on("select_node.jstree", singleClick);
            $(element).delegate("a", "dblclick", click);

        },
        attached = function (view) {
            var tree = $("#solution-explorer-panel"),
                searchInput = $("#search-solution-tree");
            solutionExplorerToggleButton = $("#solution-explorer-toggle-button");
            $("#solution-explorer-panel a.jstree-clicked").css("color", "black");
            $(view).on("click", "form.sidebar-search a.remove", function () {
                tree.jstree("clear_search");
                searchInput.val("");
            });

            const search = _.debounce(function () {
                const text = $(this).val();

                if (!text) {
                    tree.jstree("clear_search");
                } else {
                    const f = `""${text}""`;
                    console.log(f);
                    tree.jstree("search", f);
                }
            }, 400);
            searchInput.keyup(search);
            const connection = $.connection("/signalr_solution");

            connection.received(createTree);
            connection.start().done(createTree);

            $(document).on("keyup", function (e) {
                if (e.ctrlKey && (e.keyCode === 188 || e.keyCode === 192)) {
                    solutionExplorerToggleButton.trigger("click");
                    if ($("#search-solution-tree").is(":visible")) {
                        $("#search-solution-tree").focus().select();
                    }
                }
            });

            setTimeout(function() {
                const height = $("body").height() - 100;
                tree.css({
                    "max-height": height,
                    "overflow-y": "auto"

                });
            },500);



        };

      const vm = {
          attached: attached,
          click: click,
          singleClick: singleClick,
          isBusy: isBusy,
          transforms: transforms,
          wds: wds,
          triggers: triggers,
          activate: activate,
          newItemService: newItemService
      };
      
      return vm;

  });
