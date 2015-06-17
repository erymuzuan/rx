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
            isBusy = ko.observable(false),
            load = function () {
                return $.getJSON("/custom-forms/routes").done(list)
                .done(function () {
                    list.push({
                        "role": null,
                        "groupName": "Main",
                        "route": "",
                        "moduleId": "viewmodels/public.index",
                        "title": "Home",
                        "nav": true,
                        "icon": "fa fa-home",
                        "caption": "Home",
                        "settings": null,
                        "isAdminPage": false,
                        "showWhenLoggedIn": false,
                        "error": ""
                    });
                });
            },
            activate = function () {
                return load();
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
                                context.post(ko.toJSON(dialog.route), "/custom-forms/route")
                                .done(function () {
                                    list.push(dialog.route());
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
                                context.post(ko.toJSON(dialog.route), "/custom-form/route")
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
            deleteRoute = function (route) {
                app.showMessage("Are you sure you want to remove this custom route permanently", "Remove Route", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.send(null, "/custom-forms/route/" + ko.unwrap(route.route), "DELETE")
                                .fail(function (er, msg) {
                                    logger.error(er);
                                    logger.error(msg);
                                })
                                .done(function () {
                                    list.remove(route);
                                });

                        }
                    });
            };

        var vm = {
            load: load,
            list: list,
            addNew: addNew,
            deleteRoute: deleteRoute,
            edit: edit,
            editViewModel: editViewModel,
            editView: editView,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
