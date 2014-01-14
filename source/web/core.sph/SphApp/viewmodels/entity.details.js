/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../objectbuilders.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.system],
    function (context, logger, router, system) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            isBusy = ko.observable(false),
            member = ko.observable(new bespoke.sph.domain.Member()),
            activate = function (id2) {
                var id = parseInt(id2);
                if (id) {
                    var query = String.format("EntityDefinitionId eq {0}", id);
                    var tcs = new $.Deferred();
                    context.loadOneAsync("EntityDefinition", query)
                        .done(function (b) {
                            entity(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                }

                entity(new bespoke.sph.domain.EntityDefinition(system.guid()));
                return Task.fromResult(true);

            },
            attached = function () {

            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(entity);
                isBusy(true);

                context.post(data, "/EntityDefinition/Save")
                    .then(function (result) {
                        isBusy(false);


                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            publishAsync = function () {
                
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(entity);
                isBusy(true);

                context.post(data, "/EntityDefinition/Publish")
                    .then(function (result) {
                        isBusy(false);


                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            entity: entity,
            member : member,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                    {
                        command: publishAsync,
                        caption: 'Publish',
                        icon: "fa fa-sign-out"
                    }])
            }
        };

        return vm;

    });
