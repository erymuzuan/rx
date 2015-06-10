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

        var list = ko.observableArray(),
            views = ko.observableArray(),
            dialogs = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {
                $.getJSON("/custom-forms/dialogs").done(dialogs);
                $.getJSON("/custom-forms/partial-views").done(views);
                return $.getJSON("/config/custom-routes").done(list);
            },
            attached = function (view) {

            },
            addNew = function () {
                require(["viewmodels/custom.form.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.route({
                        "role": ko.observable(),
                        "groupName": ko.observable(),
                        "route": ko.observable(),
                        "moduleId": ko.observable(),
                        "title": ko.observable(),
                        "nav": ko.observable(),
                        "icon": ko.observable(),
                        "caption": ko.observable(),
                        "settings": ko.observable(),
                        "showWhenLoggedIn": ko.observable(),
                        "isAdminPage": ko.observable(),
                        "startPageRoute": ko.observable()
                    });
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.route), "/config/custom-route")
                                .done(function () {
                                    list.push(dialog.route());
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },
            addNewPartialView = function () {

                require(["viewmodels/custom.form.partial.view.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.pv({
                        "name": ko.observable(),
                        "useViewModel": ko.observable()
                    });
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.pv), "/custom-forms/partial-view")
                                .done(function () {
                                    views.push(dialog.pv());
                                });
                            }
                        });

                });
                return Task.fromResult(0);
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
                                    dialogs.push(dialog.dlg());
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },
            edit = function (route) {
                require(["viewmodels/custom.form.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.route(ko.mapping.fromJS(route));
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.route), "/config/custom-route")
                                .done(function () {
                                    list.remove(route);
                                    list.push(dialog.route());
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },
            editViewModel = function (route) {
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
            editView = function (route) {
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
            editPartialView = function (pv) {
                if (typeof pv.Name !== "function") {
                    pv = ko.mapping.fromJS(pv);
                }
                pv.originalName(ko.unwrap(pv.name));
                require(["viewmodels/custom.form.partial.view.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.pv(pv);
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.pv), "/custom-forms/rename/partial-view")
                                .done(function () {
                                    views.remove(pv);
                                    views.push(dialog.pv());
                                });
                            }
                        });

                });
                return Task.fromResult(0);

            },
            editDialog = function (dlg) {
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
                                    dialogs.remove(dlg);
                                    dialogs.push(dialog.dlg());
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },
            deletePartialView = function (pv) {
                app.showMessage("Are you sure you want to remove this partial view permanently", "Remove Partial View", ["Yes", "No"])
                   .done(function (dialogResult) {
                       if (dialogResult === "Yes") {
                           context.send(null, "/custom-forms/partial-view/" + ko.unwrap(pv.name), "DELETE")
                               .fail(function (er, msg) {
                                   logger.error(er);
                                   logger.error(msg);
                               })
                               .done(function () {
                                   views.remove(pv);
                               });

                       }
                   });
            },
            deleteDialog = function(dlg) {
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
            },
            deleteRoute = function (route) {
                app.showMessage("Are you sure you want to remove this custom route permanently", "Remove Route", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.send(null, "/config/custom-route/" + ko.unwrap(route.route), "DELETE")
                                .fail(function (er, msg) {
                                    logger.error(er);
                                    logger.error(msg);
                                })
                                .done(function () {
                                    list.remove(route);
                                });

                        }
                    });
            },
            importPackage = function () {
                var tcs = new $.Deferred();
                require(["viewmodels/custom.forms.import", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function () {
                            $.get("/config/custom-routes")
                                .done(list)
                                .fail(tcs.reject)
                                .done(tcs.resolve);
                        });
                });

                return tcs.promise();
            };

        var vm = {
            editPartialView: editPartialView,
            editDialog: editDialog,
            deletePartialView: deletePartialView,
            deleteDialog: deleteDialog,
            dialogs: dialogs,
            views: views,
            list: list,
            deleteRoute: deleteRoute,
            edit: edit,
            editViewModel: editViewModel,
            editView: editView,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                importCommand: importPackage,
                groupCommands: [{
                    caption: "New Page",
                    commands: [
                            {
                                caption: "Add New Form with Route",
                                icon: "fa fa-file-text-o",
                                command: addNew
                            },
                            {
                                caption: "Add New Partial View",
                                icon: "fa fa-file-o",
                                command: addNewPartialView
                            },
                            {
                                caption: "Add New Dialog",
                                icon: "fa fa-file-code-o",
                                command: addNewDialog
                            }]

                }]
            }
        };

        return vm;

    });
