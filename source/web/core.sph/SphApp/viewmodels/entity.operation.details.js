/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


define(['services/datacontext', 'services/logger', objectbuilders.system, objectbuilders.config],
    function (context, logger, system, config) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            errors = ko.observableArray(),
            operation = ko.observable(new bespoke.sph.domain.EntityOperation()),
            isBusy = ko.observable(false),
            activate = function (eid, name) {
                var query = String.format("EntityDefinitionId eq {0}", eid),
                    tcs = new $.Deferred();
                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        entity(b);
                        var o = _(b.EntityOperationCollection()).find(function (v) {
                            return v.Name() == name;
                        });
                        if (!o) {
                            o = new bespoke.sph.domain.EntityOperation({
                                WebId: system.guid(),
                                Name: name
                            });
                            entity().EntityOperationCollection.push(o);
                        }
                        operation(o);
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            attached = function (view) {

            },
            save = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);
                isBusy(true);

                context.post(data, "/EntityDefinition/Save")
                    .then(function (result) {
                        tcs.resolve(true);
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            entity().EntityDefinitionId(result.id);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your entity, !!!");
                        }
                    });
                return tcs.promise();
            };

        var vm = {
            entity: entity,
            operation: operation,
            isBusy: isBusy,
            config: config,
            activate: activate,
            attached: attached,
            toolbar : {
                saveCommand : save
            }
        };

        return vm;

    });
