define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
objectbuilders.app],

function(context, logger, router, system, validation, eximp, dialog, watcher, config, app) {

    var message = ko.observable(),
        errors = ko.observableArray(),
        form = ko.observable(new bespoke.sph.domain.EntityForm()),
        watching = ko.observable(false),
        id = ko.observable(),
        partial = partial || {},
        i18n = null,
        activate = function(entityId) {
            id(entityId);
            var tcs = new $.Deferred();
            context.loadOneAsync("EntityForm", "Route eq 'borang-kelulusan-permohonan-kuarters'")
                .then(function(f) {
                    form(f);
                    return watcher.getIsWatchingAsync("Permohonan Kuarters", entityId);
                })
                .then(function(w) {
                    watching(w);
                    return $.getJSON("i18n/" + config.lang + "/borang-kelulusan-permohonan-kuarters");
                })
                .then(function(n) {
                    i18n = n[0];
                    if (!entityId || entityId === "0") {
                        return Task.fromResult({
                            WebId: system.guid()
                        });
                    }
                    return context.get("/api/workflow-forms/borang-kelulusan-permohonan-kuarters/activities/08749ac0-e998-4ce2-e8dc-5ad0e41d5538/schema");
                })
                .then(function(b) {
                        message(ko.mapping.fromJS(b));
                    }, function(e) {
                        if (e.status == 404) {
                            app.showMessage("Sorry, but we cannot find any Permohonan Kuarters with location : " + "/api/permohonan-kuarters/v1" + entityId, "Engineering Team Development", ["OK"]);
                        }
                    })
                .always(function() {
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

        kelulusan = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/wf/permohonan-kuarters/v1")
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
            validation.init($('#borang-kelulusan-permohonan-kuarters-form'), form());

            if (typeof partial.attached === "function") {
                partial.attached(view);
            }

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
            return  Task.fromResult(0);
        };
    var vm = {
        partial: partial,
        activate: activate,
        config: config,
        //attached: attached,
        compositionComplete: compositionComplete,
        message: message,
        errors: errors,
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