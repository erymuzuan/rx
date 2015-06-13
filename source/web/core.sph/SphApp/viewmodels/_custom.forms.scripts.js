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

        var scripts = ko.observableArray(),
            isBusy = ko.observable(false),
            load = function () {
                return $.getJSON("/custom-forms/scripts").done(scripts);
            },
            activate = function () {
                return load();
            },
            attached = function (view) {

            },

            addNew = function () {
                require(["viewmodels/custom.form.script.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.script({
                        "name": ko.observable()
                    });
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.script), "/custom-forms/script")
                                .done(function () {
                                    load();
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },

            editScript = function (route) {
                var params = [
                        "height=" + screen.height,
                        "width=" + screen.width,
                        "toolbar=0",
                        "location=0",
                        "fullscreen=yes"
                ].join(","),
                file = ko.unwrap(route.name);

                var editor = window.open("/sph/editor/file?id=/sphapp/services/" + file + ".js", "_blank", params);
                editor.moveTo(0, 0);
            },


            renameScript = function (script) {
                if (typeof script.Name !== "function") {
                    script = ko.mapping.fromJS(script);
                }
                script.originalName(ko.unwrap(script.name));
                require(["viewmodels/custom.form.script.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.script(script);
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.script), "/custom-forms/rename/script")
                                .done(function () {
                                    load();
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },

            deleteScript = function (js) {
                app.showMessage("Are you sure you want to remove this script permanently", "Remove Dialog", ["Yes", "No"])
                .done(function (dialogResult) {
                    if (dialogResult === "Yes") {
                        context.send(null, "/custom-forms/script/" + ko.unwrap(js.name), "DELETE")
                            .fail(function (er, msg) {
                                logger.error(er);
                                logger.error(msg);
                            })
                            .done(function () {
                                scripts.remove(js);
                            });

                    }
                });
            };

        var vm = {
            load: load,
            addNew: addNew,
            renameScript: renameScript,
            deleteScript: deleteScript,
            scripts: scripts,
            editScript: editScript,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });
