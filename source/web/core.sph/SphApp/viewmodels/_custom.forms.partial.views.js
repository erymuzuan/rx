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

        var views = ko.observableArray(),
            isBusy = ko.observable(false),
            load = function () {
                return $.getJSON("/custom-forms/partial-views").done(views);
            },
            activate = function () {
                return load();
            },
            attached = function (view) {

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
                                    return load();
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
                file = "viewmodels/" + ko.unwrap(route.name);

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
                file = ko.unwrap(route.name);

                var editor = window.open("/sph/editor/file?id=/sphapp/views/" + file + ".html", "_blank", params);
                editor.moveTo(0, 0);
            },
            renamePartialView = function (pv) {
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
                                    return load();
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
            };

        var vm = {
            load: load,
            addNewPartialView: addNewPartialView,
            renamePartialView: renamePartialView,
            deletePartialView: deletePartialView,
            views: views,
            editJavascript: editJavascript,
            editHtml: editHtml,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
