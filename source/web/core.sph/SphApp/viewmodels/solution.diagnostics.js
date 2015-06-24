/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var errors = ko.observableArray(),
            warnings = ko.observableArray(),
            jsTreeData = {
                text: "Diagnostics",
                state: {
                    opened: true,
                    selected: true
                }
            },
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {

            },
            addErrors = function (node) {
                var item = node.data;
                _(item.Errors).each(function (v) {
                    v.Uri = item.Uri;
                    var error = {
                        text: v.Message,
                        state: "open",
                        type: "errors",
                        data: v
                    };
                    node.children.push(error);
                });

            },
            addWarnings = function (node) {
                var item = node.data;
                node.children = _(item.Warnings).map(function (v) {
                    v.Uri = item.Uri;
                    return {
                        text: v.Message,
                        state: "open",
                        type: "warnings",
                        data: v
                    };
                });
            },
            recurseChildMember = function (node) {
                var list = [];
                for (var d in node.data) {
                    if (!node.data.hasOwnProperty(d)) {
                        continue;
                    }
                    var item = node.data[d];
                    _(item.Warnings).each(function (v) { warnings.push(v); });
                    _(item.Errors).each(function (v) { errors.push(v); });


                    var errorsCount = item.Errors.length > 0,
                        warningsCount = item.Warnings.length > 0,
                        type = "ok";
                    if (errorsCount) {
                        type = "errors";
                    }
                    if (warningsCount) {
                        type = "warnings";
                    }
                    if (errorsCount && warningsCount) {
                        type = "errors-warnings";
                    }

                    list.push({
                        text: d,
                        state: "open",
                        type: type,
                        data: item
                    });
                }
                node.children = list;
                _(node.children).each(addWarnings);
                _(node.children).each(addErrors);

            },
            draw = function (diagnostics) {

                warnings([]);
                errors([]);
                var list = [];
                for (var d in diagnostics) {
                    if (diagnostics.hasOwnProperty(d)) {
                        list.push({
                            text: d,
                            state: "open",
                            type: d,
                            data: diagnostics[d]
                        });
                    }
                }
                jsTreeData.children = list;
                _(jsTreeData.children).each(recurseChildMember);

                $("#diagnostics-tree").jstree({
                    "core": {
                        "animation": 0,
                        "check_callback": true,
                        "themes": { "stripes": true },
                        'data': jsTreeData
                    },
                    "contextmenu": {
                        "items": [
                            {
                                label: "Open",
                                icon: "fa fa-folder-open-o",
                                action: function () {
                                    var parent = $("#diagnostics-tree").jstree("get_selected", true),
                                        data = parent[0].data;
                                    if (typeof data.Uri === "string") {
                                        router.navigate(data.Uri);
                                    }
                                    logger.info("opening " + data.Uri);
                                    return true;
                                }
                            }
                        ]
                    },
                    "types": {

                        "formsDiagnostics": {
                            "icon": "fa fa-file-text-o",
                            "valid_children": []
                        },
                        "entitiesDiagnostics": {
                            "icon": "fa fa-file-o",
                            "valid_children": []
                        },
                        "viewsDiagnostics": {
                            "icon": "fa fa-table",
                            "valid_children": []
                        },
                        "ok": {
                            "icon": "fa fa-check alert-success"
                        },
                        "errors": {
                            "icon": "fa fa-exclamation-circle alert-danger",
                            "valid_children": []
                        },
                        "warnings": {
                            "icon": "fa fa-warning alert-warning",
                            "valid_children": []
                        },
                        "errors-warnings": {
                            "icon": "fa fa-exclamation",
                            "valid_children": []
                        }
                    },
                    "plugins": ["contextmenu", "dnd", "types", "search"]
                });

            },
            startDiagnostics = function () {
                return context.post(ko.toJSON({}), "/solution/diagnostics").done(draw);
            };

        var vm = {
            errors: errors,
            warnings: warnings,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                commands: ko.observableArray([
                    {
                        command: startDiagnostics,
                        caption: "Start Diagnostics",
                        icon: "fa fa-file-o"
                    }
                ])
            }
        };

        return vm;

    });
