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
define('jquery', function () {
    return jQuery;
});
define('knockout', ko);


require(['services/offline-datacontext', 'jquery', 'services/app', 'services/system', objectbuilders.logger],
    function (context, jquery, app, system, logger, db) {
        var list = ko.observableArray(),
            online = ko.observable(window.navigator.onLine),
            errors = ko.observableArray(),
            removeFromOffline = function (item) {
                list.remove(item);
                return context.deleteAsync(ko.unwrap(item.WebId));
            },
            send = function (operation) {
                var item = this;
                return function () {
                    var tcs = new $.Deferred();

                    context.sendAsync({body: ko.mapping.toJSON(item),
                        method: "POST",
                        url: "/Patient/" + operation})
                        .then(function (result) {
                            if (result.success) {
                                errors.removeAll();
                                
                                var message = "Your data has been successfully sent to the server with this id " + result.id;
                                logger.info(message);
                                removeFromOffline(item).then(tcs.resolve);
                                app.showMessage(message, "SPH Platform Showcase", ["OK"]);



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
                online: online,
                list: list,
                entity: ko.observable(new bespoke.dev_2002.domain.Patient({ WebId: system.guid() })),
                sendOperation: send
            };

        ko.applyBindings(vm);


        context.openAsync({database: 'dev', store: 'Patient'})
            .then(function () {
                console.log("Successfully open the database");
                return context.loadAsync();
            })
            .then(function (results) {
                console.log("list", results);
                list(results);
            });

        function updateOnlineStatus(event) {
            var condition = navigator.onLine ? "online" : "offline";

            logger.info("Event: " + event.type + "; Status: " + condition);
            online(window.navigator.onLine);
        }

        window.addEventListener('online', updateOnlineStatus);
        window.addEventListener('offline', updateOnlineStatus);
    });
      