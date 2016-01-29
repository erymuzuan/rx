define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app, "services/_ko.list"],
    function (context, logger, router, system, app) {

        var errors = ko.observableArray(),
            originalEntity = "",
            warnings = ko.observableArray(),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            query = ko.observable(new bespoke.sph.domain.EntityQuery({ WebId: system.guid() })),
            activate = function (id) {


                var predicate = String.format("Id eq '{0}'", id);

                return context.loadOneAsync("EntityQuery", predicate)
                    .then(function (qry) {
                        query(qry);
                        qry.Name.subscribe(function (v) {
                            qry.Route(v.toLowerCase().replace(/\W+/g, "-"));
                        });
                        return context.loadOneAsync("EntityDefinition", "Name eq '" + ko.unwrap(qry.Entity) + "'");
                    }).then(function (b) {
                        entity(b);
                        window.typeaheadEntity = b.Name();
                    });

            },
            attached = function () {
                originalEntity = ko.toJSON(query);
            },
            publish = function () {

                var data = ko.mapping.toJSON(query);
                return context.post(data, "/entity-query/" + ko.unwrap(query().Id) + "/publish")
                    .fail(function (response) {
                        var result = response.responseJSON;
                        errors(result.Errors);
                        logger.error("There are errors in your query endpoint, !!!");

                    })
                    .done(function (result) {
                        logger.info(result.message);
                        errors.removeAll();
                        query().IsPublished(true);
                        originalEntity = ko.toJSON(query);
                    });
            },
            save = function () {
                var data = ko.mapping.toJSON(query);

                return context.post(data, "/entity-query")
                    .then(function (result) {
                        logger.info("Query endpoint " + result.id + " has been successfully saved");
                        originalEntity = ko.toJSON(query);
                    });
            },
            canDeactivate = function () {
                var tcs = new $.Deferred();
                if (originalEntity !== ko.toJSON(query)) {
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
            remove = function () {

                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want to delete this Query? This action cannot be undone.", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.sendDelete("/entity-query/" + query().Id())
                                .done(function () {
                                    window.location = "/sph#dev.home";
                                })
                                .fail(function (v) {
                                    logger.error(v.statusText);
                                    tcs.reject(v);
                                })
                                .then(tcs.resolve);

                        }
                    });
                return tcs.promise();
            },

        depublishAsync = function () {

            var data = ko.mapping.toJSON(query);

            return context.post(data, "/enity-query/depublish")
                 .then(function (result) {
                     if (result.success) {
                         query().IsPublished(false);
                         logger.info(result.message);
                         errors.removeAll();
                     } else {
                         logger.error("There are errors in your query, !!!");
                     }
                 });
        };

        var vm = {
            warnings: warnings,
            errors: errors,
            attached: attached,
            activate: activate,
            canDeactivate: canDeactivate,
            query: query,
            entity: entity,
            toolbar: {
                commands: ko.observableArray([{
                    caption: "Clone",
                    icon: "fa fa-copy",
                    command: function () {
                        query().Name(query().Name() + " Copy (1)");
                        query().Route("");
                        query().Id("0");
                        return Task.fromResult(0);
                    }
                },
                {
                    caption: "Publish",
                    icon: "fa fa-sign-in",
                    command: publish,
                    enable: ko.computed(function () {
                        return query().Id() && query().Id() !== "0";
                    })
                },
                {
                    caption: "Depublish",
                    icon: "fa fa-sign-out",
                    command: depublishAsync,
                    enable: ko.computed(function () {
                        return query().Id() && query().Id() !== "0" && query().IsPublished();
                    })
                }
                ]),
                saveCommand: save,
                removeCommand: remove
            }
        };

        return vm;

    });


