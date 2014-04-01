
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config, objectbuilders.app],
        function (context, logger, router, system, validation, eximp, dialog, watcher,config,app) {

            var entity = ko.observable(new bespoke.dev_2.domain.Permohonan({WebId:system.guid()})),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                activate = function (entityId) {
                    id(parseInt(entityId));

                    var query = String.format("PermohonanId eq {0}", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync("Permohonan", query),
                        formTask = context.loadOneAsync("EntityForm", "Route eq 'mohon-kuarters'"),
                        watcherTask = watcher.getIsWatchingAsync("Permohonan", entityId);

                    $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new bespoke.dev_2.domain.Permohonan({WebId:system.guid()}));
                        }
                        form(f);
                        watching(w);

                        tcs.resolve(true);
                    });

                    return tcs.promise();
                },
                mohon = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Permohonan/Mohon" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().PermohonanId(result.id);
                                 errors.removeAll();

                                 
                                    app.showMessage("Permohonan anda sudah didaftar kan", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='/'
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
                lulus = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Permohonan/Lulus" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().PermohonanId(result.id);
                                 errors.removeAll();

                                 
                                    app.showMessage("Permohonan sudah diluluskan", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='/sph#agih-kuarters'
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
                masukanDalamMenunggu = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Permohonan/MasukanDalamMenunggu" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().PermohonanId(result.id);
                                 errors.removeAll();

                                 
                                    app.showMessage("Sudah masuk Menunggu", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='/sph#permohonan'
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
                agih = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Permohonan/Agih" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().PermohonanId(result.id);
                                 errors.removeAll();

                                 
                                    app.showMessage("Sudah di agih", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='/sph#permohonan'
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
                bayarDeposit = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Permohonan/BayarDeposit" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().PermohonanId(result.id);
                                 errors.removeAll();

                                 
                                    app.showMessage("Sudah bayar deposit", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='/sph#serah-kunci'
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
                serahKunci = function(){

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, "/Permohonan/SerahKunci" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().PermohonanId(result.id);
                                 errors.removeAll();

                                 
                                    app.showMessage("Kunci sudah serah", "SPH Platform showcase", ["OK"])
	                                    .done(function (dialogResult) {
                                            console.log();
                                            window.location='/sph#permohonan'
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
                    validation.init($('#mohon-kuarters-form'), form());

                },

                                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

                        

                    context.post(data, "/Permohonan/Save")
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
                    mohon : mohon,
                    lulus : lulus,
                    masukanDalamMenunggu : masukanDalamMenunggu,
                    agih : agih,
                    bayarDeposit : bayarDeposit,
                    serahKunci : serahKunci,
                //


                toolbar : {
                                        
                    saveCommand : mohon,
                    commands : ko.observableArray([])
                }
            };

            return vm;
        });
