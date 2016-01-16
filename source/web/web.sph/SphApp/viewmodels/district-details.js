define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
objectbuilders.app],

function(context, logger, router, system, validation, eximp, dialog, watcher, config, app) {

    var entity = ko.observable(new bespoke.DevV1_district.domain.District(system.guid())),
        errors = ko.observableArray(),
        form = ko.observable(new bespoke.sph.domain.EntityForm()),
        watching = ko.observable(false),
        id = ko.observable(),
        i18n = null,
        activate = function(entityId) {
            id(entityId);

            var query = String.format("Id eq '{0}'", entityId),
                tcs = new $.Deferred(),
                itemTask = context.loadOneAsync("District", query),
                formTask = context.loadOneAsync("EntityForm", "Route eq 'district-details'"),
                watcherTask = watcher.getIsWatchingAsync("District", entityId),
                i18nTask = $.getJSON("i18n/" + config.lang + "/district-details");

            $.when(itemTask, formTask, watcherTask, i18nTask).done(function(b, f, w, n) {
                if (b) {
                    var item = context.toObservable(b);
                    entity(item);
                } else {
                    entity(new bespoke.DevV1_district.domain.District(system.guid()));
                }
                form(f);
                watching(w);
                i18n = n[0];
                tcs.resolve(true);


            });

            return tcs.promise();
        },

        save = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/District/Save")
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
            validation.init($('#district-details-form'), form());

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

        saveCommand = function() {
            return save()
                .then(function(result) {
                if (result.success) return app.showMessage("Olrait", ["OK"]);
                else return Task.fromResult(false);
            })
                .then(function(result) {
                if (result) router.navigate("#district");
            });
        };

    var vm = {

        activate: activate,
        config: config,
        attached: attached,
        compositionComplete: compositionComplete,
        entity: entity,
        errors: errors,
        save: save,
        toolbar: {
            saveCommand: saveCommand,

        }, // end toolbar

        commands: ko.observableArray([])
    };

    return vm;
});