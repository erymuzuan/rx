
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config, objectbuilders.app],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config,app) {

            var entity = ko.observable(new bespoke.dev_5004.domain.District({WebId:system.guid()})),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                activate = function (entityId) {
                    id(parseInt(entityId));

                    var query = String.format("DistrictId eq {0}", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("District", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'district-details'"),
                        watcherTask = watcher.getIsWatchingAsync("District", entityId);

                    $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new bespoke.dev_5004.domain.District({WebId:system.guid()}));
                        }
                        form(f);
                        watching(w);

                        tcs.resolve(true);
                    });

                    return tcs.promise();
                },
                attached = function (view) {
                    // validation
                    validation.init($('#district-details-form'), form());

                },

                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

                        

                    context.post(data, "/District/Save")
                        .then(function(result) {
                            tcs.resolve(result);
                        });
                    

                    return tcs.promise();
                },
                remove = function() {
                    var tcs = new $.Deferred();
                    $.ajax({
                        type: "DELETE",
                        url: "/District/Remove/" + entity().DistrictId(),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: tcs.reject,
                        success: function() {
                            tcs.resolve(true);
                            app.showMessage("Your item has been successfully removed", "Removed", ["OK"])
                              .done(function () {
                                window.location = "#district";
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
        //


        toolbar : {
                removeCommand :remove,
                        
            saveCommand : save,
            
            commands : ko.observableArray([])
        }
    };

    return vm;
    });
