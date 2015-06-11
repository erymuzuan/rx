/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", "services/logger", "services/datacontext"],
    function (dialog, logger, context) {

        var folder = ko.observable(),
            routes = ko.observableArray(),
            views = ko.observableArray(),
            dialogs = ko.observableArray(),
            scripts = ko.observableArray(),
            attached = function (view) {
                $("#file-upload-panel").show();
                $("#import-files").kendoUpload({
                    async: {
                        saveUrl: "/custom-forms/upload",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function (e) {
                        logger.logError(e, e, this, true);
                    },
                    success: function (e) {
                        if (!e.response.success) {
                            logger.error(e.response.message);
                            return;
                        }
                        var uploaded = e.operation === "upload";
                        if (uploaded) {
                            routes(e.response.routes);
                            dialogs(e.response.dialogs);
                            views(e.response.views);
                            scripts(e.response.scripts);
                            folder(e.response.folder);
                        }
                        $("#file-upload-panel").hide();
                        $("#import-custom-forms-dialog-form").show();
                    }
                });
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            importCommand = function (diff, type, file) {
                return function () {
                    console.log("import " + file);
                    return context.post(ko.toJSON({ folder: folder, file: file, type : type, diff: diff }), "custom-forms/import")
                    .done(function () {

                    });
                }
            },
            diffCommand = function (file) {
                return function () {
                    console.log("diff " + file);
                }
            },
            importAllCommand = function () {
                return context.post(ko.toJSON({ folder: folder }), "custom-forms/import-all");
            };

        var vm = {
            importAllCommand: importAllCommand,
            importCommand: importCommand,
            diffCommand: diffCommand,
            attached: attached,
            folder: folder,
            routes: routes,
            dialogs: dialogs,
            views: views,
            scripts: scripts,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
