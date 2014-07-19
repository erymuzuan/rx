/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var operation = ko.observable(),
            isBusy = ko.observable(false),
            activate = function (id, uuid) {
                var tcs = new $.Deferred();

                $.get("/httpadapter/operation/" + id + "/" + uuid)
                    .done(function (op) {
                        operation(op);
                        tcs.resolve(op);
                    });

                return tcs.promise();

            },
            attached = function (view) {

            },
            save = function(){

            };

        var vm = {
            operation: operation,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar :{
                saveCommand : save,
                commands : ko.observableArray([

                ])

            }
        };

        return vm;

    });
