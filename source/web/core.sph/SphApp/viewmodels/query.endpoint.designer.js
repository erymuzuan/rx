define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app, "services/_ko.list"],
    function (context, logger, router, system, app) {

        var errors = ko.observableArray(),
            originalEntity = "",
            warnings = ko.observableArray(),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            query = ko.observable(new bespoke.sph.domain.QueryEndpoint({ WebId: system.guid() })),
            activate = function (id) {


                var predicate = String.format("Id eq '{0}'", id);

                return context.loadOneAsync("QueryEndpoint", predicate)
                    .then(function (qry) {
                        query(qry);
                        qry.Name.subscribe(function (v) {
                            if (!ko.unwrap(qry.Route)) {
                                qry.Route(v.toLowerCase().replace(/\W+/g, "-"));
                            }
                        });
                        return context.loadOneAsync("EntityDefinition", "Name eq '" + ko.unwrap(qry.Entity) + "'");
                    }).then(function (b) {
                        entity(b);
                        window.typeaheadEntity = b.Name();
                    });

            },
            attached = function (view) {
                originalEntity = ko.toJSON(query);
                $(view).on("click", "#expand-collapse-property-tabe", function () {
                    if ($(this).html().indexOf("fa-expand") > -1) {

                        $("#view-column-designer")
                                .removeClass("col-lg-8").addClass("col-lg-4")
                                .removeClass("col-md-8").addClass("col-md-4");
                        $("#view-properties-tab")
                            .removeClass("col-lg-4").addClass("col-lg-8")
                            .removeClass("col-md-4").addClass("col-md-8");
                        $(this).html('<i class="fa fa-compress"></i>');
                    } else {

                        $("#view-column-designer")
                            .removeClass("col-lg-4").addClass("col-lg-8")
                            .removeClass("col-md-4").addClass("col-md-8");
                        $("#view-properties-tab")
                            .removeClass("col-lg-8").addClass("col-lg-4")
                            .removeClass("col-md-8").addClass("col-md-4");
                        $(this).html('<i class="fa fa-expand"></i>');
                    }
                });
            },
            publish = function () {

                var data = ko.mapping.toJSON(query);
                return context.post(data, "/query-endpoints/" + ko.unwrap(query().Id) + "/publish")
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

                return context.post(data, "/query-endpoints")
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
                            context.sendDelete("/query-endpoints/" + query().Id())
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


