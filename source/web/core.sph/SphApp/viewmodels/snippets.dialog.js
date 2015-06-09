/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />



define(["services/datacontext", "plugins/dialog", "services/logger"],
    function (context, dialog, logger) {

        var lang = ko.observable(""),
            snippets = ko.observableArray(),
            snippet = ko.observable({ title: ko.observable(), code: ko.observable(), note: ko.observable(), lang: ko.observable() }),
            activate = function () {
                var tcs = new $.Deferred();
                lang.subscribe(function (lg) {
                    $.getJSON("/sph/editor/snippets/" + lg)
                        .done(function (result) {
                            var list = _(result).map(function (v) {
                                var t = {};
                                for (var name in v) {
                                    if (v.hasOwnProperty(name)) {
                                        t[name] = ko.observable(v[name]);
                                    }
                                }
                                return t;
                            });
                            snippets(list);
                        });
                });
                setTimeout(tcs.resolve, 500);
                return tcs.promise();
            },
            saveItem = function () {
                var json = ko.mapping.toJSON(snippet);
                return context.post(json, "/sph/editor/SaveSnippet");

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function (view) {
                $("#snippets-list-ul").on("click", "li>a", function (e) {
                    e.preventDefault();
                    snippet(ko.dataFor(this));
                });
                $("#import-snippets").kendoUpload({
                    async: {
                        saveUrl: "/sph/editor/upload",
                        autoUpload: true
                    },
                    localization: {
                        select: "Import"
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

                        }
                    }
                });
            },
            add = function () {
                var item = { title: ko.observable(), code: ko.observable(), note: ko.observable(), lang: lang(), };
                snippets.push(item);
                snippet(item);
            },
            deleteItem = function (item) {
                snippets.remove(item);
            },
            exportSnippets = function () {
                window.open("/sph/editor/download", "_blank");
                return Task.fromResult(0);

            },
            importCommand = function () {

                return Task.fromResult(0);
            };


        var vm = {
            snippet: snippet,
            snippets: snippets,
            lang: lang,
            attached: attached,
            activate: activate,
            add: add,
            saveItem: saveItem,
            deleteItem: deleteItem,
            cancelClick: cancelClick,
            toolbar: {
                saveCommand: saveItem,
                exportCommand: exportSnippets,
                importCommand: importCommand
            }
        };


        return vm;

    });

