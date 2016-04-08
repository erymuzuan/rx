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
        partial = partial || {},
        i18n = null,
        activate = function(entityId) {
            id(entityId);
            var tcs = new $.Deferred();
            context.loadOneAsync("EntityForm", "Route eq 'patient-details'")
                .then(function(f) {
                form(f);
                return watcher.getIsWatchingAsync("Patient", entityId);
            })
                .then(function(w) {
                watching(w);
                return $.getJSON("i18n/" + config.lang + "/patient-details");
            })
                .then(function(n) {
                i18n = n[0];
                if (!entityId || entityId === "0") {
                    return Task.fromResult({
                        WebId: system.guid()
                    });
                }
                return context.get("/api/patients/" + entityId);
            }).then(function(b) {
                entity(new bespoke.DevV1_patient.domain.Patient(b[0] || b));
            }, function(e) {
                if (e.status == 404) {
                    app.showMessage("Sorry, but we cannot find any Patient with location : " + "/api/patients/" + entityId, "Engineering Team Development", ["OK"]);
                }
            }).always(function() {
                if (typeof partial.activate === "function") {
                    partial.activate(ko.unwrap(entity))
                        .done(tcs.resolve)
                        .fail(tcs.reject);
                } else {
                    tcs.resolve(true);
                }
            });
            return tcs.promise();

        },

        register = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/api/patients/")
                .fail(function(response) {
                var result = response.responseJSON;
                errors.removeAll();
                _(result.rules).each(function(v) {
                    errors(v.ValidationErrors);
                });
                logger.error("There are errors in your entity, !!!");
                tcs.resolve(false);
            })
                .then(function(result) {
                logger.info(result.message);
                entity().Id(result.id);
                errors.removeAll();
                tcs.resolve(result);
            });
            return tcs.promise();
        },
        attached = function(view) {
            // validation
            validation.init($('#patient-details-form'), form());

            if (typeof partial.attached === "function") {
                partial.attached(view);
            }

        },

        dob = function() {

            var data = ko.mapping.toJSON(entity);
            return context.post(data, "/Sph/BusinessRule/Validate?dob");

        },

        update = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/api/patients/update")
                .fail(function(response) {
                var result = response.responseJSON;
                errors.removeAll();
                _(result.rules).each(function(v) {
                    errors(v.ValidationErrors);
                });
                logger.error("There are errors in your entity, !!!");
                tcs.resolve(false);
            })
                .then(function(result) {
                logger.info(result.message);
                entity().Id(result.id);
                errors.removeAll();
                tcs.resolve(result);
            });
            return tcs.promise();
        },
        postUpdateCommand = function() {
            return update()
                .then(function(result) {
                if (result.success) return app.showMessage("saved....", ["OK"]);
                else return Task.fromResult(false);
            });
        },
        compositionComplete = function() {
            $("[data-i18n]").each(function(i, v) {
                var $label = $(v),
                    text = $label.data("i18n");
                if (i18n && typeof i18n[text] === "string") {
                    $label.text(i18n[text]);
                }
            });
        },
        saveCommand = function() {
            return register()
                .then(function(result) {
                if (result.success) return app.showMessage("Patient is successfully saved", ["OK"]);
                else return Task.fromResult(false);
            })
                .then(function(result) {
                if (result) router.navigate("#patient");
            });
        };
    var vm = {
        partial: partial,
        dob: dob,
        activate: activate,
        config: config,
        attached: attached,
        compositionComplete: compositionComplete,
        entity: entity,
        errors: errors,
        postUpdateCommand :postUpdateCommand,
        toolbar: {
            saveCommand: saveCommand,
            canExecuteSaveCommand: ko.computed(function() {
                if (typeof partial.canExecuteSaveCommand === "function") {
                    return partial.canExecuteSaveCommand();
                }
                return true;
            }),

        }, // end toolbar

        commands: ko.observableArray([])
    };

    return vm;
});