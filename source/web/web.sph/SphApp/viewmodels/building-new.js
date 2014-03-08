
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config) {

            var entity = ko.observable(new bespoke.dev_2001.domain.Building({WebId:system.guid()})),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                activate = function (entityId) {
                    id(parseInt(entityId));

                    var query = String.format("BuildingId eq {0}", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("Building", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'building-new'"),
                        watcherTask = watcher.getIsWatchingAsync("Building", entityId);

                    $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new bespoke.dev_2001.domain.Building({WebId:system.guid()}));
                        }
                        form(f);
                        watching(w);

                        tcs.resolve(true);
                    });

                    return tcs.promise();
                },
                attached = function (view) {
                    // validation
                    validation.init($('#building-new-form'), form());

                },

                                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

                        

                    context.post(data, "/Building/Save")
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
                save : save,
                toolbar : {
                        emailCommand : {
                        entity : "Building",
                        id :id
                    },
                                            printCommand :{
                        entity : 'Building',
                        id : id
                    },
                                            
                    watchCommand: function() {
                        return watcher.watch("Building", entity().BuildingId())
                            .done(function(){
                                watching(true);
                            });
                    },
                    unwatchCommand: function() {
                        return watcher.unwatch("Building", entity().BuildingId())
                            .done(function(){
                                watching(false);
                            });
                    },
                    watching: watching,

                    saveCommand : save,
                    commands : ko.observableArray([])
                }
            };

            return vm;
        });
