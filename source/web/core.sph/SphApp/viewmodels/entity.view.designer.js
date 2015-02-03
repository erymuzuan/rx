﻿
define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app],
    function (context, logger, router, system, app) {

        var errors = ko.observableArray(),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            view = ko.observable(new bespoke.sph.domain.EntityView({ WebId: system.guid() })),
            activate = function (entityid, viewid) {


                var query = String.format("Id eq '{0}'", entityid),
                    tcs = new $.Deferred();

                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        entity(b);
                        window.typeaheadEntity = b.Name();
                        if (viewid === "0") {
                            tcs.resolve(true);
                        }

                    });
                if (viewid && viewid !== "0") {
                    context.loadOneAsync("EntityView", "Id eq '" + viewid + "'")
                    .done(function (f) {

                        view(f);
                        tcs.resolve(true);
                    });
                } else {

                    view(new bespoke.sph.domain.EntityView({ WebId: system.guid() }));
                    view().IconStoreId("sph-img-list");

                    view().Name.subscribe(function (v) {
                        view().Route(v.toLowerCase().replace(/\W+/g, "-"));

                    });
                }
                view().EntityDefinitionId(entityid);

                return tcs.promise();

            },
            attached = function () {



            },
            publish = function () {

                // get the sorted element
                var columns = _($('ul#column-design>li:not(:last)')).map(function (div) {
                    return ko.dataFor(div);
                });
                view().ViewColumnCollection(columns);

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(view);

                context.post(data, "/Sph/EntityView/Publish")
                    .then(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                            view().Id(result.id);
                            errors.removeAll();
                            view().IsPublished(true);
                        } else {
                            errors(result.Errors);
                            logger.error("There are errors in your entity, !!!");
                        }

                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            save = function () {
                // get the sorted element
                var columns = _($('ul#column-design>li:not(:last)')).map(function (div) {
                    return ko.dataFor(div);
                });
                view().ViewColumnCollection(columns);

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(view);

                context.post(data, "/Sph/EntityView/Save")
                    .then(function (result) {
                        view().Id(result.id);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            remove = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(view);
                app.showMessage("Are you sure you want to delete this view> This action cannot be undone", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.send(data, "/Sph/EntityView", "DELETE")
                                .done(function (result) {
                                    window.location = "/sph#dev.home";
                                })
                                .fail(function (v) {
                                    logger.error(v.statusText);
                                })
                                .then(tcs.resolve);

                        }
                    });
                return tcs.promise();
            },

        depublishAsync = function () {

            var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(view);

            context.post(data, "/EntityView/Depublish")
                .then(function (result) {
                    if (result.success) {
                        view().IsPublished(false);
                        logger.info(result.message);
                        errors.removeAll();
                    } else {
                        logger.error("There are errors in your view, !!!");
                    }
                    tcs.resolve(result);
                });
            return tcs.promise();
        },
        partialEditor = null,
        editCode = function () {
            if (null == partialEditor || partialEditor.closed) {
                var partial = "partial/" + view().Route();
                partialEditor = window.open("/sph/editor/file?id=/sphapp/" + partial + ".js", '_blank', 'height=600px,width=800px,toolbar=0,location=0');
                view().Partial(partial);
            } else {
                partialEditor.focus();
            }

            return Task.fromResult(true);

        };

        var vm = {
            errors: errors,
            attached: attached,
            activate: activate,
            view: view,
            entity: entity,
            formsQuery: ko.computed(function () {
                return String.format("EntityDefinitionId eq '{0}'", entity().Id());
            }),
            toolbar: {
                commands: ko.observableArray([{
                    caption: 'Clone',
                    icon: 'fa fa-copy',
                    command: function () {
                        view().Name(view().Name() + ' Copy (1)');
                        view().Route('');
                        view().Id("0");
                        return Task.fromResult(0);
                    }
                },
                {
                    caption: 'Publish',
                    icon: 'fa fa-sign-in',
                    command: publish,
                    enable: ko.computed(function () {
                        return view().Id() && view().Id() !== "0";
                    })
                },
                {
                    caption: 'Depublish',
                    icon: 'fa fa-sign-out',
                    command: depublishAsync,
                    enable: ko.computed(function () {
                        return view().Id() && view().Id() !== "0" && view().IsPublished();
                    })
                },
                {
                    command: editCode,
                    caption: 'Edit Code',
                    icon: "fa fa-code",
                    enable: ko.computed(function () {
                        return view().Route();
                    })
                }
                ]),
                saveCommand: save,
                removeCommand: remove
            }
        };

        return vm;

    });