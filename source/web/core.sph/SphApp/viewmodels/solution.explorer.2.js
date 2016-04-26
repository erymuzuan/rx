/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/jquery.signalR-2.2.0.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/jstree.min.js" />
/// <reference path="../services/datacontext.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, "services/new-item"],
  function (context, logger, router, system, newItemService) {
      "use strict";
      var triggers = ko.observableArray(),
        selected = ko.observable(),
        wds = ko.observableArray(),
        transforms = ko.observableArray(),
        isBusy = ko.observable(true),
        solutionExplorerToggleButton = null,
        activate = function () {
            return true;
        },
        singleClick = function (e, data) {
            selected(data);
        },
        click = function (e) {
            e.stopPropagation();
            var hide = function () { solutionExplorerToggleButton.trigger("click"); };

            var data = selected().node.data;
            if (data.url) {
                router.navigate(data.url);
                hide();
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

            var element = document.getElementById("solution-explorer-panel");
            $(element).jstree({
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

            $(element).on("select_node.jstree", singleClick);
            $(element).delegate("a", "dblclick", click);

        },
        attached = function (view) {
            var tree = $("#solution-explorer-panel"),
                searchInput = $("#search-solution-tree");
            solutionExplorerToggleButton = $("#solution-explorer-toggle-button");
            $("#solution-explorer-panel a.jstree-clicked").css("color", "black");
            $(view).on("click", "form.sidebar-search a.remove", function() {
                tree.jstree("clear_search");
                searchInput.val("");
            });

            var search = _.debounce(function () {
                var text = $(this).val();

                if (!text) {
                    tree.jstree("clear_search");
                } else {
                    var f = "\"\"" + text + "\"\"";
                    console.log(f);
                    tree.jstree("search",  f);
                }
            }, 400);
            searchInput.keyup(search);
            var connection = $.connection("/signalr_solution");

            connection.received(function (data) {
                createTree(data);
            });

            connection.start().done(function (d) {
                console.log(d);
                createTree(d);
                console.log("started...connection to message connection");
            });

            $(document).on("keyup", function (e) {
                if (e.ctrlKey && (e.keyCode === 188 || e.keyCode === 192)) {
                    solutionExplorerToggleButton.trigger("click");
                }
            });


        };

      var vm = {
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
