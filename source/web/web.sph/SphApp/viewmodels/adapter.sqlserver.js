/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function(context, logger, router) {

        var adapter = ko.observable(),
            isBusy = ko.observable(false),
            connected = ko.observable(false),
            activate = function(sid) {
                if(parseInt(sid) === 0){
                    adapter({
                        $type : "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter",
                        Name : ko.observable(),
                        Description : ko.observable(),
                        Server : ko.observable(),
                        TrustedConnection : ko.observable(false),
                        UserId : ko.observable(),
                        Password: ko.observable(),
                        Database : ko.observable()
                    });
                }

            },
            attached = function(view) {

            },
            connect = function(){

            },
            generate = function(){

            },
            save = function(){

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(adapter);
                isBusy(true);

                context.post(data, "/sph/adapter/save")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            }
            ;

        var vm = {
            adapter: adapter,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                    {
                        caption: 'Connect',
                        icon: 'fa fa-exchange',
                        command: connect
                    },
                    {
                        caption: 'Publish',
                        icon: 'fa fa-sign-in',
                        command: generate,
                        enable: connected
                    }
                ])
            }
        };

        return vm;

    });
