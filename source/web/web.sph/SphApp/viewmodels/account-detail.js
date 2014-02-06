
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog],
        function (context, logger, router, system, validation, eximp, dialog) {

            var entity = ko.observable(new bespoke.dev_2.domain.Account({WebId:system.guid()})),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                activate = function (id) {

                        var query = String.format("AccountId eq {0}", id),
                            tcs = new $.Deferred(),
                            itemTask = context.loadOneAsync("Account", query),
                            formTask = context.loadOneAsync("EntityForm", "Route eq 'account-detail'");

                        $.when(itemTask, formTask)
                            .done(function(b,f) {
                                if(b){

                                    var item = context.toObservable(b, /Bespoke\.Dev_2\.Domain\.(.*?),/);
                                    entity(item);
                                }else  {
                                    entity(new bespoke.dev_2.domain.Account({WebId:system.guid()}));
                                }
                                form(f);
                                tcs.resolve(true);
                            });

                        return tcs.promise();



                },
                attached = function (view) {
                    validation.init($('#account-detail-form'), form());
                },

                  Website = function(){

                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

                    context.post(data, "/Sph/BusinessRule/Validate/?rule=Website&ed=2" )
                        .then(function (result) {
                            tcs.resolve(result);
                        });
                    return tcs.promise();
                },
                                save = function() {
                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

                        

                    context.post(data, "/Account/Validate?rules=Website")
                        .then(function(result) {
                            if(result.success){
                                context.post(data, "/Account/Save")
                                   .then(function(result) {
                                       tcs.resolve(result);
                                   });
                            }else{
                                tcs.resolve(result);
                            }
                        });
                    

                    return tcs.promise();
                };

            return {
                    Website : Website,
                activate: activate,
                attached: attached,
                entity: entity,
                save : save,
                toolbar : { 
                        emailCommand : function(){
                        console.log("Sending email");
                        return Task.fromResult(true);
                    },
                                            emailCommand :{},
                    saveCommand : save,
                    commands : ko.observableArray([])
                }
            };
        });
