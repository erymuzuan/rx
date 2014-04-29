var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


require.config({
baseUrl: "/SphApp",
waitSeconds: 15,
paths: {
    'durandal': '/Scripts/durandal',
    'plugins': '/Scripts/durandal/plugins'
    }
});
define('jquery', function () { return jQuery; });
define('knockout', ko);


require(['services/datacontext', 'jquery', 'services/app', 'services/system'], function (context, jquery, app, system) {
    var entity = ko.observable(new bespoke.dev_2002.domain.Patient({ WebId: system.guid() })),
        errors = ko.observableArray(),
        save = function () {
                var tcs = new $.Deferred();
                localforage.getItem('patients').then(function(r){
                    var sr = r || "[]",
                        patients = ko.mapping.fromJSON(sr);

                    var item = _(patients()).find(function(v){
                        return ko.unwrap(v.WebId) === ko.unwrap(entity().WebId);
                    });
                    if(item){
                        patients.remove(item);
                    }
                    patients.push(entity());
                    
                    var json = ko.mapping.toJSON(patients);    

                    localforage.setItem('patients', json).then( function(){
                     app.showMessage("Your item has been saved in local storage", "SPH Platform showcase", ["OK"])
                        .done(function () {
                            tcs.resolve(true);
                        });
                    });
                });

                return tcs.promise();
            },
        vm = {
            errors: errors,
            entity: entity,
            validate: function () { },
            save : save
        };
    if(window.location.hash){
        var id = window.location.hash.replace('#','');
        localforage.getItem('patients').then(function(r){
            var sr = r || "[]",
                patients = ko.mapping.fromJSON(sr);
            var item = _(patients()).find(function(v){
                return ko.unwrap(v.WebId) == id;
            })
            if(item){
                entity(item);
            }
                    
        });
    }

    ko.applyBindings(vm);
});
