/// <reference path="../objectbuilders.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />

define(["durandal/system", "services/system", "plugins/router", "services/logger", "services/datacontext", objectbuilders.config, objectbuilders.cultures, "viewmodels/messages"],
    function (system, system2, router, logger, context, config, cultures, messagesConfig) {

        var activate = function () {
            return router.map(config.routes)
                .buildNavigationModel()
                .mapUnknownRoutes("viewmodels/not.found", "not.found")
                .activate();
        },
            attached = function (view) {
                $(document).on("click", "a#rx-context-help", function (e) {
                    e.preventDefault();
                    var topic = window.location.hash;
                    window.open("/docs/" + topic);
                });
                $(document).on("click", "a#help-dialog", function (e) {
                    e.preventDefault();
                    var topic = $(this).data("dialog");
                    window.open("/docs/#" + topic);
                });

                var dropDown = function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var button = $(this);
                    button.parent().addClass("open");

                    $(document).one("click", function () {
                        button.parent().removeClass("open");
                    });
                };

                $(document).on("mouseenter", ".view-model-modal .modal-header", function (e) {
                    e.preventDefault();
                    var elem = $(this).parents(".view-model-modal"),
                        draggable = elem.data("draggable") || elem.data("ui-draggable") || elem.data("uiDraggable");

                    if (!draggable) {
                        elem.draggable({
                            handle: ".modal-header"
                        });
                        $(".modal-header").css("cursor", "move");
                        console.log("draggagle modal");
                        elem.find("div.modal-header>button").
                            after("<a class=\"pull-right\" id=\"help-dialog\" data-dialog=\"" + elem.attr("id") + "\" href=\"#\" title=\"see help on this topic\" style=\"margin-right:10px; color:gray\"><i class=\"fa fa-question-circle\"></i></a>");
                    }
                });

                $(document).on("click", "a.dropdown-toggle", dropDown);

                $(document).on("click", "table.table-striped th", function (e) {
                    e.preventDefault();
                    var table = $(this).parents("table.table-striped");
                    if (table && !table.prop("sorted")) {
                        console.dir(e);
                        table.tablesorter();
                        table.prop("sorted", "1");
                        $(this).trigger("click");
                    }
                });

                return messagesConfig.attached(view).done(function () {
                    var inboxHeader = document.getElementById("header_inbox_bar");

                    if (inboxHeader && !ko.dataFor(inboxHeader)) {
                        ko.applyBindings(messagesConfig, inboxHeader);
                    }
                });

            },

            print = function (commandParameter) {
                var parameter = typeof commandParameter === "function" ? commandParameter() : commandParameter,
                    url = String.format("/sph/print/{0}/{1}", parameter.entity, +parameter.id());
                window.open(url);
                return Task.fromResult(true);
            },
            email = function (commandParameter) {
                var parameter = typeof commandParameter === "function" ? commandParameter() : commandParameter,
                    url = String.format("/sph/print/{0}/{1}", parameter.entity, +parameter.id()),
                    tcs = new $.Deferred();



                require(["viewmodels/email.entity.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.entity(parameter.entity);
                    dialog.id(parameter.id());
                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {

                            }
                            tcs.resolve(true);
                        });

                });
                return tcs.promise();
            };

        var shell = {
            config: config,
            activate: activate,
            attached: attached,
            router: router,
            printCommand: print,
            emailCommand: email
        };

        return shell;


    });