/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var watch = function (entity, id) {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id: id, entity: entity });

            context.post(data, "/Watch/Register")
                .then(function (result) {
                    tcs.resolve(result);
                    logger.log("Anda sudah didaftar", data, "watcher", true);
                });
            return tcs.promise();
        },
            unwatch = function (entity, id) {
                var tcs = new $.Deferred();
                var data = JSON.stringify({ id: id, entity: entity });

                context.post(data, "/Watch/Deregister")
                    .then(function (result) {
                        tcs.resolve(result);
                        logger.log("Anda sudah dibuang dari senarai",data, "watcher",true);
                    });
                return tcs.promise();
            };

        var vm = {
            watch: watch,
            unwatch: unwatch
        };

        return vm;

    });
