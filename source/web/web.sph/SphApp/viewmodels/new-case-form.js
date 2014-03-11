
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config, objectbuilders.app],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config,app) {

            var entity = ko.observable(new bespoke.dev_3001.domain.Deceased({WebId:system.guid()})),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                activate = function (entityId) {
                    id(parseInt(entityId));

                    var query = String.format("DeceasedId eq {0}", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("Deceased", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'new-case-form'"),
                        watcherTask = watcher.getIsWatchingAsync("Deceased", entityId);

                    $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new bespoke.dev_3001.domain.Deceased({WebId:system.guid()}));
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

                     context.post(data, "/Deceased/Register" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().DeceasedId(result.id);
                                 errors.removeAll();

                                 
                                    app.showMessage("Your detah case has been saved", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='/sph#deceased'
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
                    validation.init($('#new-case-form-form'), form());

                },

                                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

                        

                    context.post(data, "/Deceased/Save")
                        .then(function(result) {
                            tcs.resolve(result);
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
                toolbar : {
                        emailCommand : {
                        entity : "Deceased",
                        id :id
                    },
                                            printCommand :{
                        entity : 'Deceased',
                        id : id
                    },
                                            
                    watchCommand: function() {
                        return watcher.watch("Deceased", entity().DeceasedId())
                            .done(function(){
                                watching(true);
                            });
                    },
                    unwatchCommand: function() {
                        return watcher.unwatch("Deceased", entity().DeceasedId())
                            .done(function(){
                                watching(false);
                            });
                    },
                    watching: watching,

                    saveCommand : register,
                    commands : ko.observableArray([])
                }
            };

            return vm;
        });
