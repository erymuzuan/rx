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
            loadingSchemas = ko.observable(false),
            loadingDatabases = ko.observable(false),
            connected = ko.observable(false),
            databaseOptions = ko.observableArray(),
            schemaOptions = ko.observableArray(),
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
                adapter().Database.subscribe(function (db) {
                    if (!db) {
                        return;
                    }
                    loadingSchemas(true);
                    context.post(ko.mapping.toJSON(adapter) ,"sqlserver-adapter/schema" ).done(function(result) {
                        schemaOptions(result.schema);
                        loadingSchemas(false);
                    });
                });
            },
            connect = function(){
                var tcs = new $.Deferred();
                loadingSchemas(true);
                loadingDatabases(true);
                context.post(ko.mapping.toJSON(adapter), "sqlserver-adapter/databases")
                    .done(function (result) {
                        loadingSchemas(false);
                        loadingDatabases(false);
                        if (result.success) {
                            connected(true);
                            databaseOptions(result.databases);
                            logger.info("You are now connected, please select your schema");
                        } else {
                            connected(false);
                            logger.error(result.message);
                        }
                        tcs.resolve(true);
                    });

                return tcs.promise();
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
            schemaOptions: schemaOptions,
            databaseOptions: databaseOptions,
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
