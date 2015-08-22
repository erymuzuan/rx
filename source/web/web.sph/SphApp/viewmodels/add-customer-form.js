
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
        objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
        objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
        objectbuilders.app ],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config,app
            ) {

            var entity = ko.observable(new bespoke.DevV1_customer.domain.Customer({WebId:system.guid()})),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                i18n = null,
                activate = function (entityId) {
                    id(entityId);

                    var query = String.format("Id eq '{0}'", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("Customer", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'add-customer-form'"),
                        watcherTask = watcher.getIsWatchingAsync("Customer", entityId),
                        i18nTask = $.getJSON("i18n/" + config.lang + "/add-customer-form");

                    $.when(itemTask, formTask, watcherTask, i18nTask).done(function(b,f,w,n) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new bespoke.DevV1_customer.domain.Customer({WebId:system.guid()}));
                        }
                        form(f);
                        watching(w);
                        i18n = n[0];
                            tcs.resolve(true);
                        
                    });

                    return tcs.promise();
                },
                promoteTo = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, "/Customer/PromoteTo" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                                 window.location='#customer'
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error("There are errors in your entity, !!!");
                             }
                         });
                 },
                demote = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, "/Customer/Demote" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                                 window.location='/sph#customer'
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error("There are errors in your entity, !!!");
                             }
                         });
                 },
                createOrder = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, "/Customer/CreateOrder" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                                 
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error("There are errors in your entity, !!!");
                             }
                         });
                 },
                i7 = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, "/Customer/i7" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

                                 
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
                    validation.init($('#add-customer-form-form'), form());



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

                  checkTheRevenue = function(){

                    var data = ko.mapping.toJSON(entity);

                   return context.post(data, "/Sph/BusinessRule/Validate?checkTheRevenue" );
                },
                  verifyTheGrade = function(){

                    var data = ko.mapping.toJSON(entity);

                   return context.post(data, "/Sph/BusinessRule/Validate?verifyTheGrade" );
                },
                  verifyTheAge = function(){

                    var data = ko.mapping.toJSON(entity);

                   return context.post(data, "/Sph/BusinessRule/Validate?verifyTheAge" );
                },
                  mustBeMalaysian = function(){

                    var data = ko.mapping.toJSON(entity);

                   return context.post(data, "/Sph/BusinessRule/Validate?mustBeMalaysian" );
                },
                                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var data = ko.mapping.toJSON(entity);

                        

                    return context.post(data, "/Sph/BusinessRule/Validate?Customer;CheckTheRevenue;VerifyTheGrade;VerifyTheAge;MustBeMalaysian")
                        .then(function(result) {
                            if(result.success){
                                context.post(data, "/Customer/Save")
                                   .then(function(result) {
                                       entity().Id(result.id);
                                       app.showMessage("Your Customer has been successfully saved", "SPH Platform Showcase", ["OK"]);
                                   });
                            }else{
                                var ve = _(result.validationErrors).map(function(v){
                                    return {
                                        Message : v.message
                                    };
                                });
                                errors(ve);
                                logger.error("There are errors in your entity, !!!");
                            }
                        });
                    

                },
                remove = function() {
                    return $.ajax({
                        type: "DELETE",
                        url: "/Customer/Remove/" + entity().Id(),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function() {
                            app.showMessage("Your item has been successfully removed", "Removed", ["OK"])
                              .done(function () {
                                  window.location = "#customer";
                              });
                        }
                    });


                };

            var vm = {
                                        checkTheRevenue : checkTheRevenue,
                    verifyTheGrade : verifyTheGrade,
                    verifyTheAge : verifyTheAge,
                    mustBeMalaysian : mustBeMalaysian,
                activate: activate,
                config: config,
                attached: attached,
                compositionComplete:compositionComplete,
                entity: entity,
                errors: errors,
                save : save,
                    promoteTo : promoteTo,
                    demote : demote,
                    createOrder : createOrder,
                    i7 : i7,
                //


                toolbar : {
                        emailCommand : {
                        entity : "Customer",
                        id :id
                    },
                                            printCommand :{
                        entity : 'Customer',
                        id : id
                    },
                                                                
                    watchCommand: function() {
                        return watcher.watch("Customer", entity().Id())
                            .done(function(){
                                watching(true);
                            });
                    },
                    unwatchCommand: function() {
                        return watcher.unwatch("Customer", entity().Id())
                            .done(function(){
                                watching(false);
                            });
                    },
                    watching: watching,
                                            
                    saveCommand : save,
                    
                    commands : ko.observableArray([{ caption :"Demote", command : demote, icon:"fa fa-star-o" }])
                }
            };

            return vm;
        });
