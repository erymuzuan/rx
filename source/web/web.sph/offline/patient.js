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


require(['services/datacontext', 'jquery', 'services/app', 'services/system', objectbuilders.logger], 
    function (context, jquery, app, system, logger) {
        var list = ko.observableArray(),
            online = ko.observable(window.navigator.onLine),
            errors = ko.observableArray(),
            removeFromOffline = function(item){
                var tcs = new $.Deferred();
                list.remove(item);

                var json = ko.mapping.toJSON(list);    

                localforage.setItem('patients', json).then( function(e){
                    tcs.resolve(true);
                });

                return tcs.promise();
                // 019-9607794
            },
            send = function(operation){
                var item = this;
                return function(){
                    var tcs = new $.Deferred();

                    context.post(ko.mapping.toJSON(item), "/Patient/" + operation)
                    .then(function (result) {
                        if (result.success) {
                            logger.info(result.message);
                            //item.PatientId(result.id);
                            errors.removeAll();

                            app.showMessage("Ok done", "SPH Platform showcase", ["OK"])
                                .done(function (dialogResult) {
                                    removeFromOffline(item).then(tcs.resolve)
                                });

                        } else {
                            errors.removeAll();
                            _(result.rules).each(function (v) {
                                errors(v.ValidationErrors);
                            });
                            logger.error("There are errors in your entity, !!!");
                        }
                        tcs.resolve(result);
                    });

                    return tcs.promise();

                };
            },
            vm = {
                    errors: errors,
                    online : online,
                    list: list,
                    entity: ko.observable(new bespoke.dev_2002.domain.Patient({ WebId: system.guid() })),
                    sendOperation: send
                };

        ko.applyBindings(vm);

         localforage.getItem('patients').then(function(r){
            var sr = r || "[]",
                patients = ko.mapping.fromJSON(sr);
            list(patients());
        });

      function updateOnlineStatus(event) {
        var condition = navigator.onLine ? "online" : "offline",
            status = {};

        status.className = condition;
        status.innerHTML = condition.toUpperCase();

        logger.info("Event: " + event.type + "; Status: " + condition);
        online(window.navigator.onLine);
      }

      window.addEventListener('online',  updateOnlineStatus);
      window.addEventListener('offline', updateOnlineStatus);
});
      