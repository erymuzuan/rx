
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
        objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
        objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
        objectbuilders.app ],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config,app
            ) {

            var entity = ko.observable(new bespoke.DevV1_patient.domain.Patient({WebId:system.guid()})),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                i18n = null,
                activate = function (entityId) {
                    id(entityId);

                    var query = String.format("Id eq '{0}'", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("Patient", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'patient-emergency-transfer'"),
                        watcherTask = watcher.getIsWatchingAsync("Patient", entityId),
                        i18nTask = $.getJSON("i18n/" + config.lang + "/patient-emergency-transfer");

                    $.when(itemTask, formTask, watcherTask, i18nTask).done(function(b,f,w,n) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new bespoke.DevV1_patient.domain.Patient({WebId:system.guid()}));
                        }
                        form(f);
                        watching(w);
                        i18n = n[0];
                            tcs.resolve(true);
                        
                    });

                    return tcs.promise();
                },
                register = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, "/Patient/Register" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                                  
                                    app.showMessage("Ok done", "SPH Platform Showcase", ["OK"])
	                                    .done(function () {
                                            window.location='#patient'
	                                    });
                                 
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error("There are errors in your entity, !!!");
                             }
                         });
                 },
                discharge = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, "/Patient/Discharge" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                                 window.location='#patient'
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error("There are errors in your entity, !!!");
                             }
                         });
                 },
                transfer = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, "/Patient/Transfer" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                                 window.location='#patient'
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error("There are errors in your entity, !!!");
                             }
                         });
                 },
                admit = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, "/Patient/Admit" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                                 window.location='#patient'
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error("There are errors in your entity, !!!");
                             }
                         });
                 },
                attached = function (view) {
                    // validation
                    validation.init($('#patient-emergency-transfer-form'), form());



                },
                compositionComplete = function() {
                    $("[data-i18n]").each(function (i, v) {
                        var $label = $(v),
                            text = $label.data("i18n");
                        if (typeof i18n[text] === "string") {
                            $label.text(i18n[text]);
                        }
                    });
                },

                                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var data = ko.mapping.toJSON(entity);

                        

                    return context.post(data, "/Patient/Save")
                        .then(function(result) {
                            entity().Id(result.id);
                            app.showMessage("Your Patient has been successfully saved", "SPH Platform Showcase", ["OK"]);

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
                              .done(function () {
                                  window.location = "#patient";
                              });
                        }
                    });


                };

            var vm = {
                                    activate: activate,
                config: config,
                attached: attached,
                compositionComplete:compositionComplete,
                entity: entity,
                errors: errors,
                save : save,
                    register : register,
                    discharge : discharge,
                    transfer : transfer,
                    admit : admit,
                //


                toolbar : {
                        emailCommand : {
                        entity : "Patient",
                        id :id
                    },
                                            printCommand :{
                        entity : 'Patient',
                        id : id
                    },
                                                                
                    watchCommand: function() {
                        return watcher.watch("Patient", entity().Id())
                            .done(function(){
                                watching(true);
                            });
                    },
                    unwatchCommand: function() {
                        return watcher.unwatch("Patient", entity().Id())
                            .done(function(){
                                watching(false);
                            });
                    },
                    watching: watching,
                                            
                    saveCommand : transfer,
                    
                    commands : ko.observableArray([])
                }
            };

            return vm;
        });
