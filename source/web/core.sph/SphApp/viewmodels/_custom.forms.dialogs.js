/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {

        var dialogs = ko.observableArray(),
            isBusy = ko.observable(false),
            load = function () {
                return $.getJSON("/custom-forms/dialogs").done(dialogs);
            },
            activate = function () {
                return load();
            },
            attached = function (view) {

            },
            addNewDialog = function () {
                require(["viewmodels/custom.form.dialog.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.dlg({
                        "name": ko.observable()
                    });
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.dlg), "/custom-forms/dialog")
                                .done(function () {
                                    load();
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },
            editJavascript = function (route) {
                var params = [
                        "height=" + screen.height,
                        "width=" + screen.width,
                        "toolbar=0",
                        "location=0",
                        "fullscreen=yes"
                ].join(","),
                file = ko.unwrap(route.moduleId) || "viewmodels/" + ko.unwrap(route.name);

                var editor = window.open("/sph/editor/file?id=/sphapp/" + file + ".js", "_blank", params);
                editor.moveTo(0, 0);
            },
            editHtml = function (route) {
                var params = [
                    "height=" + screen.height,
                    "width=" + screen.width,
                    "toolbar=0",
                    "location=0",
                    "fullscreen=yes"
                ].join(","),
                file = typeof route.moduleId !== "undefined" ? ko.unwrap(route.moduleId).replace("viewmodels/", "") : ko.unwrap(route.name);

                var editor = window.open("/sph/editor/file?id=/sphapp/views/" + file + ".html", "_blank", params);
                editor.moveTo(0, 0);
            },
            renameDialog = function (dlg) {
                if (typeof dlg.Name !== "function") {
                    dlg = ko.mapping.fromJS(dlg);
                }
                dlg.originalName(ko.unwrap(dlg.name));
                require(["viewmodels/custom.form.dialog.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.dlg(dlg);
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.dlg), "/custom-forms/rename/dialog")
                                .done(function () {
                                    load();
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },
            deleteDialog = function (dlg) {
                app.showMessage("Are you sure you want to remove this dialog permanently", "Remove Dialog", ["Yes", "No"])
                .done(function (dialogResult) {
                    if (dialogResult === "Yes") {
                        context.send(null, "/custom-forms/dialog/" + ko.unwrap(dlg.name), "DELETE")
                            .fail(function (er, msg) {
                                logger.error(er);
                                logger.error(msg);
                            })
                            .done(function () {
                                dialogs.remove(dlg);
                            });

                    }
                });
            }

        var vm = {
            load: load,
            addNewDialog: addNewDialog,
            renameDialog: renameDialog,
            deleteDialog: deleteDialog,
            dialogs: dialogs,
            editJavascript: editJavascript,
            editHtml: editHtml,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
