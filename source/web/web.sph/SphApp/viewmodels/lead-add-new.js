
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog],
        function (context, logger, router, system, validation, eximp, dialog) {

            var entity = ko.observable(new bespoke.dev_1.domain.Customer({WebId:system.guid()})),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                activate = function (id) {

                    var query = String.format("CustomerId eq {0}", id),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("Customer", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'lead-add-new'");

                            $.when(itemTask, formTask).done(function(b,f) {  
                                if (b) {
                                    var item = context.toObservable(b, /Bespoke\.Dev_1\.Domain\.(.*?),/);
                                    entity(item);
                                } 
                                else {
                                    entity(new bespoke.dev_1.domain.Customer({WebId:system.guid()}));
                                }
                                form(f);
                                
                                tcs.resolve(true);
                            });

                        return tcs.promise();
                },
                attached = function (view) {
                    // validation
                    validation.init($('#lead-add-new-form'), form());
                    
                },

                                  save = function(){
                    
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
                        });
                    

                    return tcs.promise();
                };

            return {
                activate: activate,
                attached: attached,
                entity: entity,
                save : save,
                toolbar : { 
                                        saveCommand : save,
                    commands : ko.observableArray([])
                }
            };
        });
