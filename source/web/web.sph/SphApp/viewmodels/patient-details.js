/// <reference path="../../scripts/jquery-2.2.0.intellisense.js" />
define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
    objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
    objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
    objectbuilders.app, "services/_ko.list"
],

  function (context, logger, router, system, validation, eximp, dialog, watcher, config, app) {

      var entity = ko.observable(new bespoke.DevV1_patient.domain.Patient(system.guid())),
        errors = ko.observableArray(),
        form = ko.observable(new bespoke.sph.domain.EntityForm()),
        watching = ko.observable(false),
        id = ko.observable(),
        i18n = null,
        activate = function (entityId) {
            id(entityId);

            return context.loadOneAsync("EntityForm", "Route eq 'patient-details'")
               .then(function (f) {
                   form(f);
                   return watcher.getIsWatchingAsync("Patient", entityId);
               })
               .then(function (w) {
                   watching(w);
                   return $.getJSON("i18n/en-US/patient-details");
               })
               .then(function (n) {
                   i18n = n[0];
                   return context.get("/api/patients/" + entityId);
               }).then(function (b) {
                   var c = b[0] || b;
                   c.$type = "Bespoke.DevV1_patient.Domain.Patient, DevV1.Patient";
                   var item = context.toObservable(c);
                   entity(item);
               }, function (e) {
                   console.error(e);
                   entity(new bespoke.DevV1_patient.domain.Patient(system.guid()));
               });

        },
        register = function () {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.post(data, "/api/patients/register")
                .fail(function (result, error) {
                    errors.removeAll();
                    _(result.rules).each(function (v) {
                        errors(v.ValidationErrors);
                    });
                    logger.error("There are errors in your entity, !!!");

                    tcs.reject(result);
                })
              .done(function (result) {
                  logger.info(result.message);
                  entity().Id(result.id);
                  errors.removeAll();
                  tcs.resolve(result);
              });

            return tcs.promise();
        },
        attached = function (view) {
            // validation
            validation.init($('#patient-details-form'), form());

        },
        compositionComplete = function () {
            $("[data-i18n]").each(function (i, v) {
                var $label = $(v),
                  text = $label.data("i18n");
                if (typeof i18n !== "undefined" && typeof i18n[text] === "string") {
                    $label.text(i18n[text]);
                }
            });
        },
        saveCommand = function () {
            return register()
                .fail(function () {

                })
              .then(function (result) {
                  if (result.success) return app.showMessage("Patient is successfully saved", "DevV1 application show case", ["OK"]);
                  else return Task.fromResult(false);
              })
              .then(function (result) {
                  if (result) router.navigate("#patient");
              });
        };
      return {
          activate: activate,
          config: config,
          attached: attached,
          compositionComplete: compositionComplete,
          entity: entity,
          errors: errors,
          toolbar: {
              saveCommand: saveCommand,
              commands: ko.observableArray([])
          }
      };

  });
