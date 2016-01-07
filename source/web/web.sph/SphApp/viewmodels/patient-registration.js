define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
objectbuilders.app, 'partial/patient-registration'],

function(context, logger, router, system, validation, eximp, dialog, watcher, config, app, partial) {

    var entity = ko.observable(new bespoke.DevV1_patient.domain.Patient({
        WebId: system.guid()
    })),
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
                formTask = context.loadOneAsync("EntityForm", "Route eq 'patient-registration'"),
                watcherTask = watcher.getIsWatchingAsync("Patient", entityId),
                i18nTask = $.getJSON("i18n/" + config.lang + "/patient-registration");

            $.when(itemTask, formTask, watcherTask, i18nTask).done(function(b, f, w, n) {
                if (b) {
                    var item = context.toObservable(b);
                    entity(item);
                } else {
                    entity(new bespoke.DevV1_patient.domain.Patient({
                        WebId: system.guid()
                    }));
                }
                form(f);
                watching(w);
                i18n = n[0];

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

            var data = ko.mapping.toJSON(entity);

            return context.post(data, "/Patient/Register")
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
            });
        },
        discharge = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity);

            return context.post(data, "/Patient/Discharge")
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
            });
        },
        transfer = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity);

            return context.post(data, "/Patient/Transfer")
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
            });
        },
        admit = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity);

            return context.post(data, "/Patient/Admit")
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
            });
        },
        attached = function(view) {
            // validation
            validation.init($('#patient-registration-form'), form());



            if (typeof partial.attached === "function") {
                partial.attached(view);
            }



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

        validate = function() {
            var tcs = new $.Deferred();
            context.post(ko.mapping.toJSON(entity), '/patient/validate/Dob,ChildMarriage')
                .done(function(result) {
                if (result.success) {
                    logger.info(result.message);
                    errors.removeAll();
                    app.showMessage("OK done", "SPH Platform showcase", ["OK"]);;

                } else {
                    errors.removeAll();
                    _(result.rules).each(function(v) {
                        errors(v.ValidationErrors);
                    });
                    logger.error("There are errors in your entity, !!!");
                }
                tcs.resolve(true);
            });

            return tcs.promise();
        },
        save = function() {
            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity);



            return context.post(data, "/Patient/Save")
                .then(function(result) {
                entity().Id(result.id);
                app.showMessage("Your Patient has been successfully saved", "Engineering Team Development", ["OK"]);

            });


        },
        remove = function() {
            return $.ajax({
                type: "DELETE",
                url: "/Patient/Remove/" + entity().Id(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function() {
                    app.showMessage("Your item has been successfully removed", "Removed", ["OK"])
                        .done(function() {
                        window.location = "#patient";
                    });
                }
            });


        };

    var vm = {

        partial: partial,

        activate: activate,
        config: config,
        attached: attached,
        compositionComplete: compositionComplete,
        entity: entity,
        errors: errors,
        save: save,
        register: register,
        discharge: discharge,
        transfer: transfer,
        admit: admit,
        //

        validate: validate,


        toolbar: {

            saveCommand: register,

            canExecuteSaveCommand: ko.computed(function() {
                if (typeof partial.canExecuteSaveCommand === "function") {
                    return partial.canExecuteSaveCommand();
                }
                return true;
            }),


            commands: ko.observableArray([])
        }
    };

    return vm;
});