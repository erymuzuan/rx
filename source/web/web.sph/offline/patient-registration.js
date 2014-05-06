var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


require.config({
    baseUrl: "/SphApp",
    waitSeconds: 15
});
define('jquery', function () {
    return jQuery;
});
define('knockout', ko);


require(['services/offline-datacontext', 'services/system', 'services/app'], function (context, system,app) {
    var entity = ko.observable(new bespoke.dev_2002.domain.Patient({ WebId: system.guid() })),
        errors = ko.observableArray(),
        save = function () {
            return context.openAsync({database: 'dev', store: 'Patient'})
                .then(function () {
                    return  context.saveAsync(entity);
                })
                .then(function(){
                    var message = "Your data has been successfully saved locally";
                    app.showMessage(message, "SPH Platform showcase", ["OK"]);
                });

        },
        vm = {
            errors: errors,
            entity: entity,
            validate: function () {
            },
            save: save
        };
    if (window.location.hash) {
        var id = window.location.hash.replace('#', '');
        console.log("Load " + id);
        context.openAsync({database: 'dev', store: 'Patient'})
            .then(function(){
                return context.loadOneAsync(id);
            })
            .then(function(e){
                entity(e);
            });
    }

    ko.applyBindings(vm);
});
