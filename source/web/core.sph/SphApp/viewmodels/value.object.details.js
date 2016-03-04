/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../objectbuilders.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app, "services/app"],
    function (context, logger, router, system, app) {

        var vod = ko.observable(new bespoke.sph.domain.ValueObjectDefinition(system.guid())),
            originalEntity = "",
            isBusy = ko.observable(false),
            errors = ko.observableArray(),
            valueObjectOptions = ko.observableArray(),
            member = ko.observable(new bespoke.sph.domain.Member(system.guid())),
            activate = function (id) {

                member(new bespoke.sph.domain.Member("-"));

                var query = String.format("Id eq '{0}'", id);
                return context.loadOneAsync("ValueObjectDefinition", query)
                    .then(function (b) {
                        vod(b);
                        originalEntity = ko.toJSON(b);
                        errors([]);

                        return context.getListAsync("ValueObjectDefinition", "Name ne '" + ko.unwrap(b.Name) + "'", "Name");
                    }).then(function (list) {

                        valueObjectOptions(list);
                    });




            },
            attached = function () {

                var setDesignerHeight = function () {
                    if ($("#schema-tree-panel").length === 0) {
                        return;
                    }

                    var dev = $("#developers-log-panel").height(),
                        top = $("#schema-tree-panel").offset().top,
                        height = dev + top;
                    $("#schema-tree-panel").css("max-height", $(window).height() - height);

                };
                $("#developers-log-panel-collapse,#developers-log-panel-expand").on("click", setDesignerHeight);
                setDesignerHeight();
            },
            save = function () {
                if (!document.getElementById("vod-form").checkValidity()) {
                    logger.error("Please correct all the validations errors");
                    return Task.fromResult(0);
                }
                var data = ko.mapping.toJSON(vod);
                isBusy(true);

                return context.post(data, "/api/value-object-definition")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            originalEntity = ko.toJSON(vod);
                            logger.info(result.message);

                            vod().Id(result.id);
                            errors.removeAll();
                        } else {
                            errors(result.errors);
                            logger.error("There are errors in your entity, !!!");
                        }
                    });
            },
            canDeactivate = function () {
                var tcs = new $.Deferred();
                if (originalEntity !== ko.toJSON(vod)) {
                    app.showMessage("Save change to the item", "Rx Developer", ["Yes", "No", "Cancel"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {
                                save().done(function () {
                                    tcs.resolve(true);
                                });
                            }
                            if (dialogResult === "No") {
                                tcs.resolve(true);
                            }
                            if (dialogResult === "Cancel") {
                                tcs.resolve(false);
                            }

                        });
                } else {
                    return true;
                }
                return tcs.promise();
            },
            removeAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(vod);
                isBusy(true);
                app.showMessage("Are you sure you want to permanently remove this ValueObjectDefinition, this action cannot be undone", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.send(data, "/api/value-object-definition/" + ko.unwrap(vod().Id), "DELETE")
                                .then(function (result) {
                                    isBusy(false);
                                    if (result.success) {
                                        logger.info(result.message);
                                        setTimeout(function () {
                                            window.location = "/sph#value.object.list";
                                        }, 2000);
                                    } else {

                                        errors(result.Errors);
                                        logger.error("There are errors in your schema, !!!");
                                    }
                                    tcs.resolve(result);
                                })
                            .fail(function (e) {
                                var list = _(e.responseJSON).map(function (v) {
                                    return { Message: v , Code : ""};
                                });
                                errors(list);
                                tcs.resolve(false);
                            });
                        } else {

                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();
            };


        var vm = {
            errors: errors,
            valueObjectOptions: valueObjectOptions,
            isBusy: isBusy,
            activate: activate,
            canDeactivate: canDeactivate,
            attached: attached,
            vod: vod,
            member: member,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAsync,
                canExecuteRemoveCommand: function () {
                    return false;
                },
                commands: ko.observableArray([{
                    caption: "Clone",
                    icon: "fa fa-copy",
                    command: function () {
                        vod().Name(vod().Name() + " Copy (1)");
                        vod().Id("0");
                        return Task.fromResult(0);
                    }
                }])
            }
        };

        return vm;

    });
