/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', objectbuilders.config],
    function (context, logger, config) {

        var watch = function (entity, id) {
            var tcs = new $.Deferred(),
                data = JSON.stringify({ id: id, entity: entity });

            context.post(data, "/Sph/Watch/Register")
                .then(function (result) {
                    tcs.resolve(result);
                    logger.log("Your watch has been registered", data, "watcher", true);
                });
            return tcs.promise();
        },
            unwatch = function (entity, id) {
                var tcs = new $.Deferred(),
                    data = JSON.stringify({ id: id, entity: entity });

                context.post(data, "/Sph/Watch/Deregister")
                    .then(function (result) {
                        tcs.resolve(result);
                        logger.log("Your watch has been de-registered", data, "watcher", true);
                    });
                return tcs.promise();
            },
            getIsWatchingAsync = function (entity, id) {
                var tcs = new $.Deferred(),
                    query = String.format("EntityName eq '{0}' and EntityId eq '{1}' and User eq '{2}'", entity, id, config.userName);

                context.loadOneAsync("Watcher", query)
                   .done(function (w) {
                       tcs.resolve(null !== w);
                   });
                return tcs.promise();
            };

        var vm = {
            watch: watch,
            unwatch: unwatch,
            getIsWatchingAsync: getIsWatchingAsync
        };

        return vm;

    });
