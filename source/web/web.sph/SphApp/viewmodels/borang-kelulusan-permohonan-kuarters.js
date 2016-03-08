define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
objectbuilders.system, objectbuilders.validation,
objectbuilders.dialog, objectbuilders.config, objectbuilders.app],

function(context, logger, router, system, validation, dialog, config, app) {

    var message = ko.observable(),
        errors = ko.observableArray(),
        form = ko.observable(new bespoke.sph.domain.WorkflowForm()),
        id = ko.observable(),
        partial = partial || {},
        i18n = null,
        activate = function() {
            var tcs = new $.Deferred();
            context.loadOneAsync("WorkflowForm", "Route eq 'borang-kelulusan-permohonan-kuarters'")
                .then(function(f) {
                form(f);
                return $.getJSON("i18n/" + config.lang + "/borang-kelulusan-permohonan-kuarters");
            })
                .then(function(n) {
                i18n = n[0];
                return context.get("api/workflow-forms/borang-kelulusan-permohonan-kuarters/activities/08749ac0-e998-4ce2-e8dc-5ad0e41d5538");
            }).then(function(b) {
                message(ko.mapping.fromJS(b));
            }, function(e) {
                if (e.status == 404) {
                    app.showMessage("Sorry, but we cannot find any Permohonan Kuarters with location : " + "/api/permohonan-kuarters/v1", "Engineering Team Development", ["OK"]);
                }
            }).always(function() {
                if (typeof partial.activate === "function") {
                    partial.activate(ko.unwrap(message))
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

            var data = ko.mapping.toJSON(message),
                tcs = new $.Deferred();

            context.post(data, "/wf/permohonan-kuarters/v1")
                .fail(function(response) {
                var result = response.responseJSON;
                errors.removeAll();
                _(result.rules).each(function(v) {
                    errors(v.ValidationErrors);
                });
                logger.error("There are errors in your message, !!!");
                tcs.resolve(false);
            })
                .then(function(result) {
                logger.info(result.message);
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
            return 08749ac0 - e998 - 4ce2 - e8dc - 5ad0e41d5538();
        };
    var vm = {
        partial: partial,
        activate: activate,
        config: config,
        attached: attached,
        compositionComplete: compositionComplete,
        entity: entity,
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