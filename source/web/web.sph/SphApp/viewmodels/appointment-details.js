
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config, objectbuilders.app],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config,app) {

            var entity = ko.observable(new bespoke.DevV1_appointment.domain.Appointment({WebId:system.guid()})),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                activate = function (entityId) {
                    id(parseInt(entityId));

                    var query = String.format("AppointmentId eq {0}", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("Appointment", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'appointment-details'"),
                        watcherTask = watcher.getIsWatchingAsync("Appointment", entityId);

                    $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new bespoke.DevV1_appointment.domain.Appointment({WebId:system.guid()}));
                        }
                        form(f);
                        watching(w);

                        tcs.resolve(true);
                    });

                    return tcs.promise();
                },
                register = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Appointment/Register" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().AppointmentId(result.id);
                                 errors.removeAll();

                                  
                                    app.showMessage("The appointment has been set", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='#patient'
	                                    });
                                 
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error("There are errors in your entity, !!!");
                             }
                             tcs.resolve(result);
                         });
                     return tcs.promise();
                 },
                attached = function (view) {
                    // validation
                    validation.init($('#appointment-details-form'), form());

                },

                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

                        

                    context.post(data, "/Appointment/Save")
                        .then(function(result) {
                            tcs.resolve(result);
                        });
                    

                    return tcs.promise();
                },
                remove = function() {
                    var tcs = new $.Deferred();
                    $.ajax({
                        type: "DELETE",
                        url: "/Appointment/Remove/" + entity().AppointmentId(),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: tcs.reject,
                        success: function() {
                            tcs.resolve(true);
                            app.showMessage("Your item has been successfully removed", "Removed", ["OK"])
                              .done(function () {
                                window.location = "#appointment";
                            });
                        } 
                    });


                    return tcs.promise();
                };

            var vm = {
        activate: activate,
        config: config,
        attached: attached,
        entity: entity,
        errors: errors,
        save : save,
                    register : register,
        //


        toolbar : {
                removeCommand :remove,
                        
            saveCommand : save,
            
            commands : ko.observableArray([])
        }
    };

    return vm;
    });
