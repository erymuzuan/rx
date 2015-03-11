define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
        objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
        objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
        objectbuilders.app, 'sphapp/partial/admission-form'
    ],
    function(context, logger, router, system, validation, eximp, dialog, watcher, config, app, partial) {

        var entity = ko.observable(new bespoke.DevV1_patient.domain.Patient({
                WebId: system.guid()
            })),
            errors = ko.observableArray(),
            form = ko.observable(new bespoke.sph.domain.EntityForm()),
            watching = ko.observable(false),
            id = ko.observable(),
            activate = function(entityId) {
                id(parseInt(entityId));

                var query = String.format("PatientId eq {0}", entityId),
                    tcs = new $.Deferred(),
                    itemTask = context.loadOneAsync("Patient", query),
                    formTask = context.loadOneAsync("EntityForm", "Route eq 'admission-form'"),
                    watcherTask = watcher.getIsWatchingAsync("Patient", entityId);

                $.when(itemTask, formTask, watcherTask).done(function(b, f, w) {
                    if (b) {
                        var item = context.toObservable(b);
                        entity(item);
                    } else {
                        entity(new bespoke.dev_2002.domain.Patient({
                            WebId: system.guid()
                        }));
                    }
                    form(f);
                    watching(w);

                    if (typeof partial.activate === "function") {
                        var pt = partial.activate(entity());
                        if (typeof pt.done === "function") {
                            pt.done(tcs.resolve);
                        } else {
                            tcs.resolve(true);
                        }
                    }

                });

                return tcs.promise();
            },
            register = function() {

                if (!validation.valid()) {
                    return Task.fromResult(false);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);

                context.post(data, "/Patient/Register")
                    .then(function(result) {
                        if (result.success) {
                            logger.info(result.message);
                            entity().PatientId(result.id);
                            errors.removeAll();

                            window.location = '#patient'
                        } else {
                            errors.removeAll();
                            _(result.rules).each(function(v) {
                                errors(v.ValidationErrors);
                            });
                            logger.error("There are errors in your entity, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            discharge = function() {

                if (!validation.valid()) {
                    return Task.fromResult(false);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);

                context.post(data, "/Patient/Discharge")
                    .then(function(result) {
                        if (result.success) {
                            logger.info(result.message);
                            entity().PatientId(result.id);
                            errors.removeAll();

                            window.location = '#patient'
                        } else {
                            errors.removeAll();
                            _(result.rules).each(function(v) {
                                errors(v.ValidationErrors);
                            });
                            logger.error("There are errors in your entity, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            transfer = function() {

                if (!validation.valid()) {
                    return Task.fromResult(false);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);

                context.post(data, "/Patient/Transfer")
                    .then(function(result) {
                        if (result.success) {
                            logger.info(result.message);
                            entity().PatientId(result.id);
                            errors.removeAll();

                            window.location = '#patient'
                        } else {
                            errors.removeAll();
                            _(result.rules).each(function(v) {
                                errors(v.ValidationErrors);
                            });
                            logger.error("There are errors in your entity, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            admit = function() {

                if (!validation.valid()) {
                    return Task.fromResult(false);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);

                context.post(data, "/Patient/Admit")
                    .then(function(result) {
                        if (result.success) {
                            logger.info(result.message);
                            entity().PatientId(result.id);
                            errors.removeAll();

                            window.location = '#patient'
                        } else {
                            errors.removeAll();
                            _(result.rules).each(function(v) {
                                errors(v.ValidationErrors);
                            });
                            logger.error("There are errors in your entity, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            attached = function(view) {
                // validation
                validation.init($('#admission-form-form'), form());

                if (typeof partial.attached === "function") {
                    partial.attached(view);
                }

            },

            save = function() {
                if (!validation.valid()) {
                    return Task.fromResult(false);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);

                context.post(data, "/Patient/Save")
                    .then(function(result) {
                        tcs.resolve(result);
                        entity().PatientId(result.id);
                        app.showMessage("Your Patient has been successfully saved", "SPH Platform showcase", ["ok"]);

                    });

                return tcs.promise();
            },
            remove = function() {
                var tcs = new $.Deferred();
                $.ajax({
                    type: "DELETE",
                    url: "/Patient/Remove/" + entity().PatientId(),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: tcs.reject,
                    success: function() {
                        tcs.resolve(true);
                        app.showMessage("Your item has been successfully removed", "Removed", ["OK"])
                            .done(function() {
                                window.location = "#patient";
                            });
                    }
                });

                return tcs.promise();
            };

        var vm = {
            partial: partial,
            activate: activate,
            config: config,
            attached: attached,
            entity: entity,
            errors: errors,
            save: save,
            register: register,
            discharge: discharge,
            transfer: transfer,
            admit: admit,
            toolbar: {
                emailCommand: {
                    entity: "Patient",
                    id: id
                },
                printCommand: {
                    entity: 'Patient',
                    id: id
                },
                watchCommand: function() {
                    return watcher.watch("Patient", entity().PatientId())
                        .done(function() {
                            watching(true);
                        });
                },
                unwatchCommand: function() {
                    return watcher.unwatch("Patient", entity().PatientId())
                        .done(function() {
                            watching(false);
                        });
                },
                watching: watching,
                saveCommand: admit,
                commands: ko.observableArray([])
            }
        };

        return vm;
    });