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


require(['services/offline-datacontext', 'services/system'], function (context, system) {
    var entity = ko.observable(new bespoke.@(Model.ApplicationName)_@(Model.EntityDefinitionId).domain.@(Model.Entity)({ WebId: system.guid() })),
        errors = ko.observableArray(),
        save = function () {
            return context.openAsync({database: '@(Model.ApplicationName)', store: '@(Model.Entity)'})
                .then(function () {
                    return  context.saveAsync(entity);
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
        context.openAsync({database: '@(Model.ApplicationName)', store: '@(Model.Entity)'})
            .then(function(){
                return context.loadOneAsync(id);
            })
            .then(function(e){
                entity(e);
            });
    }

    ko.applyBindings(vm);
});
