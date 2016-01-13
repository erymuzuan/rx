define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
objectbuilders.app],

function(context, logger, router, system, validation, eximp, dialog, watcher, config, app) {

    var entity = ko.observable(new bespoke.DevV1_patient.domain.Patient(system.guid())),
        errors = ko.observableArray(),
        form = ko.observable(new bespoke.sph.domain.EntityForm()),
        watching = ko.observable(false),
        id = ko.observable(),
        i18n = null,
        activate = function(entityId) {
            id(entityId);

            var query = String.format("Id eq '{0}'", entityId),
                tcs = new $.Deferred(),
                itemTask = context.loadOneAsync("Patient", query),
                formTask = context.loadOneAsync("EntityForm", "Route eq 'admission-form'"),
                watcherTask = watcher.getIsWatchingAsync("Patient", entityId),
                i18nTask = $.getJSON("i18n/" + config.lang + "/admission-form");

            $.when(itemTask, formTask, watcherTask, i18nTask).done(function(b, f, w, n) {
                if (b) {
                    var item = context.toObservable(b);
                    entity(item);
                } else {
                    entity(new bespoke.DevV1_patient.domain.Patient(system.guid()));
                }
                form(f);
                watching(w);
                i18n = n[0];
                tcs.resolve(true);


            });

            return tcs.promise();
        },

        register = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/Patient/Register")
                .then(function(result) {
                if (result.success) {
                    logger.info(result.message);
                    entity().Id(result.id);
                    errors.removeAll();

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

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/Patient/Discharge")
                .then(function(result) {
                if (result.success) {
                    logger.info(result.message);
                    entity().Id(result.id);
                    errors.removeAll();

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

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/Patient/Transfer")
                .then(function(result) {
                if (result.success) {
                    logger.info(result.message);
                    entity().Id(result.id);
                    errors.removeAll();

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

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/Patient/Admit")
                .then(function(result) {
                if (result.success) {
                    logger.info(result.message);
                    entity().Id(result.id);
                    errors.removeAll();

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

        },

        compositionComplete = function() {
            $("[data-i18n]").each(function(i, v) {
                var $label = $(v),
                    text = $label.data("i18n");
                if (typeof i18n[text] === "string") {
                    $label.text(i18n[text]);
                }
            });
        },

        save = function() {
            return admit()
                .then(function(result) {
                if (result.success) return app.showMessage("Done", ["OK"]);
                else return Task.fromResult(false);
            })
                .then(function(result) {
                if (result) router.navigate("#patient");
            });
        };

    var vm = {

        activate: activate,
        config: config,
        attached: attached,
        compositionComplete: compositionComplete,
        entity: entity,
        errors: errors,
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
                return watcher.watch("Patient", entity().Id())
                    .done(function() {
                    watching(true);
                });
            },
            unwatchCommand: function() {
                return watcher.unwatch("Patient", entity().Id())
                    .done(function() {
                    watching(false);
                });
            },
            watching: watching,
            saveCommand: save,

        }, // end toolbar

        commands: ko.observableArray([])
    };

    return vm;
});