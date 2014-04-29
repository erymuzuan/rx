
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config, objectbuilders.app],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config,app) {

            var entity = ko.observable(new bespoke.dev_1.domain.Customer({WebId:system.guid()})),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                activate = function (entityId) {
                    id(parseInt(entityId));

                    var query = String.format("CustomerId eq {0}", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("Customer", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'customer-form-2'"),
                        watcherTask = watcher.getIsWatchingAsync("Customer", entityId);

                    $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new bespoke.dev_1.domain.Customer({WebId:system.guid()}));
                        }
                        form(f);
                        watching(w);

                        tcs.resolve(true);
                    });

                    return tcs.promise();
                },
                promoteTo = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Customer/PromoteTo" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().CustomerId(result.id);
                                 errors.removeAll();

                                  
                                    app.showMessage("Wholaaa 123", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='#customer'
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
                demote = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Customer/Demote" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().CustomerId(result.id);
                                 errors.removeAll();

                                  
                                    app.showMessage("You had been demoted", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='/sph#customer'
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
                createOrder = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Customer/CreateOrder" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().CustomerId(result.id);
                                 errors.removeAll();

                                  
                                    app.showMessage("Just a test op", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            
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
                    validation.init($('#customer-form-2-form'), form());

                },

                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

                        

                    context.post(data, "/Customer/Save")
                        .then(function(result) {
                            tcs.resolve(result);
                            entity().CustomerId(result.id);
                            app.showMessage("Your Customer has been successfully saved", "SPH Platform showcase", ["ok"]);

                        });
                    

                    return tcs.promise();
                },
                remove = function() {
                    var tcs = new $.Deferred();
                    $.ajax({
                        type: "DELETE",
                        url: "/Customer/Remove/" + entity().CustomerId(),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: tcs.reject,
                        success: function() {
                            tcs.resolve(true);
                            app.showMessage("Your item has been successfully removed", "Removed", ["OK"])
                              .done(function () {
                                  window.location = "#customer";
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
                    promoteTo : promoteTo,
                    demote : demote,
                    createOrder : createOrder,
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
                        return watcher.watch("Customer", entity().CustomerId())
                            .done(function(){
                                watching(true);
                            });
                    },
                    unwatchCommand: function() {
                        return watcher.unwatch("Customer", entity().CustomerId())
                            .done(function(){
                                watching(false);
                            });
                    },
                    watching: watching,
                    commands : ko.observableArray([])
                }
            };

            return vm;
        });